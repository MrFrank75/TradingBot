using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public class OrderBookConverter : IOrderBookConverter
    {
        public void PopulateFromBidAskEntries(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries)
        {
           throw new NotImplementedException();
        }

        public void PopulateFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity)
        {
            initialSnapshot.AsksToUpdate.ForEach(ask =>
            {
                AddToPriceLevels(entries, ask, priceGranularity);
            });

            initialSnapshot.BidsToUpdate.ForEach(bid =>
            {
                AddToPriceLevels(entries, bid, priceGranularity);
            });

        }

        private static void AddToPriceLevels(List<OrderBookEntry> entries, BidAskEntry ask, int priceGranularity)
        {
            var roundedPricefloorLevel = ask.PriceLevel - (ask.PriceLevel % priceGranularity);
            if (ask.PriceLevel%priceGranularity> (priceGranularity / 2))
                roundedPricefloorLevel += priceGranularity;

            var existingEntry = entries.SingleOrDefault(entry => entry.PriceLevel == roundedPricefloorLevel);
            if (existingEntry != null)
            {
                existingEntry.Quantity += Convert.ToDouble(ask.Quantity);
            }
            else
            {
                entries.Add(new OrderBookEntry
                {
                    PriceLevel = roundedPricefloorLevel,
                    Quantity = Convert.ToDouble(ask.Quantity)
                });
            }
        }
    }
}
