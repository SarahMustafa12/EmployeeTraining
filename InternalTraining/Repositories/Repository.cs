using InternalTraining.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using InternalTraining.Repositories.IRepository;


namespace InternalTraining.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private ApplicationDbContext dbContext;
        private readonly DbSet<T> dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        public void Create(T entity)
        {
            dbSet.Add(entity);
            Commit();

        }
        public void Create(List<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            Commit();
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            Commit();
        }
        public void Delete(List<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query.ToList();
        }
        public T? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            return Get(filter, includes, tracked).FirstOrDefault();

        }
    }
}
