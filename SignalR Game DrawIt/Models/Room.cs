namespace SignalR_Game_DrawIt.Models;

public sealed class Room
{
    public Room(Guid roomId)
    {
        RoomId = roomId.ToString();
    }

    public string RoomId { get; set; }
    
    private static readonly Dictionary<string, User> _users = new();
    private static readonly ICollection<string> _coords = new List<string>();

    public void AddCoord(string coord) => _coords.Add(coord);
    public void ClearCoords() => _coords.Clear();
    public void RemoveUser(string connectionId) => _users.Remove(connectionId);
    public void AddUser(User user) => _users.Add(user.ConnectionId, user);
    public bool CheckingAppend(string connectionId) => !_users.ContainsKey(connectionId);
    public ICollection<string> GetAllConnectionsId() => _users.Select(x => x.Key).ToArray();
}