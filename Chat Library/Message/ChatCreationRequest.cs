namespace ChatLibrary.Message;

public class ChatCreationRequest
{
    public string Title { get; init; }

    public Guid SenderID { get; init; }
}