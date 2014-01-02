using Logistics.Domain.Entities;
using Logistics.Domain.Repository;

using Logistics.Domain.Entities;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public class StockDetailRep : BaseRep<StockDetail>, IStockDetailRep {
        //public IEnumerable<StockDetailsView> GetDetailList(int pageIndex, int pageSize, string where,
        //    string sort, bool asc, out int count) {
        //    var totalcnt = new SqlParameter {
        //        ParameterName = "@totalcnt",
        //        SqlDbType = SqlDbType.Int,
        //        Direction = ParameterDirection.Output
        //    };
        //    SqlParameter[] sqlParams = new SqlParameter[] {
        //        new SqlParameter{ ParameterName="@pageno",Value=pageIndex},
        //        new SqlParameter{ ParameterName="@pagesize",Value=pageSize},
        //        new SqlParameter{ ParameterName="@cnd",Value=where="1=1"},
        //        new SqlParameter{ ParameterName="@orderby",Value=sort},
        //        totalcnt
        //    };
        //    var list = db.Database
        //        .SqlQuery<StockDetailsView>("sp_pagedList_group_stock_detail @pageno,@pagesize,@cnd,@orderby,@totalcnt out",
        //                                    sqlParams
        //                                 ).ToList();
        //    count = (int)totalcnt.Value;
        //    return list;
        //}
    }
}