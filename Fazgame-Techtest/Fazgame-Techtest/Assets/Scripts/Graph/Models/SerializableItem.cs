using System.Xml;
using System.Collections.Generic;

public interface SerializableItem
{
    XmlNode ToXML(XmlDocument doc);
    void FillFromXML(XmlElement currentElement);
}