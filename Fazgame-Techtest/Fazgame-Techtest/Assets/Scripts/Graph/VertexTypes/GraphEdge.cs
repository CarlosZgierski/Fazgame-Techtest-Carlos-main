using UnityEngine;
using UnityEngine.UI;

public class GraphEdge : MonoBehaviour
{
    internal BasicVertex src, dest;
    private GraphManager manager;

    public Image arrow, check;

    
    public CheckSprites and, or;

    //public LineRenderer linedArrow;
    private const int VERTEX_COUNT = 10;
    private const float TANGENT_MULTIPLIER = 1f;
    private Button checkBtn;

    public void Setup(BasicVertex src, BasicVertex dest, GraphManager manager)
    {
        this.src = src;
        this.dest = dest;
        this.manager = manager;
        checkBtn = check.GetComponent<Button>();
        UpdateState();
    }

    void Start()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        enabled = (src != null) && (dest != null);
    }

    /// <summary>
    /// Update both the position and the rotation of the arrow so that it points from pos1 to pos2.
    /// </summary>
    /// <param name="arrow">The transform of the arrow itself.</param>
    /// <param name="pos1">Position of the source of the arrow.</param>
    /// <param name="pos2">Position of the destination of the arrow.</param>
    public static void SetArrowTransform(RectTransform arrow, Vector3 pos1, Vector3 pos2)
    {
        arrow.localRotation = GetRotation(pos1, pos2);
        Vector2 size = arrow.sizeDelta;
        size.x = Vector2.Distance(pos1, pos2);
        arrow.sizeDelta = size;
    }

    void Update()
    {
        bool typeAnd =  dest.interaction.DepType == VirtualInteraction.DependencyType.AND;
        var curSprites = typeAnd ? and : or ;
        check.sprite = curSprites.normal;
        
        var state = checkBtn.spriteState;
        state.highlightedSprite = curSprites.over;
        checkBtn.spriteState = state;

        Vector3 pos1 = src.transform.position;
        Vector3 pos2 = dest.transform.position;

        Vector3 newPos = (pos1 + pos2) / 2f;

        transform.position = newPos;

        check.transform.rotation = Quaternion.identity; // do not rotate the check!
    }

    public void SwitchDependencyType()
    {
        dest.SwitchDependencyType();
        arrow.GetComponent<Button>().Select();
    }

    public static Quaternion GetRotation(Vector2 src, Vector2 dest)
    {
        float angle = Vector3.Angle(dest- src, Vector3.right);
        if (dest.y < src.y)
            angle = -Mathf.Abs(angle);
        return Quaternion.Euler(0, 0, angle);
    }

    public void Delete()
    {
        iTween.ScaleTo(gameObject, ElementVertex.GetFadeOutHash(gameObject, "OnFadeOutComplete"));

    }

    private void OnFadeOutComplete()
    {
        dest.interaction.RemoveDependencie(src.interaction);
        manager.Delete(this);
        Destroy(gameObject);
    }

    [System.Serializable]
    public class CheckSprites
    {
        public Sprite normal, over;
    }
}