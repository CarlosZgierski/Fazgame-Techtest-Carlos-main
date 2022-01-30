using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(CanvasGroup))]
public class UIWindow : MonoBehaviour {

	private CanvasGroup canvasGroup;
	private bool _active;
	public bool beginEnabled = false;

	public bool IsActive{
		get{
			return this._active;		
		}
		set{
			if(value){
				Enable();
			}
			else{
				Close ();
			}
		}
	}

	protected virtual void Awake(){
		Setup ();
		if (beginEnabled) Enable ();
		else Close ();
	}

	public void Setup(){
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	public void Close(){
		canvasGroup.alpha = 0f;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		this._active = false;
	}

	public void Enable(){
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		this._active = true;
	}

	public void Block(){
		canvasGroup.interactable = false;
	}

	public void Unblock(){
		canvasGroup.interactable = true;
	}
}
