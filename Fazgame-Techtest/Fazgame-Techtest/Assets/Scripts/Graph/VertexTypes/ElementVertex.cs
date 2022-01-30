using UnityEngine;
using UnityEngine.UI;

public class ElementVertex : BasicVertex
{
    public Image elementSprite;
    public GameObject editIcon;

    public override void Setup(VirtualInteraction interaction, GraphManager manager)
    {
        base.Setup(interaction, manager);
        if (!interaction.Editable)
            Destroy(editIcon);
    }

    public void SetSprite()
    {

    }

    public override GraphVertexModel Serialized
    {
        get
        {
            return new GraphVertexModel(this.interaction, Vector3.zero);
        }
    }

    protected override void UpdateSprite(VirtualInteraction interaction, GraphManager graphManager)
    {
        
    }
}