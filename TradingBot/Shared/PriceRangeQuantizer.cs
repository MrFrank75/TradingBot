using System.Security.Cryptography.Xml;

namespace TradingBot.Shared
{
    public class PriceRangeQuantizer : IPriceRangeQuantizer
    {
        public int GetMaxLevel(double lowPriceRange, double highPriceRange, double priceGranularity)
        {
            double range = highPriceRange- lowPriceRange;
            int maxLevel = Convert.ToInt32(range / priceGranularity);
            return maxLevel;
        }

        /// <summary>
        /// Quantize price returning a level comprised between ZERO and a max level based on granularity and range
        /// - Every price below low range will return a quantized value of zero
        /// - Every price above high range will return a quantized value of max level
        /// - every price will be rounded to the level below or above based on granularity:
        /// - E.G. if range is 100 to 200, a price of 149 will be rounded to level 0
        /// - E.G. if range is 100 to 200, a price of 151 will be rounded to level 1
        /// </summary>
        /// <param name="price"></param>
        /// <param name="lowRange"></param>
        /// <param name="highRange"></param>
        /// <param name="priceGranularity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int QuantizePrice(double price, double lowRange, double highRange, double priceGranularity)
        {
            if (priceGranularity == 0)
                throw new ArgumentOutOfRangeException(nameof(priceGranularity));

            var maxLevel = GetMaxLevel(lowRange, highRange, priceGranularity);

            if (price < lowRange) return 0;
            if (price > highRange) return maxLevel;

            double value = (price - lowRange) / priceGranularity;
            int level = Convert.ToInt32(value);
            return level;
        }
    }
}
