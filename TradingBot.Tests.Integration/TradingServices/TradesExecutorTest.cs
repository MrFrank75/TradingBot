using Microsoft.Extensions.Logging;
using TradingBot.Tests.Integration.GoogleServices;
using TradingBot.Tests.Integration.XUnitUtilities;
using TradingBot.TradingServices;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.TradingServices
{
    public class TradesExecutorTest
    {
        private readonly ILogger<TradesExecutorTest> _logger;
        private readonly ITradesExecutor _tradesExecutor;

        public TradesExecutorTest(ITestOutputHelper testOutputHelper, ITradesExecutor tradesExecutor)
        {
            _logger = XUnitLogger.CreateLogger<TradesExecutorTest>(testOutputHelper);
            _tradesExecutor = tradesExecutor;
        }

        [Fact]
        public void CanLaunchTradeExecutor()
        {
            bool hasStarted = _tradesExecutor.Start();
            Assert.True(hasStarted);
        }
    }
}
