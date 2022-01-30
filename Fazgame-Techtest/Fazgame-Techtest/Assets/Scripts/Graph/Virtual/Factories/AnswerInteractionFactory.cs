using UnityEngine;
using System.Collections.Generic;

public class AnswerInteractionFactory : InteractionFactory
{
	public List<Interaction> createInteraction(VirtualInteraction virtualInteraction, InteractionController interactionController)
	{
		VirtualAnswer answer = (VirtualAnswer) virtualInteraction;
		VirtualDialog dialog = answer.OwnerDialog;
		List<Interaction> list = interactionController.GetInteractions(dialog);
		DialogueQuestionInteraction dialogQuestion = list[list.Count -1] as DialogueQuestionInteraction;
		AnswerInteraction answerInteraction = dialogQuestion.addAnswer(answer.Text);
		interactionController.AddCreatedGameObject(answerInteraction.gameObject);
		List<Interaction> interactionList = new List<Interaction>();
		interactionList.Add(answerInteraction);
		return interactionList;
	}
	
}
