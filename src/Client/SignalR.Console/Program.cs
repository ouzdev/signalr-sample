using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5279/notify-tracker")
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
    Console.WriteLine("Hub'a bağlantı başarılı!");
    
    // await connection.InvokeAsync("JoinGroup", "groupName");
}
catch (Exception ex)
{
    Console.WriteLine("Bağlantı hatası: " + ex.Message);
}

Console.ReadLine();