// To go beyond the requirements I included titles that you can earn
// after reaching certain point thresholds.

using System;
class Program
{
    static string saveFile = "goals.txt";
    static GoalManager manager = new GoalManager();

    static void Main()
    {
        manager.Load(saveFile);

        while (true)
        {
            Console.WriteLine("--- Eternal Quest ---");
            Console.WriteLine("\n1. Make a new goal");
            Console.WriteLine("2. Record completed goal");
            Console.WriteLine("3. View achievements");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1": manager.MakeGoal(); manager.Save(saveFile); break;
                case "2": manager.RecordGoal(); manager.Save(saveFile); break;
                case "3": manager.ViewAchievements(); break;
                case "quit": return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }
}
