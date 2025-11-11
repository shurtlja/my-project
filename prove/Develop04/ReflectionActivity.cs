class ReflectionActivity : Activity
{
    public ReflectionActivity(string name, string description)
        : base(name, description) { }

    private List<string> _prompts = new List<string>
    {
        "Think of a time when you overcame a challenge.",
        "Reflect on a moment of personal growth.",
        "Consider a time when you helped someone in need.",
        "Recall an experience that made you feel grateful."
    };
    
    private List<string> _questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "What did you learn about yourself?",
        "How can you apply this lesson in the future?",
        "What positive impact did this have on others?",
        "How did this experience change your perspective?",
        "What strengths did you discover in yourself?",
        "How can you build on this experience moving forward?"
    };

    public override void RunActivity()
    {
        string prompt = _prompts[new Random().Next(_prompts.Count)];
        Console.WriteLine(prompt);
        Spinner();
        while (_countdown > 0 && _questions.Count > 0)
        {
            string question = _questions[new Random().Next(_questions.Count)];
            Console.WriteLine(question);
            _questions.Remove(question);
            Spinner();
            Spinner();
        }
    }
}