using Xunit.Abstractions;

namespace TradingBot.Tests
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

        [Fact]
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