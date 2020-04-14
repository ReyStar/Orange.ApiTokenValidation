using AutoMapper;
using Newtonsoft.Json.Linq;
using Orange.ApiTokenValidation.Domain.Models;
using Orange.ApiTokenValidation.Repositories.EntityFramework.Models;

namespace Orange.ApiTokenValidation.Repositories.EntityFramework
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TokenDbModel, TokenDescriptor>()
                .ForMember(p => p.PayLoad, x => x.Ignore())
                .AfterMap((m, d) => { m.PayLoad = m.PayLoad?.ToString(); });

            CreateMap<TokenDescriptor, TokenDbModel>()
                .ForMember(p => p.CreatedTime, x => x.Ignore())
                .ForMember(p => p.Creator, x => x.Ignore())
                .ForMember(p => p.UpdatedTime, x => x.Ignore())
                .ForMember(p => p.Updater, x => x.Ignore())
                .ForMember(p => p.PayLoad, x => x.Ignore())
                .AfterMap((d, m) => { d.PayLoad = !string.IsNullOrWhiteSpace(m.PayLoad) ? JObject.Parse(m.PayLoad) : null; });
        }
    }
}
