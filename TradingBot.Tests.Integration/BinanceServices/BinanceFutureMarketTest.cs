using Binance.Spot.Models;
using TradingBot.BinanceServices;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.BinanceServices
{
    public class BinanceFutureMarketTest
    {

        public BinanceFutureMarketTest(ITestOutputHelper testOutputHelper)
        {
        }

        [Fact]
        public async void CanOpenNewOrder() {

            if (File.Exists("BinanceApiKeys.txt") == false)
                throw new Exception("Please place a file containing api key and secret in the folder where the tests are running. Normally it is bin/debug/net7.0");

            var apiKeyAndSecret = File.ReadAllLines("BinanceApiKeys.txt");
            var apiKey = apiKeyAndSecret[0];
            var apiSecret = apiKeyAndSecret[1];

            try
            {
                //the testnet specified here must be replaced with the proper address to have this test working. There is no testnet to open orders
                var sut = new FutureMarket("http://testnet.binancefutures.com", apiKey: apiKey, apiSecret: apiSecret);
                var result = await sut.NewOrder("BNBUSDT", Side.BUY, OrderType.MARKET, quantity: 1);
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        [Fact]
        public async void CanListOpenOrders()
        {

            if (File.Exists("BinanceApiKeys.txt") == false)
                throw new Exception("Please place a file containing api key and secret in the folder where the tests are running. Normally it is bin/debug/net7.0");

            var apiKeyAndSecret = File.ReadAllLines("BinanceApiKeys.txt");
            var apiKey = apiKeyAndSecret[0];
            var apiSecret = apiKeyAndSecret[1];

            try
            {
                var sut = new FutureMarket(apiKey: apiKey, apiSecret: apiSecret);
                var result = await sut.CurrentOpenOrders("LTCUSDT", 5000);
                Assert.NotNull(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}