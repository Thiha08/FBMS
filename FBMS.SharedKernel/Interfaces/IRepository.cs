using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace FBMS.SharedKernel.Interfaces
{
    public interface IRepository
    {
        Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseEntity, IAggregateRoot;
        Task<List<TEntity>> ListAsync<TEntity>() where TEntity : BaseEntity, IAggregateRoot;
        Task<List<TEntity>> ListAsync<TEntity>(ISpecification<TEntity> spec) where TEntity : BaseEntity, IAggregateRoot;
        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot;
        Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot;
        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot;
    }
}
