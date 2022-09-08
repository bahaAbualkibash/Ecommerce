﻿using ECommerce.Data;
using ECommerce.Models;
using ECommerce.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Repo
{
    public class GenaricRepo<T> : IGenaricRepo<T> where T: BaseEntity
    {
        private readonly StoreContext storeContext;

        public GenaricRepo(StoreContext storeContext)
        {
            this.storeContext = storeContext;
        }
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await storeContext.Set<T>().ToListAsync();
        }
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await storeContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(storeContext.Set<T>().AsQueryable(), spec);
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public  void Add(T entity)
        {
            storeContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            storeContext.Set<T>().Attach(entity);
            storeContext.Entry(entity).State = EntityState.Modified;

        }

        public void Delete(T entity)
        {
            storeContext.Set<T>().Remove(entity);

        }

        public void Detach(T entity)
        {
            storeContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
