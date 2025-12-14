using Avalonia.Controls;

namespace FinalProject.Activities;

public abstract class LearningActivity
{
    protected readonly AppViewModel ViewModel;
    protected LearningActivity(AppViewModel viewModel)
    {
        ViewModel = viewModel;
    }
    public abstract Task Run(Window owner);
}
