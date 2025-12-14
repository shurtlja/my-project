using LLama;
using LLama.Common;

public class LocalModel
{
    private LLamaWeights weights;
    private LLamaContext context;
    private InteractiveExecutor executor;
    private ChatSession session;
    private InferenceParams inferenceParams;
    private List<string> stopSequences = new List<string> { "\n" };

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

            // Stop if any of the configured stop sequences appears in the output
            if (stopSequences != null && stopSequences.Count > 0 && stopSequences.Any(s => !string.IsNullOrEmpty(s) && result.Contains(s)))
            {
                break;
            }
        }

        // If the result ends with any of the stop sequences, remove the longest matching suffix.
        if (!string.IsNullOrEmpty(result) && stopSequences != null && stopSequences.Count > 0)
        {
            // order sequences by length to prefer longest match
            var candidates = stopSequences.Where(s => !string.IsNullOrEmpty(s)).OrderByDescending(s => s.Length);
            foreach (var seq in candidates)
            {
                if (result.EndsWith(seq))
                {
                    result = result.Substring(0, result.Length - seq.Length);
                    break;
                }
            }
        }

        return result;
    }

    // Configure a stop sequence (e.g. "AI:") so callers can control when streaming stops.
    // Configure a single stop sequence (backwards-compatible)
    public void SetStopSequence(string sequence)
    {
        stopSequences = new List<string> { sequence ?? string.Empty };
    }

    // Configure multiple stop sequences
    public void SetStopSequences(IEnumerable<string> sequences)
    {
        stopSequences = sequences?.Where(s => s != null).Select(s => s ?? string.Empty).ToList() ?? new List<string>();
    }
}
