using Orange.ApiTokenValidation.Application.NotifyMessages;

namespace Orange.ApiTokenValidation.Application.Interfaces
{
    public interface ITokenNotifier
    {
        void Notify(TokenNotifyMessage message);
    }
}
