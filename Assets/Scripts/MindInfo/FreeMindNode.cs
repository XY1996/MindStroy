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
    [SerializeField]
    private string m_text;
    [SerializeField]
    private long m_modified;
    [SerializeField]
    private long m_created;
    [SerializeField]
    private List<FreeMindArrowLink> m_link = new List<FreeMindArrowLink>();

    [SerializeField]
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

