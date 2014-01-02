using System.Linq;
using Logistics.Domain.Entities;

namespace Logistics.Domain.Repository {
    public interface ILoginUserRep : IBaseRep<LoginUser> {
        IQueryable<dynamic> GetUserView();
    }
}