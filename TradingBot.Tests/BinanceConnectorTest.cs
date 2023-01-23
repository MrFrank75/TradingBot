using Xunit.Abstractions;

namespace TradingBot.Tests
{
    public class BinanceConnectorTest
    {
        private readonly BinanceConnector _sut;

        public BinanceConnectorTest(ITestOutputHelper testOutputHelper)
        {
            var webApp = new WebApplication(testOutputHelper);
            _sut = webApp.Services.GetService(typeof(IBinanceConnector)) as BinanceConnector;
        }

        [Fact]
        public async Task CanOpenWebsocketConnection()
        {
            //Arrange
            //done in the constructor

            // Act
            var result = await _sut.OpenConnection();

            // Assert
            Assert.True(result == 1);
        }

    }
}