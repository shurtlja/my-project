using Avalonia.Controls;
using System.Threading.Tasks;

namespace FinalProject.Activities;

public class AIChatActivity : LearningActivity
{
    public AIChatActivity(AppViewModel vm) : base(vm) { }

    public override Task Run(Window owner)
    {
        ViewModel.ShowAIChatWindow();
        return Task.CompletedTask;
    }
}
