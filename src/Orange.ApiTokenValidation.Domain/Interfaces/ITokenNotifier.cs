using Orange.ApiTokenValidation.Domain.NotifyMessages;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface ITokenNotifier
    {
        void Notify(TokenNotifyMessage message);
    }
}
