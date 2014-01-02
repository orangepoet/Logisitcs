using Logistics.Domain.Entities;
using System.Linq;

namespace Logistics.Domain.Repository {
    public interface IMenuRep : IBaseRep<Menu> {
        IQueryable<Menu> GetMenusByRole(int roleid);
    }
}