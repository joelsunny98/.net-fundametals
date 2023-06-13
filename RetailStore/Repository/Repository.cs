﻿using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using RetailStore.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly RetailStoreDbContext dbContext;
    private readonly DbSet<T> dbSet;

    public Repository(RetailStoreDbContext _dbContext)
    {
        this.dbContext = _dbContext;
        this.dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T> GetById(int id)
    {
        var customer = await dbSet.FindAsync(id);
        if(customer == null)
            return null;
        else
        return customer;
    }

    public async Task<T> Create(T entity)
    {
        dbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Delete(int id)
    {
        var entity = await dbSet.FindAsync(id);
        if (entity == null)
            return null;
        dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> Update(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        var entityId = idProperty?.GetValue(entity);
        var existingEntity = await dbSet.FindAsync(entityId);
        dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
}


