using System;

class Program
{
    static void Main(string[] args)
    {
        string firstName;
        string lastName;
        Console.WriteLine("What is your first name?");
        firstName = Console.ReadLine();
        Console.WriteLine("What is your last name?");
        lastName = Console.ReadLine();
        Console.WriteLine($"Your name is {firstName}, {firstName} {lastName}.");
    }
}