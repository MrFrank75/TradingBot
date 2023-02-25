using Microsoft.Extensions.Logging;
using TradingBot.BinanceServices;
using TradingBot.Tests.Integration.XUnitUtilities;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.BinanceServices
{
    public class BinanceTradeConnectorTest
    {

        private readonly ILogger<BinanceBrokerConnector> _logger;

        public BinanceTradeConnectorTest(ITestOutputHelper testOutputHelper)
        {
            _logger = XUnitLogger.CreateLogger<BinanceBrokerConnector>(testOutputHelper);
        }


        [Fact]
        public async Task CanReadSingleOrder()
        {
            if (File.Exists("BinanceApiKeys.txt") == false)
                throw new Exception("Please place a file containing api key and secret in the folder where the tests are running. Normally it is bin/debug/net7.0");

            var apiKeyAndSecret = File.ReadAllLines("BinanceApiKeys.txt");
            var apiKey = apiKeyAndSecret[0];
            var apiSecret = apiKeyAndSecret[1];

            var futureMarket = new FutureMarket(apiKey: apiKey, apiSecret: apiSecret);
            var sut = new BinanceBrokerConnector(_logger, futureMarket);

            var singleOrder = await sut.GetOpenOrders("LTCUSDT");
            Assert.NotNull(singleOrder);
        }

        [Fact]
        public async Task CanReadListOfOrders()
        {
            if (File.Exists("BinanceApiKeys.txt") == false)
                throw new Exception("Please place a file containing api key and secret in the folder where the tests are running. Normally it is bin/debug/net7.0");

            var apiKeyAndSecret = File.ReadAllLines("BinanceApiKeys.txt");
            var apiKey = apiKeyAndSecret[0];
            var apiSecret = apiKeyAndSecret[1];

            var futureMarket = new FutureMarket(apiKey: apiKey, apiSecret: apiSecret);
            var sut = new BinanceBrokerConnector(_logger, futureMarket);

            var listOfOrders = await sut.GetAllOpenOrders();
            Assert.NotNull(listOfOrders);
        }
    }
}
