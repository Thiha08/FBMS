using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ISchedulingHostedService
    {
        Task StartAsync();

        Task StopAsync();

        Task<bool> IsRunning();

        Task RecurringTransactionJob();
    }
}
