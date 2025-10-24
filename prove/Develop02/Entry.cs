class Entry
{
    private string Timestamp;
    private string Content;

    public Entry(string content, string timestamp = null)
    {
        if (timestamp != null)
        {
            Timestamp = timestamp;
        }
        else
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        Content = content;
    }

    public override string ToString() => Content;

    public string GetDate() => Timestamp;
}