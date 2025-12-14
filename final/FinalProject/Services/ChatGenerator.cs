using System.Threading.Tasks;

public class ChatGenerator : AIService
{
    private readonly string message;

    public ChatGenerator(string message)
    {
        this.message = message;
    }

    public override async Task<object?> Generate()
    {
        model.SetStopSequences(new[] { "\n", "You:", "AI:" });
        var res = await model.Run(message);
        return res;
    }
}
