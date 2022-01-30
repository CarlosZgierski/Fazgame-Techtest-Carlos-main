using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public class VirtualMessage : VirtualInteraction, SerializableItem
{
    public string content;

    private VirtualMessage() : base(0, InteractionType.MESSAGE) { }

    public VirtualMessage(string content)
        : base(InteractionType.MESSAGE)
    {
        this.content = content;
    }

    public static VirtualMessage Empty()
    {
        return new VirtualMessage();
    }

    public override XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = base.ToXML(doc) as XmlElement;

        root.SetAttribute("content", content);

        return root;
    }

    public override void FillFromXML(XmlElement currentElement)
    {
        base.FillFromXML(currentElement);
        content = currentElement.GetAttribute("content");
    }

    public override InteractionFactory GetFactory()
    {
        return new MessageInteractionFactory();
    }
}