using UnityEngine;
using System.Collections.Generic;

public class InteractionPanel : MonoBehaviour
{
    public InteractionManager manager;
    public RectTransform helpPanel;

    private void Start()
    {
        helpPanel.gameObject.SetActive(false);
    }

    internal void OnItemUsed(InteractionItem item, VirtualInteraction.InteractionType type)
    {
        manager.Create(type);
    }

    public void ToggleHelp()
    {
        var obj = helpPanel.gameObject;
        bool isOpen = obj.activeSelf;
        

        if (!isOpen)
        {
            obj.SetActive(true);

            iTween.ValueTo(helpPanel.gameObject, iTween.Hash(
               iT.ValueTo.from, 0, 
               iT.ValueTo.to, 1,
               iT.ValueTo.onupdate, "OnPanelMove",
               iT.ValueTo.onupdatetarget, gameObject,
               iT.ValueTo.time, .5
               
           ));
        }
        else
        {
            iTween.ValueTo(helpPanel.gameObject, iTween.Hash(
               iT.ValueTo.from, 1,
               iT.ValueTo.to, 0,
               iT.ValueTo.onupdate, "OnPanelMove",
               iT.ValueTo.onupdatetarget, gameObject,
               
                iT.MoveTo.oncompletetarget, gameObject,
                iT.MoveTo.oncomplete, "DisableHelpPanel",
               iT.ValueTo.time, .5f
           ));
        }
        
    }

    public void OnPanelMove(float pctg)
    {
        var pos = helpPanel.anchoredPosition;
        pos.x = -pctg * helpPanel.sizeDelta.x;
        helpPanel.anchoredPosition = pos;
    }

    private void DisableHelpPanel()
    {
        helpPanel.gameObject.SetActive(false);
    }
}