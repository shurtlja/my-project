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

    public async Task<List<VocabularyWord>> GenerateVocabulary(int count, string language, string? topic = null)
    {
        var topicClause = string.IsNullOrWhiteSpace(topic) ? string.Empty : $" about {topic}";
        var prompt = $"Generate {count} vocabulary words in {language} about {topicClause}. Return only a JSON array of objects with keys \"word\" and \"english\". Example: [{{\"word\":\"你好\",\"english\":\"hello\"}}, ...]";
        model.SetStopSequence("]");
        var json = await model.Run(prompt);
        Console.WriteLine("Model Output: " + json);

        // Try to extract JSON array from model output in case it includes extra text
        var start = json.IndexOf('[');
        var end = json.LastIndexOf(']');
        if (start >= 0 && end > start)
        {
            json = json.Substring(start, end - start + 1);
        }

        try
        {
            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Temporary DTO matching the expected output
            var dtos = System.Text.Json.JsonSerializer.Deserialize<List<TempWordDto>>(json, options);
            if (dtos == null) return new List<VocabularyWord>();

            var result = dtos.Select(d => new VocabularyWord { Word = d.word ?? string.Empty, Meaning = d.english ?? string.Empty }).ToList();
            return result;
        }
        catch
        {
            return new List<VocabularyWord>();
        }
    }


    public async Task<string> GenerateContextSentence(string word)
    {
        model.SetStopSequence("\n");
        return await model.Run($"Generate a sample sentence using the word: {word}");
    }

    public async Task<string> GenerateChatResponse(string message)
    {
        model.SetStopSequence("\n");
        return await model.Run(message);
    }
}