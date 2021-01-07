using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity GetById<TEntity>(int id) where TEntity : BaseEntity, IAggregateRoot
        {
            return _dbContext.Set<TEntity>().SingleOrDefault(e => e.Id == id);
        }

        public Task<TEntity> GetByIdAsync<TEntity>(int id) where TEntity : BaseEntity, IAggregateRoot
        {
            return _dbContext.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> GetBySpecificationAsync<TEntity>(ISpecification<TEntity> specification) where TEntity : BaseEntity, IAggregateRoot
        {
            return (await ListAsync(specification)).FirstOrDefault();
        }

        public Task<List<TEntity>> ListAsync<TEntity>() where TEntity : BaseEntity, IAggregateRoot
        {
            return _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> ListAsync<TEntity>(ISpecification<TEntity> spec) where TEntity : BaseEntity, IAggregateRoot
        {
            var specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : BaseEntity, IAggregateRoot
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<TEntity> ApplySpecification<TEntity>(ISpecification<TEntity> spec) where TEntity : BaseEntity
        {
            var evaluator = new SpecificationEvaluator<TEntity>();
            return evaluator.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
        }
    }
}
