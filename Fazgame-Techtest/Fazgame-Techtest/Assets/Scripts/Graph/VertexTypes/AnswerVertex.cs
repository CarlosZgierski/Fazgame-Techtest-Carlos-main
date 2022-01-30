using UnityEngine;
using UnityEngine.UI;

public class AnswerVertex : BasicVertex
{
    public Text answerText;
    public GameObject answerArrowPrefab;
    public float arrowRelativeZ = 0.4f;

    private Transform parent;
    private Transform arrow;


    public void Setup(VirtualAnswer answer, int answerIndex, GraphManager manager, DialogVertex parent)
    {
        base.Setup(answer, manager);

        answerText.text = (answerIndex + 1).ToString();

        this.parent = parent.transform;
        GameObject arrowObject = Instantiate(answerArrowPrefab) as GameObject;
        arrow = arrowObject.transform;
        arrow.SetParent(transform, false);
        arrow.localScale = new Vector3(1, 1, 1);
        UpdateArrow();
    }

    protected override void Update()
    {
        UpdateArrow();
    }

    private void UpdateArrow()
    {
        Vector3 p1 = parent.position;
        Vector3 p2 = transform.position;
        arrow.position = (p1 + p2) / 2f;
        Vector3 diff = p2 - p1;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        arrow.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 localPos = arrow.localPosition;
        localPos.z = arrowRelativeZ;
        arrow.localPosition = localPos;
    }

    public override bool CanHaveDependent
    {
        get
        {
            return false;
        }
    }

    protected override void UpdateSprite(VirtualInteraction interaction, GraphManager graphManager)
    {
        //do nothing!
    }
}