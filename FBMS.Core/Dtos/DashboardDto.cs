using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos
{
    public class DashboardDto
    {
        public bool IsRunningTransactionJob { get; set; }

        public bool IsRunningSchedulingJob { get; set; }
    }
}
