using System.Runtime.CompilerServices;
using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.Models
{
    public class Symbol : ISymbol
    {
        private readonly string _ticker;
        private readonly DateTime _timeStamp;
        private double _price;

        public Symbol(string ticker, double price, DateTime timeStamp)
        {
            _timeStamp = timeStamp;
            _ticker = ticker;
            _price = price;
        }

        public string Ticker => _ticker;

        public double Price => _price;

        public DateTime Timestamp => _timeStamp;

    }
    
}
