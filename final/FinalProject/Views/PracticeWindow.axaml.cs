using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
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
            ProgressBar.Value = ((currentIndex + 1) * 100) / deck.Count;
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
            var card = deck[currentIndex];
            card.IncrementCorrect();

            if (currentIndex < deck.Count - 1)
            {
                currentIndex++;
                DisplayCard();
            }
            else
            {
                WordText.Text = "Practice Complete!";
                CorrectBtn.IsEnabled = false;
                IncorrectBtn.IsEnabled = false;
                HintBtn.IsEnabled = false;
            }
        }

        private void OnIncorrectClick(object sender, RoutedEventArgs e)
        {
            var card = deck[currentIndex];
            card.ResetCorrect();

            if (currentIndex < deck.Count - 1)
            {
                currentIndex++;
                DisplayCard();
            }
            else
            {
                WordText.Text = "Practice Complete!";
                CorrectBtn.IsEnabled = false;
                IncorrectBtn.IsEnabled = false;
                HintBtn.IsEnabled = false;
            }
        }

    private void OnBackClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
