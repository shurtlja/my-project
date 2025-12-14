public class FlashcardSession
{
    private List<Flashcard> deck;
    private int currentIndex;
    private LanguageManager manager;


    public FlashcardSession(LanguageManager manager)
    {
        this.manager = manager;
        deck = new List<Flashcard>();
    }


    public void LoadNewDeck(List<VocabularyWord> words)
    {
        deck.Clear();
        currentIndex = 0;


        foreach (var w in words)
        {
            if (!manager.IsWellKnown(w.GetWord()))
            deck.Add(new Flashcard(w));
        }
    }


    public Flashcard GetCurrentFlashcard() 
    {
        return deck[currentIndex];
    }


    public bool HasNext() 
    {
        return currentIndex < deck.Count;
    }


    public void Next() 
    {
        currentIndex++;
    }


    public void MarkCorrect()
    {
        var card = deck[currentIndex];
        card.IncrementCorrect();


        if (card.IsMastered()) manager.AddWellKnown(card.GetWordData());


        MoveBack(10);
    }


    public void MarkIncorrect()
    {
        deck[currentIndex].ResetCorrect();
        MoveBack(5);
    }


    public void MoveBack(int positions)
    {
        int newIndex = currentIndex + positions;
        if (newIndex >= deck.Count) return;


        var card = deck[currentIndex];
        deck.RemoveAt(currentIndex);
        deck.Insert(newIndex, card);
    }
}