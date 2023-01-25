using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TradingBot.BinanceServices;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration
{

    public class BinanceConnectorTest
    {
        private readonly ILogger<BinanceConnectorWrapper> _logger;

        public BinanceConnectorTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<BinanceConnectorWrapper>(testOutputHelper);
        }

        [Fact]
        public async Task CanCancel_After_StartListeningToSingleStream()
        {
            //ARRANGE
            var sut = new BinanceConnectorWrapper(_logger);
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = "bnbusdt@aggTrade";
            var tasks = new List<Task>();

            //ACT
            Task<int> taskListen = sut.ListenToSingleStream(stream, cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(async () =>
            {
                await Task.Delay(5000);
                cancellationTokenSource.Cancel();
            });

            tasks.Add(taskCancelToken);
            tasks.Add(taskListen);

            var taskResult = Task.WhenAll(tasks);
            try
            {
                await taskResult;
            }
            catch { }

            //ASSERT
            Assert.True(taskCancelToken.IsCompletedSuccessfully == true);
            Assert.True(taskListen.IsCompletedSuccessfully == true);
        }

        [Fact]
        public async Task CanReceiveMessages_WhenListeningToSingleStream()
        {
            //ARRANGE
            var sut = new BinanceConnectorWrapper(_logger);
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = "ethusdt@aggTrade";
            var tasks = new List<Task>();

            //ACT
            Task<int> taskListen = sut.ListenToSingleStream(stream, cancellationTokenSource.Token);
            Task taskCountingMessage = Task.Run(async () =>
            {
                while (sut.Messages.Count < 5)
                {
                    await Task.Delay(1000);
                }
                cancellationTokenSource.Cancel();
            });

            tasks.Add(taskCountingMessage);
            tasks.Add(taskListen);

            var taskResult = Task.WhenAll(tasks);
            try
            {
                await taskResult;
            }
            catch { }

            //ASSERT
            Assert.True(sut.Messages.Count >= 2);
        }




        [Fact]
        public async Task CanReceiveOrderBookMessages_WhenListeningToOrderBook()
        {
            //ARRANGE
            var sut = new BinanceConnectorWrapper(_logger);
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = "btcusdt@depth@500ms";
            var tasks = new List<Task>();

            //ACT
            Task<int> taskListen = sut.ListenToOrderBook(stream, cancellationTokenSource.Token);
            Task taskCountingMessage = Task.Run(async () =>
            {
                while (sut.OrderBookMessages.Count < 20)
                {
                    await Task.Delay(1000);
                }
                cancellationTokenSource.Cancel();
            });

            tasks.Add(taskCountingMessage);
            tasks.Add(taskListen);

            var taskResult = Task.WhenAll(tasks);
            try
            {
                await taskResult;
            }
            catch { }

            //ASSERT
            Assert.True(sut.OrderBookMessages.Count >= 2);
        }


    }
}