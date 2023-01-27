using Microsoft.Extensions.Logging;
using TradingBot.BinanceServices;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration
{
    public  class BinanceOrderBookTest
    {
        private readonly ILogger<BinanceOrderBook> _logger;
        private readonly ILogger<BinanceConnectorWrapper> _loggerConnector;

        public BinanceOrderBookTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<BinanceOrderBook>(testOutputHelper);
            _loggerConnector = XUnitLogger.CreateLogger<BinanceConnectorWrapper>(testOutputHelper);
        }

        [Fact]
        public async void CanCancel_PopulateOrderBook_Request() {

            //ARRANGE
            var cancellationTokenSource = new CancellationTokenSource();
            var binanceConnectorWrapper = new BinanceConnectorWrapper(_loggerConnector);
            var symbol = "BTCUSDT";
            var tasks = new List<Task>();
            var sut = new BinanceOrderBook(_logger, binanceConnectorWrapper);


            //ACT
            Task populateOrderBook = sut.Populate(symbol,cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(() => {
                cancellationTokenSource.Cancel();
                return Task.CompletedTask;
            });
            tasks.Add(populateOrderBook);
            tasks.Add(taskCancelToken);
            var taskResult = Task.WhenAll(tasks);
            try
            {
                await taskResult;
            }
            catch { }

            //ASSERT
            Assert.True(taskCancelToken.IsCompletedSuccessfully == true);
            Assert.True(populateOrderBook.IsCompletedSuccessfully == true);
        }

    }
}
