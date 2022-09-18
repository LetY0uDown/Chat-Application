namespace ChatLibrary.Message;

public class NetworkMessage
{
    public NetworkMessage(string jsonData, Type messageType)
    {
        JsonData = jsonData;
        MessageType = messageType;
    }

    public string JsonData { get; set; }

    public Type MessageType { get; set; }

    public enum Type
    {
        RegistrationRequest,
        LoginRequest,
        ChatMessage,
        ChatConnectionRequest,
        DisconnectionMessage,
        ChatListRequest,
        ChatCreationRequest,

        ChatCreationResponse,
        ChatListResponse,
        ChatConnectionResponse,
        ChatUpdateRequest,
        LoginResponse,
        Error
    }
}