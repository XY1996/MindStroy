using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

public class FreeMindeReader : XmlReaderBase
{
    private Dictionary<string, FreeMindNode> id2Node = new Dictionary<string, FreeMindNode>();

    public Dictionary<string, FreeMindNode> Dic
    {
        get { return id2Node; }
    }


    public FreeMindeReader(string path) : base(path)
    {

    }

   
    /// <summary>
    /// 获得Command队列
    /// </summary>
    /// <returns></returns>
    public Queue<string> CommandQueue()
    {
        Queue<string> result = new Queue<string>();
       
        List<FreeMindNode> data = SelectNodes();
        //只有一个入口！！
        Debug.Assert(data.Count==1);
        //用栈来访问SelectNodes后得到的节点数据，类似先序遍历
        Stack<FreeMindNode> accessStack = new Stack<FreeMindNode>();
        //作为开始和结束
        accessStack.Push(data[0]);
        FreeMindNode accessNode;

        while (accessStack.Count != 0)
        {
            accessNode = accessStack.Pop();
            //字典中去除已访问的节点，防止重复访问
            id2Node.Remove(accessNode.Id);
            result.Enqueue(accessNode.Text);
            //先把不属于自己的Link节点push进栈
            foreach (var link in accessNode.Link)
            {
                if (accessNode.Nodes.Find(p => p.Id == link.DestinationId) == null
                    && Dic.ContainsKey(link.DestinationId))
                    accessStack.Push(Dic[link.DestinationId]);
            }
            //再把属于自己的Link节点push进栈
            foreach (var link in accessNode.Link)
            {
                if (accessNode.Nodes.Find(p => p.Id == link.DestinationId) != null
                    && Dic.ContainsKey(link.DestinationId))
                    accessStack.Push(Dic[link.DestinationId]);
            }
        }



        return result;
    }

    /// <summary>
    /// 读取所有的XML节点，形成节点树
    /// </summary>
    /// <returns></returns>
    public List<FreeMindNode> SelectNodes()
    {
        id2Node.Clear();

        List<FreeMindNode> nodes = new List<FreeMindNode>();
        XmlNode mapNodeList = XmlDocument.SelectSingleNode("map");
        XmlNodeList firstNodes = mapNodeList.SelectNodes("node");

        foreach (XmlNode node in firstNodes)
        {
            FreeMindNode listNode = XmlNode2FreeMindNode(node);
            nodes.Add(listNode);
            if (!Dic.ContainsKey(listNode.Id))
                Dic.Add(listNode.Id, listNode);
        }
        return nodes;
        //return nodes;
    }
    /// <summary>
    /// 递归读取XML文件中的节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private FreeMindNode XmlNode2FreeMindNode(XmlNode node)
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
                FreeMindNode listNode = XmlNode2FreeMindNode(secondNode);
                tempNode.Nodes.Add(listNode);
                if (!Dic.ContainsKey(listNode.Id))
                    Dic.Add(listNode.Id, listNode);
                //tempNode.Nodes.Add(XmlNode2FreeMindNode(secondNode));
            }
        }
        return tempNode;

    }

}
