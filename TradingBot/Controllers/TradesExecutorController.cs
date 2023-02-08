using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingBot.BinanceServices;

namespace TradingBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradesExecutorController : ControllerBase
    {
        private readonly ILogger<TradesExecutorController> _logger;

        public TradesExecutorController(ILogger<TradesExecutorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StartTradesExecutor()
        {
            return Ok();
            //return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("Stop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StopTradesExecutor()
        {
            return Ok();
            //return product == null ? NotFound() : Ok(product);
        }
    }
}
