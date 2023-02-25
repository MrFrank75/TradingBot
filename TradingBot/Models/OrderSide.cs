namespace TradingBot.Models
{
    public struct OrderSide
    {
        private OrderSide(string value)
        {
            this.Value = value;
        }

        public static OrderSide LONG { get => new OrderSide("LONG"); }
        public static OrderSide SHORT { get => new OrderSide("SHORT"); }

        public string Value { get; private set; }

        public static implicit operator string(OrderSide enm) => enm.Value;

        public override string ToString() => this.Value.ToString();
    }
}
