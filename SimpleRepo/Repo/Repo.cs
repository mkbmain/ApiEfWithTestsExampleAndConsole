using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace SimpleRepo.Repo
{
    public class Repo<db> : IRepo<db> where db : DbContext
    {
        private readonly db _dbContext;

        public Repo(db context)
        {
            _dbContext = context;
        }
 
        public Task<List<T>> GetAll<T>() where T : class
        {
            return GetAll<T>(t => t != null);
        }
        public Task<List<T>> GetAll<T>(Expression<Func<T, bool>> func) where T : class
        {
            return GetAll<T,T>(func, t => t);
        }
        public Task<List<Tout>> GetAll<T, Tout>(Expression<Func<T, bool>> func,
            Expression<Func<T, Tout>> projection) where T : class
        {
            return _dbContext.Set<T>().Where(func).Select(projection).ToListAsync();
        }

        public Task<Tout> Get<T, Tout>(Expression<Func<T, bool>> func, Expression<Func<T, Tout>> projection)
            where T : class
        {
            return _dbContext.Set<T>().Where(func).Select(projection).FirstOrDefaultAsync();
        }

        public Task Add<T>(T item) where T : class
        {
            _dbContext.Set<T>().Add(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task Update<T>(T item) where T : class
        {
            _dbContext.Set<T>().Update(item);
            return _dbContext.SaveChangesAsync();
        }

        public Task Delete<T>(T item) where T : class
        {
            _dbContext.Set<T>().Remove(item);
            return _dbContext.SaveChangesAsync();
        }
    }
}