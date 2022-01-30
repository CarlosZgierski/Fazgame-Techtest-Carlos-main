using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NewTalkDialog : SetupDialog<VirtualDialog>
{
    private List<MessageItem> messages = new List<MessageItem>();

    public GameObject messagePrefab, questionPrefab;
    public GameObject buttonsHolder;
    public ScrollRect scrollRect;
    public GraphManager graphManager;

    int index = 0;

    protected override void SetupFromSaved(VirtualDialog di)
    {
        if(di.Speeches == null)
        {
            return;
        }

        CleanUp();
        Speech[] speeches = di.Speeches;

        int count = speeches.Length;
        for (int i = 0; i < count; i++)
        {
            MessageItem msg = CreateMessage(messagePrefab);
            Speech speech = speeches[i];
            msg.speech = speech;
        }
        if (di.HasQuestion)
        {
            QuestionItem q = AddQuestion();
            q.QuestionSetup(di.Question, di.Answers);
        }
        buttonsHolder.SetActive(!di.HasQuestion);
        ScrollUp();
    }

    protected override void SetupForNew()
    {
 	    CleanUp();
        ScrollUp();
        buttonsHolder.SetActive(true);
    }

    private MessageItem CreateMessage(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab) as GameObject;
        MessageItem msg = obj.GetComponent<MessageItem>();

        if (messages.Count > 0)
        {
            MessageItem lastMessage = messages[messages.Count - 1];
        }

        messages.Add(msg);
        msg.transform.SetParent(scrollRect.content, false);
		msg.Setup(this);
		msg.gameObject.SetActive (true);
		buttonsHolder.transform.SetAsLastSibling ();
        return msg;
    }

    public void AddBlankMessage()
    {
        CreateMessage(messagePrefab);
        ScrollDown();
    }

    public QuestionItem AddQuestion()
    {
        MessageItem resp = CreateMessage(questionPrefab);
        QuestionItem item = resp as QuestionItem;
        item.AddBlankAnswers();
        buttonsHolder.SetActive(false);
        return item;
    }

	public void AddBlankQuestion(){
		AddQuestion ();
        ScrollDown();
	}

    private void ScrollUp()
    {
        //ref: http://answers.unity3d.com/questions/801380/force-scrollbar-to-scroll-down-with-scrollrect.html
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1;
        Canvas.ForceUpdateCanvases();
    }

    private void ScrollDown()
    {
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
    }

    private System.Collections.Hashtable GetMovementHash(Vector3 dest)
    {
        Vector3 localPos = scrollRect.content.InverseTransformPoint(dest);
        return iTween.Hash(iT.MoveTo.time, 1.5f, iT.MoveTo.position, localPos, iT.MoveTo.islocal, true);
    }

    protected override void SubmitNew()
    {
        VirtualDialog i = new VirtualDialog();
		if(messages.Count > 0){
	        i.Speeches = MakeSpeeches(messages);

	        SetupForQuestion(i, messages);

	        graphManager.AddNewInteraction(i);
		}
		else{
			base.CheckError = true;
		}
    }

    protected override void SubmitToSaved(VirtualDialog i)
    {
		if(messages.Count > 0 ){
	        i.Speeches = MakeSpeeches(messages);
	        SetupForQuestion(i, messages);
		}
		else{
			base.CheckError = true;
		}
    }

    public void ToggleCharPanel(MessageItem item)
    {

    }


    internal void Delete(MessageItem messageItem)
    {
        int index = messages.IndexOf(messageItem);
        messages.Remove(messageItem);
        Destroy(messageItem.gameObject);
        Rearrange(index);
    }

    internal void DeleteQuestion(QuestionItem question)
    {
        Delete(question);
        buttonsHolder.SetActive(true);
    }

    private void Rearrange(int startingIndex)
    {

    }

    private void CleanUp()
    {
        foreach (MessageItem msg in messages)
        {
            Destroy(msg.gameObject);
        }
        messages.Clear();
    }

    private static Speech[] MakeSpeeches(List<MessageItem> msgs)
    {
        List<Speech> speeches = new List<Speech>();
        foreach (MessageItem msg in msgs)
        {
            if (!msg.IsQuestion)
                speeches.Add(msg.speech);
        }
        return speeches.ToArray();
    }

    private static bool HasQuestion(List<MessageItem> msgs)
    {
        return msgs.Count > 0 && msgs[msgs.Count - 1].IsQuestion;
    }

    protected static void SetupForQuestion(VirtualDialog di, List<MessageItem> msgs)
    {
        if (HasQuestion(msgs))
        {
            QuestionItem q = msgs[msgs.Count - 1] as QuestionItem;
            di.SetQuestion(q.speech, q.Answers);
        }
        else
            di.UndoQuestion();

    }
}