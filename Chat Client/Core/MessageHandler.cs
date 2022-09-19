using Chat_Client.Views.Windows;
using ChatLibrary;
using ChatLibrary.Message;
using ChatLibrary.User;
using System;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace Chat_Client.Core;

internal static class MessageHandler
{
    internal static void HandleMessage(string message)
    {
        var deserializedMessage = JsonSerializer.Deserialize<NetworkMessage>(message);

        Application.Current.Dispatcher.Invoke(() =>
        {
            switch (deserializedMessage.MessageType)
            {
                case NetworkMessage.Type.ChatConnectionResponse:
                    HandleChatConnection(deserializedMessage.JsonData);
                    break;

                case NetworkMessage.Type.ChatCreationResponse:
                    HandleChatCreation(deserializedMessage.JsonData);
                    break;

                case NetworkMessage.Type.LoginResponse:
                    HandleLoginResponse(deserializedMessage.JsonData);
                    break;

                case NetworkMessage.Type.ChatListResponse:
                    HandleChatList(deserializedMessage.JsonData);
                    break;

                case NetworkMessage.Type.ChatMessage:
                    HandleChatUpdate(deserializedMessage.JsonData);
                    break;

                case NetworkMessage.Type.Error:
                    HandleError(deserializedMessage.JsonData);
                    break;
            }
        });
    }

    // Can only handle messages for now
    private static void HandleChatUpdate(string jsonData)
    {
        var chatMessage = JsonSerializer.Deserialize<ChatMessage>(jsonData);

        var chatToUpd = App.Chats.FirstOrDefault(c => c.ID == chatMessage.ChatID);
        chatToUpd.Messages.Add(chatMessage);
    }

    private static void HandleChatList(string jsonData)
    {
        var response = JsonSerializer.Deserialize<ChatListResponse>(jsonData);
        App.Chats.Clear();

        foreach (var chatData in response.Data)
        {
            App.Chats.Add(new(chatData));
        }
    }

    private static void HandleChatConnection(string jsonData)
    {
        var chatData = JsonSerializer.Deserialize<ChatData>(jsonData);

        App.SetCurrentChat(new(chatData));
    }

    private static void HandleError(string message)
    {
        var errorMessage = JsonSerializer.Deserialize<ErrorMessage>(message);

        MessageBox.Show(errorMessage.Message, errorMessage.Source);
    }

    private static void HandleLoginResponse(string jsonData)
    {
        var userData = JsonSerializer.Deserialize<PublicUserData>(jsonData);

        App.CurrentClient.Data = userData;

        App.ChangeMainWindow(new MainWindow());

        if (App.Chats.Count > 0)
        {
            var chatsRequest = MessageFactory.CreateChatListRequest();

            App.CurrentClient.SendMessageToServer(chatsRequest);
        }
    }

    private static void HandleChatCreation(string jsonData)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        ChatData response = JsonSerializer.Deserialize<ChatData>(jsonData, options);

        MessageWindow.Show("Чат создан!", $"Чат {response.Title} был создан.\nID для подключения: {response.ID}");

        App.SetCurrentChat(new Chat(response));
    }
}