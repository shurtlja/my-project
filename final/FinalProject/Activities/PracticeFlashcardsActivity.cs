using Avalonia.Controls;
using System.Text.Json;


namespace FinalProject.Activities;

public class PracticeFlashcardsActivity : LearningActivity
{
    public PracticeFlashcardsActivity(AppViewModel vm) : base(vm) { }

    public override async Task Run(Window owner)
    {
        var projDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory ?? Environment.CurrentDirectory, "..", "..", ".."));
        var setsDir = Path.Combine(projDir, "FlashcardSets");

        if (!Directory.Exists(setsDir))
        {
            ViewModel.ShowPracticeWindow();
            return;
        }

        var files = Directory.GetFiles(setsDir, "*.json").OrderByDescending(f => File.GetLastWriteTimeUtc(f)).ToArray();
        if (files.Length == 0)
        {
            ViewModel.ShowPracticeWindow();
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
                dialog.Directory = setsDir;
            }
            catch { }

            var result = await dialog.ShowAsync(owner);
            if (result != null && result.Length > 0)
                chosenPath = result[0];
        }

        if (chosenPath == null)
        {
            return;
        }

        try
        {
            var json = await File.ReadAllTextAsync(chosenPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var dtos = JsonSerializer.Deserialize<List<TempWordDto>>(json, options) ?? new List<TempWordDto>();
            var words = dtos.Select(d => {
                var v = new VocabularyWord();
                v.SetWord(d.word ?? string.Empty);
                v.SetMeaning(d.english ?? string.Empty);
                return v;
            }).ToList();
            var flashcards = words.Select(w => new Flashcard(w)).ToList();
            ViewModel.ShowPracticeWindow(flashcards);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load flashcard set: {ex.Message}");
            ViewModel.ShowPracticeWindow();
        }
    }
}

internal class TempWordDto
{
    public string? word { get; set; }
    public string? english { get; set; }
}
