using System;

namespace Logistics.Portal.Models {
    public class StockDetailsView {
        public string Did { get; set; }
        public string Buttressno { get; set; }
        public string Memorycard { get; set; }
        public string Floors { get; set; }
        public int? Unit { get; set; }
        public int? Hottoys { get; set; }
        public int? Curinrpieces { get; set; }
        public decimal? CurinWeight { get; set; }
        public int? Curoutrpieces { get; set; }
        public decimal? CuroutWeight { get; set; }
        public int? Rpieces { get; set; }
        public decimal? Weight { get; set; }
    }
}