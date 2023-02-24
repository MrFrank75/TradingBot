namespace TradingBot.Models
{
    public struct OrderSide
    {
        private OrderSide(string value)
        {
            this.Value = value;
        }

        public static OrderSide BUY { get => new OrderSide("BUY"); }
        public static OrderSide SELL { get => new OrderSide("SELL"); }

        public string Value { get; private set; }

        public static implicit operator string(OrderSide enm) => enm.Value;

        public override string ToString() => this.Value.ToString();
    }
}
