using ChatLibrary.Message;
using ChatLibrary.User;
using System.Text.Json;
using Chat_Server.Data;
using ChatLibrary;

namespace Chat_Server.Messages;

internal static class MessageHandlerServer
{
    internal static void HandleMessage(string messageJson, ServerClient client)
    {
        var deserializedMessage = JsonSerializer.Deserialize<NetworkMessage>(messageJson);

        switch (deserializedMessage.MessageType)
        {
            case NetworkMessage.Type.RegistrationRequest:
                HandleRegistrationRequest(deserializedMessage.JsonData, client);
                break;

            case NetworkMessage.Type.LoginRequest:
                HandleLoginRequest(deserializedMessage.JsonData, client);
                break;

            case NetworkMessage.Type.ChatMessage:
                HandleChatMessage(deserializedMessage.JsonData);
                break;

            case NetworkMessage.Type.ChatConnectionRequest:
                HandleChatConnection(deserializedMessage.JsonData, client);
                break;

            case NetworkMessage.Type.DisconnectionMessage:
                HandleDisconnectionMessage(deserializedMessage.JsonData);
                break;

            case NetworkMessage.Type.ChatListRequest:
                HandleChatListRequest(deserializedMessage.JsonData, client);
                break;

            case NetworkMessage.Type.ChatCreationRequest:
                HandleChatCreation(deserializedMessage.JsonData, client);
                break;

            default: throw new Exception($"Unknown user message type {deserializedMessage.MessageType}");
        };
    }

    private static void HandleChatCreation(string message, ServerClient client)
    {
        var request = JsonSerializer.Deserialize<ChatCreationRequest>(message);

        var senderData = DataManager.GetUserDataById(request.SenderID);
        PublicUserData senderPublicData = new()
        {
            ID = senderData.ID,
            Username = senderData.Username,
            ChatIDs = senderData.ChatIDs
        };
        ChatData chatData = new(request.Title, senderPublicData);
        Chat chat = new(chatData, senderData.ID);

        Database.Chats.Add(chat);

        var response = MessageFactory.CreateChatConnectionResponse(chatData, NetworkMessage.Type.ChatCreationResponse);

        client.SendMessageToUser(response);
    }

    private static void HandleChatListRequest(string message, ServerClient client)
    {
        ChatListRequest listRequest = JsonSerializer.Deserialize<ChatListRequest>(message);
        List<ChatData> chatsData = new();

        for(int i = 0; i < listRequest.IDs.Count; i++)
        {
            chatsData.Add(DataManager.GetChatById(listRequest.IDs[i]).PackData());
        }

        var response = MessageFactory.CreateChatListResponse(chatsData);

        client.SendMessageToUser(response);
    }

    private static void HandleDisconnectionMessage(string message)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message);

        Database.Clients[userData.ID].Disconnect();
        Database.Clients.Remove(userData.ID);
    }

    private static void HandleChatConnection(string message, ServerClient client)
    {
        ChatConnectionRequest connectionRequest = JsonSerializer.Deserialize<ChatConnectionRequest>(message);

        if (connectionRequest.Disconnect)
        {
            DataManager.GetChatById(connectionRequest.ChatID)
                       .DisconnectClient(connectionRequest.SenderID);
        }
        else
        {
            var chat = DataManager.GetChatById(connectionRequest.ChatID);

            chat.ConnectClient(connectionRequest.SenderID);

            var chatData = chat.PackData();

            var response = MessageFactory.CreateChatConnectionResponse(chatData, NetworkMessage.Type.ChatConnectionResponse);

            client.SendMessageToUser(response);
        }
    }

    private static void HandleChatMessage(string message)
    {
        ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(message);

        DataManager.GetChatById(chatMessage.ChatID)
                   .RecieveMessage(message);
    }

    private static void HandleLoginRequest(string message, ServerClient client)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message);

        if (DataManager.ValidateUser(userData.Username, userData.Password, out userData))
        {
            var dataFromDB = DataManager.GetUserDataById(userData.ID);

            var response = MessageFactory.CreateLoginResponse(dataFromDB);

            Database.Clients[dataFromDB.ID] = client;
            client.SendMessageToUser(response);

            return;
        }

        var errorMessage = MessageFactory.CreateErrorMessage("Ошибка входа", "Неверный логин и/или пароль");

        client.SendMessageToUser(errorMessage);
    }

    private static void HandleRegistrationRequest(string message, ServerClient client)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message);

        if (DataManager.CheckNameExist(userData.Username))
        {
            var errorMessage = MessageFactory.CreateErrorMessage("Ошибка регистрации", "Данный ник уже занят!");

            client.SendMessageToUser(errorMessage);

            return;
        }

        Database.UsersData.Add(userData);
        Database.Clients[userData.ID] = client;

        var response = MessageFactory.CreateLoginResponse(userData);
        client.SendMessageToUser(response);
    }
}