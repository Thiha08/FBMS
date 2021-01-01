using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ITransactionHostedService
    {
        Task StartAsync();

        Task StopAsync();
    }
}
