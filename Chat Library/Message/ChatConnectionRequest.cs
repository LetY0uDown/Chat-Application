namespace ChatLibrary.Message;

public class ChatConnectionRequest
{
    public Guid ChatID { get; init; }

    public Guid SenderID { get; init; }

    public bool Disconnect { get; init; }
}