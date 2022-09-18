using ChatLibrary.User;

namespace Chat_Server.Data;

internal static class DataManager
{
    internal static bool ValidateUser(string username, string password, out PrivateUserData userData)
    {
        userData = Database.UsersData.Find(u => u.Username == username && u.Password == password);

        return userData is not null;
    }

    internal static bool CheckNameExist(string username)
    {
        return Database.UsersData.Any(u => u.Username == username);
    }

    internal static Chat GetChatById(Guid id)
    {
        return Database.Chats.Find(c => c.ID == id);
    }

    internal static PrivateUserData GetUserDataById(Guid id)
    {
        return Database.UsersData.Find(u => u.ID == id);
    }

    internal static PublicUserData CreatePublicDataFromPrivate(PrivateUserData privateData)
    {
        return new()
        {
            ID = privateData.ID,
            Username = privateData.Username,
            ChatIDs = privateData.ChatIDs
        };
    }
}