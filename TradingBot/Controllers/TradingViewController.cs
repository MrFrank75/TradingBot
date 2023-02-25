﻿using Microsoft.AspNetCore.Http.HttpResults;
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
