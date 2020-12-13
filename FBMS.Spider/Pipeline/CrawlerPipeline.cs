using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Pipeline
{
    public class CrawlerPipeline : ICrawlerPipeline
    {
        private readonly IRepository _repository;

        public CrawlerPipeline(IRepository repository)
        {
            _repository = repository;
        }

        public async Task RunAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity, IAggregateRoot
        {
            foreach (var entity in entities)
            {
                await _repository.AddAsync(entity);
            }
        }
    }
}
