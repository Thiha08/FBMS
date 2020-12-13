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
        Task RunAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity, IAggregateRoot;
    }
}
