using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SimpleRepo.Repo;

public interface IRepo<db> where db : DbContext
{
    Task Update<T>(T item) where T : class;
    Task<List<T>> GetAll<T>(Expression<Func<T, bool>> func) where T : class;
    Task<List<T>> GetAll<T>() where T : class;

    Task<List<Tout>> GetAll<T, Tout>(Expression<Func<T, bool>> func,
        Expression<Func<T, Tout>> projection) where T : class;

    Task<Tout> Get<T, Tout>(Expression<Func<T, bool>> func, Expression<Func<T, Tout>> projection)
        where T : class;

    Task Add<T>(T item) where T : class;
    

    Task Delete<T>(T item) where T : class;
}