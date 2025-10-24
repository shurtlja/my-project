class Word
{
    private string text;
    private bool hidden;

    public Word(string text)
    {
        this.text = text;
        this.hidden = false;
    }

    public string GetText()
    {
        return text;
    }

    public string GetDisplayText()
    {
        return hidden ? new string('_', text.Length) : text;
    }

    public void Hide()
    {
        hidden = true;
    }

    public void Unhide()
    {
        hidden = false;
    }

    public bool IsHidden()
    {
        return hidden;
    }
}