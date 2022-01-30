using UnityEngine;

public class DialogVertex : BasicVertex
{
    public RectTransform answersHolder;
    public GameObject answerPrefab;
    public float angleBetween = 30f;
    public float radius = .45f;
    public int testCount = 1;

    public override void Setup(VirtualInteraction interaction, GraphManager manager)
    {
        base.Setup(interaction, manager);
        Refresh();
    }

    private VirtualDialog dialogInteraction
    {
        get
        {
            return interaction as VirtualDialog;
        }
    }

    private void CleanUpAnswers()
    {
        foreach (Transform t in answersHolder)
        {
            AnswerVertex v = t.GetComponent<AnswerVertex>();
            v.DeleteVisually();
            //v.DeleteVisually();
            //Destroy(t.gameObject);
        }
        ChildVertices = new BasicVertex[0];
    }

    

    private void RefreshAnswers(VirtualDialog d)
    {
        VirtualAnswer[] answers = d.Answers;
        

        int count = answers.Length;
        BasicVertex[] children = new BasicVertex[count];

        float angleRange = (count - 1) * angleBetween;
        float halfAngle = angleRange / 2f;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = Vector3.right * radius;
            float pctg = 1f;
            if (count > 1)
                pctg = ((float)i) / (count - 1f);
            float angle = Mathf.Lerp(halfAngle, -halfAngle, pctg);
            Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle) , new Vector3(1,1,1));
            pos = mat.MultiplyPoint3x4(pos);

            GameObject obj = Instantiate(answerPrefab) as GameObject;
            Transform t = obj.transform;
            t.SetParent(answersHolder, false);
            t.localPosition = pos;
            t.localScale = new Vector3(1, 1, 1);

            AnswerVertex v = obj.GetComponent<AnswerVertex>();
            v.Setup(answers[i], i, manager, this);
            children[i] = v;
        }
        this.ChildVertices = children;
        
        manager.SetupAnswerEdges(children);

    }

    public override void Refresh()
    {
        base.Refresh();
        VirtualDialog d = dialogInteraction;
        CleanUpAnswers();
        if (d.HasQuestion)
        {
            RefreshAnswers(d);
        }
    }

    private Vector3 GetAnswerLocalPos(int index, int total)
    {
        float angleRange = (total - 1) * angleBetween;
        float halfAngle = angleRange / 2f;

        Vector3 pos = Vector3.right * radius;
        float pctg = 1f;
        if (total > 1)
            pctg = ((float)index) / (total- 1f);
        float angle = Mathf.Lerp(halfAngle, -halfAngle, pctg);
        Matrix4x4 mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), new Vector3(1, 1, 1));
        return mat.MultiplyPoint3x4(pos);
    }

    void OnDrawGizmosSelected()
    {
        for (int i = 0; i < testCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(GetAnswerLocalPos(i, testCount)), 0.1f);
        }
    }

	public override bool CanHaveDependencies
	{
		get
		{
			return this.ChildVertices.Length == 0 || !(this.ChildVertices[ChildVertices.Length -1] is AnswerVertex);
		}
	}
}