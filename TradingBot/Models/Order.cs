namespace TradingBot.Models
{
    public class Order : IOrder
    {
        private decimal _quantity;

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

        public decimal Quantity
        {
            get => _quantity; set => _quantity = value;
        }
    }
}
