using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(EventTrigger))]
public class InteractionItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InteractionPanel parentPannel;
    public VirtualInteraction.InteractionType type;
    public Sprite btnHoverSprite;
    public Image btn;

    private Sprite original;

    private void Start()
    {
        original = btn.sprite;
    }

    public void OnClicked()
    {
        parentPannel.OnItemUsed(this, type);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        btn.sprite = btnHoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        btn.sprite = original;
    }
}