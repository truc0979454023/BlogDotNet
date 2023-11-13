using Blog.Core;
using Blog.Core.SeedWords;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog.Data.SeedWords
{
    public class RepositotyBase<T, Key> : IRepository<T, Key> where T : class
    {

        private readonly DbSet<T> _dbSet;

        protected readonly BlogContext _context;

        public RepositotyBase(BlogContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public void Add(T entity)
        {
            _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public Task<T> GetById(Key id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(Key id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}