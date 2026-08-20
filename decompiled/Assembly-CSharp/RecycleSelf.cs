using UnityEngine;

public class RecycleSelf : MonoBehaviour
{
	public float delay;

	private float durationTimer;

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer >= delay)
		{
			durationTimer = 0f;
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
