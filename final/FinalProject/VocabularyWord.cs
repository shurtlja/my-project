public class VocabularyWord
{
    public string Word { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public List<string> ContextSentences { get; set; } = new List<string>();

    public string GetWord() {
        return Word;
    }

    public string GetMeaning() {
        return Meaning;
    }

    public string GetRandomContext()
    {
        if (ContextSentences == null || ContextSentences.Count == 0) return "No context available.";
        var rand = new Random();
        return ContextSentences[rand.Next(ContextSentences.Count)];
    }
}