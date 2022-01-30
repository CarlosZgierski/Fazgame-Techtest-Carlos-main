using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DialogueInteraction : Interaction {

	public GameObject prefabDialogue;
    float speechTime;

    public string text = string.Empty;
    
	
	public float arriseTime = 0;
	public iTween.EaseType arriseEase = iTween.EaseType.easeOutElastic;
	
	private GameObject dialogueGameObject = null;
	
	new void Awake(){
		base.Awake ();
		prefabDialogue = Resources.Load("DialogueBox") as GameObject;
        
    }
	
	public GameObject GetDialogueGameObject(){
		if(dialogueGameObject == null){
			dialogueGameObject = Instantiate(prefabDialogue) as GameObject;
			InteractionController.ControllerInstance.AddCreatedGameObject(dialogueGameObject);
		}
        return dialogueGameObject;
	}
	
	public override void Step(){
			Do();
	}
	
	public override void _End(){
		InteractionController.ControllerInstance.RemoveBoxInteraction(this);
		Destroy(dialogueGameObject);
	}
	
	public bool DialogueInteractionEnd()
    {
        {
			End();
            return true;
		}
	}
	
	public override void _Do()
    {
		InteractionController.ControllerInstance.AddBoxInteraction(this);
        dialogueGameObject = GetDialogueGameObject();
		Command command = dialogueGameObject.GetComponent<Command>();
        command.SetAction(DialogueInteractionEnd);
        SetText();
        ApplyPlayerSprite();
		
		if(arriseTime > 0)
        {
			Vector3 scale = dialogueGameObject.transform.localScale;
			dialogueGameObject.transform.localScale = new Vector3(0,0,0);
			iTween.ScaleTo(dialogueGameObject,iTween.Hash(iT.ScaleTo.scale, scale, iT.ScaleTo.time, arriseTime, iT.ScaleTo.easetype, arriseEase));
		}
	}

    private void SetText()
    {
        Text textMesh = dialogueGameObject.transform.Find("Content").GetComponent<Text>();
        textMesh.text = text;
        speechTime = textMesh.text.Replace(" ", "").Length *0.1f;
    }
	
	private void ApplyPlayerSprite()
    {
		
	}
	
	public List<Speech> GetFalas(){
		List<Speech> falas = new List<Speech>();
		DialogueInteraction i = this;
		while(i != null){
			falas.Add(new Speech(i));
			i = i.GetDependent<DialogueInteraction>();
		}
		return falas;
	}
	
	public void AddFala(Speech f){
		DialogueInteraction i = this.GetDependent<DialogueInteraction>();
		DialogueInteraction lastDialogue = this;
		while(i != null){
			lastDialogue = i;
			i = i.GetDependent<DialogueInteraction>();
		}
		DialogueInteraction novaFala = lastDialogue.gameObject.AddComponent<DialogueInteraction>();
		novaFala.AddDependencie(lastDialogue);
		novaFala.text = f.text;
	}
	
	public List<Interaction> SetFalas(Speech[] falas){
		DialogueInteraction i = this;
		DialogueInteraction prev = null;
		DialogueInteraction next = null;
		List<Interaction> retorno = new List<Interaction>();
		if(falas == null || falas.Length == 0){
			Destroy(this);
			return retorno;
		}
		retorno.Add(this);
		foreach(Speech f in falas){
			if(i != null){
				next = i.GetDependent<DialogueInteraction>();
				prev = i.GetDependencie<DialogueInteraction>();
//				i.ClearAll();
			}
			else{
				i = gameObject.AddComponent<DialogueInteraction>();
				retorno.Add(i);
				if(prev != null){
					i.AddDependencie(prev);
				}
			}
			i.text = f.text;
			prev = i;
			i = next;
		}
		while(next != null){
			this.RemoveFala(falas.Length);
			next = next.GetDependent<DialogueInteraction>();
		}
		return retorno;
	}
	
	public void AddFalas(Speech[] falas){
		foreach(Speech f in falas){
			AddFala(f);
		}
	}
	
	public void RemoveFala(int index){
		DialogueInteraction i = this;
		while(i != null && index > 0){
			i = i.GetDependent<DialogueInteraction>();
			index --;
		}
		if(i != null){
			DialogueInteraction dialogoDependente = i.GetDependent<DialogueInteraction>();
			DialogueInteraction dialogoDependencia = i.GetDependencie<DialogueInteraction>();
			if(dialogoDependente != null){
				dialogoDependente.RemoveDependencie(i);
			}
			if(dialogoDependencia != null){
				i.RemoveDependencie(dialogoDependencia);
			}
			if(dialogoDependente != null && dialogoDependencia != null){
				dialogoDependente.AddDependencie(dialogoDependencia);
			}
			Destroy(i);
		}
	}

    public string get_text()
    {
        return this.text;
    }
}