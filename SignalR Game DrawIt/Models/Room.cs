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
            throw new ArgumentException();
        
        _users.Remove(connectionId);
    }

    public void TryAddUser(User user)
    {
        if (!_users.TryAdd(user.ConnectionId, user))
            throw new ArgumentException("User is already in the room");
    }

    public bool CheckingUserAppend(string connectionId) =>
        !_users.ContainsKey(connectionId);

    public ICollection<string> GetAllConnectionsId() =>
        _users.Select(x => x.Key).ToArray();

    public bool CheckingAnyUsersInRoom() =>
        _users.Any();
}