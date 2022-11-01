using System.Reflection.Metadata.Ecma335;

namespace SignalR_Game_DrawIt.Models;

public sealed class Room
{
    public Room(Guid roomId)
    {
        RoomId = roomId.ToString();
    }

    public string RoomId { get; set; }

    private static readonly Dictionary<string, User> _users = new();
    private static readonly List<Coord> _coords = new(10000);

    public void AddCoords(ICollection<Coord> coords) =>
        _coords.AddRange(coords);

    public ICollection<Coord> GetAllCoords() =>
        _coords;

    public void ClearCoords() =>
        _coords.Clear();

    public void RemoveUser(string connectionId)
    {
        if (!_users.TryGetValue(connectionId, out _))
            return;
        
        _users.Remove(connectionId);
    }

    public void AddUser(User user)
    {
        _users.TryAdd(user.ConnectionId, user);
    }

    public bool CheckingAppend(string connectionId) =>
        !_users.ContainsKey(connectionId);

    public ICollection<string> GetAllConnectionsId() =>
        _users.Select(x => x.Key).ToArray();

    public bool CheckingAnyUsersInRoom() =>
        _users.Any();
}