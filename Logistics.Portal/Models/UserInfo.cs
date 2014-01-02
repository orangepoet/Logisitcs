using System;

namespace Logistics.Portal.Models {
    [Serializable]
    public class UserInfo {
        public string UserId { get; set; }
        public int RoleId { get; set; }
    }
}