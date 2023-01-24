using Binance.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TradingBot.BinanceServices;

namespace TradingBot.Backend.IntegrationTests
{
    public class BinanceConnectorTest
    {
        [Fact]
        public async Task CanCancel_After_StartListeningToSingleStream()
        {
            //ARRANGE
            var sut = new BinanceConnectorWrapper(NullLogger<BinanceConnectorWrapper>.Instance);
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = "bnbusdt@aggTrade";
            var tasks = new List<Task>();

            //ACT
            Task<int> taskListen = sut.ListenToSingleStream(stream, cancellationTokenSource.Token);
            Task taskCancelToken = Task.Run(async () => {
                await Task.Delay(5000);
                cancellationTokenSource.Cancel();
            });

            tasks.Add(taskCancelToken);
            tasks.Add(taskListen);

            var taskResult = Task.WhenAll(tasks);
            try
            {
                taskResult.Wait();
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
            var sut = new BinanceConnectorWrapper(NullLogger<BinanceConnectorWrapper>.Instance);
            var cancellationTokenSource = new CancellationTokenSource();
            var stream = "bnbusdt@aggTrade";
            var tasks = new List<Task>();

            //ACT
            Task<int> taskListen = sut.ListenToSingleStream(stream, cancellationTokenSource.Token);
            Task taskCountingMessage = Task.Run(async () => {
                    while (sut.Messages.Count < 2)
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
                taskResult.Wait();
            }
            catch { }

            //ASSERT
            Assert.True(sut.Messages.Count>=2);
        }




        public async Task CanSubscribeToMultipleStream()
        {
            var sut = new BinanceConnectorWrapper(NullLogger<BinanceConnectorWrapper>.Instance);
            var cancellationTokenSource = new CancellationTokenSource();
            
            var stream2 = "{\r\n\"method\": \"SUBSCRIBE\",\r\n\"params\":\r\n[\r\n\"ethusdt@aggTrade\",\r\n\"ethusdt@depth\"\r\n],\r\n\"id\": 1\r\n}";
            int result = await sut.ListenToSingleStream(stream2, cancellationTokenSource.Token);
        }


    }
}