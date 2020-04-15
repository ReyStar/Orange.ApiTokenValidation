using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenModel, Controllers.V1.DTO.TokenValidationRequest>()
                .ForPath(x => x.Token, memberOptions => memberOptions.MapFrom(p => p.Value));

            CreateMap<Controllers.V1.DTO.TokenValidationRequest, TokenModel>()
                .ForPath(x => x.Value, memberOptions => memberOptions.MapFrom(p => p.Token));

            CreateMap<Controllers.V1.DTO.TokenRequest, TokenDescriptor>()
                .ForPath(x => x.Audience, memberOptions => memberOptions.Ignore())
                .ForPath(x => x.Issuer, memberOptions => memberOptions.Ignore());

            CreateMap<TokenDescriptor, Controllers.V1.DTO.TokenResponse>();
            
            CreateMap<TokenValidationResult, Controllers.V1.DTO.TokenValidationResponse>();
        }
    }
}
