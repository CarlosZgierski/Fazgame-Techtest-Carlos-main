using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class VirtualEndGame : VirtualInteraction, SerializableItem {

    public string content;

    private VirtualEndGame() : base(0, InteractionType.GAME_END) { }

    public VirtualEndGame(string content)
        : base(InteractionType.GAME_END)
    {
        this.content = content;
    }

    public static VirtualEndGame Empty()
    {
        return new VirtualEndGame();
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
        this.content = currentElement.GetAttribute("content");
    }

    public override InteractionFactory GetFactory()
    {
        return new EndGameInteractionFactory();
    }

}
