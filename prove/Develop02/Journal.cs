class Journal
{
    private List<Entry> entries = new();
    private string savePath = "journal_entries.txt";
    private string[] prompts = new[]
    {
        "How do you feel right now? what happened today to make you feel that way?",
        "What is one thing you did today that you are proud of?",
        "Who is someone you met today that you want to know better?",
        "What is one thing you learned today?",
        "What is one thing you are grateful for today?"
    };

    public void NewEntry()
    {
        Random rand = new();
        string prompt = prompts[rand.Next(prompts.Length)];
        Console.Clear();
        Console.WriteLine($"{prompt}\ntype your response. type '.' on a new line to finish:");
        string content = "", line;
        while ((line = Console.ReadLine()) != ".")
            content += line + Environment.NewLine;

        var entry = new Entry(content.TrimEnd());
        entries.Add(entry);
        Save();
        Console.WriteLine("Entry saved!\nPress Enter to continue...");
        Console.ReadLine();
    }

    public void ViewEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No entries yet.");
            return;
        }
        Console.Clear();

        for (int i = 0; i < entries.Count; i++)
            Console.WriteLine($"{i + 1}. {entries[i].GetDate()}");

        Console.Write("\nChoose an entry number: ");
        if (int.TryParse(Console.ReadLine(), out int index) &&
            index >= 1 && index <= entries.Count)
        {
            Console.Clear();
            Console.WriteLine(entries[index - 1].GetDate());
            Console.WriteLine(entries[index - 1].ToString());
            Console.ReadLine();
        }
    }

    public void Load()
    {
        if (!File.Exists(savePath)) return;
        string[] lines = File.ReadAllLines(savePath);
        string currentKey = null, content = "";

        foreach (var line in lines)
        {
            if (line.StartsWith("### "))
            {
                if (currentKey != null)
                    entries.Add(new Entry(content.TrimEnd(), currentKey));
                currentKey = line.Substring(4);
                content = "";
            }
            else
                content += line + Environment.NewLine;
        }

        if (currentKey != null)
            entries.Add(new Entry(content.TrimEnd(), currentKey));
    }

    public void Save()
    {
        using StreamWriter sw = new(savePath);
        foreach (var entry in entries)
        {
            sw.WriteLine("### " + entry.GetDate());
            sw.WriteLine(entry.ToString());
        }
    }
}