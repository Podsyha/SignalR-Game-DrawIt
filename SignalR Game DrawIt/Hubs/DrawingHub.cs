using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using SignalR_Game_DrawIt.Models;

namespace SignalR_Game_DrawIt.Hubs;

public class DrawingHub : Hub
{
    private static readonly Dictionary<string, Room> _rooms = new();
    private static readonly Dictionary<string, User> _users = new();

    public string CreateRoom()
    {
        string currentConnectionId = Context.ConnectionId;
        if (!_users.TryGetValue(currentConnectionId, out User user))
            throw new UnauthorizedAccessException("Login to create a room");

        if (string.IsNullOrEmpty(user.Name))
            throw new UnauthorizedAccessException("Enter a nickname to create a room");

        Room room = new Room(Guid.NewGuid());
        room.AddUser(user);
        _rooms.Add(room.RoomId, room);

        return room.RoomId;
    }

    public User CreateUserConnection(string userName)
    {
        string currentConnectionId = Context.ConnectionId;
        if (_users.TryGetValue(currentConnectionId, out User user))
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                user.Name = userName;
            }

            return user;
        }

        user = new User(currentConnectionId, userName);
        _users.Add(currentConnectionId, user);

        return user;
    }

    public void ConnectToRoom(string roomId, string userName)
    {
        if (!_rooms.TryGetValue(roomId, out Room room))
            throw new NavigationException("Non-existent room");

        User user = CreateUserConnection(userName);
        if (room.CheckingAppend(user.ConnectionId))
            room.AddUser(user);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        if (!_users.TryGetValue(Context.ConnectionId, out User user))
            return;

        await OnUserLeaveAsync(user);
        await base.OnDisconnectedAsync(exception);
    }

    private async Task OnUserLeaveAsync(User leavedUser)
    {
        _users.Remove(leavedUser.ConnectionId);

        Room room = leavedUser.Room;
        if (room is null) return;

        room.RemoveUser(leavedUser.ConnectionId);

        await Clients.Clients(room.GetAllConnectionsId())
            .SendAsync("onUserLeave", leavedUser.ConnectionId);
    }
}