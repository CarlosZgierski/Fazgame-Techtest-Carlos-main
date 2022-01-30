using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

public class InteractionGraph : SerializableItem
{
    public List<GraphVertexModel> vertices = new List<GraphVertexModel>();

    public void Clear()
    {
        foreach (GraphVertexModel v in vertices)
            v.interaction.Delete();
        vertices.Clear();
    }

    public XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = doc.CreateElement("graph");
        foreach (GraphVertexModel v in vertices)
            root.AppendChild(v.ToXML(doc));
        return root;
    }

    public ulong O2M_count()
    {
        return (ulong)vertices.Where( v => v.interaction.dependents.Count > 1 ).Count();
    }

    public ulong Alternate_count()
    {
        return (ulong)vertices.Where( v => v.interaction.dependents.Count == 0 ).Count();
    }

    public ulong M2O_count()
    {
        return (ulong)vertices.Where( v => v.interaction.dependencies.Count > 1 ).Count();
    }

    public void FillFromXML(XmlElement currentElement)
    {
        vertices.Clear();
        XmlNodeList vertexList = currentElement.GetElementsByTagName("vertex");
        if (vertexList == null || vertexList.Count <= 0)
        {
            //Debug.LogError("Interaction Graph Error: No vertex was found on Vertex List.");
        }
        else
        {
            foreach (XmlElement vertex in vertexList)
            {
                GraphVertexModel model = GraphVertexModel.Empty();
                model.FillFromXML(vertex);
                vertices.Add(model);
            }

            //make a dictionary for all interactions: ID -> interaction
            Dictionary<ulong, VirtualInteraction> interactions = new Dictionary<ulong, VirtualInteraction>();
            foreach (GraphVertexModel vertex in vertices)
            {
                VirtualInteraction virtualInteraction = vertex.interaction;

                //add this interaction and its children recursivelly
                AddToDict(interactions, virtualInteraction);
            }

            //get the id of each dependent and search for it in the dictionary
            for (int i = 0; i < vertices.Count; i++)
            {
                GraphVertexModel v = vertices[i];
                VirtualInteraction vi = v.interaction;
                XmlNodeList dependencyList = vertexList[i]["interaction"].GetElementsByTagName("dependency");
                foreach (XmlElement dep in dependencyList)
                {
                    ulong depCode = ulong.Parse(dep.InnerXml);
                    vi.AddDependencie(interactions[depCode]);
                }
            }
        }
    }

    private static void AddToDict(Dictionary<ulong, VirtualInteraction> interactionDictionary, VirtualInteraction virtualInteraction)
    {
        interactionDictionary[virtualInteraction.ID] = virtualInteraction;
        foreach (VirtualInteraction child in virtualInteraction.Children)
            AddToDict(interactionDictionary, child);
    }

    public void RemoveRelatedInteractions(SimpleObject element)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            GraphVertexModel v = vertices[i];
            VirtualInteraction interaction = v.interaction;
            if (interaction.IsRelatedTo(element))
            {
                interaction.Delete();
                vertices.RemoveAt(i);
                i--;
            }
        }
    }
}