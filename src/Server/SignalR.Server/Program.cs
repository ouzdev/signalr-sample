using System.Text.Json.Nodes;
using SignalR.Server.Hubs;
using SignalR.Server.Services;
using SignalR.Server.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.Configure<CoreSettings>(builder.Configuration.GetSection("Core"));
builder.Services.AddScoped<ISignalRService, SignalRService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/notify-to-clients", (IServiceProvider provider) =>
    {
       var scope = provider.CreateScope();
       
       var signalRService = scope.ServiceProvider.GetRequiredService<ISignalRService>();

       signalRService.Push(NotifyTrackerHub.HubMethodType.NotifyEvent, new JsonObject{}, "notify-tracker");
    })
    .WithName("NotifyToClients")
    .WithOpenApi();

app.MapHub<NotifyTrackerHub>("/notify-tracker");

app.Run();