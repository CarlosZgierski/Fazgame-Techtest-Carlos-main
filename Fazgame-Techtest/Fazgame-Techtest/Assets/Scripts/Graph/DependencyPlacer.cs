using UnityEngine;
using System.Linq;

public class DependencyPlacer : MonoBehaviour
{
    private BasicVertex currentSource;
    public Camera viewCamera;
    public GraphManager manager;

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        RectTransform rt = transform as RectTransform;
        
        Vector2 srcPos = (currentSource.transform as RectTransform).position;
        Vector2 mousePos = Input.mousePosition;
        
        //rt.position = srcPos; // (srcPos + mousePos) / 2;
        GraphEdge.SetArrowTransform(rt, srcPos, mousePos);
#if (UNITY_ANDROID || UNITY_IOS) && !(UNITY_EDITOR)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#else
        if (Input.GetMouseButtonDown(0))
#endif
        {
            BasicVertex hoveredVertex = manager.hoveredVertex;
            if (CanStablishDependency(currentSource, hoveredVertex))
                manager.StablishNewDependecy(currentSource, hoveredVertex);
            Deactivate();
        }
    }

    /// <summary>
    /// Checks if destination vertex is non-null, is different from the source, 
    /// is not one of its children and that source can have deps.
    /// </summary>
    /// <param name="src">Source vertex.</param>
    /// <param name="dest">Candidate destination vertex.</param>
    /// <param name="manager">Graph manager</param>
    /// <returns></returns>
    private static bool CanStablishDependency(BasicVertex src, BasicVertex dest)
    {
        return dest &&
            dest != src &&
            src.CanHaveDependencies &&
            dest.CanHaveDependent &&
            !src.ChildVertices.Contains(dest) &&
            !dest.ChildVertices.Contains(src);
    }

    public void Deactivate()
    {
        currentSource = null;
        gameObject.SetActive(false);
    }

    public void Activate(BasicVertex src)
    {
        this.currentSource = src;
        RectTransform rt = transform as RectTransform;
        Vector2 srcPos = (currentSource.transform as RectTransform).position;
        Vector2 size = rt.sizeDelta;
        size.x = 0;
        rt.sizeDelta = size;
        rt.position = srcPos;
        gameObject.SetActive(true);
    }
}