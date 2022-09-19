using Chat_Server.Data;
using Chat_Server.Messages;
using ChatLibrary;
using ChatLibrary.Message;
using ChatLibrary.User;
using System.Text.Json;

namespace Chat_Server;

internal sealed class Chat
{
    public Chat(ChatData chatData, Guid adminID)
    {
        Title = chatData.Title;
        ID = chatData.ID;

        _members = new() { Database.Clients[adminID] };
        _messages = new(chatData.Messages);
        _membersData = new(chatData.Users);
    }

    private readonly List<ChatMessage> _messages = new();
    private readonly List<ServerClient> _members = new();
    private readonly List<PublicUserData> _membersData = new();

    public string Title { get; set; }

    public Guid ID { get; init; }

    private void SendUpdateRequest(string messageJson)
    {
        var message = MessageFactory.CreateChatMessage(messageJson);

        foreach (var client in _members)
            client.SendMessageToUser(message);
    }
    // TODO: Change update method probably
    internal void RecieveMessage(string messageJson)
    {
        var message = JsonSerializer.Deserialize<ChatMessage>(messageJson);

        _messages.Add(message);
        SendUpdateRequest(messageJson);
    }

    internal void ConnectClient(Guid id)
    {
        _members.Add(Database.Clients[id]);
        var userData = DataManager.GetUserDataById(id);
        _membersData.Add(DataManager.CreatePublicDataFromPrivate(userData));
    }

    internal void DisconnectClient(Guid id)
    {
        _members.Remove(Database.Clients[id]);
        var userData = DataManager.GetUserDataById(id);
        _membersData.Remove(DataManager.CreatePublicDataFromPrivate(userData));
    }

    internal ChatData PackData()
    {
        return new ChatData(ID, Title, new(_messages), new(_membersData));
    }
}
