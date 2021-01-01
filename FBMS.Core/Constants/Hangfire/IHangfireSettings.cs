using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants.Hangfire
{
    public interface IHangfireSettings
    {
        string RecurringJobIdentifier { get; set; }

        string CronExpression { get; set; }
    }
}
