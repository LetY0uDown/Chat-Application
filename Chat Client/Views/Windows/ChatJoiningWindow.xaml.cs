using MVVM_Classes;
using Chat_Client.Core;
using System.Windows;
using System;

namespace Chat_Client.Views.Windows;

public partial class ChatJoiningWindow : Window
{
    public ChatJoiningWindow()
    {
        InitializeComponent();

        DataContext = this;

        JoinChatCommand = new(o =>
        {
            var chatID = Guid.Parse(ChatID);

            var request = MessageFactory.CreateChatConnectionRequest(chatID);

            App.CurrentClient.SendMessageToServer(request);

        }, b => !string.IsNullOrEmpty(ChatID));
    }

    public string ChatID { get; set; }

    public Command JoinChatCommand { get; private init; }
}