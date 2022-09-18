using MVVM_Classes;
using Chat_Client.Views.Windows;
using System.Windows.Controls;
using Chat_Client.Core;
using System.Collections.ObjectModel;

namespace Chat_Client.ViewModels;

internal sealed class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        CreateChatCommand = new(o => {
            new ChatCreationWindow().ShowDialog();
        });

        JoinChatCommand = new(o => {
            new ChatJoiningWindow().ShowDialog();
        });
    }

    public Chat SelectedChat {
        set => App.SetCurrentChat(value);
    }

    public ObservableCollection<Chat> Chats => App.Chats;

    public Page CurrentChatPage { get; set; }

    public Command CreateChatCommand { get; private init; }

    public Command JoinChatCommand { get; private init; }
}