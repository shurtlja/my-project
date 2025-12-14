using Avalonia.Controls;

namespace FinalProject.Activities;

public class CreateFlashcardsActivity : LearningActivity
{
    public CreateFlashcardsActivity(AppViewModel vm) : base(vm) { }

    public override Task Run(Window owner)
    {
        ViewModel.ShowNewFlashcardWindow();
        return Task.CompletedTask;
    }
}
