using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IMemberService
    {
        Task<MemberDto> GetMember(int memberId);

        Task<MemberDto> GetMember(string userName);

        Task<MemberTransactionTemplateDto> GetMemberWithTransactionTemplate(int memberId);

        Task UpdateMemberWithTransactionTemplate(MemberTransactionTemplateDto dto);

        Task<List<MemberDto>> GetMembers();

        Task<List<MemberDto>> GetMembers(MemberFilterDto filterDto);

        Task EnableMember(int memberId);

        Task DisableMember(int memberId);

        Task EnableMembers(MemberFilterDto filterDto);

        Task DisableMembers(MemberFilterDto filterDto);

        Task DeleteMember(int memberId);

        Task DeleteMembers(MemberFilterDto filterDto);

        Task CrawlMembers();

        Task<List<int>> CrawlActiveMembers();
    }
}
