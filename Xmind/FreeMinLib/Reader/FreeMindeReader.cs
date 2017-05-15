using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using FreeMinLib.MindInfo;
using FreeMinLib.Reader;

public class FreeMindeReader : XmlReaderBase, ITreeNodeGenerate
{
 
    public FreeMindeReader(string path) : base(path)
    {

    }

    /// <summary>
    /// 获得Command队列
    /// </summary>
    /// <returns></returns>
    public Queue<string> CommandQueue(FreeMindNode node)
    {
        Queue<string> result = new Queue<string>();

        //找到所有的CommandNode
        List<FreeMindNode> commandsNode = node.Nodes;
        Stack<FreeMindNode> accessStack = new Stack<FreeMindNode>();
        result.Enqueue(node.Text);
        accessStack.Push(node);
        while (accessStack.Any())
        {
            
            List<FreeMindArrowLink> selfLink = accessStack.Pop().Link
                .Where(p => commandsNode.Any(o => p.DestinationId == o.Id))
                .ToList();
            if (!selfLink.Any())
            {             
                continue;
            }
            else if (selfLink.Count == 1)
            {
                FreeMindNode templeFreeMindNode = commandsNode.First(p => p.Id == selfLink.First().DestinationId);
                result.Enqueue(templeFreeMindNode.Text);
                accessStack.Push(templeFreeMindNode);
            }
            else
            {
                foreach (var tempArrowLink in selfLink)
                {
                    FreeMindNode templeFreeMindNode = commandsNode.First(p => p.Id == tempArrowLink.DestinationId);
                    result.Enqueue(templeFreeMindNode.Text);
                    accessStack.Push(templeFreeMindNode);
                }
            }

        }
        return result;
    }
    public TreeNode GenerateTreeNode()
    {
        TreeNode result = new TreeNode()
        {
            Id = new Guid().ToString(),
            Text = "开始",
        };
        Dictionary<string, FreeMindNode> allFreeMindNodes;
        List<FreeMindNode> dataList = SelectNodes(out allFreeMindNodes);

        foreach (var freeMindNode in dataList)
        {
            result.LeftChildNode.Add(FreeMindNodeToTreeNode(freeMindNode, allFreeMindNodes));
        }
        return result;

    }

    /// <summary>
    /// 读取所有的XML节点，形成节点树
    /// </summary>
    /// <returns></returns>
    private List<FreeMindNode> SelectNodes(out Dictionary<string, FreeMindNode> id2Node)
    {
        id2Node=new Dictionary<string, FreeMindNode>();

        List<FreeMindNode> nodes = new List<FreeMindNode>();
        XmlNode mapNodeList = XmlDocument.SelectSingleNode("map");
        XmlNodeList firstNodes = mapNodeList.SelectNodes("node");

        foreach (XmlNode node in firstNodes)
        {
            FreeMindNode listNode = XmlNode2FreeMindNode(node, id2Node);
            nodes.Add(listNode);
            if (!id2Node.ContainsKey(listNode.Id))
                id2Node.Add(listNode.Id, listNode);
        }
        return nodes;
        //return nodes;
    }
    /// <summary>
    /// 递归读取XML文件中的节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private FreeMindNode XmlNode2FreeMindNode(XmlNode node, Dictionary<string, FreeMindNode> id2Node)
    {
        FreeMindNode tempNode = new FreeMindNode();
        tempNode.Id = node.Attributes["ID"].Value;
        tempNode.Created= node.Attributes["CREATED"].Value;
        tempNode.Modified= node.Attributes["MODIFIED"].Value;
        tempNode.Text = node.Attributes["TEXT"].Value;
        foreach(XmlNode arrowLink in node.SelectNodes("arrowlink"))
        {
            FreeMindArrowLink tempArrowLink = new FreeMindArrowLink();
            tempArrowLink.Id = arrowLink.Attributes["ID"].Value;
            tempArrowLink.DestinationId = arrowLink.Attributes["DESTINATION"].Value;
            tempNode.Link.Add(tempArrowLink);
        }

        XmlNodeList Nodes;
        if ((Nodes = node.SelectNodes("node")).Count != 0)
        {
            foreach(XmlNode secondNode in Nodes)
            {
                FreeMindNode listNode = XmlNode2FreeMindNode(secondNode, id2Node);
                tempNode.Nodes.Add(listNode);
                if (!id2Node.ContainsKey(listNode.Id))
                    id2Node.Add(listNode.Id, listNode);
                //tempNode.Nodes.Add(XmlNode2FreeMindNode(secondNode));
            }
        }
        return tempNode;

    }

 

    public static TreeNode FreeMindNodeToTreeNodeWithoutChild(FreeMindNode freeMindNode)
    {
        return new TreeNode()
        {
            Id = freeMindNode.Id,
            Text = freeMindNode.Text,
        };
    }

    public static TreeNode FreeMindNodeToTreeNode(FreeMindNode freeMindNode, Dictionary<string, FreeMindNode> AllNode)
    {
        TreeNode treeNode = FreeMindNodeToTreeNodeWithoutChild(freeMindNode);

        foreach (var link in freeMindNode.Link)
        {
            if (freeMindNode.Nodes.Any(p => p.Id == link.DestinationId))
            {
                treeNode.LeftChildNode.Add(FreeMindNodeToTreeNode(AllNode[link.DestinationId],AllNode));
            }            
            else
            {
                treeNode.RightChildNode.Add(FreeMindNodeToTreeNode(AllNode[link.DestinationId], AllNode));
            }
        }
        return treeNode;
    }
}
