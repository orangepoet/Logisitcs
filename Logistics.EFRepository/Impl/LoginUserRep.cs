using Logistics.Domain.Entities;
using System.Linq;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public class LoginUserRep : BaseRep<LoginUser>, ILoginUserRep {
        public IQueryable<dynamic> GetUserView() {
            var query = from u in db.LoginUsers
                        join r in db.Roles on u.RoleId equals r.Id into roles
                        from ur in roles.DefaultIfEmpty()
                        select new {
                            UserId = u.UserId,
                            RoleName = ur == null ? "" : ur.RoleName,
                            Phone = u.Phone,
                            Fax = u.Fax,
                            Email = u.Email,
                            QQ = u.QQ,
                            NickName = u.NickName,
                            Address = u.Address,
                            RealName = u.RealName,
                            Sex = u.Sex ? "男" : "女",
                            LastLoginTime = u.LastLoginTime
                        };
            return query;
        }
    }
}