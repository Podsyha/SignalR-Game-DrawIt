namespace SignalR_Game_DrawIt.Models;

public sealed class User
{
    public string Name { get; set; }
    public string ConnectionId { get; set; }
    public Room Room { get; set; }
}