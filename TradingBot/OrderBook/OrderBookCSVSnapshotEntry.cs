﻿namespace TradingBot.OrderBook
{
    public class OrderBookCSVSnapshotEntry
    {
        public string GenerationUtcDateTime { get; set; }
        public double PriceLevel { get; set; }
        public double Quantity { get; set; }
    }
}
