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
                var language = LanguageCombo.SelectedItem?.ToString() ?? "Spanish";
                var count = (int)(WordCountUpDown.Value ?? 20);

                var words = await aiService.GenerateVocabulary(count, language);
                
                GeneratedFlashcards = words.Select(w => new Flashcard(w)).ToList();
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
