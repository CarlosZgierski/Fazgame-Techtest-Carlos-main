using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Xml;

public class GraphManager : MonoBehaviour
{
    // prefabs
    public GraphPrefabs prefabs;

    public RectTransform graphsContentArea;
    public DependencyPlacer dependencyPlacer;
    public ScrollRect scrollableArea;
    public RectTransform vertexArea;
    public VertexImageCollection collection;
    public Text title;

    private List<BasicVertex> vertices = new List<BasicVertex>();
    private List<GraphEdge> edges = new List<GraphEdge>();
    private BasicVertex selectedVertex;
    private int vertexCount = 0;
    private float widthValue;
    private Vector3 rectFirstPosition;
    private float contentBufferwidth;

    [HideInInspector]
    public BasicVertex hoveredVertex;

    public InteractionGraph interactionGraph;

    public BasicVertex SelectedVertex
    {
        set
        {
            selectedVertex = value;
        }
    }
    private void Start()
    {
        contentBufferwidth = graphsContentArea.sizeDelta.y;

        Debug.Log($"Content Buffer Width {contentBufferwidth}");

        UpdateGraphIcons();
        Setup();
    }

    private void Update()
    {
        float highestDistance = 0;

        for (int i = 0; i < vertexArea.childCount; i++)
        {
            var child = vertexArea.GetChild(i) as RectTransform;
            highestDistance = child.localPosition.x > highestDistance ? child.localPosition.x : highestDistance;
        }

        Debug.Log($"Highest Distance {highestDistance}");

        if(highestDistance >= 0)
            graphsContentArea.sizeDelta = new Vector2(highestDistance + contentBufferwidth, graphsContentArea.sizeDelta.y);
    }


    public void CleanUp(bool shouldSave)
    {
        if(shouldSave)
            Save();
        foreach (GraphEdge edge in edges)
            Destroy(edge.gameObject);

        edges.Clear();
        foreach (BasicVertex vertex in vertices)
            Destroy(vertex.gameObject);

        vertices.Clear();
    }


    public void Setup()
    {
        CleanUp(false);
        InteractionGraph graph = new InteractionGraph();
        if(PlayerPrefs.HasKey("Graph"))
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(PlayerPrefs.GetString("Graph"));
            XmlElement graphElement = doc["graph"];
            graph.FillFromXML(graphElement);
        }

        SetupVertices(graph.vertices);
        edges.ForEach((edge) =>
        {
            if (!edge)
                edges.Remove(edge);
        });
        SetupEdges(vertices);

