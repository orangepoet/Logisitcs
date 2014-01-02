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
    
    internal partial class VenueStaff_Mapping : EntityTypeConfiguration<VenueStaff>
    {
        public VenueStaff_Mapping()
        {					
    		this.HasKey(t => t.Id);		
    		this.ToTable("VenueStaffs");
    		this.Property(t => t.Id).HasColumnName("Id");
    		this.Property(t => t.Contactor).HasColumnName("Contactor").IsRequired().HasMaxLength(20);
    		this.Property(t => t.Idcard).HasColumnName("Idcard").HasMaxLength(20);
    		this.Property(t => t.Sexs).HasColumnName("Sexs");
    		this.Property(t => t.Phone).HasColumnName("Phone").HasMaxLength(20);
    		this.Property(t => t.Birthdate).HasColumnName("Birthdate");
    		this.Property(t => t.Jobsid).HasColumnName("Jobsid");
    		this.Property(t => t.Storeid).HasColumnName("Storeid");
    		this.Property(t => t.Storein).HasColumnName("Storein");
    		this.Property(t => t.Storeout).HasColumnName("Storeout");
    		this.Property(t => t.Remark).HasColumnName("Remark").HasMaxLength(255);
    		this.Property(t => t.Status).HasColumnName("Status").IsRequired().HasMaxLength(20);
    		this.Property(t => t.Modifyuser).HasColumnName("Modifyuser").IsRequired().HasMaxLength(12);
    		this.Property(t => t.Modifytime).HasColumnName("Modifytime");
    	}
    }
}

