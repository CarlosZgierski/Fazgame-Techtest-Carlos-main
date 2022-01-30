using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class VirtualDialog : VirtualInteraction
{
    private Speech[] speeches;
    private Speech question;

    private VirtualDialog(ulong uid) : base(0, InteractionType.DIALOG) { }

    public VirtualDialog() : base(InteractionType.DIALOG)
    {
        this.speeches = new Speech[0];
    }

    public Speech[] Speeches
    {
        get
        {
            return speeches;
        }
        set
        {
            speeches = value;
        }
    }

    public Speech Question
    {
        get
        {
            return question;
        }
    }

    public VirtualAnswer[] Answers
    {
        get
        {
            List<VirtualAnswer> v = new List<VirtualAnswer>();
            foreach (VirtualInteraction child in Children)
                v.Add(child as VirtualAnswer);
            return v.ToArray();
            //if (answers == null)
            //    return new VirtualAnswer[0];
            //return answers;
        }
    }

    public bool HasQuestion
    {
        get
        {
            return question != null;
        }
    }

    /// <summary>
    /// converts this dialog back to a non-question dialog
    /// </summary>
    public void UndoQuestion()
    {
        question = null;
        foreach (VirtualAnswer va in Answers)
            va.Delete();
        ClearChildren();
    }

    public void SetQuestion(Speech question, string[] answersStr)
    {
        this.question = question;

        List<VirtualAnswer> answersToRemove = new List<VirtualAnswer>();
        VirtualInteraction[] children = Children;

        for (int i = 0; i < Mathf.Max(children.Length, answersStr.Length); i++)
        {
            if (i >= children.Length)
            {
                VirtualAnswer newAnswer = new VirtualAnswer(answersStr[i]);
                AddChild(newAnswer);
            }
            else
            {
                //saved answer exists
                VirtualAnswer oldAnswer = Children[i] as VirtualAnswer;

                bool inStringsRange = i < answersStr.Length;
                if (inStringsRange)
                    oldAnswer.Text = answersStr[i];
                else
                    answersToRemove.Add(oldAnswer);
            }
        }

        foreach (VirtualAnswer oldAnswer in answersToRemove)
        {
            oldAnswer.Delete();
            RemoveChild(oldAnswer);
        }
    }
	
	public override InteractionFactory GetFactory(){
		return new DialogueInteractionFactory();
	}

    public override XmlNode ToXML(XmlDocument doc)
    {
        XmlElement root = base.ToXML(doc) as XmlElement;

        foreach (Speech f in Speeches)
            root.AppendChild(f.ToXML(doc));

        if (HasQuestion)
        {
            XmlElement q = Question.ToXML(doc) as XmlElement;
            q.SetAttribute("question", "1");
            root.AppendChild(q);
        }

        return root;
    }

    public override void FillFromXML(XmlElement currentElement)
    {
        base.FillFromXML(currentElement);
        question = null;
        List<Speech> speechList = new List<Speech>();
        foreach (XmlElement e in currentElement.GetElementsByTagName("speech"))
        {
            Speech s = Speech.Empty();
            s.FillFromXML(e);
            string questionAttr = e.GetAttribute("question");
            if (questionAttr.Equals("1"))
                question = s;
            else
                speechList.Add(s);
        }
        if (speechList.Count == 0 && question == null)
        {
            Speech s = Speech.Empty();
            s.text = "";
            speechList.Add(s);
        }
        speeches = speechList.ToArray();
    }

    internal static VirtualInteraction Empty()
    {
        return new VirtualDialog(0);
    }
}