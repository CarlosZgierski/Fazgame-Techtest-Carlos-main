using UnityEngine;
using System.Xml;

public abstract class SimpleObject
{
	public const double COUNTABLE_TIME_WITHOUT_SAVING = 180f;
	public static double elapsedTimeWithoutSaving;
    public bool active = true;

    private ulong id;
	private ulong elapsedSeconds;

	private float deltaTime = 0f;

    public SimpleObject()
    {
        id = 0;
    }

    public SimpleObject(ulong existingID)
    {
        this.id = existingID;
    }

	public ulong ElapsedSeconds{
		get{
			return this.elapsedSeconds;		
		}

		set{
			this.elapsedSeconds = value;		
		}
	}

    public ulong ID
    {
        get
        {
            return id;
        }
    }

	public void UpdateDeltaTime()
    {
        if (!active)
            return;

		if (elapsedTimeWithoutSaving < COUNTABLE_TIME_WITHOUT_SAVING) 
        {
			this.deltaTime += Time.deltaTime;
			this.elapsedSeconds += (ulong)this.deltaTime;
			this.deltaTime %= 1f;
		}
	}

    public override bool Equals(object obj)
    {
        if (obj is SimpleObject)
        {
            SimpleObject other = obj as SimpleObject;
            return id == other.id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (int)id;
    }

    public static bool operator ==(SimpleObject o1, SimpleObject o2)
    {
        if (((object)o1 == null) && ((object)o2 == null))
            return true;
        else if (((object)o1) == null || ((object)o2) == null)
            return false;
        return o1.Equals(o2);
    }

    public static bool operator !=(SimpleObject o1, SimpleObject o2)
    {
        return !(o1 == o2);
    }

    public static implicit operator bool(SimpleObject o)
    {
        return o != null;
    }

    protected void SetupToXML(XmlElement dest)
    {
        dest.SetAttribute("id", id.ToString());
		if (/*elapsedSeconds != null &&*/ elapsedSeconds != 0) {
			dest.SetAttribute ("elapsedSeconds", elapsedSeconds.ToString ());
		}
	}

    protected void SetupFromXML(XmlElement currentElement)
    {
        id = ulong.Parse(currentElement.GetAttribute("id"));
		if (currentElement.HasAttribute ("elapsedSeconds")) {
			this.elapsedSeconds = ulong.Parse(currentElement.GetAttribute("elapsedSeconds"));		
		}
    }
}