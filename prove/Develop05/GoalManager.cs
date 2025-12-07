class GoalManager
{
    public List<Goal> Goals = new List<Goal>();
    public int TotalPoints = 0;

    public void Save(string path)
    {
        var lines = new List<string>();
        lines.Add(TotalPoints.ToString());
        foreach (var g in Goals) lines.Add(g.Serialize());
        File.WriteAllLines(path, lines);
    }

    public void Load(string path)
    {
        if (!File.Exists(path)) return;
        var lines = File.ReadAllLines(path);
        TotalPoints = int.Parse(lines[0]);
        Goals.Clear();

        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split('|');
            switch (parts[0])
            {
                case "Simple":
                    Goals.Add(new SimpleGoal
                    {
                        Name = parts[1],
                        Points = int.Parse(parts[2]),
                        Done = bool.Parse(parts[3])
                    });
                    break;

                case "Eternal":
                    Goals.Add(new EternalGoal
                    {
                        Name = parts[1],
                        Points = int.Parse(parts[2])
                    });
                    break;

                case "Checklist":
                    Goals.Add(new ChecklistGoal
                    {
                        Name = parts[1],
                        Points = int.Parse(parts[2]),
                        Target = int.Parse(parts[3]),
                        Count = int.Parse(parts[4]),
                        Bonus = int.Parse(parts[5])
                    });
                    break;
            }
        }
    }

    public void MakeGoal()
    {
        Console.Write("Goal name: ");
        string name = Console.ReadLine();

        Console.WriteLine("Type: 1=Simple  2=Eternal  3=Checklist");
        string t = Console.ReadLine();

        Console.Write("Points earned: ");
        int p = int.Parse(Console.ReadLine());

        switch (t)
        {
            case "1":
                Goals.Add(new SimpleGoal { Name = name, Points = p });
                break;

            case "2":
                Goals.Add(new EternalGoal { Name = name, Points = p });
                break;

            case "3":
                Console.Write("Times required: ");
                int target = int.Parse(Console.ReadLine());
                Console.Write("Bonus on completion: ");
                int bonus = int.Parse(Console.ReadLine());
                Goals.Add(new ChecklistGoal
                {
                    Name = name,
                    Points = p,
                    Target = target,
                    Bonus = bonus
                });
                break;
        }
    }

    public void RecordGoal()
    {
        List<Goal> UncompletedGoals = new List<Goal>();
        for (int i = 0; i < Goals.Count; i++)
        {
            if (!Goals[i].IsComplete())
            {
                UncompletedGoals.Add(Goals[i]);
            }
        }
        Console.Clear();
        if (UncompletedGoals.Count == 0)
        {
            Console.WriteLine("No goals available.");
            return;
        }

        for (int i = 0; i < UncompletedGoals.Count; i++)
            Console.WriteLine($"{i + 1}. {UncompletedGoals[i].Name}");

        Console.Write("Choose a goal: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        int earned = UncompletedGoals[index].Record();
        if (UncompletedGoals[index].IsComplete())
        {
            Console.WriteLine("Congratulations! You've completed this goal!");
        }
        TotalPoints += earned;
        Console.WriteLine($"Gained {earned} points!");
    }

    public void ViewAchievements()
    {
        Console.Clear();
        Console.WriteLine($"Total Points: {TotalPoints}");

        string title =
            TotalPoints >= 20000 ? "Legend (Nothing is impossible!)" :
            TotalPoints >= 10000 ? "Grand Master (look how far you've come!)" :
            TotalPoints >= 5000 ? "Worrior (A force to be reckoned with)" :
            TotalPoints >= 2000 ? "Ranger (You've seen a thing or two)" :
            TotalPoints >= 1000 ? "Traveler (The world is your oyster!)" :
            "Novice (it's the beginning of the adventure!)";

        Console.WriteLine($"Title: {title}");

        Console.WriteLine("\nGoals:");
        foreach (var g in Goals)
            Console.WriteLine($"- {g.Name} (Complete: {g.IsComplete()})");
    }
}