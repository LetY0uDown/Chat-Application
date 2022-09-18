using ChatLibrary.Message;
using ChatLibrary.User;
using System;
using System.Text.Json;

namespace Chat_Client.Core;

internal static class MessageFactory
{
    internal static string CreateChatListRequest()
    {
        ChatListRequest chatListRequest = new()
        {
            IDs = App.CurrentClient.Data.ChatIDs
        };

        NetworkMessage message = new(JsonSerializer.Serialize(chatListRequest), NetworkMessage.Type.ChatListRequest);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateChatMessage(PublicUserData userData, string text, Guid chatID)
    {
        ChatMessage chatMessage = new()
        {
            SenderData = userData,
            Text = text,
            ChatID = chatID,
            Date = DateTime.Now.ToString("g")
        };

        NetworkMessage message = new(JsonSerializer.Serialize(chatMessage), NetworkMessage.Type.ChatMessage);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateLoginRequest(PrivateUserData userData, bool createNewAcc)
    {
        NetworkMessage message = new(JsonSerializer.Serialize(userData),
                                     createNewAcc ? NetworkMessage.Type.RegistrationRequest
                                                  : NetworkMessage.Type.LoginRequest);
        
        return JsonSerializer.Serialize(message);
    }

    internal static string CreateChatConnectionRequest(Guid chatID)
    {
        ChatConnectionRequest connectionRequest = new()
        {
            SenderID = App.CurrentClient.Data.ID,
            ChatID = chatID
        };

        NetworkMessage message = new(JsonSerializer.Serialize(connectionRequest), NetworkMessage.Type.ChatConnectionRequest);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateChatDisconnectRequest(Guid chatID)
    {
        ChatConnectionRequest connectionRequest = new()
        {
            Disconnect = true,
            SenderID = App.CurrentClient.Data.ID,
            ChatID = chatID
        };

        NetworkMessage message = new(JsonSerializer.Serialize(connectionRequest), NetworkMessage.Type.ChatConnectionRequest);

        return JsonSerializer.Serialize(message);
    }

    internal static string CreateChatCreationRequest(string title)
    {
        ChatCreationRequest creationRequest = new()
        {
            Title = title,
            SenderID = App.CurrentClient.Data.ID
        };

        NetworkMessage message = new(JsonSerializer.Serialize(creationRequest), NetworkMessage.Type.ChatCreationRequest);

        return JsonSerializer.Serialize(message);
    }
}