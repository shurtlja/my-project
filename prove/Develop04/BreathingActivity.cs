class BreathingActivity : Activity
{
    public BreathingActivity(string name, string description)
        : base(name, description) { }

    public override void RunActivity()
    {
        while (_countdown > 0)
        {
            Console.WriteLine("Breathe in...");
            Spinner();
            Console.WriteLine("Breathe out...");
            Spinner();
        }
    }
}