        interactionGraph = graph;
    }

    public void SetupVertices(List<GraphVertexModel> serializedVertices)
    {
        for (int i = 0; i < serializedVertices.Count; i++)
        {
            GraphVertexModel vertexModel = serializedVertices[i];
            VirtualInteraction interaction = vertexModel.interaction;

            BasicVertex vertex = Instantiate(prefabs.GetVertex(interaction)) as BasicVertex;

            vertex.transform.SetParent(vertexArea, false);
            vertex.transform.localPosition = Vector3.one;

            vertex.Setup(interaction, this);
            vertices.Add(vertex);
        }
    }

    public void SetupAnswerEdges(BasicVertex [] newVertices)
    {
        BasicVertex source, destination;
        for (int i = 0; i < vertices.Count; i++)
        {
            for (int j = 0; j < newVertices.Length; j++)
            {
                source = destination = null;
                BasicVertex firstVertex = vertices[i];
                BasicVertex secondVertex = newVertices[j];
                if (firstVertex.DependsOn(secondVertex))
                {
                    destination = firstVertex; source = secondVertex;
                }
                else 
                    if (secondVertex.DependsOn(firstVertex))
                    {
                        destination = secondVertex; source = firstVertex;
                    }

                if (source != null)
                    StablishVisualDependecy(source, destination);
            }
        }
    }

    public void SetupEdges(List<BasicVertex> vertices)
    {
        List<BasicVertex> verticesToCheck = new List<BasicVertex>(vertices);
        int count = verticesToCheck.Count;
        for (int i = 0; i < count; i++)
        {
            foreach (BasicVertex vertex in verticesToCheck[i].ChildVertices)
                verticesToCheck.Add(vertex);
        }

        BasicVertex source, destination;
        for (int i = 0; i < verticesToCheck.Count - 1; i++)
        {
            for (int j = i + 1; j < verticesToCheck.Count; j++)
            {
                source = destination = null;
                BasicVertex firstVertex = verticesToCheck[i];
                BasicVertex secondVertex = verticesToCheck[j];
                if (firstVertex.interaction.DependsOn(secondVertex.interaction))
                {
                    destination = firstVertex; source = secondVertex;
                }
                else if (secondVertex.interaction.DependsOn(firstVertex.interaction))
                {
                    destination = secondVertex; source = firstVertex;
                }

                if (source != null)
                    StablishVisualDependecy(source, destination);
            }
        }
    }

    /// <summary>
    /// Saves the temporary content of this dictionary to the model classes.
    /// </summary>
    public void Save()
    {
        XmlDocument doc = new XmlDocument();
        interactionGraph.ToXML(doc);

        PlayerPrefs.SetString("graph", doc.InnerText);
    }

    public static void SSave()
    {

    }

    private InteractionGraph Serialize()
    {
        InteractionGraph serializedGraph = new InteractionGraph();
        foreach (BasicVertex vertex in vertices)
        {
            if (!vertex)
                break;
            GraphVertexModel model = vertex.Serialized;
            serializedGraph.vertices.Add(model);
        }
        return serializedGraph;
    }

    public Vector3 RandomGridPos
    {
        get
        {
            BasicVertex[] vertices = FindObjectsOfType(typeof(BasicVertex)) as BasicVertex[];
            if (vertices == null || vertices.Length == 0)
                return Vector3.zero;

            float x = 0, y = 0;
            bool first = true;

            foreach (BasicVertex vertex in vertices)
            {
                //Debug.Log(vertex.name);
                Vector2 pos = vertex.transform.localPosition;
                if (first || pos.x > x)
                {
                    x = pos.x;
                    y = pos.y;
                }
                first = false;
            }

            Rect rectTransform = (vertices[0].transform as RectTransform).rect;
            widthValue = rectTransform.width;
            x += rectTransform.width * 1.5f;
            return new Vector3(x, y, 0);
        }
    }

    public void CreateNewDependecy(BasicVertex source)
    {
        dependencyPlacer.Activate(source);
    }

    public void SetHoveredVertex(BasicVertex vertex)
    {
        this.hoveredVertex = vertex;
    }

    public void VertexHoveredOut(BasicVertex vertex)
    {
        if (ReferenceEquals(this.hoveredVertex, vertex))
        {
            this.hoveredVertex = null;
        }
    }


    private GraphEdge StablishVisualDependecy(BasicVertex src, BasicVertex dest)
    {
        GraphEdge edge = Instantiate(prefabs.edgePrefab) as GraphEdge;
        edge.transform.SetParent(vertexArea, false);
        edge.transform.SetAsFirstSibling(); // make all edges render before the vertices!
        edge.Setup(src, dest, this);
        edges.Add(edge);
        return edge;
    }

    public void StablishNewDependecy(BasicVertex src, BasicVertex dest)
    {
        dest.interaction.AddDependencie(src.interaction);
        StablishVisualDependecy(src, dest);
        Save();
    }

    public void AddNewInteraction(VirtualInteraction interaction)
    {
        BasicVertex vertex = Instantiate(prefabs.GetVertex(interaction), vertexArea, false) as BasicVertex;
        Vector3 pos = RandomGridPos;
        vertex.transform.localPosition = pos;
        vertex.Setup(interaction, this);
        vertices.Add(vertex);
        iTween.ScaleFrom(vertex.gameObject, iTween.Hash(iT.ScaleFrom.scale, Vector3.zero, iT.ScaleFrom.time, 1f));
    }

    public VertexImages GetTypeSprite(VirtualInteraction.InteractionType type)
    {
        return collection.Get(type);
    }

    public void Delete(BasicVertex vertex, bool keepDependencies = false)
    {
        List<GraphEdge> edgesToRemove = new List<GraphEdge>();
        List<BasicVertex> involvedVertices = new List<BasicVertex>();
        involvedVertices.Add(vertex);
        involvedVertices.AddRange(vertex.ChildVertices);

        foreach (GraphEdge edge in edges)
        {
            //if ((edge.src == vertex) || (edge.dest == vertex))
            if (involvedVertices.Contains(edge.src) || involvedVertices.Contains(edge.dest))
            {
                if(!keepDependencies)
                    edge.dest.interaction.RemoveDependencie(edge.src.interaction);
                edgesToRemove.Add(edge);
            }
        }
        foreach (GraphEdge edge in edgesToRemove)
        {
            edges.Remove(edge);
            Destroy(edge.gameObject);
        }
        vertices.Remove(vertex);
    }

    public void Delete(GraphEdge edge)
    {
        edges.Remove(edge);
        Save();
    }

    private void UpdateGraphIcons()
    {
        foreach (BasicVertex vertex in vertices)
            vertex.Refresh();
    }


    [System.Serializable]
    public class GraphPrefabs
    {
        public BasicVertex dialogVertex, generalVertex, endGameVertex;
        public GraphEdge edgePrefab;

        public BasicVertex GetVertex(VirtualInteraction interaction)
        {
            switch (interaction.Type)
            {
                case VirtualInteraction.InteractionType.DIALOG:
                    return dialogVertex;
                case VirtualInteraction.InteractionType.GAME_END:
                    return endGameVertex;
            }
            return generalVertex;
        }
    }
}