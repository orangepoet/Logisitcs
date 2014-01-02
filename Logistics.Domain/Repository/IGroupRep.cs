using System.Collections.Generic;
using Logistics.Domain.Entities;

namespace Logistics.Domain.Repository {
    public interface IGroupRep : IBaseRep<Group> {
        IEnumerable<Customer> FindMembers(int groupid, bool include);
        void AddMember(int groupid, Customer item);
        void RemoveMems(int groupid, IEnumerable<int> custIds);
    }
}