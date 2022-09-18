using Chat_Client.Core;
using System.Windows;

namespace Chat_Client.Views.Windows;

public partial class ChatCreationWindow : Window
{
    internal ChatCreationWindow()
    {
        InitializeComponent();
    }

    private void CreateChatButtonClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(chatTitleTB.Text)) return;

        var request = MessageFactory.CreateChatCreationRequest(chatTitleTB.Text);
        App.CurrentClient.SendMessageToServer(request);

        Hide();
    }
}