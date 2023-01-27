using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TradingBot.BinanceServices.PayloadModels.Websockets
{
    public class BidAskEntryConverter : JsonConverter
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
            var response = new List<BidAskEntry>();
            // Loading the JSON Array
            JArray arrayOfBidAsksEntries = JArray.Load(reader);

            // Looping through all the properties. C# treats it as key value pair
            foreach (var entry in arrayOfBidAsksEntries)
            {
                // Finally I'm deserializing the value into an actual object
                var p = JsonConvert.DeserializeObject<List<string>>(entry.ToString());
                var bidAskEntry = new BidAskEntry();
                bidAskEntry.PriceLevel = p[0];
                bidAskEntry.Quantity = p[1];
                response.Add(bidAskEntry);
            }
            return response;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(List<BidAskEntry>);
    }


}
