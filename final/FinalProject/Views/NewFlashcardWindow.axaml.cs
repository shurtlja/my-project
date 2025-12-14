using Avalonia.Controls;
using Avalonia.Interactivity;

#nullable enable

namespace FinalProject.Views;

public partial class NewFlashcardWindow : Window
{
        private AIService aiService;
        public List<Flashcard> GeneratedFlashcards { get; private set; }
        public event Action<List<Flashcard>>? FlashcardsGenerated;

        public NewFlashcardWindow()
        {
            InitializeComponent();
            aiService = new AIService();
            GeneratedFlashcards = new List<Flashcard>();
        }

        private async void OnGenerateClick(object sender, RoutedEventArgs e)
        {
            GenerateBtn.IsEnabled = false;
            GenerateBtn.Content = "Generating...";

            try
            {
                string language;
                if (LanguageCombo.SelectedItem is ComboBoxItem cbi && cbi.Content != null)
                {
                    language = cbi.Content.ToString() ?? "Spanish";
                }
                else
                {
                    language = LanguageCombo.SelectedItem?.ToString() ?? "Spanish";
                }
                var count = (int)(WordCountUpDown.Value ?? 20);

                var topic = TopicBox.Text?.Trim();
                Console.WriteLine($"Generating {count} words in {language} about topic: {topic}"); // debug for prompt
                var words = await aiService.GenerateVocabulary(count, language, topic);
                
                GeneratedFlashcards = words.Select(delegate(VocabularyWord w) { return new Flashcard(w); }).ToList();
                FlashcardsGenerated?.Invoke(GeneratedFlashcards);
                
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
