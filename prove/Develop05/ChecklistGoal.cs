class ChecklistGoal : Goal
{
    public int Target;
    public int Count;
    public int Bonus;

    public override int Record()
    {
        if (Count >= Target) return 0;
        Count++;
        if (Count == Target) return Points + Bonus;
        return Points;
    }

    public override bool IsComplete()
    {
        return Count >= Target;
    }
     

    public override string Serialize()
    {
        return $"Checklist|{Name}|{Points}|{Target}|{Count}|{Bonus}";
    }
}
