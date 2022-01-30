using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    protected Dictionary<VirtualInteraction, List<Interaction>> virtualInteractionsLinks;
    protected HashSet<GameObject> createdGameObjects;
    protected List<Interaction> boxInteractions;

    protected Transform interactionHolder;

    protected static InteractionController instance = null;
    protected bool firstScene = true;

    public GraphManager graphManager;

    public static InteractionController ControllerInstance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObj = new GameObject("InteractionControllerHolder");
                instance = gameObj.AddComponent<InteractionController>();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Debug.LogError("Interaction Controller Error: Duplicated instance of Interaction Controller.");
        }
    }

    private void _SetupPreview()
    {
        ClearCreatedGameObjects();
        boxInteractions = new List<Interaction>();
        createdGameObjects = new HashSet<GameObject>();
        virtualInteractionsLinks = new Dictionary<VirtualInteraction, List<Interaction>>();
        InteractionGraph graph = graphManager.interactionGraph;
        foreach (GraphVertexModel vertexModel in graph.vertices)
        {
            VirtualInteraction interaction = vertexModel.interaction;
            CreateInteraction(interaction);
        }
        AdjustDependencies();
        virtualInteractionsLinks.Clear();
    }

    public void AddBoxInteraction(Interaction interaction)
    {
        if (boxInteractions == null)
        {
            boxInteractions = new List<Interaction>();
        }
        boxInteractions.Add(interaction);
    }

    public bool RemoveBoxInteraction(Interaction interaction)
    {
        if (boxInteractions == null)
        {
            Debug.LogError("Interaction Controller Error: Cannot remove from inexistent list.");
            return false;
        }

        return boxInteractions.Remove(interaction);
    }

    public bool HaveBoxInteraction()
    {
        return boxInteractions != null && boxInteractions.Count > 0;
    }

    /// <summary>
    /// Creates a fresh GameObject to hold an interaction.
    /// It is placed under the "Interaction Holder" gameobject just for project clarity.
    /// </summary>
    /// <param name="name">Name of the new object.</param>
    /// <returns>The created object.</returns>
    public GameObject CreateInteractionGameObject(string name)
    {
        if (!interactionHolder)
            interactionHolder = new GameObject("Interaction Holder").transform;

        GameObject obj = new GameObject(name);
        obj.transform.parent = interactionHolder;
        createdGameObjects.Add(obj);
        return obj;
    }

    public void AddCreatedGameObject(GameObject go)
    {
        createdGameObjects.Add(go);
    }

    public void ClearCreatedGameObjects()
    {
        if (createdGameObjects != null)
        {
            foreach (GameObject go in createdGameObjects)
            {
                Destroy(go);
            }
            createdGameObjects.Clear();
        }
    }

    private void CreateInteraction(VirtualInteraction virtualInteraction)
    {
        InteractionFactory factory = virtualInteraction.GetFactory();
        List<Interaction> created = factory.createInteraction(virtualInteraction, this);

        virtualInteractionsLinks.Add(virtualInteraction, created);
        foreach (Interaction interaction in created)
            interaction.depType = virtualInteraction.DepType;

        foreach (VirtualInteraction virtualInteractionChild in virtualInteraction.Children)
        {
            CreateInteraction(virtualInteractionChild);
        }
    }

    public void AdjustDependencies()
    {
        foreach (KeyValuePair<VirtualInteraction, List<Interaction>> link in virtualInteractionsLinks)
        {
            VirtualInteraction interaction = link.Key;
            List<Interaction> interactionList = link.Value;
            foreach (VirtualInteraction interactionDependency in interaction.dependencies)
            {
                List<Interaction> listIDependencie = null;
                if (!virtualInteractionsLinks.TryGetValue(interactionDependency, out listIDependencie) || listIDependencie == null)
                {
                    Debug.LogError("Interaction Controller Error: Could not find dependency list on dependency dictionary.");
                    listIDependencie = new List<Interaction>();
                    continue;
                }

                if (listIDependencie.Count <= 0)
                {
                    Debug.LogError("Interaction Controller Error: The dependency list is empty.");
                    continue;
                }

                DialogueInteraction dialogue = listIDependencie[0] as DialogueInteraction;
                if (dialogue == null)
                {
                    interactionList[0].AddDependencie(listIDependencie[0]);
                }
                else
                {
                    interactionList[0].AddDependencie(listIDependencie[listIDependencie.Count - 1]);
                }
            }
        }
    }

    public List<Interaction> GetInteractions(VirtualInteraction virtualInteraction)
    {
        if (virtualInteractionsLinks != null)
        {
            List<Interaction> interactionList = null;
            if (!virtualInteractionsLinks.TryGetValue(virtualInteraction, out interactionList))
            {
                Debug.LogError("Interaction Controller Error: Could not find interaction list on interaction dictionary.");
            }

            return interactionList;
        }
        return null;
    }
}
