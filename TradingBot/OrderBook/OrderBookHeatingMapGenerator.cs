using TradingBot.BinanceServices;
using TradingBot.GoogleServices;

namespace TradingBot.OrderBook
{
    public class OrderBookHeatingMapGenerator
    {
        private readonly IOrderBook binanceOrderBook;
        private readonly OrderBookGoogleSheetUploader orderBookGoogleSheetUploader;

        public OrderBookHeatingMapGenerator(IOrderBook binanceOrderBook, OrderBookGoogleSheetUploader orderBookGoogleSheetUploader)
        {
            this.binanceOrderBook = binanceOrderBook;
            this.orderBookGoogleSheetUploader = orderBookGoogleSheetUploader;
        }
        public async Task Run(double lowPriceRange, double highPriceRange, double priceGranularity, int secondsIntervalBetweenAcquisition, string symbol, CancellationToken cancellationToken)
        {
            Task populateOrderBook = binanceOrderBook.Build(symbol, cancellationToken);
            Task continuoslyUploadOrderBookData = orderBookGoogleSheetUploader.ContinuoslyUpdateOrderBookInGoogleSheetByRow(cancellationToken, secondsIntervalBetweenAcquisition, binanceOrderBook.Entries, lowPriceRange, highPriceRange, priceGranularity, binanceOrderBook);
            await Task.WhenAll(populateOrderBook, continuoslyUploadOrderBookData);
        }
    }
}
