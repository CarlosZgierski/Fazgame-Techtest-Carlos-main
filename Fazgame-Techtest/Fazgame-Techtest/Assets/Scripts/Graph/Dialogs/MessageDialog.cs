using UnityEngine;
using UnityEngine.UI;

public class MessageDialog : SetupDialog<VirtualMessage>
{
    public InputField input;
    public GraphManager graphManager;

    protected override void SetupForNew()
    {
        input.text = string.Empty;
    }

    protected override void SubmitNew()
    {
        base.SubmitNew();
        VirtualMessage interaction = new VirtualMessage(input.text);
        graphManager.AddNewInteraction(interaction);
    }

    protected override void SetupFromSaved(VirtualMessage obj)
    {
        base.SetupFromSaved(obj);
        input.text = obj.content;
    }

    protected override void SubmitToSaved(VirtualMessage obj)
    {
        obj.content = input.text;
    }
}