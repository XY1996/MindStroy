using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// FreeMind(.mm) NodeBase
/// </summary>
[Serializable]
public class FreeMindNode : NodeBase
{

    private string m_text;

    private long m_modified;

    private long m_created;

    private List<FreeMindArrowLink> m_link = new List<FreeMindArrowLink>();

    private List<FreeMindNode> m_node = new List<FreeMindNode>();

    public List<FreeMindArrowLink> Link
    {
        get
        {
            return m_link;
        }

        set
        {
            m_link = value;
        }
    }

    public List<FreeMindNode> Nodes
    {
        get
        {
            return m_node;
        }

        set
        {
            m_node = value;
        }
    }

    public string Text
    {
        get
        {
            return m_text;
        }

        set
        {
            m_text = value;
        }
    }

}

