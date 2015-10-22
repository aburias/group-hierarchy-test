using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Models.DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BH.Data.Databases
{
    public class BHDbContext : IdentityDbContext<User>
    {
        public BHDbContext()
            : base("BHDbContext", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Group> Groups { get; set; }

        public static BHDbContext Create()
        {
            return new BHDbContext();
        }
    }
}
