using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    class TokenDbContextFactory: ITokenDbContextFactory
    {
        private readonly IOptions<ConnectionStrings> _connectionString;
        private readonly IOptions<CommonConfiguration> _commonConfiguration;

        public TokenDbContextFactory(IOptions<ConnectionStrings> connectionString,
                                     IOptions<CommonConfiguration> commonConfiguration)
        {
            _connectionString = connectionString;
            _commonConfiguration = commonConfiguration;
        }

        public TokenDbContext CreateDbContext()
        {
            var dbContext = new TokenDbContext(_connectionString, 
                                      _commonConfiguration);

            dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return dbContext;
        }
    }
}
