using System.Xml;
using UnityEngine;

public class Serializer
{
    public static void SerializeVector3(XmlElement root, Vector3 vec, string prefix = "")
    {
        root.SetAttribute(prefix + "x", vec.x.ToString());
        root.SetAttribute(prefix + "y", vec.y.ToString());
        root.SetAttribute(prefix + "z", vec.z.ToString());
    }

    internal static Vector3 GetVector3(XmlElement currentElement, string prefix = "")
    {
        float x = float.Parse(currentElement.GetAttribute(prefix + "x"));
        float y = float.Parse(currentElement.GetAttribute(prefix + "y"));
        float z = float.Parse(currentElement.GetAttribute(prefix + "z"));

        return new Vector3(x, y, z);
    }
}