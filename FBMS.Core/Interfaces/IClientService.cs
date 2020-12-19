using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IClientService
    {
        Task CrawlAsync();

        Task<List<Client>> ListAsync();

        Task DeleteAllAsync();
    }
}
