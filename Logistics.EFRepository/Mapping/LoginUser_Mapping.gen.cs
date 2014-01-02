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
    
    internal partial class LoginUser_Mapping : EntityTypeConfiguration<LoginUser>
    {
        public LoginUser_Mapping()
        {					
    		this.HasKey(t => t.UserId);		
    		this.ToTable("LoginUsers");
    		this.Property(t => t.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(50);
    		this.Property(t => t.UserPassword).HasColumnName("UserPassword").IsRequired().HasMaxLength(50);
    		this.Property(t => t.RoleId).HasColumnName("RoleId");
    		this.Property(t => t.Phone).HasColumnName("Phone").HasMaxLength(13);
    		this.Property(t => t.Fax).HasColumnName("Fax").HasMaxLength(13);
    		this.Property(t => t.Email).HasColumnName("Email").HasMaxLength(100);
    		this.Property(t => t.QQ).HasColumnName("QQ").HasMaxLength(13);
    		this.Property(t => t.NickName).HasColumnName("NickName").HasMaxLength(50);
    		this.Property(t => t.Address).HasColumnName("Address").HasMaxLength(200);
    		this.Property(t => t.RealName).HasColumnName("RealName").HasMaxLength(50);
    		this.Property(t => t.Sex).HasColumnName("Sex");
    		this.Property(t => t.LastLoginTime).HasColumnName("LastLoginTime");
    		this.Property(t => t.Status).HasColumnName("Status").IsRequired().HasMaxLength(2);
    		this.Property(t => t.Modifyuser).HasColumnName("Modifyuser").IsRequired().HasMaxLength(12);
    		this.Property(t => t.Modifytime).HasColumnName("Modifytime");
    		this.HasRequired(t => t.Role).WithMany(t => t.LoginUsers).HasForeignKey(d => d.RoleId);
    	}
    }
}
