namespace TradingBot.TradingServices
{
    public class TradesExecutor : ITradesExecutor
    {
        private readonly ILogger<TradesExecutor> _logger;

        public TradesExecutor(ILogger<TradesExecutor> logger)
        {
            _logger = logger;
        }

        public bool Start()
        {
            _logger.LogInformation("System starting....");
            _logger.LogInformation("System started.");
            return true;
        }
    }
}
