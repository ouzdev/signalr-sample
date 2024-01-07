using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Services;

namespace SignalR.Server.Hubs;

public class NotifyTrackerHub(ISignalRService signalRService):Hub
{
    private readonly ISignalRService _signalRService = signalRService;
    
    public async Task NotifyEvent(JsonObject message)
    {
        await Clients.All.SendAsync(nameof(NotifyEvent), message);
    }
    public enum HubMethodType
    {
        NotifyEvent
    }

}