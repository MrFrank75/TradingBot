using TradingBot.Models;

namespace TradingBot.BinanceServices.PayloadModels.API
{
    //the object returned by https://binance-docs.github.io/apidocs/futures/en/#symbol-price-ticker
    public class BinanceSymbol
    {
        public string symbol { get; set; }
        public string price { get; set; }
        public string time { get; set; }
    }
}