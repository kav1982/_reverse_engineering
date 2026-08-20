using UnityEngine;

public class Boss13HeadBulletTrail : MonoBehaviour
{
	public Boss13HeadBullet boss13HeadBullet;

	public bool follow;

	public float duration;

	public float durationTimer;

	private void OnDisable()
	{
		follow = false;
		durationTimer = 0f;
	}

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer > duration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		if (follow)
		{
			base.transform.position = boss13HeadBullet.transform.position;
		}
	}
}
