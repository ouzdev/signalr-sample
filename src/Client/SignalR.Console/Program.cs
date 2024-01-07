using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR.Client;

// Create a connection to the SignalR server
var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5279/notify-tracker", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("{_access_token}")!; //JWT Token
    })
    .Build();

// Connection Closed Event Handler
connection.Closed += async (error) =>
{
    Console.WriteLine("Connection is closed. Reconnecting...");
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await connection.StartAsync();
};

// NotifyEvent Event Handler
connection.On<JsonObject>("NotifyEvent", data => { Console.WriteLine("NotifyEvent: " + data); });


// Connection Start and Join Groups
try
{
    await connection.StartAsync();
    
    if (connection.State == HubConnectionState.Connected)
        Console.WriteLine("Hub Connection is successfully established.");
    
    await connection.InvokeAsync("JoinAuthorizedGroup", "Tenant");
    await connection.InvokeAsync("JoinGroup", "TenantWithoutAuth");
}
catch (Exception ex)
{
    Console.WriteLine("Connection Error: " + ex.Message);
}

Console.ReadLine();