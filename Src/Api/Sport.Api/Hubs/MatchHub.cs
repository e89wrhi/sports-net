using Microsoft.AspNetCore.SignalR;

namespace Sport.Api.Hubs;

public class MatchHub : Hub
{
    public async Task JoinMatchGroup(string matchId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
    }

    public async Task LeaveMatchGroup(string matchId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
    }
}
