using System;

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        library.LoadFromFile("scriptures.txt");
        library.GetScriptures();
        while (true)
        {
            Console.WriteLine("=====Scripture Memorizer=====");
            Console.WriteLine("\n1. Add new scripture");
            Console.WriteLine("2. select scripture"); 
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect an option (1-3): ");
            string choice = Console.ReadLine();
            Console.Clear();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Add new scripture selected.");
                    library.AddScripture();
                    library.SaveToFile("scriptures.txt");
                    break;
                case "2":
                    Console.WriteLine("Choose a scripture to practice.");
                    foreach (var (scripture, index) in library.GetScriptures().Select((value, index) => (value, index)))
                    {
                        Console.WriteLine($"{index + 1} {scripture.GetReference()}");
                    }
                    Console.Write("Enter the number of the scripture to practice: ");
                    if (int.TryParse(Console.ReadLine(), out int scriptureIndex) &&
                        scriptureIndex > 0 && scriptureIndex <= library.GetScriptures().Count)
                    {
                        Scripture selectedScripture = library.GetScriptures()[scriptureIndex - 1];
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(selectedScripture.display());
                            Console.WriteLine("\nPress Enter to hide a word or type 'quit' to return to the main menu.");
                            string input = Console.ReadLine();
                            if (input.ToLower() == "quit" || selectedScripture.IsCompletelyHidden())
                            {
                                Console.Clear();
                                selectedScripture.UnhideAllWords();
                                break;
                            }
                            selectedScripture.HideRandomWord();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid scripture number.");
                    }
                    break;
                case "3":
                    Console.WriteLine("Exiting the program.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select 1, 2, or 3.");
                    break;
            }
        }


        
    }
}