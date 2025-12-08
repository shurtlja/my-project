using System;
using System.Collections.Generic;
using System.Linq;

namespace FinalProject;

public class AppViewModel
{
    private LanguageManager languageManager;
    private FlashcardSession session;
    private AIService aiService;

    public AppViewModel()
    {
        languageManager = new LanguageManager();
        aiService = new AIService();
        session = new FlashcardSession(languageManager);
    }

    public void ShowNewFlashcardWindow()
    {
        var newFlashcardWindow = new Views.NewFlashcardWindow();
        newFlashcardWindow.FlashcardsGenerated += (flashcards) =>
        {
            ShowPracticeWindow(flashcards);
        };
        newFlashcardWindow.Show();
    }

    public void ShowPracticeWindow(List<Flashcard> flashcards = null)
    {
        var practiceWindow = new Views.PracticeWindow();
        
        if (flashcards != null && flashcards.Count > 0)
        {
            practiceWindow.LoadDeck(flashcards);
        }
        else
        {
            practiceWindow.LoadDeck(new List<Flashcard>());
        }
        
        practiceWindow.Show();
    }

    public void ShowAIChatWindow()
    {
        var chatWindow = new Views.AIChatWindow();
        chatWindow.Show();
    }
}

