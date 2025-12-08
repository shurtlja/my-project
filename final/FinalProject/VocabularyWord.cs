public class VocabularyWord
{
    private string Word;
    private string Meaning;
    private List<string> ContextSentences;

    public string GetWord()
    {
        return Word;
    }

    public string GetMeaning()
    {
        return Meaning;
    }

    public string GetRandomContext()
    {
        if (ContextSentences.Count == 0) return "No context available.";
        var rand = new Random();
        return ContextSentences[rand.Next(ContextSentences.Count)];
    }
}