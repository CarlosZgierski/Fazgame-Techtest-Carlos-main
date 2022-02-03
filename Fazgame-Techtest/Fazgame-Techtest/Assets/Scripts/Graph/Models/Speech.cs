using UnityEngine;
using System.Collections;
using System.Xml;

public class Speech : SerializableItem 
{
	public string text;
    public int avatarId;

    private Speech() { }
	
	public Speech(string text, int avatarId)
    {
		this.text = text;
        this.avatarId = avatarId;
	}

    public Speech(DialogueInteraction i)
    {
        this.text = i.text;
        this.avatarId = i.avatarId;
    }

    public static Speech Empty()
    {
        return new Speech();
    }

	public XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = doc.CreateElement("speech");

        root.SetAttribute("content", text);
        root.SetAttribute("id", avatarId.ToString());

        return root;
	}

    public void FillFromXML(XmlElement currentNode)
    {
        text = currentNode.GetAttribute("content");
        avatarId = int.Parse(currentNode.GetAttribute("id"));
	}
}
