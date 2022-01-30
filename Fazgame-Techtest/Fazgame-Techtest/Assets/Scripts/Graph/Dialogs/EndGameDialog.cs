using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameDialog : SetupDialog<VirtualEndGame>
{
    public GraphManager graphManager;
    public Text title;
    public InputField input;


    protected override void SubmitNew()
    {
        base.SubmitNew();
        VirtualEndGame interaction = new VirtualEndGame(input.text);
        graphManager.AddNewInteraction(interaction);
    }

    protected override void SetupFromSaved(VirtualEndGame obj)
    {
        base.SetupFromSaved(obj);
        input.text = obj.content;
    }

    protected override void SubmitToSaved(VirtualEndGame obj)
    {
        obj.content = input.text;
    }
}
