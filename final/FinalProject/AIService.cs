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

    public async Task<List<VocabularyWord>> GenerateVocabulary(int count, string language, string? topic = null, string? promptAddition = null)
    {
        var topicText = string.IsNullOrWhiteSpace(topic) ? string.Empty : topic.Trim();
        var topicClause = string.IsNullOrEmpty(topicText) ? string.Empty : $" about {topicText}";

        var addition = string.IsNullOrWhiteSpace(promptAddition) ? string.Empty : (" " + promptAddition.Trim());
        var basePrompt = $"Generate {count} vocabulary words in {language}{topicClause}.{addition} Return ONLY a JSON array of objects with keys \"word\" and \"english\". Example: [{{\"word\":\"你好\",\"english\":\"hello\"}}, ...]";

        model.SetStopSequence("]");

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

                var dtos = System.Text.Json.JsonSerializer.Deserialize<List<TempWordDto>>(jsonOut, options);
                if (dtos != null && dtos.Count > 0)
                {
                    var result = dtos.Select(d => new VocabularyWord { Word = d.word ?? string.Empty, Meaning = d.english ?? string.Empty }).ToList();
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
        model.SetStopSequences(["\n","You:","AI:"]);
        return await model.Run(message);
    }
}