using System;
using System.Collections.Generic;
using System.Linq;
using Logistics.EFRepository;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public abstract class BaseRep<T> : IBaseRep<T>
            where T : class {
        protected LogisticsEntities db = new LogisticsEntities();

        public IQueryable<T> All {
            get {
                return db.Set<T>();
            }
        }

        public void Add(T model) {
            db.Set<T>().Add(model);
        }

        public void Delete(T model) {
            db.Entry(model).State = System.Data.EntityState.Deleted;
        }

        public void Update(T model) {
            db.Entry(model).State = System.Data.EntityState.Modified;
        }

        public T Find(params object[] keys) {
            return db.Set<T>().Find(keys);
        }

        public int SaveChanges() {
            return db.SaveChanges();
        }

        public void Dispose() {
            db.Dispose();
        }

        public IEnumerable<U> ExecProcedure<U>(string procName, params object[] parms) {
            return db.Database.SqlQuery<U>(procName, parms);
        }
    }
}