using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FinalProject.Views;

public partial class PracticeWindow : Window
{
        private List<Flashcard> deck;
        private int currentIndex;
        private bool meaningShown;

        public PracticeWindow()
        {
            InitializeComponent();
            deck = new List<Flashcard>();
            currentIndex = 0;
            meaningShown = false;
        }

        public void LoadDeck(List<Flashcard> flashcards)
        {
            deck = flashcards;
            currentIndex = 0;
            meaningShown = false;
            DisplayCard();
        }

        private void DisplayCard()
        {
            if (deck.Count == 0)
            {
                WordText.Text = "No cards to practice!";
                return;
            }

            var card = deck[currentIndex];
            WordText.Text = card.GetWord();
            MeaningText.IsVisible = false;
            HintText.IsVisible = false;
            meaningShown = false;
            HintBtn.Content = "Show Hint";

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            ProgressText.Text = $"Card {currentIndex + 1} of {deck.Count}";
        }

        private void OnHintClick(object sender, RoutedEventArgs e)
        {
            if (!meaningShown)
            {
                var card = deck[currentIndex];
                MeaningText.Text = card.GetMeaning();
                MeaningText.IsVisible = true;
                meaningShown = true;
                HintBtn.Content = "Hide Hint";
            }
            else
            {
                MeaningText.IsVisible = false;
                meaningShown = false;
                HintBtn.Content = "Show Hint";
            }
        }

        private void OnCorrectClick(object sender, RoutedEventArgs e)
        {
            if (deck.Count == 0) return;

            var card = deck[currentIndex];
            card.IncrementCorrect();

            // if the card is mastered (correct twice in a row), record it
            if (card.IsMastered())
            {
                try
                {
                    var exeDir = AppContext.BaseDirectory ?? Environment.CurrentDirectory;
                    var dataDir = Path.Combine(exeDir, "data");
                    Directory.CreateDirectory(dataDir);
                    var knownPath = Path.Combine(dataDir, "knownWords.txt");

                    var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    if (File.Exists(knownPath))
                    {
                        foreach (var line in File.ReadAllLines(knownPath))
                        {
                            if (!string.IsNullOrWhiteSpace(line)) existing.Add(line.Trim());
                        }
                    }

                    var word = card.GetWord();
                    if (!existing.Contains(word))
                    {
                        File.AppendAllLines(knownPath, new[] { word });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write known word: {ex.Message}");
                }
            }

            // Move the card to the back of the deck
            deck.RemoveAt(currentIndex);
            deck.Add(card);

            // wrap index if needed
            if (currentIndex >= deck.Count) currentIndex = 0;
            DisplayCard();
        }

        private void OnIncorrectClick(object sender, RoutedEventArgs e)
        {
            if (deck.Count == 0) return;

            var card = deck[currentIndex];
            card.ResetCorrect();

            // Send the card back 3 places
            deck.RemoveAt(currentIndex);
            var insertIndex = Math.Min(currentIndex + 3, deck.Count);
            deck.Insert(insertIndex, card);

            // do not advance currentIndex so the next card is the one that replaced this index
            if (currentIndex >= deck.Count) currentIndex = 0;
            DisplayCard();
        }

    private void OnBackClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
