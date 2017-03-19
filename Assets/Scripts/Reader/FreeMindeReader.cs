using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class FreeMindeReader : XmlReaderBase
{

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
            nodes.Add(XmlNode2FreeMindNode(node));
        }        
        return nodes;
        //return nodes;
    }

    private static FreeMindNode XmlNode2FreeMindNode(XmlNode node)
    {
        FreeMindNode tempNode = new FreeMindNode();
        tempNode.Id = node.Attributes["ID"].Value;
        tempNode.Text = node.Attributes["TEXT"].Value;
        foreach(XmlNode arrowLink in node.SelectNodes("arrowlink "))
        {
            FreeMindArrowLink tempArrowLink = new FreeMindArrowLink();
            tempArrowLink.Id = arrowLink.Attributes["ID"].Value;
            tempArrowLink.DestinationId = arrowLink.Attributes["DESTINATION"].Value;
        }

        XmlNodeList Nodes;
        if ((Nodes = node.SelectNodes("node")).Count != 0)
        {
            foreach(XmlNode secondNode in Nodes)
            {
                tempNode.Nodes.Add(XmlNode2FreeMindNode(secondNode));
            }
        }
        return tempNode;

    }

}
