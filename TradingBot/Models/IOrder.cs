namespace TradingBot.Models
{
    public interface IOrder
    {
        OrderSide Side { get; }
        decimal Quantity { get; }
    }
}
