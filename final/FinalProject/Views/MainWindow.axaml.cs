using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FinalProject.Views;

public partial class MainWindow : Window
{
    private AppViewModel viewModel;

    public MainWindow()
    {
        InitializeComponent();
        viewModel = new AppViewModel();
        DataContext = viewModel;
    }

    private void OnNewFlashcardClick(object sender, RoutedEventArgs e)
    {
        viewModel.ShowNewFlashcardWindow();
    }

    private void OnPracticeFlashcardClick(object sender, RoutedEventArgs e)
    {
        viewModel.ShowPracticeWindow();
    }

    private void OnAIChatClick(object sender, RoutedEventArgs e)
    {
        viewModel.ShowAIChatWindow();
    }

    private void OnExitClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
