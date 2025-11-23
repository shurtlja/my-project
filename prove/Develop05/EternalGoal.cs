class EternalGoal : Goal
{
    public override int Record()
    {
        return Points;
    }

    public override bool IsComplete()  
    {
        return false;
    }

    public override string Serialize()
    {
        return $"Eternal|{Name}|{Points}";
    }
}