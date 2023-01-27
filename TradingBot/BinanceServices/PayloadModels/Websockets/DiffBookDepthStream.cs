using Newtonsoft.Json;
using System.Numerics;

namespace TradingBot.BinanceServices.PayloadModels.Websockets
{
    //the object returned by https://binance-docs.github.io/apidocs/futures/en/#diff-book-depth-streams
    public class DiffBookDepthStream
    {
        [JsonProperty("e")]
        public string EventType { get; set; }
        [JsonProperty("E")]
        public long EventTime { get; set; }
        [JsonProperty("T")]
        public long TransactionTime { get; set; }
        [JsonProperty("s")]
        public string Symbol { get; set; }
        [JsonProperty("U")]
        public long FirstUpdateId { get; set; } //the first update ID in this order book entry
        [JsonProperty("u")]
        public long LastUpdateId { get; set; }  //the last update ID in this order book entry
        [JsonProperty("pu")]
        public long PreviousUpdateId { get; set; }  //the last updated ID that has been sent so far
        [JsonProperty("b")]
        [JsonConverter(typeof(BidAskEntryConverter))]
        public List<BidAskEntry> BidsToUpdate { get; set; }
        [JsonProperty("a")]
        [JsonConverter(typeof(BidAskEntryConverter))]
        public List<BidAskEntry> AsksToUpdate { get; set; }
    }


}
