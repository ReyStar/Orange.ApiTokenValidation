using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using EnsureThat;
using Microsoft.Extensions.Options;
using Orange.ApiTokenValidation.Common;

namespace Orange.ApiTokenValidation.Repositories
{
    internal class DataBaseFunctionalityValidator: IFunctionalityValidator
    {
        private readonly IDataSource _dataSource;
        private readonly ConnectionStrings _configuration;

        public DataBaseFunctionalityValidator(IDataSource dataSource, IOptions<ConnectionStrings> configuration)
        {
            _dataSource = dataSource;
            _configuration = configuration.Value;

            EnsureArg.IsNotEmptyOrWhiteSpace(_configuration.DefaultConnection);
        }

        public async Task EnsureValidationAsync(CancellationToken token)
        {
            try
            {
                //TODO check version on start
                _dataSource.Connection.Open();
                var version = await _dataSource.Connection.ExecuteScalarAsync<long>("Select \"Version\" from public.\"VersionInfo\" order by \"Version\" desc limit 1");

                if (version != _configuration.RequiredVersion)
                {
                    throw new FunctionalityException($"Request database version {_configuration.RequiredVersion}, but current {version}");
                }
            }
            catch (Exception ex) when(!(ex is FunctionalityException))
            {
                throw new FunctionalityException("Can't connect to database", ex);
            }
        }
    }
}
