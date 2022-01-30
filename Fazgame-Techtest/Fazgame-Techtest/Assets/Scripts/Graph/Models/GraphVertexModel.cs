using UnityEngine;
using System.Xml;

public class GraphVertexModel : SerializableItem
{
    private VirtualInteraction _interaction;
    private Vector3 _position;

    private GraphVertexModel() { }

    public GraphVertexModel(VirtualInteraction i, Vector3 pos)
    {
        this._interaction = i;
        this._position = pos;
    }

    public VirtualInteraction interaction
    {
        get
        {
            return _interaction;
        }
    }

    public Vector3 localPosition
    {
        get
        {
            return _position;
        }
    }

    public XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = doc.CreateElement("vertex");
        Serializer.SerializeVector3(root, localPosition);
        root.AppendChild(interaction.ToXML(doc));
        return root;
    }

    public void FillFromXML(XmlElement currentElement)
    {
        string elementIdentifier = currentElement.GetAttribute("elementID");
        _position = Serializer.GetVector3(currentElement);
        XmlElement interactionElement = currentElement["interaction"];
        _interaction = VIDeserializer.MakeInteraction(interactionElement);
        _interaction.FillFromXML(interactionElement);
    }

    public static GraphVertexModel Empty()
    {
        return new GraphVertexModel();
    }
}