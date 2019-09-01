﻿using System.Threading;
using System.Threading.Tasks;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.Domain.Interfaces
{
    public interface ITokenValidationService
    {
        Task<TokenValidationResult> ValidateAsync(string audience, string token, CancellationToken cancellationToken = default);
    }
}