using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.NotifyMessages
{
    class TokenUpdatedNotifyMessage : TokenNotifyMessage
    {
        public TokenUpdatedNotifyMessage(TokenDescriptor descriptor)
        {
            MessageType = TokenNotifyMessageType.Updated;
            Descriptor = descriptor;
        }

        public TokenDescriptor Descriptor { get; }
    }
}
