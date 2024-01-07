using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5279/notify-tracker", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE3MDQ2NTI0MDksImV4cCI6MTcwNDY1NjAwOSwiaWF0IjoxNzA0NjUyNDA5LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUyNzkiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjUyNzkifQ.nMIH8hTrWouq0FMe3buibt3pmgcvL4maFE_OnrnRpPQ")!;
    })
    .Build();

connection.Closed += async (error) =>
{
    Console.WriteLine("Bağlantı kapandı, tekrar bağlanmaya çalışılıyor...");
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await connection.StartAsync();
};


connection.On<JsonObject>("NotifyEvent", data => { Console.WriteLine("NotifyEvent: " + data); });

try
{
    await connection.StartAsync();
    
    if (connection.State == HubConnectionState.Connected)
        Console.WriteLine("Hub'a bağlantı başarılı!");
    
    await connection.InvokeAsync("JoinAuthorizedGroup", "Tenant");
    await connection.InvokeAsync("JoinGroup");
}
catch (Exception ex)
{
    Console.WriteLine("Bağlantı hatası: " + ex.Message);
}

Console.ReadLine();