using UnityEngine;

public class Elite60HitEffect : MonoBehaviour
{
	public ParticleSystem particles;

	private bool isRecycled;

	private void OnEnable()
	{
		isRecycled = false;
	}

	private void Update()
	{
		if (!particles.IsAlive(withChildren: true) && !isRecycled)
		{
			base.gameObject.SetActive(value: false);
			ObjPoolMgr.Inst.RecycleGO(base.gameObject, 0.1f);
			isRecycled = true;
		}
	}
}
