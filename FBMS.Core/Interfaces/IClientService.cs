using FBMS.Core.Dtos;
using FBMS.Core.Dtos.Filters;
using FBMS.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto> GetClient(int clientId);

        Task<ClientDto> GetClient(string accountName);

        Task<List<ClientDto>> GetClients();

        Task<List<ClientDto>> GetClients(ClientFilterDto filterDto);

        Task EnableClient(int clientId);

        Task DisableClient(int clientId);

        Task DeleteClient(int clientId);

        Task DeleteClients(ClientFilterDto filterDto);

        Task CrawlClients();
    }
}
