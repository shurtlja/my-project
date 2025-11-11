class Activity
{
    protected string _name;
    protected string _description;

    protected int _duration;

    protected int _countdown;

    public Activity(string name, string description)
    {
        _name = name;
        _description = description;
    }

    public void DisplayStartingMessage()
    {
        Console.WriteLine($"Welcome to {_name}. {_description} How long, in seconds, would you like for your session?");
        string input = Console.ReadLine();
        _duration = int.Parse(input);
        _countdown = _duration;
        Console.WriteLine("Get ready...");
        Spinner();
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine($"This concludes the {_name} Activity. Well done! Your session lasted {_duration} seconds.");
        Spinner();
    }

    public void Spinner()
    {
        string[] spinner = { "|", "\\", "—", "/" };
        Console.Write("/");
        for (int i = 0; i < 3; i++)
        {
            foreach (string s in spinner)// this loop lasts for 2 seconds total
            {
                Thread.Sleep(500);
                Console.Write("\b \b");
                Console.Write(s);
            }
            _countdown -= 2;
        }
        Console.Write("\b \b");
    }

    public virtual void RunActivity()
    {
        // Base implementation (is overridden later)
        Console.WriteLine("Running base activity...");
    }
}
