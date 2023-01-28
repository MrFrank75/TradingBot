using System.Collections.Concurrent;
using TradingBot.BinanceServices;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.Tests.Integration.Dummies
{
    internal class DummyOrderBookConverter : IOrderBookConverter
    {
        public void PopulateFromBidAskEntries(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries)
        {
            //do nothing
        }

        public void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity)
        {
            //do nothing
        }
    }
}
