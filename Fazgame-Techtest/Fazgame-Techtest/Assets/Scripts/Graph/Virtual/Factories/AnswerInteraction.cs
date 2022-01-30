using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class AnswerInteraction : Interaction {
	
	public DialogueQuestionInteraction dialogueQuestionInteracion;

    private string question;
    private string answer;
	
	public override void _End()
    {
		if(dialogueQuestionInteracion && !dialogueQuestionInteracion.IsRepeatable())
        {
			dialogueQuestionInteracion.End();
		}
	}
	
	public bool MessageEndAnswerInteracion()
    {
        {

			End();
        return true;
		}
	}

    public void set_answer(string res)
    {
        this.answer = res;
    }

    public string get_answer()
    {
        return this.answer;
    }

    public void set_question(string per)
    {
        this.question = per;
    }

    public string get_question()
    {
        return this.question;
    }
}
