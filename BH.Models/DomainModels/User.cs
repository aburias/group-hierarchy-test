using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BH.Models.DomainModels
{
    public class User : IdentityUser
    {
        public User()
        {
            Groups = new List<Group>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [InverseProperty("Users")]
        public ICollection<Group> Groups { get; set; }
    }
}
