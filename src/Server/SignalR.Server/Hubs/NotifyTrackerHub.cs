using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR.Server.Services;

namespace SignalR.Server.Hubs;

public class NotifyTrackerHub(ILogger<NotifyTrackerHub> logger) : Hub
{
    /// <summary>
    /// Join Group Without Authorization
    /// </summary>
    /// <param name="groupName"></param>
    public async Task JoinGroup(string groupName)
    {
        logger.LogInformation("JoinGroup method called.");

        groupName = string.Format($"{groupName}");

        logger.LogInformation($"JoinGroup method called. GroupName: {groupName}");

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        logger.LogInformation($"JoinGroup method called. GroupName: {groupName} ConnectionId: {Context.ConnectionId}");
    }

    /// <summary>
    /// Join Authorized Group
    /// </summary>
    /// <param name="groupName"></param>
    [Authorize]
    public async Task JoinAuthorizedGroup(string groupName)
    {
        logger.LogInformation("JoinAuthorizedGroup method called.");

        groupName = string.Format($"{groupName}");

        logger.LogInformation($"JoinAuthorizedGroup method called. GroupName: {groupName}");

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        logger.LogInformation($"JoinAuthorizedGroup method called. GroupName: {groupName} ConnectionId: {Context.ConnectionId}");
    }

    /// <summary>
    /// Notify Event to Clients With Group
    /// </summary>
    /// <param name="message"></param>
    /// <param name="groupName"></param>
    public async Task NotifyEventWithGroup(JsonObject message, string groupName)
    {
        await Clients.Group(groupName).SendAsync(nameof(NotifyEvent), message);
    }

    /// <summary>
    /// Notify Event to Clients Without Group
    /// </summary>
    /// <param name="message"></param>
    public async Task NotifyEvent(JsonObject message)
    {
        await Clients.All.SendAsync(nameof(NotifyEvent), message);
    }

    public enum HubMethodType
    {
        NotifyEvent
    }
}