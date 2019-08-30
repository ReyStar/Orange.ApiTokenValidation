using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;

namespace Orange.ApiTokenValidation.API.Registration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenModel, Controllers.V1.DTO.TokenValidationRequest>();
        }
    }
}
