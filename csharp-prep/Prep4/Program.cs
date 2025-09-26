using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int>();
        int input;

        Console.WriteLine("Enter numbers (enter 0 to stop):");
        
        do
        {
            Console.Write("Number: ");
            if (int.TryParse(Console.ReadLine(), out input))
            {
                if (input != 0)
                {
                    numbers.Add(input);
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
                input = -1; // keep loop going
            }
        } while (input != 0);


        int sum = numbers.Sum();
        double average = numbers.Average();
        int max = numbers.Max();

        Console.WriteLine($"\nSum: {sum}");
        Console.WriteLine($"Average: {average:F2}");
        Console.WriteLine($"Maximum: {max}");

    }
}
