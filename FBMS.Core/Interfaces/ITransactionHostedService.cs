using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ITransactionHostedService
    {
        Task StartAsync();

        Task StopAsync();

        Task<bool> IsRunning();
    }
}
