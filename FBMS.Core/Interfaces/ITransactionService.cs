using FBMS.Core.Dtos.Crawler;
using FBMS.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ITransactionService
    {
        Task CrawlAsync(int clientId);

        Task CrawlAsync();

        Task<List<Transaction>> ListAsync(int clientId);

        Task<List<Transaction>> ListAsync();

        Task DeleteAllAsync(int clientId);

        Task DeleteAllAsync();

    }
}
