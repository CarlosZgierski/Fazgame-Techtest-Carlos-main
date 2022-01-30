using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class HoverablePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float enabledAlpha = 1;
    public Graphic [] graphics;
    public GameObject[] objects;

	public bool ignoreUI = false;

    private bool over = false;

    private void Update()
    {
		if (ignoreUI) {
			Vector2 mousePos = Input.mousePosition;
			Rect r = (transform as RectTransform).rect;
			r.center = transform.position;
			bool contains = r.Contains (mousePos);
			if (contains && !over) {
				//OnPointerEnter (null);
				SetActive(true);
			}

			if (!contains && over){
				//OnPointerExit (null);
				SetActive(false);
			}
			over = contains;
		}
    }

    private void SetActive(bool active)
    {
        foreach(var graphic in graphics)
        {
            Color c = graphic.color;
            c.a = active ? enabledAlpha : 0;
            graphic.color = c;
        }

        foreach (var obj in objects)
            obj.SetActive(active);
    }

    private void Start()
    {
        SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
		if(!ignoreUI)
        	SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if(!ignoreUI)
        	SetActive(false);
    }
}
