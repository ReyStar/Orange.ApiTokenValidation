namespace Orange.ApiTokenValidation.Domain.NotifyMessages
{
    public abstract class TokenNotifyMessage
    {
        public TokenNotifyMessageType MessageType { get; protected set; }
    }
}
