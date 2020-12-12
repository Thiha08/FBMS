using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Pipeline
{
    public interface ICrawlerPipeline
    {
        Task RunAsync<T>(IEnumerable<T> entities) where T : BaseEntity, IAggregateRoot;
    }
}
