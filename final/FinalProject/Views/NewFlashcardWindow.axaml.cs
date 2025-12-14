using Avalonia.Controls;
using Avalonia.Interactivity;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

#nullable enable

namespace FinalProject.Views;

public partial class NewFlashcardWindow : Window
{
    private AIService aiService;
    public List<Flashcard> GeneratedFlashcards { get; private set; }
    public event Action<List<Flashcard>>? FlashcardsGenerated;

    private static string GetSetsDirectory()
    {
        var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dir = Path.Combine(baseDir, "FinalProject", "FlashcardSets");
        return dir;
    }

    public NewFlashcardWindow()
    {
        InitializeComponent();
        aiService = new AIService();
        GeneratedFlashcards = new List<Flashcard>();
    }

    private string GetComboBoxSelectedText(ComboBox combo, string fallback)
    {
        if (combo.SelectedItem is ComboBoxItem cbi && cbi.Content != null)
            return cbi.Content.ToString() ?? fallback;

        if (combo.SelectedItem is string s)
            return s;

        if (combo.SelectedItem != null)
            return combo.SelectedItem.ToString() ?? fallback;

        return fallback;
    }

    private async void OnGenerateClick(object sender, RoutedEventArgs e)
    {
        GenerateBtn.IsEnabled = false;
        GenerateBtn.Content = "Generating...";

        try
        {
            var language = GetComboBoxSelectedText(LanguageCombo, "Spanish");
            var count = (int)(WordCountUpDown.Value ?? 20);

            var topic = TopicBox.Text?.Trim();
            Console.WriteLine($"Generating {count} words in {language} about topic: {topic}"); // debug for prompt

            // Load known words (if any) to compare overlap
            var exeDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;
            var dataDir = Path.Combine(exeDir, "data");
            var knownPath = Path.Combine(dataDir, "knownWords.txt");
            var knownSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (File.Exists(knownPath))
            {
                foreach (var line in File.ReadAllLines(knownPath))
                {
                    var t = line?.Trim();
                    if (!string.IsNullOrWhiteSpace(t)) knownSet.Add(t);
                }
            }

            // generate and, if too many known words appear, regenerate with added difficulty hints
            List<VocabularyWord> finalWords = new List<VocabularyWord>();
            int maxRegens = 2;
            int regenCount = 0;
            string? promptAddition = null;

            while (true)
            {
                var words = await aiService.GenerateVocabulary(count, language, topic, promptAddition);
                if (words == null) words = new List<VocabularyWord>();

                // compute overlap
                int knownMatches = 0;
                foreach (var w in words)
                {
                    if (!string.IsNullOrWhiteSpace(w.Word) && knownSet.Contains(w.Word)) knownMatches++;
                }

                var overlap = words.Count == 0 ? 0.0 : (double)knownMatches / words.Count;
                Console.WriteLine($"Known matches: {knownMatches}/{words.Count} (overlap {overlap:P1})");

                finalWords = words;

                // remove known words from the candidate list
                if (finalWords != null && finalWords.Count > 0 && knownSet.Count > 0)
                {
                    finalWords = finalWords.Where(w => !string.IsNullOrWhiteSpace(w.Word) && !knownSet.Contains(w.Word)).ToList();
                    Console.WriteLine($"Filtered out known words — remaining count: {finalWords.Count}");
                }

                if (overlap > 0.5 && regenCount < maxRegens)
                {
                    regenCount++;
                    promptAddition = "Use less common or more advanced vocabulary; avoid basic/common words.";
                    Console.WriteLine($"Overlap >50% — regenerating with difficulty hint (attempt {regenCount})...");
                    // loop to regenerate
                    continue;
                }

                break;
            }

            GeneratedFlashcards = finalWords.Select(delegate(VocabularyWord w) { return new Flashcard(w); }).ToList();
            FlashcardsGenerated?.Invoke(GeneratedFlashcards);

            // save to JSON file
            try
            {
                var setsDir = GetSetsDirectory();
                Directory.CreateDirectory(setsDir);

                // sanitize file name components
                string Sanitize(string input)
                {
                    if (string.IsNullOrWhiteSpace(input)) return "general";
                    var s = Regex.Replace(input, "[^A-Za-z0-9_ -]", "_");
                    return s.Replace(' ', '_');
                }

                var fileName = $"{Sanitize(language)}_{Sanitize(topic ?? "general")}_{DateTime.UtcNow:yyyyMMddHHmmss}.json";
                var path = Path.Combine(setsDir, fileName);

                var wordsToSave = GeneratedFlashcards.Select(f => f.WordData).ToList();
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(wordsToSave, options);
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception saveEx)
            {
                Console.WriteLine($"Failed to save flashcard set: {saveEx.Message}");
            }

            Close();
        }
        catch (Exception ex)
        {
            var errorDialog = new Window
            {
                Content = new TextBlock { Text = $"Error: {ex.Message}" },
                Width = 400,
                Height = 200
            };
            await errorDialog.ShowDialog(this);
        }
        finally
        {
            GenerateBtn.IsEnabled = true;
            GenerateBtn.Content = "Generate Flashcards";
        }
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
