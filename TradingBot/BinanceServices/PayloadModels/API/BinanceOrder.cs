using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TradingBot.BinanceServices.PayloadModels.Websockets;
using TradingBot.Models;
using TradingBot.TradingServices;

namespace TradingBot.BinanceServices.PayloadModels.API
{
    public class BinanceOrder
    {
        public string avgPrice { get; set; }
        public string clientOrderId { get; set; }
        public string cumQuote { get; set; }
        public string executedQty { get; set; }
        public long orderId { get; set; }
        public string origQty { get; set; }
        public string origType { get; set; }
        public string price { get; set; }
        public bool reduceOnly { get; set; }
        public string side { get; set; }

        public string positionSide { get; set; }
        public string status { get; set; }
        public string stopPrice { get; set; }
        public bool closePosition { get; set; }
        public string symbol { get; set; }
        public long time { get; set; }
        public string timeInForce { get; set; }
        public string type { get; set; }
        public string activatePrice { get; set; }
        public string priceRate { get; set; }
        public long updateTime { get; set; }
        public string workingType { get; set; }
        public bool priceProtect { get; set; }
    }


    public class BinanceOrderList
    {
        [JsonConverter(typeof(BinanceOrderConverter))]
        public List<BinanceOrder> OpenOrders { get; set; }
    }

    public class BinanceOrderConverter : JsonConverter
    {
        // This is used when you're converting the C# List back to a JSON format.
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        // This is when you're reading the JSON object and converting it to C#
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var response = new List<BinanceOrder>();
            // Loading the JSON Array
            JArray arrayOfBidAsksEntries = JArray.Load(reader);

            // Looping through all the properties. C# treats it as key value pair
            foreach (var entry in arrayOfBidAsksEntries)
            {
                // Finally I'm deserializing the value into an actual object
                var p = JsonConvert.DeserializeObject<List<string>>(entry.ToString());
                var bidAskEntry = new BinanceOrder();
                response.Add(bidAskEntry);
            }
            return response;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(List<BidAskEntry>);
    }

}
