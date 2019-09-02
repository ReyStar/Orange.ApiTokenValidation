using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API.Registration
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenModel, Controllers.V1.DTO.TokenValidationRequest>()
                .ForPath(x => x.Token, memberOptions => memberOptions.MapFrom(p => p.Value));

            CreateMap<Controllers.V1.DTO.TokenValidationRequest, TokenModel>()
                .ForPath(x => x.Value, memberOptions => memberOptions.MapFrom(p => p.Token));
        }
    }
}
