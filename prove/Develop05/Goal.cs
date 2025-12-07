abstract class Goal
{
    public string Name;
    public int Points;

    public abstract int Record();       // returns points earned
    public abstract bool IsComplete();
    public abstract string Serialize(); // for saving
}