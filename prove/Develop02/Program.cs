using System;
using System.Collections.Generic;
using System.IO;

class Entry
{
    public string Timestamp { get; }
    public string Content { get; set; }

    public Entry(string content)
    {
        Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Content = content;
    }

    public override string ToString() => $"{Timestamp}\n{Content}";
}

class Journal
{
    private Dictionary<string, Entry> entries = new();
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
        entries[entry.Timestamp] = entry;
        Save();
        Console.WriteLine("Entry saved!");
    }

    public void ViewEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No entries yet.");
            return;
        }
        Console.Clear();
        var keys = new List<string>(entries.Keys);
        keys.Sort();
        keys.Reverse();

        for (int i = 0; i < keys.Count; i++)
            Console.WriteLine($"{i + 1}. {keys[i]}");

        Console.Write("\nChoose an entry number: ");
        if (int.TryParse(Console.ReadLine(), out int index) &&
            index >= 1 && index <= keys.Count)
        {
            Console.Clear();
            Console.WriteLine(entries[keys[index - 1]]);
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
                    entries[currentKey] = new Entry(content.TrimEnd()) { };
                currentKey = line.Substring(4);
                content = "";
            }
            else
                content += line + Environment.NewLine;
        }

        if (currentKey != null)
            entries[currentKey] = new Entry(content.TrimEnd());
    }

    public void Save()
    {
        using StreamWriter sw = new(savePath);
        foreach (var entry in entries)
        {
            sw.WriteLine("### " + entry.Key);
            sw.WriteLine(entry.Value.Content);
        }
    }
}

class Program
{
    static void Main()
    {
        var journal = new Journal();
        journal.Load();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Journal Menu ===");
            Console.WriteLine("\n1. New Entry");
            Console.WriteLine("2. View Entries");
            Console.WriteLine("3. Exit");
            Console.Write("Select: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": journal.NewEntry(); break;
                case "2": journal.ViewEntries(); break;
                case "3": return;
                default: Console.WriteLine("Invalid."); break;
            }
        }
    }
}
