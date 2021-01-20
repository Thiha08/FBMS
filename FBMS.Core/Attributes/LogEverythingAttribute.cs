using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Logging;
using Hangfire.Storage;

namespace FBMS.Core.Attributes
{
    public class LogEverythingAttribute : JobFilterAttribute,
        IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public void OnCreating(CreatingContext filterContext)
        {
            Logger.InfoFormat(
                "Creating a job based on method `{0}`...",
                filterContext.Job.Method.Name);
        }

        public void OnCreated(CreatedContext filterContext)
        {
            Logger.InfoFormat(
                "Job that is based on method `{0}` has been created with id `{1}`",
                filterContext.Job.Method.Name,
                filterContext.BackgroundJob.Id);
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            Logger.InfoFormat(
                "Starting to perform job `{0}`",
                filterContext.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            Logger.InfoFormat(
                "Job `{0}` has been performed",
                filterContext.BackgroundJob.Id);
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                Logger.WarnFormat(
                    "Job `{0}` has been failed due to exception `{1}` but will be retried automatically until retry attempts exceeded",
                    context.BackgroundJob.Id,
                    failedState.Exception);
            }
        }

        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state was changed from `{1}` to `{2}`",
                context.BackgroundJob.Id,
                context.OldStateName,
                context.NewState.Name);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            Logger.InfoFormat(
                "Job `{0}` state `{1}` was unapplied.",
                context.BackgroundJob.Id,
                context.OldStateName);
        }
    }
}
