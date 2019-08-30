using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private readonly string _tokenTableInsertScript;
        private readonly string _tokenTableSelectScript;
        private readonly string _tokenTableUpdateScript;
        private readonly string _tokenTableDeleteScript;
        private readonly string _tokenTableSelectByIssuerAndAudienceScript;

        public TokenRepository(IDataSource dataSource, IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;

            _tokenTableInsertScript = SqlScriptLoader.Load("TokenTableInsert");
            _tokenTableSelectScript = SqlScriptLoader.Load("TokenTableSelect");
            _tokenTableSelectByIssuerAndAudienceScript = SqlScriptLoader.Load("TokenTableSelectByIssuerAndAudience");
            _tokenTableUpdateScript = SqlScriptLoader.Load("TokenTableUpdate");
            _tokenTableDeleteScript = SqlScriptLoader.Load("TokenTableDelete");
        }

        public async Task<TokenDescriptor> GetAsync(string issuer, string audience, CancellationToken cancellationToken = default(CancellationToken))
        {
            var commandDefinition = new CommandDefinition(_tokenTableSelectByIssuerAndAudienceScript,
                parameters: new
                {
                    issuer = issuer,
                    audience = audience
                }, cancellationToken: cancellationToken);

            var result = await _dataSource.Connection.QueryFirstOrDefaultAsync<TokenDbModel>(commandDefinition);

            return _mapper.Map<TokenDescriptor>(result);
        }

        public async Task<IEnumerable<TokenDescriptor>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var commandDefinition = new CommandDefinition(_tokenTableSelectScript, cancellationToken: cancellationToken);

            var result = await _dataSource.Connection.QueryAsync<TokenDbModel>(commandDefinition);

            return _mapper.Map<IEnumerable<TokenDescriptor>>(result);
        }

        public async Task<bool> AddAsync(TokenDescriptor value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dbModel = _mapper.Map<TokenDbModel>(value);

            var commandDefinition = new CommandDefinition(_tokenTableInsertScript,
                parameters: new
                {
                    issuer = dbModel.Issuer,
                    audience = dbModel.Audience,
                    private_key = dbModel.PrivateKey,
                    ttl = dbModel.Ttl,
                    expiration_time = dbModel.ExpirationDate,
                    is_active = dbModel.IsActive,
                    payload = dbModel.PayLoad,

                }, cancellationToken: cancellationToken);

            return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
        }

        public async Task<bool> UpdateAsync(string issuer, string audience, TokenDescriptor value, CancellationToken cancellationToken = default(CancellationToken))
        {
            var dbModel = _mapper.Map<TokenDbModel>(value);

            var commandDefinition = new CommandDefinition(_tokenTableUpdateScript,
                parameters: new
                {
                    issuerKey = issuer,
                    audienceKey = audience,
                    issuer = dbModel.Issuer,
                    audience = dbModel.Audience,
                    private_key = dbModel.PrivateKey,
                    ttl = dbModel.Ttl,
                    expiration_time = dbModel.ExpirationDate,
                    is_active = dbModel.IsActive,
                    payload = dbModel.PayLoad,
                }, cancellationToken: cancellationToken);

            return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
        }

        public async Task<bool> DeleteAsync(string issuer, string audience, CancellationToken cancellationToken = default(CancellationToken))
        {
            var commandDefinition = new CommandDefinition(_tokenTableDeleteScript, new { issuer = issuer, audience = audience }, cancellationToken: cancellationToken);

            return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
        }
    }
}
