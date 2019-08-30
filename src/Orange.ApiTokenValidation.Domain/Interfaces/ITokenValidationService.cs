using System.Threading.Tasks;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface ITokenValidationService
    {
        Task<AuthenticationResult> Validate(string audience, string token);
    }
}
