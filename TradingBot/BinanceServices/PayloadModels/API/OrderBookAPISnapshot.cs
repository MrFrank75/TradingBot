using Newtonsoft.Json;
using TradingBot.BinanceServices.PayloadModels.Websockets;

namespace TradingBot.BinanceServices.PayloadModels.API
{
    //the object returned by https://binance-docs.github.io/apidocs/futures/en/#order-book
    public class OrderBookAPISnapshot
    {
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }  //the last update ID in this order book snapshot
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("T")]
        public long TransactionTime { get; set; }
        [JsonProperty("bids")]
        [JsonConverter(typeof(BidAskEntryConverter))]
        public List<BidAskEntry> BidsToUpdate { get; set; }
        [JsonProperty("asks")]
        [JsonConverter(typeof(BidAskEntryConverter))]
        public List<BidAskEntry> AsksToUpdate { get; set; }
    }


}
