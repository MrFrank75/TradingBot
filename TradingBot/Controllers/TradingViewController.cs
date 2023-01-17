using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TradingBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradingViewController : ControllerBase
    {
        private readonly ILogger<TradingViewController> _logger;

        public TradingViewController(ILogger<TradingViewController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTradesOpen")]
        public IEnumerable<string> Get()
        {
            return new List<string>();
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Execute([FromBody] TradingViewMessage jsonMessage)
        {
            try
            {
                _logger.LogInformation(jsonMessage.AssetId);
                _logger.LogInformation(jsonMessage.UserId);
                _logger.LogInformation(jsonMessage.TradeAction);
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
