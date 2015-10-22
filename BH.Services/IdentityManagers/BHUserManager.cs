using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Data.Databases;
using BH.Data.Repository;
using BH.Models.DomainModels;
using BH.Services.DomainManagers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BH.Services.IdentityManagers
{
    public class BHUserManager : UserManager<User>
    {
        private Repository<User> _repo;
        private UserManager<User> _userManager = new UserManager<User>(new UserStore<User>(BHDbContext.Create()));

        public BHUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public User GetById(string id, List<string> childEntityes = null)
        {
            using (_repo = new Repository<User>(BHDbContext.Create()))
                return _repo.Find(u => u.Id == id, childEntityes).SingleOrDefault();
            //return _userManager.FindById(id);
        }

        public User UpdateUser(User user)
        {
            using (_repo = new Repository<User>(BHDbContext.Create()))
            {
                _repo.Update(user);
                return user;
            }
        }
    }
}
