using System.Net.WebSockets;
using System.Text;

namespace TradingBot
{
    public class BinanceConnector : IBinanceConnector
    {
        private readonly ILogger<IBinanceConnector> logger;

        public BinanceConnector(ILogger<BinanceConnector> logger)
        {
            this.logger = logger;
        }

        public async Task<int> OpenConnection() {
            Uri uri = new("ws://corefx-net-http11.azurewebsites.net/WebSocket/EchoWebSocket.ashx");

            using ClientWebSocket ws = new();
            await ws.ConnectAsync(uri, default);

            var bytes = new byte[1024];
            var result = await ws.ReceiveAsync(bytes, default);
            string res = Encoding.UTF8.GetString(bytes, 0, result.Count);

            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
            return 0; 
        }

    }

    public interface IBinanceConnector
    {
        Task<int> OpenConnection();
    }
}
