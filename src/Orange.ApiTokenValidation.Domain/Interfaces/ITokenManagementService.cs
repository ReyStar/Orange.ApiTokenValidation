using System.Threading;
using System.Threading.Tasks;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface ITokenManagementService
    {
        Task<TokenDescriptor> GetTokenAsync(string issuer, string audience, CancellationToken cancellationToken = default);
        Task AddTokenAsync(TokenDescriptor tokenDescriptor, CancellationToken cancellationToken = default);
        Task AddOrUpdateTokenAsync(TokenDescriptor tokenDescriptor, CancellationToken cancellationToken = default);
        Task<bool> RemoveTokenAsync(string issuer, string audience, CancellationToken cancellationToken = default);
    }
}
