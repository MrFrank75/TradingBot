using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public class OrderBookConverter : IOrderBookConverter
    {
        public void PopulateFromBidAskEntries(ConcurrentQueue<string> orderBookMessages, List<OrderBookEntry> entries)
        {
           throw new NotImplementedException();
        }

        public void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries)
        {
            throw new NotImplementedException();
        }
    }
}
