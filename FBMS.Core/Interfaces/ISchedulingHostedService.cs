using Hangfire;
using System.Threading.Tasks;

namespace FBMS.Core.Interfaces
{
    public interface ISchedulingHostedService
    {
        Task StartAsync();

        Task StopAsync();

        Task<bool> IsRunning();

        [AutomaticRetry(Attempts = 0)]
        //[AutomaticRetry(OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        Task RecurringTransactionJob();
    }
}
