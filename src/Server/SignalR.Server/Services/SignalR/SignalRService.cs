using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using SignalR.Server.Settings;

namespace SignalR.Server.Services.SignalR
{
    public class SignalRService : ISignalRService
    {
        private readonly IOptions<CoreSettings> _options;

        public SignalRService(IOptions<CoreSettings> options)
        {
            _options = options;
        }

        public async Task Push<THubMethodType>(THubMethodType method, JsonObject data, string hubUrl)
            where THubMethodType : Enum
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl($"{_options.Value.ServerUrl}/{hubUrl}")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();

            try
            {
                if (hubConnectionBuilder.State != HubConnectionState.Connected)
                {
                    // Yöntemleri çağırmadan önce hub bağlantısının başlatıldığından emin olun
                    await hubConnectionBuilder.StartAsync();
                }

                await hubConnectionBuilder.InvokeAsync(method.ToString(), data);
            }
            catch (Exception ex)
            {
                // Hataları uygun şekilde günlüğe kaydedin veya işleyin
                Console.WriteLine($"Push metodunda hata oluştu: {ex.Message}");
                throw;
            }
        }

        public async Task Ping<THubMethodType>(THubMethodType method, string hubUrl)
            where THubMethodType : Enum
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl($"{_options.Value.ServerUrl}/{hubUrl}")
                .Build();

            try
            {
                if (hubConnectionBuilder.State != HubConnectionState.Connected)
                {
                    // Yöntemleri çağırmadan önce hub bağlantısının başlatıldığından emin olun
                    await hubConnectionBuilder.StartAsync();
                }

                await hubConnectionBuilder.InvokeAsync(method.ToString());
            }
            catch (Exception ex)
            {
                // Hataları uygun şekilde günlüğe kaydedin veya işleyin
                Console.WriteLine($"Ping metodunda hata oluştu: {ex.Message}");
                throw;
            }
        }
    }
}
