public class AIService
{
    private LocalModel model;

    public AIService()
    {
        model = new LocalModel();
        string modelPath = @"Models\qwen2.5-3b-instruct-q4_k_m.gguf";
        model.LoadModel(modelPath);
    }

    public async Task<List<VocabularyWord>> GenerateVocabulary(int count, string language)
    {
        var prompt = $"Generate {count} {language} vocabulary words with meanings and one example sentence each.";
        var json = await model.Run(prompt);


        // Placeholder: you'd deserialize JSON here
        return new List<VocabularyWord>();
    }


    public async Task<string> GenerateContextSentence(string word)
    {
        return await model.Run($"Generate a sample sentence using: {word}");
    }

    public async Task<string> GenerateChatResponse(string message)
    {
        return await model.Run(message);
    }
}