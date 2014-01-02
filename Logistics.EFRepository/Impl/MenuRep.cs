using System;
using System.Collections.Generic;
using System.Linq;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public class MenuRep : BaseRep<Menu>, IMenuRep {
        public IQueryable<Menu> GetMenusByRole(int roleId) {
            if (roleId == 1) {
                return db.Menus.Include("ChildMenus").Where(m => m.Status != "D" && m.ParentId == 1).OrderBy(m => m.Sort);
            }
            return db.Roles.Find(roleId).Menus.AsQueryable();
        }
    }
}