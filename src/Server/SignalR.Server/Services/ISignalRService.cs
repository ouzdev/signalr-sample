using System.Text.Json.Nodes;

namespace SignalR.Server.Services;

public interface ISignalRService
{
    Task Push<THubMethodType>(THubMethodType method, JsonObject data, string hubUrl) where THubMethodType : Enum;

    Task Ping<THubMethodType>(THubMethodType method, string hubUrl) where THubMethodType : Enum;
}   