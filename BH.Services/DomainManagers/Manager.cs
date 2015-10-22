using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Data.Repository;
using BH.Models.Interfaces;

namespace BH.Services.DomainManagers
{
    public class Manager<TEntity> where TEntity : class
    {
        protected Repository<TEntity> _repo;
    }
}
