using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
[RequireComponent(typeof(tk2dUIItem))]
public class Hoverable : MonoBehaviour
{
    private string defaultSpriteName;
    public string hoverSpriteName = null;

    private void Awake()
    {
        defaultSpriteName = SpriteName;
    }

    private void Start()
    {
        tk2dUIItem uiItem = GetComponent<tk2dUIItem>();
        uiItem.OnHoverOut += HoverOut;
        uiItem.OnHoverOver += uiItem_OnHoverOver;
        uiItem.isHoverEnabled = true;
    }

    private void uiItem_OnHoverOver()
    {
        bool useNameFromEditor = hoverSpriteName != null && hoverSpriteName.Length != 0;
        SpriteName = useNameFromEditor ? hoverSpriteName : defaultSpriteName + "_hover";
    }

    public void HoverOut()
    {
        SpriteName = defaultSpriteName;
    }

    private string SpriteName
    {
        get
        {
            return GetComponent<tk2dBaseSprite>().CurrentSprite.name;
        }
        set
        {
            tk2dBaseSprite s = GetComponent<tk2dBaseSprite>();
            s.spriteId = s.GetSpriteIdByName(value);
        }
    }

	#region TriggerEventsSetup
	public static void AddHoverIn(EventTrigger evs, UnityAction action)
	{
		AddEvent (evs, action, EventTriggerType.PointerEnter);
	}

	public static void AddHoverOut(EventTrigger evs, UnityAction action)
	{
		AddEvent (evs, action, EventTriggerType.PointerExit);
	}

	public static void AddEvent(EventTrigger evs, UnityAction action, EventTriggerType triggerType)
	{
		// Create a nee TriggerEvent and add a listener
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
		trigger.AddListener((eventData) => action()); // you can capture and pass the event data to the listener
		// Create and initialise EventTrigger.Entry using the created TriggerEvent
		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
		// Add the EventTrigger.Entry to delegates list on the EventTrigger
		evs.triggers.Add(entry);
	}
	#endregion
}*/