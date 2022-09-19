using Chat_Server.Data;
using ChatLibrary;
using ChatLibrary.Message;
using ChatLibrary.User;
using System.Text.Json;

namespace Chat_Server.Messages;

internal static class MessageFactory
{
    internal static string CreateChatMessage(string json)
    {
        NetworkMessage message = new(json, NetworkMessage.Type.ChatMessage);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateErrorMessage(string source, string message)
    {
        ErrorMessage errorMessage = new()
        {
            Source = source,
            Message = message
        };
        
        NetworkMessage networkMessage = new(JsonSerializer.Serialize(errorMessage), NetworkMessage.Type.Error);

        return JsonSerializer.Serialize(networkMessage);
    }

    internal static string CreateChatConnectionResponse(ChatData chatData, NetworkMessage.Type type)
    {
        NetworkMessage message = new(JsonSerializer.Serialize(chatData), type);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateLoginResponse(PrivateUserData userData)
    {
        PublicUserData publicData = DataManager.CreatePublicDataFromPrivate(userData);

        NetworkMessage message = new(JsonSerializer.Serialize(publicData), NetworkMessage.Type.LoginResponse);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateChatListResponse(List<ChatData> data)
    {
        ChatListResponse chatListResponse = new()
        {
            Data = data
        };

        NetworkMessage message = new(JsonSerializer.Serialize(chatListResponse), NetworkMessage.Type.ChatListResponse);

        return JsonSerializer.Serialize(message);
    }
}