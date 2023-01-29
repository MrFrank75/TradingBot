using System.Collections.Concurrent;
using TradingBot.BinanceServices;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.Tests.Integration.Dummies
{
    internal class DummyOrderBookConverter : IOrderBookConverter
    {
        public void BuildFromBidAskEntriesStream(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries, long lastUpdateId, int priceGranularity)
        {
            //do nothing
        }

        public void BuildFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity)
        {
            //do nothing
        }
    }
}
