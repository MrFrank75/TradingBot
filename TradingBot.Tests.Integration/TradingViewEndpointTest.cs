using TradingBot.Tests.Integration;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration
{
    public class TradingViewEndpointTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;

        public TradingViewEndpointTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _client = new WebApplication(_testOutputHelper).CreateClient();
        }

        [Fact(Skip =("Keeping disabled the TradingView part for now"))]
        public async Task ReturnsSuccessWhenInvokedWithCorrectParameters()
        {
            //Arrange
            //done in the constructor

            // Act
            HttpResponseMessage response = await _client.GetAsync("/tradingview");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

    }
}