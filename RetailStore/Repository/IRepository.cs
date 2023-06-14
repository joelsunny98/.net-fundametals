using System.Linq.Expressions;

namespace RetailStore.Repository;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Create(T entity);
    Task<T> Delete(int id);
    Task<T> Update(T Entity);
    Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
