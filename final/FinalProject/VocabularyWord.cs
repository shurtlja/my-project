public class VocabularyWord
{
    private string word = string.Empty;
    private string meaning = string.Empty;
    private List<string> contextSentences = new List<string>();

    // Getters and setters
    public string GetWord() {
        return word;
    }
    public void SetWord(string value) {
         word = value ?? string.Empty;
    }
    public string GetMeaning() {
        return meaning;
    }
    public void SetMeaning(string value) {
         meaning = value ?? string.Empty;
    }

    public IReadOnlyList<string> GetContextSentences() => contextSentences.AsReadOnly();
    public void AddContextSentence(string sentence)
    {
        if (!string.IsNullOrWhiteSpace(sentence)) contextSentences.Add(sentence);
    }

    public string GetRandomContext()
    {
        if (contextSentences == null || contextSentences.Count == 0) return "No context available.";
        var rand = new Random();
        return contextSentences[rand.Next(contextSentences.Count)];
    }
}