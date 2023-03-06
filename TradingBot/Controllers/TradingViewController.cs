using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net.WebSockets;
using System.Text;
using TradingBot.BinanceServices;
using TradingBot.TradingServices;

namespace TradingBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradingViewController : ControllerBase
    {
        private readonly ILogger<TradingViewController> _logger;
        private readonly ITradesExecutor _tradesExecutor;

        public TradingViewController(ILogger<TradingViewController> logger, ITradesExecutor tradesExecutor)
        {
            _logger = logger;
            _tradesExecutor = tradesExecutor;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return new List<string>();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Execute([FromBody] TradingViewMessage jsonMessage)
        {
            try
            {
                string assetId = jsonMessage.AssetId;
                string userId = jsonMessage.UserId;
                string tradeAction = jsonMessage.TradeAction;
                string strPrice = jsonMessage.Price;

                _logger.LogInformation(assetId);
                _logger.LogInformation(userId);
                _logger.LogInformation(tradeAction);
                _logger.LogInformation(strPrice);

                if (decimal.TryParse(strPrice, out var price) == false)
                {
                    string message = "It was not possible to parse and convert the required price";
                    _logger.LogError(message);
                    throw new ArgumentException(message);
                }

                decimal quantityUSD = 1000;

                if (tradeAction == "open")
                    _tradesExecutor.OpenPosition(assetId, quantityUSD, price, Models.OrderSide.LONG, Models.OrderType.LIMIT);
                else if (tradeAction == "close")
                    _tradesExecutor.ClosePositionLongAtMarket(assetId);
                else
                {
                    string message = "Unable to parse the trade Action";
                    _logger.LogError(message);
                    return BadRequest(message);
                }    

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}
