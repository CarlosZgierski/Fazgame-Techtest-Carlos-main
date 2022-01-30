using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BasicVertex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image img;
    public Text dependencyTypeHover;
    public GameObject options;

    internal VirtualInteraction interaction;
    protected GraphManager manager;

    private BasicVertex [] childVertices = new BasicVertex[0];
    private VertexImages imgs;

    protected virtual void Update() //Why reseting the image on update?
    {
        if (VertexImages.Valid(imgs))
        {
            if (interaction != null && interaction.dependencies.Count == 0 && imgs != null && imgs.first)
                img.sprite = imgs.first;
            else
                img.sprite = imgs.original;
        }
    }

    public virtual void Setup(VirtualInteraction interaction, GraphManager manager)
    {
        this.interaction = interaction;
        this.manager = manager;

        UpdateSprite(interaction, manager);

        depType = this.interaction.DepType;
    }

    public void OnPointerEnter(PointerEventData d)
    {
        manager.SetHoveredVertex(this);
    }

    public void OnPointerExit(PointerEventData d)
    {
        manager.VertexHoveredOut(this);
    }

    public void CreateNewDependency()
    {
        manager.CreateNewDependecy(this);
    }

    public void Delete()
    {
        iTween.ScaleTo(gameObject, GetFadeOutHash(gameObject, "DeleteImmediate"));
    }

    public static System.Collections.Hashtable GetFadeOutHash(GameObject obj, string completeCallback)
    {
        return iTween.Hash(iT.ScaleTo.scale, Vector3.zero, iT.ScaleTo.time, 1f, iT.ScaleTo.oncompletetarget, obj, iT.ScaleTo.oncomplete, completeCallback, iT.ScaleTo.easetype, iTween.EaseType.easeInBounce);
    }

    public void DeleteImmediate()
    {
        manager.Delete(this, true);
        interaction.Delete();
        Destroy(gameObject);
        manager.Save();
    }

    public void DeleteVisually()
    {
        manager.Delete(this, true);
        Destroy(gameObject);
    }

    public void Edit()
    {
        InteractionManager manager = FindObjectOfType(typeof(InteractionManager)) as InteractionManager;
        manager.Edit(interaction);
    }

    public virtual GraphVertexModel Serialized
    {
        get
        {
            return new GraphVertexModel(this.interaction, Vector3.zero);
        }
    }

    public virtual void Refresh()
    {

    }

    public BasicVertex[] ChildVertices
    {
        get
        {
            return childVertices;
        }
        set
        {
            childVertices = value;
        }
    }

    private void OnDestroy()
    {
        foreach (BasicVertex vertex in ChildVertices)
        {
            if (vertex.gameObject)
                Destroy(vertex.gameObject);
        }
        ChildVertices = null;
    }

    public bool DependsOn(BasicVertex v)
    {
        return this.interaction.DependsOn(v.interaction);
    }

    public virtual bool CanHaveDependencies
    {
        get
        {
            return true;
        }
    }

	public virtual bool CanHaveDependent
	{
		get
		{
			return true;
		}
	}

    private void OnSelected()
    {
        manager.SelectedVertex = this;
    }

    protected virtual void UpdateSprite(VirtualInteraction i, GraphManager m)
    {
        VirtualInteraction.InteractionType type = i.Type;
        this.imgs = m.GetTypeSprite(type);
    }

    public void SwitchDependencyType()
    {
        int oldType = (int)interaction.DepType;
        int newType = Mathf.Abs(oldType - 1);
        depType = (VirtualInteraction.DependencyType)newType;

        manager.Save();
    }

    private VirtualInteraction.DependencyType depType
    {
        set
        {
            
            interaction.DepType = value;

            int index = (int)value;
            if (dependencyTypeHover)
            {
				string[] msgs = { "Atribuir para <Ou>", "Atribuir para <E>" };
                dependencyTypeHover.text = msgs[index];
            }
        }
    }
}