//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Logistics.Domain.Entities
{
    #pragma warning disable 1573
    using System;
    using System.Collections.Generic;
    
    public partial class StockMove
    {
        public int Id { get; set; }
        public string Did { get; set; }
        public long Memoryid { get; set; }
        public string Memorycard { get; set; }
        public string Stockno { get; set; }
        public Nullable<int> Stocktype { get; set; }
        public int Customerid { get; set; }
        public string Customername { get; set; }
        public string Mncode { get; set; }
        public string Proid { get; set; }
        public string Proname { get; set; }
        public string Brandname { get; set; }
        public string Moname { get; set; }
        public string Maname { get; set; }
        public string Baname { get; set; }
        public Nullable<int> Hottoys { get; set; }
        public string Qunit { get; set; }
        public string Wunit { get; set; }
        public Nullable<int> Curinrpieces { get; set; }
        public Nullable<decimal> CurinWeight { get; set; }
        public Nullable<int> Curoutrpieces { get; set; }
        public Nullable<decimal> CuroutWeight { get; set; }
        public Nullable<int> Rpieces { get; set; }
        public Nullable<decimal> Weight { get; set; }
        public string Yrackid { get; set; }
        public string Yrackname { get; set; }
        public string Erackid { get; set; }
        public string Erackname { get; set; }
        public string Sackid { get; set; }
        public string Sackname { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public System.DateTime Txdate { get; set; }
        public string Manualno { get; set; }
        public string Recordid { get; set; }
        public string Createuser { get; set; }
        public Nullable<System.DateTime> Createtime { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
