using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Orange.ApiTokenValidation.Common;
using Orange.ApiTokenValidation.Domain.Exceptions;
using Orange.ApiTokenValidation.Domain.Interfaces;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.EntityFramework.Models;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework.Repositories
{
    internal class TokenRepository : ITokenRepository
    {
        private readonly ITokenDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;
        private readonly string _tokenTableUpdateScript;

        public TokenRepository(ITokenDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            
            var resourceLoader = new ResourceLoader(typeof(TokenRepository).Assembly);
            _tokenTableUpdateScript = resourceLoader.LoadString($"{typeof(TokenRepository).Namespace}.SQL.TokenTableUpdate.sql");
        }

        public async Task<bool> AddAsync(TokenDescriptor value, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();
                
                var dbModel = _mapper.Map<TokenDbModel>(value);
                UpdateDefaultProperties(dbModel, dbContext);
                dbModel.CreatedTime = DateTimeOffset.UtcNow;
                dbModel.Creator = dbContext.InstanceId;

                await dbContext.Tokens.AddAsync(dbModel, cancellationToken);

                return await dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.AddOperationException, ex);
            }
        }

        public async Task<bool> AddOrUpdateAsync(string issuer, string audience, TokenDescriptor value,
                                                 CancellationToken cancellationToken = default)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                try
                {
                    var dbModel = _mapper.Map<TokenDbModel>(value);
                    dbModel.Issuer = issuer;
                    dbModel.Audience = audience;

                    UpdateDefaultProperties(dbModel, dbContext);

                    await using (var sqlCommand = new NpgsqlCommand(_tokenTableUpdateScript))
                    {
                        await using var connection = (NpgsqlConnection) dbContext.Database.GetDbConnection();
                        await connection.OpenAsync(cancellationToken);
                        sqlCommand.Connection = connection;

                        sqlCommand.Parameters.AddWithValue("@issuer", NpgsqlDbType.Varchar, dbModel.Issuer);
                        sqlCommand.Parameters.AddWithValue("@audience", NpgsqlDbType.Varchar, dbModel.Audience);
                        sqlCommand.Parameters.AddWithValue("@private_key", NpgsqlDbType.Varchar, dbModel.PrivateKey);
                        sqlCommand.Parameters.AddWithValue("@ttl", NpgsqlDbType.Integer, dbModel.Ttl);
                        sqlCommand.Parameters.AddWithValue("@expiration_time", NpgsqlDbType.TimestampTz, dbModel.ExpirationDate);
                        sqlCommand.Parameters.AddWithValue("@is_active", NpgsqlDbType.Boolean, dbModel.IsActive);

                        if (value.PayLoad != null)
                        {
                            sqlCommand.Parameters.AddWithValue("@payload", NpgsqlDbType.Jsonb, dbModel.PayLoad);
                        }
                        else
                        {
                            sqlCommand.Parameters.AddWithValue("@payload", NpgsqlDbType.Jsonb, DBNull.Value);
                        }

                        sqlCommand.Parameters.AddWithValue("@created_time", NpgsqlDbType.TimestampTz, dbModel.CreatedTime);
                        sqlCommand.Parameters.AddWithValue("@creator", NpgsqlDbType.Varchar, dbModel.Creator);
                        sqlCommand.Parameters.AddWithValue("@updated_time", NpgsqlDbType.TimestampTz, dbModel.UpdatedTime);
                        sqlCommand.Parameters.AddWithValue("@updater", NpgsqlDbType.Varchar, dbModel.Updater);
                        
                        return await sqlCommand.ExecuteNonQueryAsync(cancellationToken) > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new RepositoryException(I18n.AddOrUpdateOperationException, ex);
                }
            }
        }

        public async Task<bool> DeleteAsync(string issuer, string audience,
                                            CancellationToken cancellationToken = default)
        {
            try
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var instance = new TokenDbModel
                    {
                        Issuer = issuer,
                        Audience = audience
                    };

                    dbContext.Set<TokenDbModel>().Attach(instance);
                    dbContext.Set<TokenDbModel>().Remove(instance);

                    return await dbContext.SaveChangesAsync(cancellationToken) > 0;
                }
            }
            catch (DbUpdateConcurrencyException ex) when (ex.Data.Count == 0) //Row was already deleted or not exist
            {
                return false; 
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.DeleteOperationException, ex);
            }
        }

        public async Task<IEnumerable<TokenDescriptor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var result = await dbContext.Tokens.ToListAsync(cancellationToken);

                    return _mapper.Map<IEnumerable<TokenDescriptor>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.GetAllOperationException, ex);
            }
        }

        public async Task<TokenDescriptor> GetAsync(string issuer, string audience,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                using (var dbContext = _dbContextFactory.CreateDbContext())
                {
                    var result = await dbContext.Tokens.FirstOrDefaultAsync(
                        x => x.Audience == audience && x.Issuer == issuer, cancellationToken: cancellationToken);

                    return _mapper.Map<TokenDescriptor>(result);
                }
            }
            catch (Exception ex)
            {
                throw new RepositoryException(I18n.GetOperationException, ex);
            }
        }

        private void UpdateDefaultProperties(TokenDbModel dbModel, TokenDbContext dbContext)
        {
            var currentTime = DateTimeOffset.UtcNow;
            dbModel.UpdatedTime = currentTime;
            dbModel.Updater = dbContext.InstanceId;
            dbModel.CreatedTime = currentTime;
            dbModel.Creator = dbContext.InstanceId;
        }
    }
}
