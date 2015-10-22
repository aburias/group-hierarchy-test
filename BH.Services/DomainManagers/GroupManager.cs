using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Data.Databases;
using BH.Data.Interfaces;
using BH.Data.Repository;
using BH.Models.DomainModels;

namespace BH.Services.DomainManagers
{
    public class GroupManager : Manager<Group>
    {
        public Group Create(Group group)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
            {
                var createdGroup = _repo.Create(group);
                createdGroup.TopGroupId = createdGroup.Id;
                _repo.Update(createdGroup);
                return createdGroup;
            }
        }

        public ICollection<Group> GetAll()
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.GetAll().ToList();
        }

        public Group GetByName(string name)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.Find(g => g.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();
        }

        public Group GetById(int id)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.GetById(id);
        }

        public Group Update(Group group)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.Update(group);
        }

        public bool Delete(int id)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
            {
                var groupToDelete = _repo.GetById(id, new List<string>() {"ParentGroup", "ChildGroups"});
                var parentGroup = groupToDelete.ParentGroupId != null
                    ? _repo.GetById(groupToDelete.ParentGroupId.Value, new List<string>() {"ChildGroups"})
                    : null;
                var childGroups = groupToDelete.ChildGroups.ToList();
                foreach (var childGroup in childGroups)
                {
                    childGroup.ParentGroupId = parentGroup != null ? parentGroup.Id : (int?) null;
                    childGroup.TopGroupId = childGroup.Id;

                    if (parentGroup != null)
                    {
                        if (parentGroup.ChildGroups == null)
                            parentGroup.ChildGroups = new List<Group>();

                        childGroup.TopGroupId = parentGroup.TopGroupId;
                        parentGroup.ChildGroups.Add(childGroup);
                        _repo.Update(parentGroup);
                    }

                    _repo.Update(childGroup);
                }
                return _repo.Delete(id);
            }
        }

        public void AssignParent(Group childGroup, Group parentGroup)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
            {
                var pg = _repo.GetById(parentGroup.Id, new List<string>() {"ChildGroups"});
                var cg = _repo.GetById(childGroup.Id, new List<string>() {"ParentGroup"});
                cg.ParentGroupId = pg.Id;
                pg.ChildGroups.Add(cg);
                cg.TopGroupId = pg.TopGroupId;
                _repo.Update(cg);
                _repo.Update(pg);
            }
        }

        public Group GetParent(int childGroupId)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.GetById(childGroupId, new List<string>() {"ParentGroup", "ParentGroup.ParentGroup"}).ParentGroup;
        }

        public void DeleteAll()
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
            {
                var allGroups = _repo.GetAll().ToList();
                foreach (var group in allGroups)
                    _repo.Delete(group.Id);
            }
        }

        public void AddChild(Group parentGroup, Group childGroup)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
            {
                var pg = _repo.GetById(parentGroup.Id, new List<string>() { "ChildGroups" });
                var cg = _repo.GetById(childGroup.Id, new List<string>() { "ParentGroup" });
                cg.ParentGroupId = pg.Id;
                pg.ChildGroups.Add(cg);
                cg.TopGroupId = pg.TopGroupId;
                _repo.Update(cg);
                _repo.Update(pg);
            }
        }

        public ICollection<Group> GetChildren(int parentId)
        {
            using (_repo = new Repository<Group>(BHDbContext.Create()))
                return _repo.GetById(parentId, new List<string>() {"ChildGroups", "ChildGroups.ChildGroups"}).ChildGroups;
        }
    }
}
