using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    internal class DataBaseFunctionalityValidator : IFunctionalityValidator
    {
        private readonly ITokenDbContextFactory _dbContextFactory;
        private readonly ConnectionStrings _configuration;

        public DataBaseFunctionalityValidator(ITokenDbContextFactory dbContextFactory,
            IOptions<ConnectionStrings> configuration)
        {
            _dbContextFactory = dbContextFactory;
            EnsureArg.IsNotEmptyOrWhiteSpace(_configuration.DefaultConnection);
            _configuration = configuration.Value;
        }

        public async Task EnsureValidationAsync(CancellationToken token)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var versionInfo = await dbContext.Versions.OrderByDescending(x => x.Version)
                                                          .FirstAsync(cancellationToken: token);
                    if (versionInfo.Version != _configuration.RequiredVersion)
                    {
                        throw new FunctionalityException($"Request database version {_configuration.RequiredVersion}, but current {versionInfo.Version}");
                    }
                }
                catch (Exception ex) when (!(ex is FunctionalityException))
                {
                    throw new FunctionalityException("Can't connect to database", ex);
                }
            }
        }
    }
}
