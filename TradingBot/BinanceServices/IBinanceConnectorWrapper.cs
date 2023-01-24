namespace TradingBot.BinanceServices
{
    public interface IBinanceConnectorWrapper
    {
        Task<int> ListenToSingleStream(string stream, CancellationToken token);
    }
}
