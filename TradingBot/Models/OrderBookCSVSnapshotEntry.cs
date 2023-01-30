namespace TradingBot.Models
{
    public class OrderBookCSVSnapshotEntry
    {
        public string GenerationId { get; set; }
        public double PriceLevel { get; set; }
        public double Quantity { get; set; }
    }
}
