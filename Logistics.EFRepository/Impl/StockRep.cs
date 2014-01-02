using System;
using System.Linq;
using Logistics.Domain.Entities;
using Logistics.Domain.Repository;

namespace Logistics.EFRepository.Impl {
    public class StockRep : BaseRep<Stock>, IStockRep {
        public dynamic GetStockView(string did) {
            if (string.IsNullOrEmpty(did)) {
                throw new Exception("库存管理号码不存在");
            }

            //stock head result
            Stock stock = db.Stocks.Find(did);
            if (stock == null) {
                throw new Exception(string.Format("库存管理号码{0}数据不存在", did));
            }

            //memorycar result
            var memorycard = db.Memorycards.Where(m => m.Memoryid == stock.Memoryid
                                                            && m.MemorycardName == stock.Memorycard
                                                            && stock.Status != "D")
                                                    .SingleOrDefault();
            //stock detail result
            var stockDetails = (from r in
                                    (from d in db.StockDetails
                                     where d.Did == did && d.Memoryid == memorycard.Memoryid
                                     select new {
                                         Did = d.Did,
                                         Memorycard = d.Memorycard,
                                         Buttressno = d.Buttressno,
                                         Floors = d.Floors,
                                         Unit = d.Unit,
                                         Hottoys = d.Hottoys,
                                         Ispacklist = d.Ispacklist,
                                         Fir = d.Detailseqno == 1 ? d.Weight : 0,
                                         Sec = d.Detailseqno == 2 ? d.Weight : 0,
                                         Thi = d.Detailseqno == 2 ? d.Weight : 0,
                                         Fou = d.Detailseqno == 2 ? d.Weight : 0,
                                         Fif = d.Detailseqno == 2 ? d.Weight : 0,
                                         Six = d.Detailseqno == 2 ? d.Weight : 0,
                                         Curinrpieces = d.Curinrpieces,
                                         Curoutrpieces = d.Curoutrpieces,
                                         Rpieces = d.Rpieces,
                                         CurinWeight = d.CurinWeight,
                                         CuroutWeight = d.CuroutWeight,
                                         Weight = d.Weight
                                     })
                                group r by new { r.Did, r.Memorycard, r.Buttressno, r.Floors, r.Unit, r.Hottoys, r.Ispacklist } into g
                                select new {
                                    Did = g.Key.Did,
                                    Memorycard = g.Key.Memorycard, Buttressno = g.Key.Buttressno,
                                    Floors = g.Key.Floors,
                                    Unit = g.Key.Unit,
                                    Hottoys = g.Key.Hottoys,
                                    Ispacklist = g.Key.Ispacklist,
                                    Fir = g.Max(s => s.Fir),
                                    Sec = g.Max(s => s.Sec),
                                    Thi = g.Max(s => s.Thi),
                                    Fou = g.Max(s => s.Fou),
                                    Fif = g.Max(s => s.Fif),
                                    Six = g.Max(s => s.Six),
                                    Curinrpieces = g.Sum(s => s.Curinrpieces),
                                    CurinWeight = g.Sum(s => s.CurinWeight),
                                    Curoutrpieces = g.Sum(s => s.Curoutrpieces),
                                    CuroutWeight = g.Sum(s => s.CuroutWeight),
                                    Rpieces = g.Sum(s => s.Rpieces),
                                    Weight = g.Sum(s => s.Weight),
                                }).ToList();
            string wunit = stock.Wunit;
            string qunit = stock.Qunit;

            var stockView = new {
                //from stock result
                Did = stock.Did, Memoryid = stock.Memoryid, Memorycard = stock.Memorycard, Stockinid = stock.Stockinid, Recargoid = stock.Recargoid,
                Customerid = stock.Customerid, Customername = stock.Customername, Conid = stock.Conid, Sid = stock.Sid,
                Proid = stock.Proid, Proname = stock.Proname, Smanualno = stock.Smanualno, Manualno = stock.Manualno, Mncode = stock.Mncode, Carno = stock.Carno,
                Contactor = stock.Contactor, Brandname = stock.Brandname, Moname = stock.Maname, Remark = stock.Remark, Baname = stock.Baname, Maname = stock.Maname,

                //from memorycard
                Arrivalwayid = memorycard.Arrivalwayid, Arrivalwayname = memorycard.Arrivalwayname, Acceptance = memorycard.Acceptance,
                Acceptanceid = memorycard.Acceptanceid, Curinrpieces = FormatDecimal(memorycard.Scurinrpieces, 0) + qunit, Curoutrpieces = FormatDecimal(memorycard.Scuroutrpieces, 0) + qunit,
                CurinWeight = FormatDecimal(memorycard.ScurinWeight, 3) + wunit, CuroutWeight = FormatDecimal(memorycard.ScuroutWeight, 3) + wunit, Weight = FormatDecimal(memorycard.Sweight, 3) + wunit,
                Rpieces = FormatDecimal(memorycard.Srpieces, 0) + qunit, SupplyUnit = memorycard.Supplyunit,

                //from stock detail
                StockDetails = new { Rows = stockDetails, Total = stockDetails.Count }
            };
            return stockView;
        }

        private string FormatDecimal(decimal? d, int precision) {
            if (d == null || d == 0) {
                return "0";
            } else {
                string format = "0.";

                for (int i = 0; i < precision; i++) {
                    format += "0";
                }
                return d.Value.ToString(format);
            }
        }
    }
}