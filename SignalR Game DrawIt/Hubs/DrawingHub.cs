using Microsoft.AspNetCore.SignalR;
using SignalR_Game_DrawIt.Models;

namespace SignalR_Game_DrawIt.Hubs;

public class DrawingHub : Hub
{
    private static readonly Dictionary<string, Room> _rooms = new();

    private static readonly Dictionary<string, User> _users = new();
}