using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{
    public Text title, message;

    private static ConfirmationDialog instance;

    private Action confirmationCallback, cancelCallback;

    private static ConfirmationDialog Instance
    {
        get
        {
            if (!instance)
            {
                ConfirmationDialog prefab = (Resources.Load("ConfirmationDialog") as GameObject).GetComponent<ConfirmationDialog>();
                instance = Instantiate(prefab);
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    public static void Open(string message_key, string title = null, Action confirmationCallback = null, Action cancelCallback = null)
    {
        if (title == null)
            title = "Título da Confirmação";
        ConfirmationDialog dialog = Instance;

        dialog.title.text = title;
        dialog.message.text = message_key;
        //dialog.message.text = MessageProvider.getMessage(message_key);
        dialog.gameObject.SetActive(true);

        dialog.confirmationCallback = confirmationCallback;
        dialog.cancelCallback = cancelCallback;
    }

    public void Confirm()
    {
        if (confirmationCallback != null)
            confirmationCallback();
        Dismiss();
    }

    public void Cancel()
    {
        if (cancelCallback != null)
            cancelCallback();
        Dismiss();
    }

    private void Dismiss()
    {
        confirmationCallback = null;
        cancelCallback = null;
        gameObject.SetActive(false);
    }
}
