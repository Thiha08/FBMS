using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Hangfire
{
    public class HangfireSettings : IHangfireSettings
    {
        public string RecurringJobIdentifier { get; set; }

        public string CronExpression { get; set; }
    }
}
