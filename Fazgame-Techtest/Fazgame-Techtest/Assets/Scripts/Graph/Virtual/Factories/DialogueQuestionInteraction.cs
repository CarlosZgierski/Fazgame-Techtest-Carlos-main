using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueQuestionInteraction : DialogueInteraction {
	
	public GameObject answerPrefab;
	
	public List<string> answersTexts;
	public List<AnswerInteraction> answersInteractions;
	
	private Transform grid;

    private string question;
	
	
	new void Awake(){
		base.Awake();

		prefabDialogue = Resources.Load("DialogueQuestionBox") as GameObject;
		answerPrefab = Resources.Load("Answer") as GameObject;
		if(answersTexts == null){
			answersTexts = new List<string>();
		}
		if(answersInteractions == null){
			answersInteractions = new List<AnswerInteraction>();
		}
	}
	
	public override void _Do(){
        grid = GetDialogueGameObject().transform.Find("Grid");
        base._Do();

		if(answersInteractions.Count == 0){
			foreach(string answerText in answersTexts){
				GameObject newAnswerGameObject = InstantiateAnswerPrefab();
				AnswerInteraction answerInteraction = newAnswerGameObject.GetComponent<AnswerInteraction>();
				answersInteractions.Add(answerInteraction);
				Text textMesh = newAnswerGameObject.GetComponentInChildren<Text>();
				textMesh.text = answerText;
			}
		}
		else if(answersInteractions.Count == answersTexts.Count){
			for(int i =0; i < answersTexts.Count; i++){
				AnswerInteraction answerInteraction = answersInteractions[i];
				answerInteraction.dialogueQuestionInteracion = this;
				GameObject newAnswerGameObject = Instantiate(answerPrefab) as GameObject;
				InteractionController.ControllerInstance.AddCreatedGameObject(newAnswerGameObject);
                newAnswerGameObject.transform.SetParent(grid, false);
				Command command = newAnswerGameObject.GetComponent<Command>();
				//command.target = answerInteraction.gameObject;
				//command.method = "MessageEndAnswerInteracion";
				//command.param = answerInteraction.GetInstanceID();
                command.SetAction(answerInteraction.MessageEndAnswerInteracion);

				Text textMesh = newAnswerGameObject.GetComponentInChildren<Text>();
				textMesh.text = answersTexts[i];

                answerInteraction.set_answer(answersTexts[i]);
                answerInteraction.set_question(get_text());
			}
		}
		else if(answersInteractions.Count != answersTexts.Count){
			Debug.Log("O número de interações é diferente do numero de textos.");
		}
	}
	
	private GameObject InstantiateAnswerPrefab()
    {
		GetDialogueGameObject();
		GameObject newAnswerGameObject = Instantiate(answerPrefab) as GameObject;
		InteractionController.ControllerInstance.AddCreatedGameObject(newAnswerGameObject);
		newAnswerGameObject.transform.parent = grid;
		Command command = newAnswerGameObject.GetComponent<Command>();
		//command.target = newAnswerGameObject;
		//command.method = "MessageEndAnswerInteracion";
		AnswerInteraction answerInteraction = newAnswerGameObject.AddComponent<AnswerInteraction>();
        //command.param = answerInteraction.GetInstanceID();
        command.SetAction(answerInteraction.MessageEndAnswerInteracion);
		return newAnswerGameObject;
	}
	
	public AnswerInteraction addAnswer(string t){
		GameObject newAnswerGameObject = new GameObject("AnswerObject"+t);
		AnswerInteraction answerInteraction = newAnswerGameObject.AddComponent<AnswerInteraction>();
		answerInteraction.dialogueQuestionInteracion = this;
		answersTexts.Add(t);
		answersInteractions.Add(answerInteraction);
		return answerInteraction;
	}
	
	public AnswerInteraction getAnswerInteraction(string answerText){
		int i = 0;
		foreach(string a in answersTexts){
			if(a.Equals(answerText)){
				return answersInteractions[i];
			}
			i++;
		}
		return null;
	}
}
