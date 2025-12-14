using System.Text.Json;

public class VocabularyGenerator : AIService
{
    private class TempWordDto
    {
        public string? word { get; set; }
        public string? english { get; set; }
    }

    private readonly int count;
    private readonly string language;
    private readonly string? topic;
    private readonly string? promptAddition;

    public VocabularyGenerator(int count, string language, string? topic = null, string? promptAddition = null)
    {
        this.count = count;
        this.language = language;
        this.topic = topic;
        this.promptAddition = promptAddition;
    }

    public override async Task<object?> Generate()
    {
        var count = this.count;
        var language = this.language;
        var topic = this.topic;
        var promptAddition = this.promptAddition;

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

            var start = jsonOut.IndexOf('[');
            var end = jsonOut.LastIndexOf(']');
            if (start >= 0 && end > start)
            {
                jsonOut = jsonOut.Substring(start, end - start + 1);
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                List<TempWordDto>? dtos = null;

                var opens = new List<int>();
                var closes = new List<int>();
                for (int i = 0; i < jsonOut.Length; i++)
                {
                    if (jsonOut[i] == '[') opens.Add(i);
                    else if (jsonOut[i] == ']') closes.Add(i);
                }

                bool parsed = false;

                foreach (var startIdx in opens)
                {
                    foreach (var endIdx in closes.Where(c => c > startIdx))
                    {
                        var candidate = jsonOut.Substring(startIdx, endIdx - startIdx + 1);
                        try
                        {
                            using var doc = System.Text.Json.JsonDocument.Parse(candidate);
                            if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Array) continue;
                            var arr = doc.RootElement;
                            if (arr.GetArrayLength() == 0) continue;
                            var first = arr[0];
                            if (first.ValueKind == System.Text.Json.JsonValueKind.Object)
                            {
                                // parse this candidate into dtos
                                var list = new List<TempWordDto>();
                                    foreach (var el in arr.EnumerateArray())
                                {
                                    if (el.ValueKind == System.Text.Json.JsonValueKind.Object)
                                    {
                                        string? w = null, e = null;
                                        foreach (var p in el.EnumerateObject())
                                        {
                                            var name = p.Name.Trim().ToLowerInvariant();
                                            if (w == null && name == "word")
                                                w = p.Value.ValueKind == System.Text.Json.JsonValueKind.String ? p.Value.GetString() : p.Value.ToString();
                                            if (e == null && (name.Contains("eng") || name.Contains("mean") || name.Contains("def")))
                                                e = p.Value.ValueKind == System.Text.Json.JsonValueKind.String ? p.Value.GetString() : p.Value.ToString();
                                        }
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
                                       
                                }
                                if (list.Count > 0)
                                {
                                    dtos = list;
                                    parsed = true;
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                    if (parsed) break;
                }

                if (!parsed)
                {
                    var firstBracket = jsonOut.IndexOf('[');
                    if (firstBracket < 0) continue;
                    var closingIndices = new List<int>();
                    for (int i = firstBracket; i < jsonOut.Length; i++) if (jsonOut[i] == ']') closingIndices.Add(i);

                    foreach (var endIdx in closingIndices)
                    {
                        var candidate = jsonOut.Substring(firstBracket, endIdx - firstBracket + 1);
                        try
                        {
                            dtos = JsonSerializer.Deserialize<List<TempWordDto>>(candidate, options);
                            if (dtos != null && dtos.Count > 0)
                            {
                                parsed = true;
                                break;
                            }
                        }
                        catch { }
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
        }
        return new List<VocabularyWord>();
    }
}
