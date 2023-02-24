using TradingBot.BinanceServices;
using TradingBot.Models;

namespace TradingBot.TradingServices
{
    public class TradesExecutor : ITradesExecutor
    {
        private readonly ILogger<TradesExecutor> _logger;
        private readonly ITradeConnector _tradeConnector;

        public TradesExecutor(ILogger<TradesExecutor> logger, ITradeConnector tradeConnector)
        {
            _logger = logger;
            _tradeConnector = tradeConnector;
        }

        public bool OpenNewOrder(string symbol, OrderSide side, decimal quantity, decimal price, OrderType orderType)
        {
            throw new NotImplementedException();
        }

        public bool Start()
        {
            _logger.LogInformation("System starting....");
            _logger.LogInformation("System started.");
            return true;
        }
    }
}
