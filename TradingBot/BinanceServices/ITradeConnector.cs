using TradingBot.BinanceServices.PayloadModels.API;

namespace TradingBot.BinanceServices
{
    public interface ITradeConnector
    { 
        Task<List<BinanceOrder>> GetAllOpenOrders();

        Task<List<BinanceOrder>> GetOpenOrders(string symbol);
    }
}
