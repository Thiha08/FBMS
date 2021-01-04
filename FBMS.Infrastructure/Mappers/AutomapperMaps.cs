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
            CreateMap<Member, MemberCto>().ReverseMap();
            CreateMap<Member, MemberDto>().ReverseMap();

            CreateMap<TransactionFilterDto, TransactionFilter>().ReverseMap();
            CreateMap<Transaction, TransactionCto>()
                .ForMember(cto => cto.TransactionDate, x => x.Ignore())
                .ForMember(cto => cto.TransactionType, x => x.Ignore())
                .ForMember(cto => cto.Amount, x => x.Ignore())
                .ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}
