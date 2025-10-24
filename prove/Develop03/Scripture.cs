class Scripture
{
    private Reference reference;
    private List<Word> words;
    private bool completelyHidden = false;
    public Scripture(string referenceText, string scriptureText)
    {
        reference = new Reference(referenceText);
        words = new List<Word>();
        string[] wordArray = scriptureText.Split(' ');
        foreach (string wordText in wordArray)
        {
            words.Add(new Word(wordText));
        }
    }

    public string GetReference()
    {
        return reference.ToString();
    }

    public string GetText()
    {
        List<string> wordTexts = new List<string>();
        foreach (Word word in words)
        {
            wordTexts.Add(word.GetText());
        }
        return string.Join(" ", wordTexts);
    }

    public string display()
    {
        List<string> displayWords = new List<string>();
        foreach (Word word in words)
        {
            displayWords.Add(word.GetDisplayText());
        }
        return $"{reference.ToString()} {string.Join(" ", displayWords)}";
    }

    public void HideRandomWord()
    {
        List<Word> visibleWords = words.Where(w => !w.IsHidden()).ToList();
        if (visibleWords.Count > 0)
        {
            Random rand = new Random();
            int index = rand.Next(visibleWords.Count);
            visibleWords[index].Hide();
        }
    }

    public bool IsCompletelyHidden()
    {
        return words.All(w => w.IsHidden());
    }

    public void UnhideAllWords()
    {
        foreach (Word word in words)
        {
            if (word.IsHidden())
            {
                // Assuming Word class has a method to unhide the word
                word.Unhide();
            }
        }
    }
}