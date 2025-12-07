using System;

class Program
{
    static void DisplayMenu()
    {
        Console.WriteLine("1. Breathing Activity");
        Console.WriteLine("2. Reflection Activity");
        Console.WriteLine("3. Listening Activity");
        Console.WriteLine("4. Body Scan Activity");
        Console.Write("Choose an activity: ");
    }

    static void Main()
    {
        DisplayMenu();
        
        string choice = Console.ReadLine();

        Activity activity = choice switch
        {
            "1" => new BreathingActivity("Breathing", "This activity will help you relax and focus on your breathing."),
            "2" => new ReflectionActivity("Reflection", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life."),
            "3" => new ListeningActivity("Listening", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area."),
            "4" => new BodyScanActivity("Body Scan", "This activity will guide you through a body scan meditation to help you relax and become aware of your physical body."),
            _ => null
        };

        if (activity != null)
        {
            activity.DisplayStartingMessage();
            activity.RunActivity();
            activity.DisplayEndingMessage();
        }
    }
}
