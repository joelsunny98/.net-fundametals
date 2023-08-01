using Microsoft.EntityFrameworkCore;
using RetailStore.Persistence;
using System.Linq.Expressions;

namespace RetailStore.Repository;

/// <summary>
/// Context of Repository
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly RetailStoreDbContext dbContext;
    private readonly DbSet<T> dbSet;

    /// <summary>
    /// Injects dependencies
    /// </summary>
    /// <param name="_dbContext"></param>
    public Repository(RetailStoreDbContext _dbContext)
    {
        this.dbContext = _dbContext;
        this.dbSet = _dbContext.Set<T>();
    }

    /// <summary>
    /// Method to Get All records
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    /// <summary>
    /// Method to Get record by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> GetById(int id)
    {
        return await dbSet.FindAsync(id); ;
    }

    /// <summary>
    /// Method to create
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<T> Create(T entity)
    {
        dbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Methd to Delete record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> Delete(int id)
    {
        var entity = await dbSet.FindAsync(id);
        dbSet.Remove(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Method to update record
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<T> Update(T entity)
    {
        var idProperty = typeof(T).GetProperty("Id");
        var entityId = idProperty?.GetValue(entity);
        var existingEntity = await dbSet.FindAsync(entityId);
        dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Method to find record
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }
}



