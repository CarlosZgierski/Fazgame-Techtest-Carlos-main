using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DescriptableElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject description;
    private Vector3 backupPosition;

    void Start()
    {
		if (!description) {
			Destroy (this);
			return;
		}
        OnPointerExit(null);
    }

    public virtual void OnPointerEnter(PointerEventData d)
    {
        description.SetActive(true);
    }

    public void OnPointerExit(PointerEventData d)
    {
        description.SetActive(false);
    }

	//ref: http://answers.unity3d.com/questions/781726/how-do-i-add-a-listener-to-onpointerenter-ugui.html
	#region TriggerEventsSetup
	private void AddEventTrigger(UnityAction action, EventTriggerType triggerType)
	{
		// Create a nee TriggerEvent and add a listener
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
		trigger.AddListener((eventData) => action()); // you can capture and pass the event data to the listener
		// Create and initialise EventTrigger.Entry using the created TriggerEvent
		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
		// Add the EventTrigger.Entry to delegates list on the EventTrigger
		GetComponent<EventTrigger>().triggers.Add(entry);
	}
	private void AddEventTrigger(UnityAction<BaseEventData> action, EventTriggerType triggerType)
	{
		// Create a nee TriggerEvent and add a listener
		EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
		trigger.AddListener((eventData) => action(eventData)); // you can capture and pass the event data to the listener
		// Create and initialise EventTrigger.Entry using the created TriggerEvent
		EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };
		// Add the EventTrigger.Entry to delegates list on the EventTrigger
		GetComponent<EventTrigger>().triggers.Add(entry);
	}
	#endregion
}