using TradingBot.BinanceServices.PayloadModels.API;
using TradingBot.Models;
using TradingBot.OrderBook;

namespace TradingBot.TradingServices
{
    public class TradesExecutor : ITradesExecutor
    {
        private readonly ILogger<TradesExecutor> _logger;
        private readonly IBrokerConnector _brokerConnector;

        public TradesExecutor(ILogger<TradesExecutor> logger, IBrokerConnector brokerConnector)
        {
            _logger = logger;
            _brokerConnector = brokerConnector;
        }

        /// <summary>
        /// Places an order on the specified side, with the specified type (LIMIT, MARKET, etc.)
        /// It doesn't mean that the order will be executed / filled
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>

        public async Task<bool> OpenPosition(string symbol, decimal quantity, decimal price, OrderSide orderSide, OrderType orderType)
        {
            BrokerConnectorReturnErrorCode returnErrorCode = await _brokerConnector.OpenNewOrder(symbol, orderSide, quantity, price, orderType);
            if (returnErrorCode == BrokerConnectorReturnErrorCode.SUCCESS)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ClosePositionLongAtMarket(string symbol)
        {
            return await ClosePositionAtMarket(symbol, OrderSide.LONG);
        }

        public async Task<bool> ClosePositionShortAtMarket(string symbol)
        {
            return await ClosePositionAtMarket(symbol, OrderSide.SHORT);
        }

        private async Task<bool> ClosePositionAtMarket(string symbol, OrderSide side)
        { 
            //this might be wrong, here we should get the current POSITION not ORDERS, to be understood 
            var currentOpenOrderInfo = await _brokerConnector.GetOpenOrders(symbol);

            List<IOrder> longOrders = currentOpenOrderInfo.Where(order => order.Side == side).ToList();
            var orderToClose = longOrders.Single();
            if (orderToClose== null)
            {
                _logger.LogError("An order was expected but the query returned null");
                return false;
            }

            //to close an order, at the low level, we open an order on the opposite side
            var oppositeSide = side == OrderSide.LONG ? OrderSide.SHORT : OrderSide.LONG; 

            BrokerConnectorReturnErrorCode returnErrorCode = await _brokerConnector.OpenNewOrder(symbol,oppositeSide, orderToClose.Quantity, OrderType.MARKET);
            if (returnErrorCode == BrokerConnectorReturnErrorCode.SUCCESS)
            {
                return true;
            }

            return false;
        }
        


        public bool Start()
        {
            _logger.LogInformation("System starting....");
            _logger.LogInformation("System started.");
            return true;
        }
    }
}
