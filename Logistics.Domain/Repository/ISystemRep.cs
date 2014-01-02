using System;
using System.Collections.Generic;
using System.Linq;
using Logistics.Domain.Entities;

namespace Logistics.Domain.Repository {
    public interface ISystemRep {
        LoginUser GetUser(string userId, string userPassword);

        IQueryable<Button> GetButtons(int roleId, string menuNo);

        void SaveChanges();

        //UserInfo SetCurrentUserInfo(SysUser user);

        bool CheckRight(int p, string controller, string action);

        IEnumerable<Menu> GetMenusByRole(int roleId);
    }
}