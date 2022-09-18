using Chat_Client.Core;
using Chat_Client.ViewModels;
using ChatLibrary;
using System.Windows.Controls;

namespace Chat_Client.Views.Pages;

public partial class ChatPage : Page
{
    internal ChatPage(Chat chat)
    {
        InitializeComponent();

        DataContext = new ChatViewModel(chat);
    }
}