using UnityEngine;
using System.Collections;
using System.Xml;

public class Speech : SerializableItem 
{
	public string text;

    private Speech() { }
	
	public Speech(string text)
    {
		this.text = text;
	}

    public Speech(DialogueInteraction i)
    {
        this.text = i.text;
    }

    public static Speech Empty()
    {
        return new Speech();
    }

	public XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = doc.CreateElement("speech");
        root.SetAttribute("content", text);
        
        return root;
	}

    public void FillFromXML(XmlElement currentNode)
    {
        text = currentNode.GetAttribute("content");
	}
}
