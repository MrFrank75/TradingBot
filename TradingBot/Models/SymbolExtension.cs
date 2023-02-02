using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.Models
{
    public static class SymbolExtension {
        public static Symbol ToSymbol(this BinanceSymbol binanceSymbol)
        {
            double price = double.MinValue;
            DateTime symbolDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double timespanMs = 0;

            double.TryParse(binanceSymbol.price.Replace(".",","), out price);
            if (double.TryParse(binanceSymbol.time, out timespanMs) == true)
            {
                TimeSpan time = TimeSpan.FromMilliseconds(timespanMs);
                symbolDateTime = symbolDateTime.Add(time);
            }

            return new Symbol(
                binanceSymbol.symbol,
                price,
                symbolDateTime
            );

        }
    }
    
}
