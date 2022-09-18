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
                HandleRegistrationRequest(deserializedMessage, client);
                break;

            case NetworkMessage.Type.LoginRequest:
                HandleLoginRequest(deserializedMessage, client);
                break;

            case NetworkMessage.Type.ChatMessage:
                HandleChatMessage(deserializedMessage);
                break;

            case NetworkMessage.Type.ChatConnectionRequest:
                HandleChatConnection(deserializedMessage, client);
                break;

            case NetworkMessage.Type.DisconnectionMessage:
                HandleDisconnectionMessage(deserializedMessage);
                break;

            case NetworkMessage.Type.ChatListRequest:
                HandleChatListRequest(deserializedMessage, client);
                break;

            case NetworkMessage.Type.ChatCreationRequest:
                HandleChatCreation(deserializedMessage, client);
                break;

            default: throw new Exception($"Unknown user message type {deserializedMessage.MessageType}");
        };
    }

    private static void HandleChatCreation(NetworkMessage message, ServerClient client)
    {
        var request = JsonSerializer.Deserialize<ChatCreationRequest>(message.JsonData);

        var senderData = DataManager.GetUserDataById(request.SenderID);
        PublicUserData senderPublicData = new()
        {
            ID = senderData.ID,
            Username = senderData.Username,
            ChatIDs = senderData.ChatIDs
        };
        ChatData chatData = ChatData.CreateWithAdmin(request.Title, senderPublicData);
        Chat chat = new(chatData, senderData.ID);

        Database.Chats.Add(chat);

        var response = MessageFactory.CreateChatConnectionResponse(chatData, NetworkMessage.Type.ChatCreationResponse);

        client.SendMessageToUser(response);
    }

    private static void HandleChatListRequest(NetworkMessage message, ServerClient client)
    {
        ChatListRequest listRequest = JsonSerializer.Deserialize<ChatListRequest>(message.JsonData);
        List<ChatData> chatsData = new();

        for(int i = 0; i < listRequest.IDs.Count; i++)
        {
            chatsData.Add(DataManager.GetChatById(listRequest.IDs[i]).PackData());
        }

        var response = MessageFactory.CreateChatListResponse(chatsData);

        client.SendMessageToUser(response);
    }

    private static void HandleDisconnectionMessage(NetworkMessage message)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message.JsonData);

        Database.Clients[userData.ID].Disconnect();
        Database.Clients.Remove(userData.ID);
    }

    private static void HandleChatConnection(NetworkMessage message, ServerClient client)
    {
        ChatConnectionRequest connectionRequest = JsonSerializer.Deserialize<ChatConnectionRequest>(message.JsonData);

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

    private static void HandleChatMessage(NetworkMessage message)
    {
        ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(message.JsonData);

        DataManager.GetChatById(chatMessage.ChatID)
                   .RecieveMessage(message.JsonData);
    }

    private static void HandleLoginRequest(NetworkMessage message, ServerClient client)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message.JsonData);

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

    private static void HandleRegistrationRequest(NetworkMessage message, ServerClient client)
    {
        PrivateUserData userData = JsonSerializer.Deserialize<PrivateUserData>(message.JsonData);

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