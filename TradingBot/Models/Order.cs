namespace TradingBot.Models
{
    public class Order : IOrder
    {
        public string side { get; set; }

        public OrderSide Side
        {
            get
            {
                if (side.ToUpperInvariant() == OrderSide.LONG.ToString())
                    return OrderSide.LONG;
                if (side.ToUpperInvariant() == OrderSide.SHORT.ToString())
                    return OrderSide.SHORT;
                throw new NotSupportedException();
            }
        }
    }
}
