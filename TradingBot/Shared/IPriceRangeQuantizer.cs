namespace TradingBot.Shared
{
    public interface IPriceRangeQuantizer
    {
        int GetMaxLevel(double lowPriceRange, double highPriceRange, double priceGranularity);
        int QuantizePrice(double price, double lowRange, double highRange, double priceGranularity);
    }
}