public class LanguageManager
{
    private List<VocabularyWord> wellKnownWords = new List<VocabularyWord>();


    public bool IsWellKnown(string word)
    {
        return wellKnownWords.Exists(w => w.GetWord() == word);
    }


    public void AddWellKnown(VocabularyWord word)
    {
        if (!IsWellKnown(word.GetWord())) wellKnownWords.Add(word);
    }
}