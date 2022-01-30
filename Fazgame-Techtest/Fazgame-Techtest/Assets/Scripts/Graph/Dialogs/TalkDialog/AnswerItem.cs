using UnityEngine;
using UnityEngine.UI;

public class AnswerItem : MonoBehaviour
{
    public Text label;
    public InputField contentField;
    private QuestionItem parent;

    public void Setup(QuestionItem parent, int index, string text = null)
    {
        if (text == null)
            text = string.Empty;
        this.Text = text;
        this.Index = index;
        this.parent = parent;
    }

    public string Text
    {
        get
        {
            return contentField.text;
        }
        set
        {
            contentField.text = value;
        }
    }

    public int Index
    {
        set
        {
            this.label.text = (value + 1) + ")";
        }
    }

    public void Delete()
    {
        parent.Delete(this);
    }
}