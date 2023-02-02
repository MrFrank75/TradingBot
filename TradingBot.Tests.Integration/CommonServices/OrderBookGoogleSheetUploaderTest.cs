using Microsoft.Extensions.Logging;
using TradingBot.BinanceServices;
using TradingBot.Shared;
using TradingBot.GoogleServices;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;
using static System.Net.WebRequestMethods;

namespace TradingBot.Tests.Integration.CommonServices
{
    public class OrderBookGoogleSheetUploaderTest
    {
        private readonly ILogger<OrderBookGoogleSheetUploader> _loggerGoogleSheetUploader;
        private readonly ILogger<BinanceOrderBook> _loggerOrderBook;
        private readonly ILogger<OrderBookBuilder> _loggerOrderBookBuilder;
        private readonly ILogger<BinanceConnectorWrapper> _loggerBinanceConnector;

        public OrderBookGoogleSheetUploaderTest(ITestOutputHelper testOutputHelper)
        {
            _loggerGoogleSheetUploader = XUnitLogger.CreateLogger<OrderBookGoogleSheetUploader>(testOutputHelper);
            _loggerOrderBook = XUnitLogger.CreateLogger<BinanceOrderBook>(testOutputHelper);
            _loggerOrderBookBuilder = XUnitLogger.CreateLogger<OrderBookBuilder>(testOutputHelper);
            _loggerBinanceConnector = XUnitLogger.CreateLogger<BinanceConnectorWrapper>(testOutputHelper);
        }

        [Fact]
        public async void CanUploadOrderBookData_FromRealOrderBookRequest()
        {
            string PRODUCTION_ADDRESS_FOR_FUTURES_WEBSOCKET_STREAM = "wss://fstream.binance.com"; //as explained in https://binance-docs.github.io/apidocs/futures/en/#diff-book-depth-streams

            //ARRANGE
            var cancellationTokenSource = new CancellationTokenSource();
            var binanceConnectorWrapper = new BinanceConnectorWrapper(_loggerBinanceConnector, new FutureMarket(), PRODUCTION_ADDRESS_FOR_FUTURES_WEBSOCKET_STREAM);
            var symbol = "BTCUSDT";
            var tasks = new List<Task>();
            var binanceOrderBook = new BinanceOrderBook(_loggerOrderBook, binanceConnectorWrapper, new OrderBookBuilder(_loggerOrderBookBuilder));
            IPriceRangeQuantizer priceRangeQuantizer = new PriceRangeQuantizer();
            IGoogleSheetWriter googleSheetWriter = new GoogleSheetWriter(priceRangeQuantizer);
            
            var sut = new OrderBookGoogleSheetUploader(_loggerGoogleSheetUploader, "1PMYJvYX8ryckzLiH8xkDdrrYDi7Q64GbPNsMnLDATyE", priceRangeQuantizer, googleSheetWriter);

            //ACT
            int minutesAcquisitionDuration = 30;
            int secondsIntervalBetweenAcquisition = 30;
            Task populateOrderBook = binanceOrderBook.Build(symbol, cancellationTokenSource.Token);
            Task continuoslyUploadOrderBookData = sut.ContinuoslyUpdateOrderBookInGoogleSheetByRow(cancellationTokenSource.Token, secondsIntervalBetweenAcquisition, binanceOrderBook.Entries, 23000,25000,200, binanceOrderBook);
            Task taskCancelToken = Task.Run(async () =>
            {
                await Task.Delay(minutesAcquisitionDuration * 60000);
                cancellationTokenSource.Cancel();
                return Task.CompletedTask;
            });
            tasks.Add(populateOrderBook);
            tasks.Add(continuoslyUploadOrderBookData);
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
