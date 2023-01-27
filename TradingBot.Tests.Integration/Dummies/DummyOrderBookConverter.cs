using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBot.BinanceServices;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.Tests.Integration.Dummies
{
    internal class DummyOrderBookConverter : IOrderBookConverter
    {
        public void PopulateFromBidAskEntries(ConcurrentQueue<string> orderBookMessages, List<OrderBookEntry> entries)
        {
            //do nothing
        }

        public void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity)
        {
            //do nothing
        }
    }
}
