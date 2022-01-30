using UnityEngine;
using System.Xml;
using System.Collections.Generic;

public abstract class Interaction : MonoBehaviour
{
	[SerializeField]
	private bool doing;
	[SerializeField]
	private bool finished;
	public bool repeatable;
    public VirtualInteraction.DependencyType depType = VirtualInteraction.DependencyType.AND;

	private List<Interaction> dependents = new List<Interaction>();
    public List<Interaction> dependencies = new List<Interaction>();
	public List<Interaction> condictions = new List<Interaction>();

    protected enum UpdateMethod { NORMAL, LATE_UPATE }
    protected UpdateMethod method = UpdateMethod.NORMAL;
	
	public virtual void _Before(){}
	
	public virtual void _Do(){}
	
	public virtual void _End(){}
	
	public virtual void Step(){}

	public void Action()
    {
		Before ();
		Do ();
		End ();
	}
	
	void Update()
    {
        if (method == UpdateMethod.NORMAL)
            PerformUpdate();
	}

    void LateUpdate()
    {
        if (method == UpdateMethod.LATE_UPATE)
            PerformUpdate();
    }

    private void PerformUpdate()
    {
        if (!HasActiveDependencie())
        {
            if (CondictionsCompleted())
            {
                Step();
            }
        }
    }
	
	public virtual bool CondictionsCompleted()
    {
		foreach(Interaction condiction in condictions)
        {
			if(!condiction.finished)
            {
				return false;
			}
		}
		return true;
	}
	
	public void Awake()
    {
        if (dependencies == null)
            dependencies = new List<Interaction>();

        if (condictions == null)
            condictions = new List<Interaction>();

		doing = false;
		finished = false;
		if(HasActiveDependencie())
        {
			foreach(Interaction dependencie in dependencies)
            {
				dependencie.dependents.Add(this);
			}
			foreach(Interaction condiction in condictions)
            {
				condiction.enabled = false;
			}
		}
	}
	
	public void AddCondiction(Interaction condiction)
    {
        if (condictions == null)
            condictions = new List<Interaction>();

		this.condictions.Add (condiction);
		if(HasActiveDependencie())
        {
			condiction.enabled = false;
		}
	}
	
	public T GetDependencie<T>()where T : Interaction
    {
		if(dependencies == null || dependencies.Count == 0)
        {
			return null;
		}
		foreach(Interaction dependencie in dependencies)
        {
			T i = dependencie as T;
			if(i != null)
            {
				return i;
			}
		}
		return null;
	}
	
	public T GetDependent<T>()where T : Interaction
    {
		if(dependents == null || dependents.Count == 0)
        {
			return null;
		}
		foreach(Interaction dependent in dependents)
        {
			T i = dependent as T;
			if(i != null)
            {
				return i;
			}
		}
		return null;
	}
	
	public T GetCondiction<T>()where T : Interaction
    {
		if(condictions == null || condictions.Count == 0)
        {
			return null;
		}
		foreach(Interaction condiction in condictions)
        {
			T i = condiction as T;
			if(i != null)
            {
				return i;
			}
		}
		return null;
	}
	
	public void AddDependencie(Interaction d)
    {
		this.dependencies.Add(d);
        d.AddDependent(this);
	}

    public void RemoveDependencie(Interaction d)
    {
		int ind =0;
		foreach(Interaction inte in dependencies)
        {
			if(inte.GetInstanceID() == d.GetInstanceID())
            {
				dependencies.RemoveAt(ind);
				d.RemoveDependent(this);
				return;
			}
			ind ++;
		}
    }

    private void AddDependent(Interaction d)
    {
        dependents.Add(d);
    }

    private void RemoveDependent(Interaction d)
    {
        int ind =0;
		foreach(Interaction inte in dependents)
        {
			if(inte.GetInstanceID() == d.GetInstanceID())
            {
				dependents.RemoveAt(ind);
				return;
			}
			ind ++;
		}
    }

    public bool Repeatable
    {
        get
        {
            return repeatable;
        }
        set
        {
            repeatable = value;
        }
    }
	
	public bool IsRepeatable()
    {
		return repeatable;
	}
	
	public bool IsFinished()
    {
		return finished;
	}
	
	public virtual bool HasActiveCondiction()
    {
		foreach(Interaction it in condictions)
        {
			if(!it.IsFinished())
            {
				return true;
			}
		}
		return false;
	}

    public virtual bool HasActiveDependencie()
    {
        if (depType == VirtualInteraction.DependencyType.AND)
        {
            foreach (Interaction it in dependencies)
                if (!it.IsFinished())
                    return true;
            return false;
        }
        else
        {
            if (dependencies.Count == 0)
                return false;

            foreach (Interaction it in dependencies)
                if (it.IsFinished())
                    return false;
            return true;
        }
    }
	
	public void CheckDependencies()
    {
		if(!HasActiveDependencie())
        {
			foreach(Interaction it in condictions)
            {
				it.enabled = true;
				it.finished = false;
			}
//			finished = false;
			this.enabled = true;
		}
	}
	
	public bool isDoing()
    {
		return doing;
	}
	
	public void Before()
    {
		_Before();
	}
	
	public void Do()
    {
		doing = true;
		_Do ();
	}
	
	public void End()
    {
		doing = false;
		finished = true;
		this.enabled = false;
		foreach(Interaction d in dependents)
        {
			d.CheckDependencies();
		}
		_End();
		if(repeatable){
			CheckDependencies();
		}
	}
	
	public bool DependsOn(Interaction interaction){
		foreach(Interaction i in dependencies){
			if(!i.IsFinished() && i == interaction){
				return true;
			}
		}
		return false;
	}
	
	public void ClearDependencies(){
		if(dependencies == null){
			dependencies = new List<Interaction>();
		}
		else{
			dependencies.Clear();
		}
	}
	
	public void ClearDependents(){
		if(dependents == null){
			dependents = new List<Interaction>();
		}
		else{
			dependents.Clear();
		}
	}
	
	public void ClearCondictions(){
		if(condictions == null){
			condictions = new List<Interaction>();
		}
		else{
			condictions.Clear();
		}
	}
	
	public void ClearAll(){
		ClearCondictions();
		ClearDependencies();
		ClearDependents();
		this.finished = false;
		this.doing = false;
	}
}