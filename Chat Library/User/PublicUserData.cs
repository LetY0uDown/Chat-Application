namespace ChatLibrary.User;

public class PublicUserData
{
    public Guid ID { get; init; }

    public string Username { get; init; }

    public List<Guid> ChatIDs { get; init; }
}