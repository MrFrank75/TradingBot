using Microsoft.Extensions.Logging;
using TradingBot.BinanceServices;
using TradingBot.Tests.Integration.Dummies;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.BinanceServices
{
    public class BinanceOrderBookTest
    {
        private readonly ILogger<BinanceOrderBook> _logger;
        private ILogger<OrderBookBuilder> _orderBookBuilderLogger;
        private readonly ILogger<BinanceOrderBookConnector> _loggerConnector;

        public BinanceOrderBookTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<BinanceOrderBook>(testOutputHelper);
            _orderBookBuilderLogger = XUnitLogger.CreateLogger<OrderBookBuilder>(testOutputHelper);
            _loggerConnector = XUnitLogger.CreateLogger<BinanceOrderBookConnector>(testOutputHelper);
        }

        

        [Fact]
        public async void CanCancel_PopulateOrderBook_Request()
        {

            //ARRANGE
            var cancellationTokenSource = new CancellationTokenSource();
            var binanceConnectorWrapper = new BinanceOrderBookConnector(_loggerConnector, new FutureMarket());
            var orderBookConverter = new DummyOrderBookConverter();
            var symbol = "BTCUSDT";
            var tasks = new List<Task>();
            var sut = new BinanceOrderBook(_logger, binanceConnectorWrapper, orderBookConverter);

            //ACT
            Task populateOrderBook = sut.Build(symbol, cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(() =>
            {
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

        [Fact]
        public async void Can_PopulateOrderBook_WithTickerInformation_Request()
        {

            //ARRANGE
            var cancellationTokenSource = new CancellationTokenSource();
            var binanceConnectorWrapper = new BinanceOrderBookConnector(_loggerConnector, new FutureMarket());
            var orderBookConverter = new DummyOrderBookConverter();
            var symbol = "BTCUSDT";
            var tasks = new List<Task>();
            var sut = new BinanceOrderBook(_logger, binanceConnectorWrapper, orderBookConverter);

            //ACT
            Task populateOrderBook = sut.Build(symbol, cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(() =>
            {
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
            Assert.True(sut.TickerInfo.Price >0);
            Assert.True(sut.TickerInfo.Timestamp>DateTime.MinValue);
            Assert.True(populateOrderBook.IsCompletedSuccessfully == true);
        }

        [Fact(Skip ="Manual test to be used for the generation of multiple CSV snapshots")]
        public async void CanGenerateOneCsv_FromRealOrderBookRequest()
        {

            //ARRANGE
            var cancellationTokenSource = new CancellationTokenSource();
            var binanceConnectorWrapper = new BinanceOrderBookConnector(_loggerConnector, new FutureMarket(), "wss://fstream.binance.com");
            var symbol = "BTCUSDT";
            var tasks = new List<Task>();
            var sut = new BinanceOrderBook(_logger, binanceConnectorWrapper, new OrderBookBuilder(_orderBookBuilderLogger));

            //ACT
            Task populateOrderBook = sut.Build(symbol, cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(async () =>
            {
                await Task.Delay(120000);
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
