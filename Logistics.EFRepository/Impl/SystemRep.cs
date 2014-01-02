using System;
using System.Collections.Generic;
using System.Linq;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public class SystemRep : ISystemRep {
        private LogisticsEntities db = new LogisticsEntities();

        public LoginUser GetUser(string userId, string userPassword) {
            return db.LoginUsers.SingleOrDefault(u => u.UserId.Equals(userId) && u.UserPassword.Equals(userPassword));
        }

        public IQueryable<Button> GetButtons(int roleId, string menuNo) {
            //var query = from rb in db.RoleButtons
            //            join b in db.Buttons on rb.ButtonId equals b.Id
            //            where rb.RoleId == roleId
            //            where b.SysController == menuNo
            //            select b;
            //return query;
            return db.Roles.Find(roleId).Buttons.Where(b => b.SysController == menuNo).AsQueryable();
        }

        //public UserInfo SetCurrentUserInfo(LoginUser user) {
        //    user.LastLoginTime = DateTime.Now;
        //    db.Entry(user).State = System.Data.EntityState.Modified;
        //    db.SaveChanges();
        //    return new UserInfo { RoleId = user.RoleId, UserId = user.UserId };
        //}

        public void SaveChanges() {
            db.SaveChanges();
        }

        public bool CheckRight(int roleId, string controller, string action) {
            if (roleId == 1) {
                return true;
            }
            return db.Roles.Find(roleId).Buttons
                    .Where(b => b.SysController == controller && b.SysAction == action).Count() > 0;
            //var query = from rb in db.RoleButtons
            //            join b in db.Buttons on rb.ButtonId equals b.Id
            //            where rb.RoleId == roleId
            //            where b.SysController == controller && b.SysController == action
            //            select b.Id;
            //return query.Count() > 0;
        }

        public IEnumerable<Menu> GetMenusByRole(int roleId) {
            if (roleId == 1) {
                return db.Menus.Include("ChildMenus").Where(m => m.Status != "D" && m.ParentId == 1).OrderBy(m => m.Sort);
            }
            return db.Roles.Find(roleId).Menus;
        }
    }
}