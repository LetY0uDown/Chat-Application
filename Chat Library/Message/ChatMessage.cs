using ChatLibrary.User;

namespace ChatLibrary.Message;

public class ChatMessage
{
    public PublicUserData SenderData { get; init; }

    public string Date { get; init; }

    public Guid ChatID { get; init; }

    public string Text { get; init; }
}