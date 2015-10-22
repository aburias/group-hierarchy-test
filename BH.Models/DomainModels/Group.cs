using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Models.Interfaces;

namespace BH.Models.DomainModels
{
    public class Group : IEntity
    {
        public Group()
        {
            ChildGroups = new List<Group>();
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? TopGroupId { get; set; }
        [ForeignKey("ParentGroup")]
        public int? ParentGroupId { get; set; }
        [InverseProperty("ChildGroups")]
        public Group ParentGroup { get; set; }
        [InverseProperty("ParentGroup")]
        public ICollection<Group> ChildGroups { get; set; }
        [InverseProperty("Groups")]
        public ICollection<User> Users { get; set; }
    }
}
