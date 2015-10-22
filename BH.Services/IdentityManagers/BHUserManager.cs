using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Data.Repository;
using BH.Models.DomainModels;
using BH.Services.DomainManagers;
using Microsoft.AspNet.Identity;

namespace BH.Services.IdentityManagers
{
    public class BHUserManager : UserManager<User>
    {
        private Repository<User> _repo;

        public BHUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}
