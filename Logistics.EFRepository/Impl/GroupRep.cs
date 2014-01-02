using System.Linq;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;
using System.Collections.Generic;
using System;

namespace Logistics.EFRepository.Impl {
    public class GroupRep : BaseRep<Group>, IGroupRep {
        public IEnumerable<Customer> FindMembers(int groupid, bool include) {
            if (include) {
                return db.Customers.Where(
                  c => c.Groups
                        .FirstOrDefault(g => g.Groupid == groupid)
                      != null);
            } else {
                return db.Customers.Where(
                   c => c.Groups
                         .FirstOrDefault(g => g.Groupid == groupid)
                       == null);
            }
        }

        public void AddMember(int groupid, Customer item) {
            db.Customers.Attach(item);
            db.Groups.Find(groupid).Customers.Add(item);
        }

        public void RemoveMems(int groupid, IEnumerable<int> custIds) {
            try {
                var group = db.Groups.Include("Customers").FirstOrDefault(g => g.Groupid == groupid);
                
                foreach (var item in group.Customers.Where(c => custIds.Contains(c.CustomerId))) {
                    group.Customers.Remove(item);
                }
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}