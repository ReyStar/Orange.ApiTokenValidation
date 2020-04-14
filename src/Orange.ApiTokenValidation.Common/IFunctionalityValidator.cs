using System.Threading;
using System.Threading.Tasks;

namespace Orange.ApiTokenValidation.Common
{
    public interface IFunctionalityValidator
    {
        Task EnsureValidationAsync(CancellationToken token = default);
    }
}
