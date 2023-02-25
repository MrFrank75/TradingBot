namespace TradingBot.TradingServices
{
    public class BrokerConnectorReturnErrorCode
    {
        private BrokerConnectorReturnErrorCode(int value)
        {
            this.Value = value;
        }

        public static BrokerConnectorReturnErrorCode SUCCESS { get => new BrokerConnectorReturnErrorCode(0); }
        public static BrokerConnectorReturnErrorCode FAIL { get => new BrokerConnectorReturnErrorCode(1); }
        private int Value { get;  set; }

    }
}
