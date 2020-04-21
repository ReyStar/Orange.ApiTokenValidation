using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Application.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> AddAsync(TokenDescriptor value, CancellationToken cancellationToken =  default);
        Task<bool> AddOrUpdateAsync(string issuer, string audience, TokenDescriptor value, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string issuer, string audience, CancellationToken cancellationToken = default);
        Task<IEnumerable<TokenDescriptor>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TokenDescriptor> GetAsync(string issuer, string audience, CancellationToken cancellationToken = default);
    }
}
