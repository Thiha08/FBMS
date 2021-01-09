using FBMS.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IMatchSchedulingService
    {
        Task<List<MatchDto>> GetMatchSchedule();
    }
}
