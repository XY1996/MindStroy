using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// This is a Nodebase Class for Mind Tools Info
/// </summary>
[System.Serializable]
public class NodeBase : IMindReadable
{
    [SerializeField]
    private string m_id;

    /// <summary>
    /// Get Id 
    /// </summary>
    public string Id
    {
        get
        {
            return m_id;
        }

        set
        {
            m_id = value;
        }
    }

}


