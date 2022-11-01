using Microsoft.AspNetCore.SignalR;
using SignalR_Game_DrawIt.Models;
using Throw;

namespace SignalR_Game_DrawIt.Hubs;

public class DrawingHub : Hub
{
    private static readonly Dictionary<string, Room> _rooms = new();
    private static readonly Dictionary<string, User> _users = new();

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (!_users.TryGetValue(Context.ConnectionId, out User user))
            return;

        await OnUserLeaveAsync(user);
        await base.OnDisconnectedAsync(exception);
    }

    public User CreateUserConnection(string userName)
    {
        userName.Throw().IfNullOrEmpty(x => x.Trim());
        string currentConnectionId = Context.ConnectionId;

        if (_users.TryGetValue(currentConnectionId, out User user))
            return user;

        user = new User(currentConnectionId, userName);
        _users.Add(currentConnectionId, user);

        return user;
    }

    public string CreateRoom()
    {
        string currentConnectionId = Context.ConnectionId;
        if (!_users.TryGetValue(currentConnectionId, out User user))
            throw new UnauthorizedAccessException("Login to create a room");
        
        Room room = new Room(Guid.NewGuid());
        room.TryAddUser(user);
        _rooms.TryAdd(room.RoomId, room);

        return room.RoomId;
    }

    public ICollection<Coord> ConnectToRoom(string roomId)
    {
        if (!_rooms.TryGetValue(roomId, out Room room))
            throw new ArgumentException("Non-existent room");

        string currentConnectionId = Context.ConnectionId;
        if (!_users.TryGetValue(currentConnectionId, out User user))
            throw new UnauthorizedAccessException("Login to create a room");

        if (room.CheckingUserAppend(user.ConnectionId))
            room.TryAddUser(user);

        return room.GetAllCoords();
    }

    public void AddCoordsToRoom(string roomId, ICollection<Coord> coords)
    {
        if (!_rooms.TryGetValue(roomId, out Room room))
            throw new ArgumentException("Non-existent room");

        room.AddCoords(coords);
    }

    private async Task OnUserLeaveAsync(User leavedUser)
    {
        _users.Remove(leavedUser.ConnectionId);

        Room room = leavedUser.Room;
        if (room == null) return;

        room.RemoveUser(leavedUser.ConnectionId);

        if (room.CheckingAnyUsersInRoom())
            await Clients.Clients(room.GetAllConnectionsId()).SendAsync("onUserLeave", leavedUser.ConnectionId);
        else
            _rooms.Remove(room.RoomId);
    }
}