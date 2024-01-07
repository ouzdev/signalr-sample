using System.Text.Json.Nodes;
using SignalR.Server.Configurations;
using SignalR.Server.Hubs;
using SignalR.Server.Services;
using SignalR.Server.Services.Jwt;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSignalRService();
builder.Services.AddSettings(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(policyBuilder => { policyBuilder.AllowAnyOrigin(); });
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/token", (IServiceProvider provider) =>
    {
        var scope = provider.CreateScope();

        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

        return jwtService.GenerateTokenAsync();
    })
    .WithName("GetToken")
    .WithOpenApi();

app.MapGet("/notify-to-clients", (IServiceProvider provider) =>
    {
        var scope = provider.CreateScope();

        var signalRService = scope.ServiceProvider.GetRequiredService<ISignalRService>();

        signalRService.Push(NotifyTrackerHub.HubMethodType.NotifyEvent, new JsonObject()
        {
            ["title"] = "Test Notify",
            ["message"] = "Test Notify Message",
            ["date"] = DateTime.Now,
            ["type"] = "info"
        }, "notify-tracker");
    })
    .WithName("NotifyToClients")
    .WithOpenApi();

app.MapHub<NotifyTrackerHub>("/notify-tracker");

app.Run();