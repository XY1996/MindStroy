using System.Collections;
using System.Collections.Generic;

public abstract class ArrowLinkBase : IMindReadable
{
    private string m_id;
    private string m_destinationId;

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
   
    public string DestinationId
    {
        get
        {
            return m_destinationId;
        }

        set
        {
            m_destinationId = value;
        }
    }

}
