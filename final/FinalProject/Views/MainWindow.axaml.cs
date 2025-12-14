using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FinalProject.Views;

public partial class MainWindow : Window
{
    private AppViewModel viewModel;
    private FinalProject.Activities.LearningActivity createActivity;
    private FinalProject.Activities.LearningActivity practiceActivity;
    private FinalProject.Activities.LearningActivity aiChatActivity;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new AppViewModel();
        DataContext = viewModel;

        // instantiate activities
        createActivity = new FinalProject.Activities.CreateFlashcardsActivity(viewModel);
        practiceActivity = new FinalProject.Activities.PracticeFlashcardsActivity(viewModel);
        aiChatActivity = new FinalProject.Activities.AIChatActivity(viewModel);
    }

    private async void OnNewFlashcardClick(object sender, RoutedEventArgs e)
    {
        await createActivity.Run(this);
    }

    private async void OnPracticeFlashcardClick(object sender, RoutedEventArgs e)
    {
        await practiceActivity.Run(this);
    }

    private async void OnAIChatClick(object sender, RoutedEventArgs e)
    {
        await aiChatActivity.Run(this);
    }

    private void OnExitClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
