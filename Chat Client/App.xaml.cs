using Chat_Client.Core;
using Chat_Client.ViewModels;
using Chat_Client.Views.Pages;
using Chat_Client.Views.Windows;
using System.Collections.ObjectModel;
using System.Windows;

namespace Chat_Client;

public partial class App : Application
{
    internal static LocalClient CurrentClient { get; private set; }

    internal static Chat CurrentChat { get; private set; }

    internal static ObservableCollection<Chat> Chats { get; } = new();

    internal static void SetCurrentChat(Chat newChat)
    {
        if (CurrentChat == newChat) return;

        if (Current.MainWindow is MainWindow mainWindow)
        {
            CurrentChat = newChat;

            var viewModel = mainWindow.DataContext as MainWindowViewModel;

            mainWindow.ChatPageFrame.Content = new ChatPage()
            {
                DataContext = new ChatViewModel(CurrentChat)
            };

            if (viewModel.Chats.Contains(newChat)) return;

            viewModel.Chats.Add(newChat);
        }       
    }

    internal static void ChangeMainWindow(Window newWindow)
    {
        Current.MainWindow.Hide();

        Current.MainWindow = newWindow;

        Current.MainWindow.Show();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LocalClient client = new();

        client.ConnectToServer("127.0.0.1", 30000);

        CurrentClient = client;
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {

    }
}