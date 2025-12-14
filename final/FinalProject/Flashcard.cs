public class Flashcard
{
    private int consecutiveCorrect;
    private VocabularyWord wordData;

    public Flashcard(VocabularyWord w)
    {
        wordData = w;
        consecutiveCorrect = 0;
    }

    public VocabularyWord GetWordData() => wordData;
    public void SetWordData(VocabularyWord w) => wordData = w;

    public string GetWord()
    {
        return wordData.GetWord();
    }
    public string GetMeaning()
    {
        return wordData.GetMeaning();
    }
    public string GetContextSentence()
    {
        return wordData.GetRandomContext();
    }

    public void IncrementCorrect()
    {
        consecutiveCorrect++;
    }

    public void ResetCorrect()
    {
        consecutiveCorrect = 0;
    }

    public bool IsMastered()
    {
        return consecutiveCorrect >= 2;
    }
}