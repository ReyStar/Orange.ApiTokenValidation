using AutoMapper;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.Models;

namespace Orange.ApiTokenValidation.Repositories
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenDbModel, TokenDescriptor>();
            CreateMap<TokenDescriptor, TokenDbModel>()
                .ForMember(p => p.CreatedTime, x => x.Ignore())
                .ForMember(p => p.Creator, x => x.Ignore())
                .ForMember(p => p.UpdatedTime, x => x.Ignore())
                .ForMember(p => p.Updater, x => x.Ignore());
        }
    }
}
