using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Scheduler
{
    public interface ICrawlerScheduler
    {
        long RetryTime { get; set; }

        Task Schedule();
    }
}
