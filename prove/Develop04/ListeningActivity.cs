class ListeningActivity : Activity
{
    public ListeningActivity(string name, string description)
        : base(name, description) { }
    
    private List<string> _prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public override void RunActivity()
    {
        string prompt = _prompts[new Random().Next(_prompts.Count)];
        Console.WriteLine(prompt);
        Spinner();
        while (_countdown > 0)
        {
            Console.WriteLine("List as many responses as you can...");
            Spinner();
            Spinner();
        }
    }
}
