using Hangfire;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ITransactionHostedService
    {
        Task StartAsync();

        Task StopAsync();

        Task<bool> IsRunning();

        [AutomaticRetry(Attempts = 0)]
        Task RecurringTransactionJob();
    }
}
