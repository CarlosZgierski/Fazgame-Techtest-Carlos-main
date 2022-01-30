using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;

public class MessageInteraction : Interaction
{
	
	public string content;
	private GameObject prefabScoreMessage;
	
	public iTween.EaseType BeforeEaseType = iTween.EaseType.easeOutBack;
	public float beforeTime = .3f;
	
	public iTween.EaseType AfterEaseType = iTween.EaseType.easeInBack;
	public float afterTime = .3f;

	private List<GameObject> gameObjectMessages;
	
	new void Awake(){
		base.Awake();
		prefabScoreMessage = Resources.Load("Message") as GameObject;
        
        gameObjectMessages = new List<GameObject>();
	}
	
	public override void Step(){
			Do();
	}
	
	public bool MessageEndAddMessageInteraction()
    {
        {
			if(gameObjectMessages.Count > 0)
            {
				iTween.ScaleTo(gameObjectMessages[0], iTween.Hash(iT.ScaleTo.scale, new Vector3(0,0,0),iT.ScaleTo.easetype, AfterEaseType, iT.ScaleTo.time, afterTime, iT.ScaleTo.oncompletetarget, gameObject, iT.ScaleTo.oncomplete, "DestroyMessage",iT.ScaleTo.oncompleteparams, this.GetInstanceID()));
			}
		}
        return true;
	}
	
	public bool DestroyMessage()
    {
		if(gameObjectMessages != null && gameObjectMessages.Count > 0)
        {
            End();
			foreach(GameObject gameObjectMessage in gameObjectMessages)
            {
				Destroy(gameObjectMessage);
			}
			gameObjectMessages.Clear();
		}
        return true;
	}

    public override void _End()
    {
        base._End();
        InteractionController.ControllerInstance.RemoveBoxInteraction(this);
    }
	
	public override void _Do()
    { 
        InteractionController.ControllerInstance.AddBoxInteraction(this);
		GameObject gameObjectMessage = Instantiate(prefabScoreMessage) as GameObject;
		InteractionController.ControllerInstance.AddCreatedGameObject(gameObjectMessage);
		gameObjectMessage.transform.Translate(0,
												   0,
												   -(0.1f * gameObjectMessages.Count));
		Vector3 atualSize = gameObjectMessage.transform.localScale;
		gameObjectMessage.transform.localScale = new Vector3(0,0,0);
		iTween.ScaleTo(gameObjectMessage, iTween.Hash(iT.ScaleTo.scale, atualSize, iT.ScaleTo.easetype, BeforeEaseType, iT.ScaleTo.time, beforeTime));
        Text textMesh = gameObjectMessage.transform.Find("Content").GetComponent<Text>();
		textMesh.text = content;
		Command command = gameObjectMessage.GetComponent<Command>();
        command.SetAction(MessageEndAddMessageInteraction);
		gameObjectMessages.Add(gameObjectMessage);
	}
}
