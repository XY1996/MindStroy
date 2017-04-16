using System.Collections;
using System.Collections.Generic;


using System;

/// <summary>
/// FreeMind(.mm) NodeBase
/// </summary>
[Serializable]
public class FreeMindNode : NodeBase
{
    
    private string m_text;
    
    private string m_modified;
    
    private string m_created;
    
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

    public string Modified
    {
        get
        {
            return m_modified;
        }

        set
        {
            m_modified = value;
        }
    }

    public string Created
    {
        get
        {
            return m_created;
        }

        set
        {
            m_created = value;
        }
    }
}

