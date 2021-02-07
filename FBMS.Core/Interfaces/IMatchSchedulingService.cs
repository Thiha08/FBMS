using FBMS.Core.Constants;
using FBMS.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IMatchSchedulingService
    {
        Task<List<MatchDto>> GetMatchSchedule();

        Task<MatchDetailDto> GetMatchDetail(string matchUrl);

        Task<string> SubmitMatchTransaction(MatchBetDto dto);

        Task<string> GetMatchTransactionUrl(TransactionType transactionType, decimal pricing, List<MatchDto> matches);

        Task<string> GetMatchTransactionMmUrl(TransactionType transactionType, MatchDto match);
    }
}
