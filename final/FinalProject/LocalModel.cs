using LLama;
using LLama.Common;

public class LocalModel
{
    private LLamaWeights weights;
    private LLamaContext context;
    private InteractiveExecutor executor;
    private ChatSession session;
    private InferenceParams inferenceParams;
    private string stopSequence = "\n";

    public void LoadModel(string path)
    {
        var parameters = new ModelParams(path)
        {
            ContextSize = 2048
        };

        weights = LLamaWeights.LoadFromFile(parameters);
        context = weights.CreateContext(parameters);
        executor = new InteractiveExecutor(context);

        session = new ChatSession(executor);

        inferenceParams = new InferenceParams()
        {
            MaxTokens = 256
        };
    }

    public async Task<string> Run(string prompt)
    {
        var result = "";

        await foreach (var token in session.ChatAsync(
            new ChatHistory.Message(AuthorRole.User, prompt),
            inferenceParams))
        {
            result += token;

            // Stop if the configured stop sequence appears in the output
            if (!string.IsNullOrEmpty(stopSequence) && result.Contains(stopSequence))
            {
                break;
            }
        }

        return result;
    }

    // Configure a stop sequence (e.g. "AI:") so callers can control when streaming stops.
    public void SetStopSequence(string sequence)
    {
        stopSequence = sequence ?? string.Empty;
    }
}
