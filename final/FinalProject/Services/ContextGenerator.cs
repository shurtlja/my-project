using System.Threading.Tasks;

public class ContextGenerator : AIService
{
    private readonly string word;

    public ContextGenerator(string word)
    {
        this.word = word;
    }

    public override async Task<object?> Generate()
    {
        model.SetStopSequence("\n");
        var res = await model.Run($"Generate a sample sentence using the word: {word}");
        return res;
    }
}
