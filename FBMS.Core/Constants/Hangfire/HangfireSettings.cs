using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Hangfire
{
    public class HangfireSettings : IHangfireSettings
    {
        public string TransactionJobIdentifier { get; set; }

        public string SchedulingJobIdentifier { get; set; }

        public string CronExpression { get; set; }
    }
}
