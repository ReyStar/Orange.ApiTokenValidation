namespace Orange.ApiTokenValidation.Application.NotifyMessages
{
    class TokenRemovedNotifyMessage : TokenNotifyMessage
    {
        public TokenRemovedNotifyMessage(string issuer, string audience)
        {
            Issuer = issuer;
            Audience = audience;
            MessageType = TokenNotifyMessageType.Removed;
        }
        
        public string Issuer { get; }
        
        public string Audience { get; }
    }
}
