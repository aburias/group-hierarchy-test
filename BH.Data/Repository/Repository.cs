using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Data.Interfaces;
using BH.Models.Interfaces;

namespace BH.Data.Repository
{
    public class Repository<TEntity> : IDisposable, IRepository<TEntity> where TEntity : class, IEntity
    {
        private DbContext _context;
        private DbSet<TEntity> _set; 
        public Repository(DbContext context)
        {
            _context = context;
            _set = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> Find(Func<TEntity, bool> query, List<string> childEntities = null)
        {
            var set = _set.AsQueryable();
            if(childEntities != null)
            {
                foreach (var childEntity in childEntities)
                    set = _set.Include(childEntity);
            }
            return set.Where(query).AsQueryable();
        }

        public IQueryable<TEntity> GetAll(List<string> childEntities = null)
        {
            var set = _set.AsQueryable();
            if (childEntities != null)
            {
                foreach (var childEntity in childEntities)
                    set = _set.Include(childEntity);
            }
            return set;
        }

        public TEntity GetById(int id, List<string> childEntities = null)
        {
            var set = _set.AsQueryable();
            if (childEntities != null)
            {
                foreach (var childEntity in childEntities)
                    set = _set.Include(childEntity);
            }
            return set.SingleOrDefault(e => e.Id == id);
        }

        public TEntity Create(TEntity entity)
        {
            _set.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public bool Delete(int id)
        {
            _set.Remove(GetById(id));
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
                _set = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
