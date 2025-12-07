public class Flashcard
{
    private int consecutiveCorrect;
    public VocabularyWord WordData { get; private set; }


    public Flashcard(VocabularyWord w)
    {
        WordData = w;
        consecutiveCorrect = 0;
    }


    public string GetWord() => WordData.Word;
    public string GetMeaning() => WordData.Meaning;
    public string GetContextSentence() => WordData.GetRandomContext();


    public void IncrementCorrect() => consecutiveCorrect++;


    public void ResetCorrect() => consecutiveCorrect = 0;


    public bool IsMastered() => consecutiveCorrect >= 2;
}