using System;
using System.Collections.Generic;
using System.Linq;

namespace Logistics.Domain.Repository {
    public interface IBaseRep<T> : IDisposable where T : class {
        IQueryable<T> All { get; }
        T Find(params object[] keys);
        void Add(T model);
        void Update(T model);
        void Delete(T model);
        int SaveChanges();
        void Dispose();

        IEnumerable<U> ExecProcedure<U>(string procName, params object[] parms);
    }
}