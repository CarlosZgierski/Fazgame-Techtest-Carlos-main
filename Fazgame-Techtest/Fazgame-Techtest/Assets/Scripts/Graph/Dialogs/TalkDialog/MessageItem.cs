using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MessageItem : MonoBehaviour
{
    public InputField messageInput;
    public AvatarItem avatarItem;

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
            avatarItem.ChooseNewAvatar(value.avatarId);
        }
        get
        {
            return new Speech(Text, avatarItem.avatarId);
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