using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//ref: https://unity3d.com/ru/learn/tutorials/modules/intermediate/live-training-archive/panels-panes-windows
public class DragPanel : MonoBehaviour, IPointerDownHandler,IBeginDragHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform root;

    private RectTransform canvasRectTransform;
    private RectTransform panelRectTransform;

    private bool moved = false;

	protected Vector3 initialDragPosition;

	public bool grandpa = false;

    public void OnPointerDown(PointerEventData data)
    {
            //		if (root != null) {
            panelRectTransform = transform.parent as RectTransform;
        Vector2 pointerOffset;
        //panelRectTransform.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out pointerOffset);
        //Debug.Log(pointerOffset);
			moved = false;
//		}
    }

	public void OnBeginDrag (PointerEventData eventData)
	{
		initialDragPosition = root.localPosition;
	}

    public virtual void OnDrag(PointerEventData data)
    {
        panelRectTransform = root.parent as RectTransform;
        canvasRectTransform = root.parent.transform as RectTransform;
        if (panelRectTransform == null)
            return;

        Vector2 pointerPostion = ClampToWindow(data.position);
		Vector2 pointerPressPostion = ClampToWindow(data.pressPosition);
        //root.rect.width*2
        //

        panelRectTransform = transform.parent as RectTransform;

        Vector2 localPointerPosition;
		Vector2 localPointerPressPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform, pointerPostion, data.pressEventCamera, out localPointerPosition) &&
		    RectTransformUtility.ScreenPointToLocalPointInRectangle(
			canvasRectTransform, pointerPressPostion, data.pressEventCamera, out localPointerPressPosition))

        {


            root.localPosition = initialDragPosition + (Vector3)(localPointerPosition - localPointerPressPosition);
        }

        moved = true;

    }

    Vector2 ClampToWindow(Vector2 pos)
    {
        canvasRectTransform = root.parent.transform as RectTransform;
		if (this.grandpa) {
			canvasRectTransform = canvasRectTransform.parent as RectTransform;		
		}
        return ClampToWindow(pos, canvasRectTransform);
    }

    Vector2 ClampToWindow(Vector2 pos, RectTransform _canvasRectTransform)
    {
        Vector2 rawPointerPosition = pos;

        Vector3[] canvasCorners = new Vector3[4];
        _canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners[0].x, canvasCorners[2].x);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners[0].y, canvasCorners[2].y);
        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (moved)
            GraphManager.SSave();
        moved = false;
    }
}