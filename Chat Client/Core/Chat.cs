using ChatLibrary;
using ChatLibrary.Message;
using ChatLibrary.User;
using System;
using System.Collections.ObjectModel;

namespace Chat_Client.Core;

internal sealed class Chat
{
    public Chat(ChatData data)
    {
        Title = data.Title;
        Members = data.Users;
        Messages = data.Messages;
        ID = data.ID;
    }

    public Guid ID { get; init; }

    public string Title { get; private set; }

    public ObservableCollection<PublicUserData> Members { get; private set; }
    public ObservableCollection<ChatMessage> Messages { get; private set; }

    internal void UpdateMessages(ObservableCollection<ChatMessage> messages) => Messages = messages;

    internal void UpdateMembers(ObservableCollection<PublicUserData> members) => Members = members;

    internal void UpdateTitle(string title) => Title = title;
}