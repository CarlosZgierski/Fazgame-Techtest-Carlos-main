using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MessageItem : MonoBehaviour
{
    public InputField messageInput;

    protected NewTalkDialog parent;

    public void Setup(NewTalkDialog parent)
    {
        this.parent = parent;
        gameObject.SetActive(true);
    }

    public string Text
    {
        get
        {
            return messageInput.text;
        }
        set
        {
            messageInput.text = value;
        }
    }

    public Speech speech
    {
        set
        {
            messageInput.text = value.text;
        }
        get
        {
            return new Speech(Text);
        }
    }


    public virtual void Delete()
    {
        parent.Delete(this);
    }

    public virtual bool IsQuestion
    {
        get
        {
            return false;
        }
    }
}