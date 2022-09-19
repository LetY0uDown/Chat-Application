using System.Text.Json.Serialization;

namespace ChatLibrary.User;

public class PrivateUserData
{
    [JsonConstructor]
    public PrivateUserData(Guid id, string username, string password, List<Guid> chatIDs)
    {
        ID = id;
        Username = username;
        Password = password;

        ChatIDs = chatIDs;
    }

    public PrivateUserData(Guid id, string username, string password)
    {
        ID = id;
        Username = username;
        Password = password;

        ChatIDs = new();
    }

    public Guid ID { get; private init; }

    public string Username { get; private init; }

    public string Password { get; private init; }

    public List<Guid> ChatIDs { get; private init; }
}