using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public interface IBinanceConnectorWrapper
    {
        ConcurrentQueue<string> OrderBookMessages { get; }

        Task<int> ListenToSingleStream(string stream, CancellationToken token);
        Task<int> ListenToOrderBookDepthStream(string stream, CancellationToken token);
        Task<OrderBookAPISnapshot?> LoadInitialOrderBookSnapshot(string symbol);
    }
}
