using System.Linq.Expressions;

namespace RetailStore.Repository;

/// <summary>
/// Interface for Repostitory
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T>
{
    /// <summary>
    /// Method to Get all
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Method to Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetById(int id);

    /// <summary>
    /// Method to create
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> Create(T entity);

    /// <summary>
    /// Method to Delete record
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> Delete(int id);

    /// <summary>
    /// Method to Update record
    /// </summary>
    /// <param name="Entity"></param>
    /// <returns></returns>
    Task<T> Update(T Entity);

    /// <summary>
    /// Method to find
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
