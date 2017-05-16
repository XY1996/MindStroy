using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System;
using System.Reflection;

public abstract class XmlReaderBase: ReaderBase
{

    private string m_sourcePath;

    private XmlDocument m_sourceXml;

    protected XmlReaderBase(string path):base(path)
    {
        m_sourcePath = path;
        m_sourceXml = new XmlDocument();
        m_sourceXml.Load(m_sourcePath);
    }

    protected XmlReaderBase(XmlDocument document)
    {
        m_sourceXml = document;
    }

    protected XmlDocument XmlDocument
    {
        get { return m_sourceXml; }
    }
    


}
