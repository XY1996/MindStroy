using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeMinLib.MindInfo
{
    /// <summary>
    /// Tree Node
    /// </summary>
    public class TreeNode:NodeBase
    {
        public string Text { get; set; }
        /// <summary>
        /// Visit first
        /// </summary>
        public List<TreeNode> LeftChildNode { get; set; } = new List<TreeNode>();
        /// <summary>
        /// Visit after left
        /// </summary>
        public List<TreeNode> RightChildNode { get; set; } = new List<TreeNode>();
    }
}
