using System.Linq;

public class AIService
{
    private LocalModel model;

    private class TempWordDto
    {
        public string? word { get; set; }
        public string? english { get; set; }
    }

    public AIService()
    {
        model = new LocalModel();
        string modelPath = @"Models\qwen2.5-3b-instruct-q4_k_m.gguf";
        model.LoadModel(modelPath);
    }

    // helper to split strings like "word - meaning" or "word: meaning"
    private (string word, string? meaning) TrySplitWordMeaning(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return (string.Empty, null);

        var separators = new[] { " - ", " — ", " -- ", ": ", " – ", "\t" };
        foreach (var sep in separators)
        {
            var parts = s.Split(new[] { sep }, 2, StringSplitOptions.None);
            if (parts.Length == 2)
            {
                return (parts[0].Trim(), parts[1].Trim());
            }
        }

        // fallback: if contains whitespace, try first token as word
        var ws = s.Trim().Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        if (ws.Length == 2) return (ws[0].Trim(), ws[1].Trim());
        return (s.Trim(), null);
    }

    public async Task<List<VocabularyWord>> GenerateVocabulary(int count, string language, string? topic = null, string? promptAddition = null)
    {
        var topicText = string.IsNullOrWhiteSpace(topic) ? string.Empty : topic.Trim();
        var topicClause = string.IsNullOrEmpty(topicText) ? string.Empty : $" about {topicText}";

        var addition = string.IsNullOrWhiteSpace(promptAddition) ? string.Empty : (" " + promptAddition.Trim());
        var basePrompt = $"Generate {count} vocabulary words in {language}{topicClause}.{addition} Return ONLY a JSON array of objects with keys \"word\" and \"english\". Example: [{{\"word\":\"你好\",\"english\":\"hello\"}}, ...]";

        model.SetStopSequence("this string will never appear"); // disable default stop sequence

        int maxAttempts = 3;
        for (int attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var prompt = attempt == 1
                ? basePrompt
                : $"The previous response was not valid JSON. {basePrompt} Respond with the JSON array only, no explanation.";

            var jsonOut = await model.Run(prompt);
            Console.WriteLine($"Model Output (attempt {attempt}): {jsonOut}");

            // Try to extract JSON array from model output in case it includes extra text
            var start = jsonOut.IndexOf('[');
            var end = jsonOut.LastIndexOf(']');
            if (start >= 0 && end > start)
            {
                jsonOut = jsonOut.Substring(start, end - start + 1);
            }

            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                List<TempWordDto>? dtos = null;

                // 1) Try direct deserialization to expected DTO list
                try
                {
                    dtos = System.Text.Json.JsonSerializer.Deserialize<List<TempWordDto>>(jsonOut, options);
                }
                catch (Exception ex1)
                {
                    Console.WriteLine($"Direct JSON deserialize failed: {ex1.Message}");

                    // 2) Try parsing with JsonDocument to handle arrays of objects or strings
                    try
                    {
                        using var doc = System.Text.Json.JsonDocument.Parse(jsonOut);
                        if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                        {
                            var list = new List<TempWordDto>();
                            foreach (var el in doc.RootElement.EnumerateArray())
                            {
                                if (el.ValueKind == System.Text.Json.JsonValueKind.Object)
                                {
                                    string? w = null, e = null;

                                    // helper to extract a string from different kinds of JSON values
                                    static string? ExtractString(System.Text.Json.JsonElement parent, string[] keys)
                                    {
                                        foreach (var key in keys)
                                        {
                                            if (parent.TryGetProperty(key, out var prop))
                                            {
                                                switch (prop.ValueKind)
                                                {
                                                    case System.Text.Json.JsonValueKind.String:
                                                        return prop.GetString();
                                                    case System.Text.Json.JsonValueKind.Number:
                                                    case System.Text.Json.JsonValueKind.True:
                                                    case System.Text.Json.JsonValueKind.False:
                                                        return prop.ToString();
                                                    case System.Text.Json.JsonValueKind.Array:
                                                        // take first element as string if possible
                                                        if (prop.GetArrayLength() > 0)
                                                        {
                                                            var first = prop[0];
                                                            if (first.ValueKind == System.Text.Json.JsonValueKind.String) return first.GetString();
                                                            return first.ToString();
                                                        }
                                                        break;
                                                    case System.Text.Json.JsonValueKind.Object:
                                                        // fallthrough: try to stringify
                                                        return prop.ToString();
                                                }
                                            }
                                        }
                                        return null;
                                    }

                                    w = ExtractString(el, new[] { "word", "text", "word_text", "term" });
                                    e = ExtractString(el, new[] { "english", "meaning", "definition" });
                                    // if still null try to look for any property with string value
                                    if (w == null)
                                    {
                                        foreach (var p in el.EnumerateObject())
                                        {
                                            if (p.Value.ValueKind == System.Text.Json.JsonValueKind.String)
                                            {
                                                w = p.Value.GetString();
                                                break;
                                            }
                                        }
                                    }

                                    list.Add(new TempWordDto { word = w, english = e });
                                }
                                else if (el.ValueKind == System.Text.Json.JsonValueKind.String)
                                {
                                    var s = el.GetString() ?? string.Empty;
                                    var pair = TrySplitWordMeaning(s);
                                    list.Add(new TempWordDto { word = pair.word, english = pair.meaning });
                                }
                                else
                                {
                                    // try to convert other primitive kinds to string
                                    list.Add(new TempWordDto { word = el.ToString(), english = null });
                                }
                            }

                            if (list.Count > 0) dtos = list;
                        }
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"JsonDocument parse failed: {ex2.Message}");
                    }

                    // 3) Fallback: extract object-like blocks with regex and try to deserialize each
                    if (dtos == null)
                    {
                        try
                        {
                            var matches = System.Text.RegularExpressions.Regex.Matches(jsonOut, "\\{[^}]*\\}");
                            var list = new List<TempWordDto>();
                            foreach (System.Text.RegularExpressions.Match m in matches)
                            {
                                try
                                {
                                    var dto = System.Text.Json.JsonSerializer.Deserialize<TempWordDto>(m.Value, options);
                                    if (dto != null) list.Add(dto);
                                }
                                catch { /* ignore individual parse failures */ }
                            }

                            if (list.Count > 0) dtos = list;
                        }
                        catch (Exception ex3)
                        {
                            Console.WriteLine($"Regex fallback parse failed: {ex3.Message}");
                        }
                    }
                }

                if (dtos != null && dtos.Count > 0)
                {
                    var result = dtos.Select(d => {
                        var v = new VocabularyWord();
                        v.SetWord(d.word ?? string.Empty);
                        v.SetMeaning(d.english ?? string.Empty);
                        return v;
                    }).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON parse attempt {attempt} failed: {ex.Message}");
            }

            // if not last attempt, loop to regenerate
        }

        // All attempts failed — return empty list
        return new List<VocabularyWord>();
    }


    public async Task<string> GenerateContextSentence(string word)
    {
        model.SetStopSequence("\n");
        return await model.Run($"Generate a sample sentence using the word: {word}");
    }

    public async Task<string> GenerateChatResponse(string message)
    {
            model.SetStopSequences(new[] { "\n", "You:", "AI:" });
        return await model.Run(message);
    }
}