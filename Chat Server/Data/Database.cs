using ChatLibrary.User;

namespace Chat_Server.Data;

internal static class Database
{
    internal static Dictionary<Guid, ServerClient> Clients { get; } = new();

    internal static List<PrivateUserData> UsersData { get; } = new();

    internal static List<Chat> Chats { get; } = new();
}