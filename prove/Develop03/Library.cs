class Library
{
    private List<Scripture> scriptures = new();

    public void AddScripture()
    {
        Console.Write("Enter the scripture reference: ");
        string reference = Console.ReadLine();
        Console.Write("Enter the scripture text: ");
        string text = Console.ReadLine();
        scriptures.Add(new Scripture(reference, text));
    }
    public List<Scripture> GetScriptures()
    {
        return scriptures;
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var scripture in scriptures)
            {
                writer.WriteLine($"####{scripture.GetReference()}");
                writer.WriteLine(scripture.GetText());
            }
        }
    }

    public void LoadFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                string reference = null;
                string contents = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("####"))
                    {
                        if (reference != null)
                        {
                            Scripture scripture = new Scripture(reference, contents.Trim());
                            scriptures.Add(scripture);
                            contents = "";
                        }
                        reference = line.Substring(4);
                        continue;
                    }
                    contents += line + "\n";

                }
                if (reference != null)
                        {
                            Scripture scripture = new Scripture(reference, contents.Trim());
                            scriptures.Add(scripture);
                        }
            }
        }
    }
}