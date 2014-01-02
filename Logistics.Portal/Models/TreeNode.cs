using System.Collections.Generic;

namespace Logistics.Portal.Models {
    public class TreeNode {
        public int id { get; set; }
        public string text { get; set; }
        public IEnumerable<TreeNode> children { get; set; }
        public string icon { get; set; }
    }
}