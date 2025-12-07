using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Text;

namespace FinalProject.Views
{
    public partial class AIChatWindow : Window
    {
        private AIService aiService;
        private StringBuilder chatHistory;
        private bool chatStarted = false;

        public AIChatWindow()
        {
            InitializeComponent();
            aiService = new AIService();
            chatHistory = new StringBuilder();
            // chat starts once user provides language/topic and clicks Start
            SendBtn.IsEnabled = false;
        }

        private async void OnStartClick(object sender, RoutedEventArgs e)
        {
            if (chatStarted) return;

            var language = (LanguageCombo.SelectedItem as Avalonia.Controls.ComboBoxItem)?.Content?.ToString() ?? "English";
            var topic = TopicBox.Text ?? "a close friend";

            var instruction = $"The following is a langauge practice dialogue between an AI and You. The AI is roleplaying as {topic} in {language}. Each response is concise and natural.\nAI:";

            StartBtn.IsEnabled = false;
            StatusText.Text = "Initializing chat...";

            try
            {
                var initResponse = await aiService.GenerateChatResponse(instruction);
                chatHistory.AppendLine($"AI: {initResponse}");
                chatHistory.AppendLine();
                ChatDisplay.Text = chatHistory.ToString();

                chatStarted = true;
                SendBtn.IsEnabled = true;
                StatusText.Text = "Ready";
            }
            catch (Exception ex)
            {
                chatHistory.AppendLine($"Error: {ex.Message}");
                ChatDisplay.Text = chatHistory.ToString();
                StartBtn.IsEnabled = true;
                StatusText.Text = "Error";
            }
        }

        private async void OnSendClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputBox.Text))
                return;

            var userMessage = InputBox.Text;
            InputBox.Text = "";
            SendBtn.IsEnabled = false;
            StatusText.Text = "AI is thinking...";

            // Add user message to chat
            chatHistory.AppendLine($"You: {userMessage}");
            chatHistory.AppendLine();

            try
            {
                // Get AI response
                var response = await aiService.GenerateChatResponse($"You: {userMessage}\nAI:");
                chatHistory.AppendLine($"AI: {response}");
                chatHistory.AppendLine();

                ChatDisplay.Text = chatHistory.ToString();
            }
            catch (Exception ex)
            {
                chatHistory.AppendLine($"Error: {ex.Message}");
                ChatDisplay.Text = chatHistory.ToString();
            }

            SendBtn.IsEnabled = true;
            StatusText.Text = "Ready";
        }

        private void OnBackClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
