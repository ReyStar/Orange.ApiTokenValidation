﻿using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API.Registration
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenModel, Controllers.V1.DTO.TokenValidationRequest>()
                .ForPath(x => x.Token, memberOptions => memberOptions.MapFrom(p => p.Value));
        }
    }
}
