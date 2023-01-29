using System.Collections.Concurrent;
using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices
{
    public class OrderBookBuilder : IOrderBookConverter
    {
        private readonly ILogger<OrderBookBuilder> _logger;

        public OrderBookBuilder(ILogger<OrderBookBuilder> logger)
        {
            _logger = logger;
        }

        public void BuildFromBidAskEntriesStream(ConcurrentQueue<DiffBookDepthStream> orderBookMessages, List<OrderBookEntry> entries, long lastSnapshotUpdateId, int priceGranularity)
        {
            List<DiffBookDepthStream> dequeuedItems = new List<DiffBookDepthStream>();
            int maxItemsToDequeueForCycle = 10;

            while (orderBookMessages.IsEmpty == false && dequeuedItems.Count < maxItemsToDequeueForCycle) { 
                if (orderBookMessages.TryDequeue(out var orderBookEntry))
                {
                    dequeuedItems.Add(orderBookEntry); 
                }
            }

            _logger.LogInformation($"Dequeing from the incoming stream. N. entries received:{dequeuedItems.Count}");

            //here begins the actual merge of data
            //sorting for the oldest snapshot
            var validItems = dequeuedItems.Where(item => item.LastUpdateId >= lastSnapshotUpdateId).ToList();
            validItems.Sort((a, b) => a.FirstUpdateId > b.FirstUpdateId ? 1 : 0);

            //if the list is empty we have only old elements, which can happen if the initial snapshot is delayed
            if (validItems.Count == 0)
            {
                return;
            }

            if (lastSnapshotUpdateId>0 && validItems.First().FirstUpdateId > lastSnapshotUpdateId)
                throw new Exception("Missing Data. A new snapshot is required");

            foreach (var diffBookEntry in dequeuedItems)
            {
                diffBookEntry.AsksToUpdate.ForEach(ask => 
                { 
                    ReplacePriceLevels(entries, ask, priceGranularity);                        
                });
                diffBookEntry.BidsToUpdate.ForEach(bid =>
                {
                    ReplacePriceLevels(entries, bid, priceGranularity);
                });
            }
        }

        public void BuildFromSnapshot(OrderBookAPISnapshot initialSnapshot, List<OrderBookEntry> entries, int priceGranularity)
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

        private static void ReplacePriceLevels(List<OrderBookEntry> entries, BidAskEntry ask, int priceGranularity)
        {
            var roundedPricefloorLevel = ask.PriceLevel - (ask.PriceLevel % priceGranularity);
            if (ask.PriceLevel % priceGranularity > (priceGranularity / 2))
                roundedPricefloorLevel += priceGranularity;

            var existingEntry = entries.SingleOrDefault(entry => entry.PriceLevel == roundedPricefloorLevel);
            if (existingEntry != null)
            {
                //replace the existing quantity
                existingEntry.Quantity = Convert.ToDouble(ask.Quantity);
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

        private static void AddToPriceLevels(List<OrderBookEntry> entries, BidAskEntry bidAskEntry, int priceGranularity)
        {
            var roundedPricefloorLevel = bidAskEntry.PriceLevel - (bidAskEntry.PriceLevel % priceGranularity);
            if (bidAskEntry.PriceLevel%priceGranularity> (priceGranularity / 2))
                roundedPricefloorLevel += priceGranularity;

            var existingEntry = entries.SingleOrDefault(entry => entry.PriceLevel == roundedPricefloorLevel);
            if (existingEntry != null)
            {
                existingEntry.Quantity += Convert.ToDouble(bidAskEntry.Quantity);
            }
            else
            {
                entries.Add(new OrderBookEntry
                {
                    PriceLevel = roundedPricefloorLevel,
                    Quantity = Convert.ToDouble(bidAskEntry.Quantity)
                });
            }
        }
    }
}
