using System;
using System.Threading.Tasks;
public abstract class AIService
{
    protected LocalModel model;

    public AIService()
    {
        model = new LocalModel();
        string modelPath = @"Models\qwen2.5-3b-instruct-q4_k_m.gguf";
        model.LoadModel(modelPath);
    }
    public abstract Task<object?> Generate();
}