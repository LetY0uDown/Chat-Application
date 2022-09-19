using MVVM_Classes;
using Chat_Client.Views.Windows;
using System.Windows.Controls;
using Chat_Client.Core;
using System.Collections.ObjectModel;
using Chat_Client.Views.Pages;

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

    private Chat _selectedChat;
    public Chat SelectedChat
    {
        get => _selectedChat;
        set
        {
            if (_selectedChat == value) return;

            _selectedChat = value;
            App.SetCurrentChat(value);            
        }
    }

    public ObservableCollection<Chat> Chats => App.Chats;

    public Command CreateChatCommand { get; private init; }

    public Command JoinChatCommand { get; private init; }
}