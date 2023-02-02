using TradingBot.Shared;
using Xunit.Abstractions;

namespace TradingBot.Tests.Integration.CommonServices
{

    public class PriceRangeQuantizerTest
    {
        private PriceRangeQuantizer _sut;

        public PriceRangeQuantizerTest(ITestOutputHelper testOutputHelper)
        {
            _sut = new PriceRangeQuantizer();
        }

        [Theory]
        [InlineData(0, 0, 0, 1, 0)]
        [InlineData(149, 100, 200, 100, 0)]
        [InlineData(151, 100, 200, 100, 1)]
        [InlineData(18000, 15000, 30000, 100, 30)]
        [InlineData(15051, 15000, 30000, 100, 1)]
        [InlineData(15050, 15000, 30000, 100, 0)]
        [InlineData(15000, 15000, 30000, 100, 0)]
        [InlineData(29949, 15000, 30000, 100, 149)]
        [InlineData(29951, 15000, 30000, 100, 150)]
        [InlineData(17000, 15000, 18000, 1000, 2)]
        public void QuantizeZeroRangeAlwaysReturnsZero(double price, double lowRange, double highRange, double priceGranularity, int expectedResult) {
            int result = _sut.QuantizePrice(price,lowRange, highRange, priceGranularity);
            Assert.Equal(expectedResult, result);
        }
    }
}
