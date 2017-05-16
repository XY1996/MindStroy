using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeMinLib.MindInfo;

namespace FreeMinLib.Reader
{
    public interface ITreeNodeGenerate
    {
        TreeNode GenerateTreeNode(out Dictionary<string, string> allTreeData);
    }
}
