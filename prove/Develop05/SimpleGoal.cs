class SimpleGoal : Goal
{
    public bool Done = false;

    public override int Record()
    {
        if (Done) return 0;
        Done = true;
        return Points;
    }

    public override bool IsComplete()
    {
        return Done;
    } 

    public override string Serialize()
    {
        return $"Simple|{Name}|{Points}|{Done}";
    }
}