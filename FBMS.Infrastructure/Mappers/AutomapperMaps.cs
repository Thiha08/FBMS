using AutoMapper;
using FBMS.Core.Ctos;
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
            CreateMap<MemberFilterDto, MemberFilter>().ReverseMap();

            CreateMap<Member, MemberDto>().ReverseMap();
            CreateMap<Member, MemberCto>().ReverseMap();

            CreateMap<Transaction, TransactionCto>().ReverseMap();
        }
    }
}
