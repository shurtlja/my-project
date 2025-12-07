using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your grade? ");
        int grade;
        grade = int.Parse(Console.ReadLine());
        Dictionary<string, (int, int)> gradeThresholds = new Dictionary<string, (int, int)>
        {
            { "A", (90, 100) },
            { "B", (80, 89) },
            { "C", (70, 79) },
            { "D", (60, 69) },
            { "F", (0, 59) }
        };
        foreach (var threshold in gradeThresholds)
        {
            if (grade >= threshold.Value.Item1 && grade <= threshold.Value.Item2)
            {
                Console.WriteLine($"Your letter grade is: {threshold.Key}");
                break;
            }
        }
        if (grade >= 70)
        {
            Console.WriteLine("Congratulations! You passed the course.");
        }
        else
        {
            Console.WriteLine("Unfortunately, you did not pass the course. Better luck next time!");
        }
    }
}