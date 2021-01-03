using AutoMapper;
using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Entities;
using FBMS.Core.Specifications.Filters;

namespace FBMS.Infrastructure.Mappers
{
    public class AutomapperMaps : Profile
    {
        public AutomapperMaps()
        {
            CreateMap<BaseFilterDto, BaseFilter>().IncludeAllDerived().ReverseMap();
            CreateMap<ClientFilterDto, ClientFilter>().ReverseMap();

            CreateMap<Client, ClientDto>();
        }
    }
}
