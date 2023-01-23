using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace TradingBot.UnitTests
{
    public class BinanceConnectorTest
    {
        [Fact]
        public void CanInstantiate()
        {
            var sut = new BinanceConnector(NullLogger<BinanceConnector>.Instance);
            sut.OpenConnection();
        }
    }
}