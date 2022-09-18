using ChatLibrary.Message;
using ChatLibrary.User;
using System.Collections.ObjectModel;

namespace ChatLibrary;

public class ChatData
{
    public static ChatData CreateWithAdmin(string title, PublicUserData admin)
    {
        return new()
        {
            Title = title,

            ID = Guid.NewGuid(),

            Messages = new(),
            Users = new() { admin }
        };
    }

    public static ChatData CreateNew(Guid id, string title, List<ChatMessage> messages, List<PublicUserData> users)
    {
        return new()
        {
            ID = id,
            Title = title,
            Messages = new(messages),
            Users = new(users)
        };
    }

    public Guid ID { get; private init; }

    public string Title { get; private init; }

    public ObservableCollection<ChatMessage> Messages { get; private init; }

    public ObservableCollection<PublicUserData> Users { get; private init; }
}