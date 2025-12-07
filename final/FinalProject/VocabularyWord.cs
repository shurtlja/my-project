public class VocabularyWord
{
    public string Word { get; set; }
    public string Meaning { get; set; }
    public List<string> ContextSentences { get; set; }


    public string GetRandomContext()
    {
        if (ContextSentences.Count == 0) return "No context available.";
        var rand = new Random();
        return ContextSentences[rand.Next(ContextSentences.Count)];
    }
}