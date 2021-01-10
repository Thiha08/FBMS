using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Hangfire
{
    public interface IHangfireSettings
    {
        string TransactionJobIdentifier { get; set; }

        string SchedulingJobIdentifier { get; set; }

        string CronExpression { get; set; }
    }
}
