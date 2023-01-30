using Binance.Common;
using static System.Net.WebRequestMethods;

namespace TradingBot.BinanceServices
{
    public class FutureMarket : BinanceService
    {
        //This constructor uses the "production" web api address as explained here: https://binance-docs.github.io/apidocs/futures/en/#diff-book-depth-streams
        public FutureMarket(string baseUrl = "https://fapi.binance.com", string apiKey = null, string apiSecret = null)
:       base(new HttpClient(), baseUrl: baseUrl, apiKey: apiKey, apiSecret: apiSecret)
        {
        }

        private const string ORDER_BOOK = "/fapi/v1/depth";

        /// <summary>
        /// | Limit               | Weight(IP)  |.<para />
        /// |---------------------|-------------|.<para />
        /// | 1-100               | 1           |.<para />
        /// | 101-500             | 5           |.<para />
        /// | 501-1000            | 10          |.<para />
        /// | 1001-5000           | 50          |.
        /// </summary>
        /// <param name="symbol">Trading symbol, e.g. BNBUSDT.</param>
        /// <param name="limit">If limit > 5000, then the response will truncate to 5000.</param>
        /// <returns>Order book.</returns>
        public async Task<string> OrderBook(string symbol, int? limit = null)
        {
            var result = await this.SendPublicAsync<string>(
                ORDER_BOOK,
                HttpMethod.Get,
                query: new Dictionary<string, object>
                {
                    { "symbol", symbol },
                    { "limit", limit },
                });

            return result;
        }
    }
}
