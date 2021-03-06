﻿using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Application.NotifyMessages
{
    internal class TokenAddedNotifyMessage : TokenNotifyMessage
    {
        public TokenAddedNotifyMessage(TokenDescriptor descriptor)
        {
            MessageType = TokenNotifyMessageType.Added;
            Descriptor = descriptor;
        }

        public TokenDescriptor Descriptor { get; }
    }
}
