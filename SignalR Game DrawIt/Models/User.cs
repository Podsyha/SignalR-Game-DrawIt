namespace SignalR_Game_DrawIt.Models;

public sealed class User
{
    public User(string connectionId, string name)
    {
        ConnectionId = connectionId;
        Name = name;
    }
    
    public User(string connectionId, string name, Room room)
    {
        ConnectionId = connectionId;
        Name = name;
        Room = room;
    }
    
    public string Name { get; set; }
    public string ConnectionId { get; private set; }
    public Room Room { get; set; }
}