namespace TradingBot.Controllers
{
    public class TradingViewMessage
    {
        public string AssetId { get; set; }
        public string UserId { get; set; }
        public string TradeAction { get; set; }
        public string Price { get; set; }
    }
}
