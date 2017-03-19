using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public List<FreeMindNode> SelectNodes()
    {
        List<FreeMindNode> nodes = new List<FreeMindNode>();
        XmlNode mapNodeList= XmlDocument.SelectSingleNode("map");
        XmlNodeList firstNodes = mapNodeList.SelectNodes("node");

        FreeMindNode tempNode;
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

    private FreeMindNode XmlNode2FreeMindNode(XmlNode node)
    {
        FreeMindNode tempNode = new FreeMindNode();
        tempNode.Id = node.Attributes["ID"].Value;
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
