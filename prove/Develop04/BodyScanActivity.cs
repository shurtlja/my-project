class BodyScanActivity : Activity
{
    public BodyScanActivity(string name, string description)
        : base(name, description) { }

    private List<string> _bodyParts = new List<string>
    {
        "toes",
        "feet",
        "ankles",
        "calves",
        "knees",
        "thighs",
        "hips",
        "lower back",
        "abdomen",
        "chest",
        "shoulders",
        "arms",
        "hands",
        "neck",
        "head"
    };
    public override void RunActivity()
    {

        for (int i = 0; i < _bodyParts.Count && _countdown > 0; i++)
        {
            Console.WriteLine($"Focus your attention on your {_bodyParts[i]}.");
            Spinner();
            Spinner();
        }
    }
}