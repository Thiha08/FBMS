using FBMS.Core.Dtos.Crawler;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ICrawlerService
    {
        Task CrawlAsync<T>(CrawlerRequestDto request) where T : BaseEntity, IAggregateRoot;
    }
}
