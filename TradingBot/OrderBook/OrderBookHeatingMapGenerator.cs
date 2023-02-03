namespace TradingBot.OrderBook
{
    public class OrderBookHeatingMapGenerator
    {
        private readonly IOrderBook _orderBook;
        private readonly OrderBookGoogleSheetUploader _orderBookGoogleSheetUploader;

        public OrderBookHeatingMapGenerator(IOrderBook orderBook, OrderBookGoogleSheetUploader orderBookGoogleSheetUploader)
        {
            _orderBook = orderBook;
            _orderBookGoogleSheetUploader = orderBookGoogleSheetUploader;
        }
        public async Task Run(string googleSheetId, double lowPriceRange, double highPriceRange, double priceGranularity, int lowVolumePercToFilterOut, int secondsIntervalBetweenAcquisition, string symbol, CancellationToken cancellationToken)
        {
            Task populateOrderBook = _orderBook.Build(symbol, cancellationToken);
            Task continuoslyUploadOrderBookData = _orderBookGoogleSheetUploader.ContinuoslyUpdateOrderBookInGoogleSheetByRow(cancellationToken, googleSheetId, secondsIntervalBetweenAcquisition, lowPriceRange, highPriceRange, priceGranularity, lowVolumePercToFilterOut,_orderBook);
            await Task.WhenAll(populateOrderBook, continuoslyUploadOrderBookData);
        }
    }
}
