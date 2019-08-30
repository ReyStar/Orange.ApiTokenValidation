using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories.Registration
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenDbModel, TokenDescriptor>();
            CreateMap<TokenDescriptor, TokenDbModel>();
        }
    }
}
