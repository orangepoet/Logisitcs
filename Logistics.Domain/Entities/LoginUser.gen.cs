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
    
    public partial class LoginUser
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public int RoleId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string NickName { get; set; }
        public string Address { get; set; }
        public string RealName { get; set; }
        public bool Sex { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string Status { get; set; }
        public string Modifyuser { get; set; }
        public System.DateTime Modifytime { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
