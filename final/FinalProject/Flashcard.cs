public class Flashcard
{
    private int consecutiveCorrect;
    public VocabularyWord WordData { get; private set; }


    public Flashcard(VocabularyWord w)
    {
        WordData = w;
        consecutiveCorrect = 0;
    }


    public string GetWord() 
    {
        return WordData.GetWord();
    }
    public string GetMeaning() 
    {
        return WordData.GetMeaning();
    }
    public string GetContextSentence() 
    {
        return WordData.GetRandomContext();
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