using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

public abstract class VirtualInteraction : SimpleObject, SerializableItem
{
    public List<VirtualInteraction> dependents = new List<VirtualInteraction>();
    public List<VirtualInteraction> dependencies = new List<VirtualInteraction>();
    private List<VirtualInteraction> children = new List<VirtualInteraction>();

    public enum InteractionType {CLICK, DIALOG, SCORE, ANSWER, SCENE_LOAD, MESSAGE, GET_ITEM, USE_ITEM, GAME_END};
    public enum DependencyType { AND, OR };

    public DependencyType DepType { get; set; }
    private InteractionType _type;
    private VirtualInteraction parent;

    /// <summary>
    /// Used to create empty interactions. The uid parameter is ignored, as the UID will be filled from XML.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="t"></param>
    protected VirtualInteraction(ulong id, InteractionType t) : base(0) {
        this._type = t;
        DepType = DependencyType.AND;
    }

    public VirtualInteraction(InteractionType t) : base()
    {
        this._type = t;
        DepType = DependencyType.AND;
    }

    public virtual bool Editable
    {
        get
        {
            return true;
        }
    }

    public virtual bool IsElementRelated
    {
        get
        {
            return false;
        }
    }


    #region DEPENDENCY_LOGIC
    public void AddDependencie(VirtualInteraction d)
    {
        if(d != this && !Children.Contains(d)){
            this.dependencies.Add(d);
            d.AddDependent(this);
        }
    }

    public void RemoveDependencie(VirtualInteraction d)
    {
        dependencies.Remove(d);
        d.RemoveDependent(this);
    }

    private void AddDependent(VirtualInteraction d)
    {
        dependents.Add(d);
    }

    private void RemoveDependent(VirtualInteraction d)
    {
        dependents.Remove(d);
    }

    public bool DependsOn(VirtualInteraction dependency)
    {
        return dependencies != null && dependencies.Contains(dependency);
    }
    #endregion
	
    public void AddChild(VirtualInteraction i)
    {
        children.Add(i);
        i.parent = this;
    }

    public void RemoveChild(VirtualInteraction i)
    {
        children.Remove(i);
        i.parent = null;
    }
	
	public bool HasChildren(){
		return this.children != null && this.children.Count > 0;
	}

    public void ClearChildren()
    {
        foreach (VirtualInteraction child in children)
            child.parent = null;
        children.Clear();
    }

    public VirtualInteraction[] Children
    {
        get
        {
            
            return children.ToArray();
        }
    }

    public VirtualInteraction Parent
    {
        get
        {
            return parent;
        }
    }

    public InteractionType Type
    {
        get
        {
            return _type;
        }
    }
	
	public virtual InteractionFactory GetFactory(){return null;}

    /// <summary>
    /// Cleans up dependencies, dependents and sets references to null.
    /// </summary>
    public virtual void Delete()
    {
        while (dependencies.Count > 0)
        {
            VirtualInteraction dep = dependencies[0];
            RemoveDependencie(dep);
        }

        foreach (VirtualInteraction v in Children)
            v.Delete();

        while (dependents.Count > 0)
        {
            VirtualInteraction dep = dependents[0];
            dep.RemoveDependencie(this);
        }

        dependencies = dependents = children = null;
    }

    public virtual XmlNode ToXML(XmlDocument doc) {
        XmlElement root = doc.CreateElement("interaction");
        SetupToXML(root);
        root.SetAttribute("type", ((int)Type).ToString());
        root.SetAttribute("depType", DepType == DependencyType.AND ? "AND" : "OR");


        foreach (VirtualInteraction dep in dependencies)
        {
            XmlNode depRoot = doc.CreateElement("dependency");
            depRoot.InnerText = dep.ID.ToString();
            root.AppendChild(depRoot);
        }
        if (children.Count > 0)
        {
            XmlElement childrenRoot = doc.CreateElement("children");
            foreach (VirtualInteraction child in children)
                childrenRoot.AppendChild(child.ToXML(doc));
            root.AppendChild(childrenRoot);
        }

        return root;
    }

    /// <summary>
    /// Clears any dependency to its children
    /// </summary>
    private void ClearInvalidDependencies()
    {
        foreach (var child in Children)
        {
            bool b = dependencies.Remove(child);
        }
    }

    public virtual void FillFromXML(XmlElement currentElement)
    {
        SetupFromXML(currentElement);
        _type = (InteractionType)int.Parse(currentElement.GetAttribute("type"));

        if (currentElement.HasAttribute("depType"))
        {
            string depStr = currentElement.GetAttribute("depType");
            DepType = depStr.Equals("OR") ? DependencyType.OR : DependencyType.AND;
        }

        ClearChildren();

        XmlElement childrenList = currentElement["children"];
        if (childrenList != null && !childrenList.IsEmpty)
        {
            foreach (XmlElement element in childrenList)
            {
                VirtualInteraction interaction = VIDeserializer.MakeInteraction(element);
                interaction.FillFromXML(element);
                AddChild(interaction);
            }
        }
    }


    /// <summary>
    /// Checks if this interaction is related to a certain object. For instance: Checks if a VirtualSceneLoad is related to specific scene or if a VirtualScoreIncrement is related to a certain Score.
    /// </summary>
    /// <param name="obj">Object to receive typecast and be checked.</param>
    /// <returns>True if this interaction is about such object.</returns>
    public virtual bool IsRelatedTo(SimpleObject obj)
    {
        return false;
    }
}