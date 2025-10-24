using System;
using System.Collections.Generic;
using System.IO;

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
