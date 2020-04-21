namespace Orange.ApiTokenValidation.Application.NotifyMessages
{
    public abstract class TokenNotifyMessage
    {
        public TokenNotifyMessageType MessageType { get; protected set; }
    }
}
