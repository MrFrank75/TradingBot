using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TradingBot.TradingServices;

namespace TradingBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradesExecutorController : ControllerBase
    {
        private readonly ILogger<TradesExecutorController> _logger;
        private readonly ITradesExecutor _tradesExecutor;

        public TradesExecutorController(ILogger<TradesExecutorController> logger, ITradesExecutor tradesExecutor)
        {
            _logger = logger;
            _tradesExecutor = tradesExecutor;
        }

        [HttpGet("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StartTradesExecutor()
        {
            _tradesExecutor.Start();
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
