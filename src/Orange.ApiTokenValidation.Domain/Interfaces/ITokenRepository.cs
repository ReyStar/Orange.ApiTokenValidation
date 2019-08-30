using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> AddAsync(TokenDescriptor value, CancellationToken cancellationToken =  default(CancellationToken));
        Task<bool> UpdateAsync(string issuer, string audience, TokenDescriptor value, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteAsync(string issuer, string audience, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<TokenDescriptor>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<TokenDescriptor> GetAsync(string issuer, string audience, CancellationToken cancellationToken = default(CancellationToken));
    }
}
