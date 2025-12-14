using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using System.Linq;
using System.Text.Json;

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

    private async void OnPracticeFlashcardClick(object sender, RoutedEventArgs e)
    {
        // look for saved flashcard sets
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var setsDir = Path.Combine(baseDir, "FinalProject", "FlashcardSets");

        if (!Directory.Exists(setsDir))
        {
            // no saved sets, open empty practice window
            viewModel.ShowPracticeWindow();
            return;
        }

        var files = Directory.GetFiles(setsDir, "*.json").OrderByDescending(f => File.GetLastWriteTimeUtc(f)).ToArray();
        if (files.Length == 0)
        {
            viewModel.ShowPracticeWindow();
            return;
        }

        string? chosenPath = null;
        if (files.Length == 1)
        {
            chosenPath = files[0];
        }
        else
        {
            var dialog = new OpenFileDialog();
            dialog.AllowMultiple = false;
            dialog.Filters.Add(new FileDialogFilter { Name = "Flashcard Sets", Extensions = { "json" } });
            try
            {
                // set initial directory when supported
                dialog.Directory = setsDir;
            }
            catch { }

            var result = await dialog.ShowAsync(this);
            if (result != null && result.Length > 0)
                chosenPath = result[0];
        }

        if (chosenPath == null)
        {
            // user cancelled
            return;
        }

        try
        {
            var json = await File.ReadAllTextAsync(chosenPath);
            var words = JsonSerializer.Deserialize<List<VocabularyWord>>(json) ?? new List<VocabularyWord>();
            var flashcards = words.Select(w => new Flashcard(w)).ToList();
            viewModel.ShowPracticeWindow(flashcards);
        }
        catch (Exception ex)
        {
            // fallback: open empty practice window
            Console.WriteLine($"Failed to load flashcard set: {ex.Message}");
            viewModel.ShowPracticeWindow();
        }
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
