//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Logistics.EFRepository.Mapping
{
    #pragma warning disable 1573
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.Infrastructure;
    using Logistics.Domain.Entities;
    
    internal partial class StockMove_Mapping : EntityTypeConfiguration<StockMove>
    {
        public StockMove_Mapping()
        {					
    		this.HasKey(t => t.Id);		
    		this.ToTable("StockMoves");
    		this.Property(t => t.Id).HasColumnName("Id");
    		this.Property(t => t.Did).HasColumnName("Did").IsRequired().HasMaxLength(20);
    		this.Property(t => t.Memoryid).HasColumnName("Memoryid");
    		this.Property(t => t.Memorycard).HasColumnName("Memorycard").HasMaxLength(50);
    		this.Property(t => t.Stockno).HasColumnName("Stockno").HasMaxLength(20);
    		this.Property(t => t.Stocktype).HasColumnName("Stocktype");
    		this.Property(t => t.Customerid).HasColumnName("Customerid");
    		this.Property(t => t.Customername).HasColumnName("Customername").HasMaxLength(50);
    		this.Property(t => t.Mncode).HasColumnName("Mncode").HasMaxLength(20);
    		this.Property(t => t.Proid).HasColumnName("Proid").IsRequired().HasMaxLength(20);
    		this.Property(t => t.Proname).HasColumnName("Proname").HasMaxLength(100);
    		this.Property(t => t.Brandname).HasColumnName("Brandname").HasMaxLength(100);
    		this.Property(t => t.Moname).HasColumnName("Moname").HasMaxLength(100);
    		this.Property(t => t.Maname).HasColumnName("Maname").HasMaxLength(100);
    		this.Property(t => t.Baname).HasColumnName("Baname").HasMaxLength(100);
    		this.Property(t => t.Hottoys).HasColumnName("Hottoys");
    		this.Property(t => t.Qunit).HasColumnName("Qunit").HasMaxLength(100);
    		this.Property(t => t.Wunit).HasColumnName("Wunit").HasMaxLength(100);
    		this.Property(t => t.Curinrpieces).HasColumnName("Curinrpieces");
    		this.Property(t => t.CurinWeight).HasColumnName("CurinWeight");
    		this.Property(t => t.Curoutrpieces).HasColumnName("Curoutrpieces");
    		this.Property(t => t.CuroutWeight).HasColumnName("CuroutWeight");
    		this.Property(t => t.Rpieces).HasColumnName("Rpieces");
    		this.Property(t => t.Weight).HasColumnName("Weight");
    		this.Property(t => t.Yrackid).HasColumnName("Yrackid").HasMaxLength(20);
    		this.Property(t => t.Yrackname).HasColumnName("Yrackname").HasMaxLength(50);
    		this.Property(t => t.Erackid).HasColumnName("Erackid").HasMaxLength(20);
    		this.Property(t => t.Erackname).HasColumnName("Erackname").HasMaxLength(50);
    		this.Property(t => t.Sackid).HasColumnName("Sackid").HasMaxLength(20);
    		this.Property(t => t.Sackname).HasColumnName("Sackname").HasMaxLength(50);
    		this.Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(255);
    		this.Property(t => t.Status).HasColumnName("Status").HasMaxLength(20);
    		this.Property(t => t.Txdate).HasColumnName("Txdate");
    		this.Property(t => t.Manualno).HasColumnName("Manualno").HasMaxLength(20);
    		this.Property(t => t.Recordid).HasColumnName("Recordid").HasMaxLength(20);
    		this.Property(t => t.Createuser).HasColumnName("Createuser").HasMaxLength(12);
    		this.Property(t => t.Createtime).HasColumnName("Createtime");
    		this.HasRequired(t => t.Customer).WithMany(t => t.StockMoves).HasForeignKey(d => d.Customerid);
    	}
    }
}
