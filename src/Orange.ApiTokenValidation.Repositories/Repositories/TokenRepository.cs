using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Dapper.Dommel;
using Orange.ApiTokenValidation.Application.Exceptions;
using Orange.ApiTokenValidation.Application.Interfaces;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private readonly string _tokenTableUpdateScript;

        public TokenRepository(IDataSource dataSource, IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;

            var resourceLoader = new ResourceLoader(typeof(TokenRepository).Assembly);
            _tokenTableUpdateScript = resourceLoader.LoadString($"{typeof(TokenRepository).Namespace}.SQL.TokenTableUpdate.sql");
        }

        public async Task<TokenDescriptor> GetAsync(string issuer, string audience, CancellationToken cancellationToken = default)
        {
            try
            {
                var result =
                    await _dataSource.Connection.FirstOrDefaultAsync<TokenDbModel>(
                        x => x.Issuer == issuer
                             && x.Audience == audience);

                return _mapper.Map<TokenDescriptor>(result);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.GetOperationException, ex);
            }
        }

        public async Task<IEnumerable<TokenDescriptor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _dataSource.Connection.GetAllAsync<TokenDbModel>();

                return _mapper.Map<IEnumerable<TokenDescriptor>>(result);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.GetAllOperationException, ex);
            }
        }

        public async Task<bool> AddAsync(TokenDescriptor value, CancellationToken cancellationToken = default)
        {
            try
            {
                var dbModel = _mapper.Map<TokenDbModel>(value);
                UpdateDefaultProperties(dbModel);
                dbModel.CreatedTime = DateTimeOffset.UtcNow;
                dbModel.Creator = _dataSource.InstanceId;

                var key = await _dataSource.Connection.InsertAsync(dbModel);
                return key != null;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.AddOperationException, ex);
            }
        }

        public async Task<bool> AddOrUpdateAsync(string issuer, string audience, TokenDescriptor value, CancellationToken cancellationToken = default)
        {
            try
            {
                var dbModel = _mapper.Map<TokenDbModel>(value);
                UpdateDefaultProperties(dbModel);
                dbModel.Audience = audience;
                dbModel.Issuer = issuer;

                var commandDefinition = new CommandDefinition(_tokenTableUpdateScript,
                    parameters: new
                    {
                        issuerKey = dbModel.Issuer,
                        audienceKey = dbModel.Audience,
                        issuer = dbModel.Issuer,
                        audience = dbModel.Audience,
                        private_key = dbModel.PrivateKey,
                        ttl = dbModel.Ttl,
                        expiration_time = dbModel.ExpirationDate,
                        is_active = dbModel.IsActive,
                        payload = dbModel.PayLoad,
                        creator = dbModel.Creator,
                        created_time = dbModel.CreatedTime,
                        updated_time = DateTimeOffset.UtcNow,
                        updater = dbModel.Updater,
                    },
                    cancellationToken: cancellationToken);

                return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.AddOrUpdateOperationException, ex);
            }
        }

        /// <summary>
        /// Hard delete entity from DB. In other version can change to soft delete
        /// </summary>
        public async Task<bool> DeleteAsync(string issuer, string audience, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _dataSource.Connection.DeleteMultipleAsync<TokenDbModel>(x => x.Audience == audience
                                                                                           && x.Issuer == issuer);
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.DeleteOperationException, ex);
            }
        }

        private void UpdateDefaultProperties(TokenDbModel dbModel)
        {
            var currentTime = DateTimeOffset.UtcNow;
            dbModel.UpdatedTime = currentTime;
            dbModel.Updater = _dataSource.InstanceId;
            dbModel.CreatedTime = currentTime;
            dbModel.Creator = _dataSource.InstanceId;
        }
    }
}
