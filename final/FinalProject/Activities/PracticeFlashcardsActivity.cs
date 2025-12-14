using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Activities;

public class PracticeFlashcardsActivity : LearningActivity
{
    public PracticeFlashcardsActivity(AppViewModel vm) : base(vm) { }

    public override async Task Run(Window owner)
    {
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var setsDir = Path.Combine(baseDir, "FinalProject", "FlashcardSets");

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
            var words = JsonSerializer.Deserialize<List<VocabularyWord>>(json) ?? new List<VocabularyWord>();
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
