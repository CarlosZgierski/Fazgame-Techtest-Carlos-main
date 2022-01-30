using UnityEngine;
using System.Collections.Generic;

public class QuestionItem : MessageItem
{
    public GameObject answerItemPrefab;
	public Transform answersContainer;
    public Transform plusButton;
    private List<AnswerItem> answers = new List<AnswerItem>();

    public override void Delete()
    {
        parent.DeleteQuestion(this);
    }

    public string[] Answers
    {
        get
        {
            string[] resp = new string[answers.Count];
            for (int i = 0; i < answers.Count; i++)
                resp[i] = answers[i].Text;
            return resp;
            //List<string> resp = new List<string>();
            //for (int i = 0; i < answers.Length; i++)
            //{
            //    string answer = answers[i].Text;
            //    //if (answer.Length > 0)
            //        resp.Add(answer);
            //}
            //return resp.ToArray();
        }
    }

    public override bool IsQuestion
    {
        get
        {
            return true;
        }
    }

    internal void QuestionSetup(Speech speech, VirtualAnswer[] answersTxt)
    {
        base.speech = speech;
        int count = Mathf.Max(answersTxt.Length, 2);
        for (int i = 0; i < count; i++)
        {
            AnswerItem item = i < answers.Count ? answers[i] : AddAnswer();
            if(i < answersTxt.Length)
                item.Text = answersTxt[i].Text;
        }

        //remove other questions
        for (int i = answersTxt.Length; i < answers.Count; i++)
            Delete(answers[i]);
    }

    public AnswerItem AddAnswer()
    {
        int nextIndex = answers.Count;
        GameObject obj = Instantiate(answerItemPrefab) as GameObject;
		obj.transform.SetParent(answersContainer,false);
		obj.SetActive (true);
        AnswerItem resp = obj.GetComponent<AnswerItem>();
        resp.Setup(this, nextIndex, string.Empty);
        answers.Add(resp);

//        plusButton.position = g[nextIndex + 1];

        return resp;
    }

	public void AddBlankAnswer(){
        if (answers.Count < 4)
            AddAnswer();
        else
			AlertDialog.OpenForInfo( "Limite de respostas alcançado!");
	}

    public void Delete(AnswerItem item)
    {
        if (answers.Count > 2)
        {
            int index = answers.IndexOf(item);
            Destroy(item.gameObject);
            answers.RemoveAt(index);
            for (int i = index; i < answers.Count; i++)
            {
                answers[i].Index = i;
//                answers[i].transform.position = g[i];
            }
//            plusButton.position = g[answers.Count];
        }
        else
        {
			AlertDialog.OpenForInfo("Não é possível ter menos de duas respostas.");
        }
    }

    internal void AddBlankAnswers()
    {
        AddAnswer();
        AddAnswer();
    }
}