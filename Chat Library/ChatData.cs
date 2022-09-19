using ChatLibrary.Message;
using ChatLibrary.User;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ChatLibrary;

public class ChatData
{
    public ChatData(string title, PublicUserData admin)
    {
        Title = title;

        ID = Guid.NewGuid();

        Messages = new();
        Users = new() { admin };
    }

    [JsonConstructor]
    public ChatData(Guid iD, string title, ObservableCollection<ChatMessage> messages, ObservableCollection<PublicUserData> users)
    {
        ID = iD;
        Title = title;
        Messages = messages;
        Users = users;
    }

    public Guid ID { get; private init; }    

    public string Title { get; private init; }

    public ObservableCollection<ChatMessage> Messages { get; private init; }

    public ObservableCollection<PublicUserData> Users { get; private init; }
}