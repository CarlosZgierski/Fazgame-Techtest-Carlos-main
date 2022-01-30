using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GraphManager graph;

    public NewTalkDialog talkDialog;
    public MessageDialog messageDialog;
    public EndGameDialog endDialog;

    public void Create(VirtualInteraction.InteractionType type)
    {
        switch (type)
        {
            case VirtualInteraction.InteractionType.DIALOG:
                talkDialog.OpenForNew();
                break;
            case VirtualInteraction.InteractionType.MESSAGE:
                messageDialog.OpenForNew();
                break;
            case VirtualInteraction.InteractionType.GAME_END:
                endDialog.OpenForNew();
                break;
                
        }
    }

    public void Edit(VirtualInteraction interaction)
    {
        switch (interaction.Type)
        {
            case VirtualInteraction.InteractionType.DIALOG:
                talkDialog.OpenFromSaved(interaction as VirtualDialog);
                break;
            case VirtualInteraction.InteractionType.MESSAGE:
                messageDialog.OpenFromSaved(interaction as VirtualMessage);
                break;
            case VirtualInteraction.InteractionType.GAME_END:
                endDialog.OpenFromSaved(interaction as VirtualEndGame);
                break;
        }
    }
}