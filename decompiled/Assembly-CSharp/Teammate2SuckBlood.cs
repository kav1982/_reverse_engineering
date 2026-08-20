using UnityEngine;

public class Teammate2SuckBlood : MonoBehaviour
{
	public float lerpSpeed;

	private LineRenderer lr;

	private int index;

	private float currentLerp;

	private void Update()
	{
		if (lr != null && lr.gameObject.activeSelf)
		{
			currentLerp = Mathf.MoveTowards(currentLerp, 1f, lerpSpeed * Time.deltaTime);
			if (currentLerp == 1f)
			{
				currentLerp = 0f;
				index--;
				if (index == 1)
				{
					ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				}
			}
			base.transform.position = Vector3.Lerp(lr.GetPosition(index), lr.GetPosition(index - 1), currentLerp);
		}
		else
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void Initialize(LineRenderer lr)
	{
		this.lr = lr;
		index = lr.positionCount - 1;
		base.transform.position = lr.GetPosition(lr.positionCount - 1);
		currentLerp = 0f;
	}
}
