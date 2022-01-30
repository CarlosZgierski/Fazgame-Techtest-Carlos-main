using System.Xml;

public class VIDeserializer
{
    public static VirtualInteraction MakeInteraction(XmlElement element)
    {
        int typeNumber = int.Parse(element.GetAttribute("type"));
        VirtualInteraction.InteractionType type = (VirtualInteraction.InteractionType)typeNumber;
        switch (type)
        {
            case VirtualInteraction.InteractionType.DIALOG:
                return VirtualDialog.Empty();
            case VirtualInteraction.InteractionType.ANSWER:
                return VirtualAnswer.Empty();
            case VirtualInteraction.InteractionType.MESSAGE:
                return VirtualMessage.Empty();
            case VirtualInteraction.InteractionType.GAME_END:
                return VirtualEndGame.Empty();
        }
        return null;
    }
}