using UnityEngine;
using System.Xml;

public class VirtualAnswer : VirtualInteraction
{
    private string text;

    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            text = value;
        }
    }
	
	public VirtualDialog OwnerDialog
	{
		get
		{
            return Parent as VirtualDialog;
            //return ownerDialog;
		}
	}
	
	public override InteractionFactory GetFactory(){
		return new AnswerInteractionFactory();
	}

    private VirtualAnswer() : base(0, InteractionType.ANSWER) { }

    public VirtualAnswer(string text) : base(InteractionType.ANSWER)
    {
        this.text = text;
    }

    public override XmlNode ToXML(XmlDocument doc)
    {
        XmlElement element = base.ToXML(doc) as XmlElement;
        element.SetAttribute("answer", Text);
        return element;
    }

    public override void FillFromXML(XmlElement currentElement)
    {
        base.FillFromXML(currentElement);
        text = currentElement.GetAttribute("answer");
    }
    internal static VirtualInteraction Empty()
    {
        return new VirtualAnswer();
    }
}