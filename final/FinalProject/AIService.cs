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
        var prompt = $"create {count} {language} vocabulary words with meanings and one example sentence each in json format.";
        var json = await model.Run(prompt);
        Console.WriteLine(json);


        return new List<VocabularyWord>();
    }


    public async Task<string> GenerateContextSentence(string word)
    {
        return await model.Run($"Generate a sample sentence using the word: {word}");
    }

    public async Task<string> GenerateChatResponse(string message)
    {
        return await model.Run(message);
    }
}