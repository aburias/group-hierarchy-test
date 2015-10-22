using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Models.Interfaces;

namespace BH.Data.Interfaces
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> Find(Func<TEntity, bool> query, List<string> childEntities = null);
        IQueryable<TEntity> GetAll(List<string> childEntities = null);
        TEntity Create(TEntity entity);
        TEntity Update(TEntity entity);
        bool Delete(int id);
    }
}
