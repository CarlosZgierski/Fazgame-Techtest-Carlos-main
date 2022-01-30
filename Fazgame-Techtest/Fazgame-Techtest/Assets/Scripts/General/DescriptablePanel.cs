using UnityEngine;
using UnityEngine.EventSystems;

public class DescriptablePanel : DescriptableElement
{
    //public tk2dSlicedSprite bg;
    public Vector2 extraPixelSize = new Vector2(5, 5);

    public override void OnPointerEnter(PointerEventData d)
    {
        base.OnPointerEnter(d);
        Bounds textBounds = description.GetComponent<Renderer>().bounds;

        Vector3 newPos = textBounds.center;
        //newPos.z = bg.transform.position.z;
        //bg.transform.position = newPos;

        //Vector2 textPixelSize = WorldToScreenSize(textBounds.size, Camera.main);

        //bg.dimensions = textPixelSize + extraPixelSize;
    }

    public static Vector2 WorldToScreenSize(Vector2 size, Camera c)
    {
        float pixelRatio = (c.orthographicSize * 2) / c.pixelHeight;
        return size / pixelRatio;
    }
}
