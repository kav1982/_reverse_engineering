using UnityEngine;

public class Boss7_Bullet : EnemyEffectBullet
{
	[Header("\ufffd\ufffd\ufffd\ufffd")]
	[Space(50f)]
	public float MaxSpeed;

	public float MinSpeed;

	public float SpeedSlowDownTime;

	public void Initialize(Vector3 direction, UnitProperty ppt, MiniObjPool pool)
	{
		base.direction = direction;
		rigid.linearVelocity = direction.normalized * speed;
		ownerPpt = ppt;
		ownerPool = pool;
	}

	public override void StateProcess()
	{
		switch (base.state)
		{
		case BulletState.Processing:
			if (changedState)
			{
				tsf_BulletHead.gameObject.SetActive(value: true);
				tsf_Shadow.gameObject.SetActive(value: true);
				tsf_BulletRoot.localScale = Vector3.one;
				tsf_Shadow.localScale = Vector3.one;
				trailParticle.Play();
			}
			rigid.linearVelocity = direction * Mathf.Lerp(MaxSpeed, MinSpeed, stateExistTime / SpeedSlowDownTime);
			break;
		case BulletState.Fade:
			if (changedState)
			{
				rigid.linearVelocity = Vector3.zero;
				thisTrigger.enabled = false;
				tsf_BulletHead.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				trailParticle.Stop();
			}
			tsf_BulletHead.localScale = Vector3.one * Mathf.Lerp(1f, 0f, stateExistTime / 0.2f);
			tsf_Shadow.localScale = tsf_BulletHead.localScale;
			if (stateExistTime > recycleTime)
			{
				ownerPool.RecycleGO(base.gameObject);
			}
			break;
		case BulletState.Hit:
			if (changedState)
			{
				rigid.linearVelocity = Vector3.zero;
				tsf_BulletHead.gameObject.SetActive(value: false);
				tsf_Shadow.gameObject.SetActive(value: false);
				trailParticle.Stop();
				hitParticle.Play();
			}
			if (stateExistTime > recycleTime)
			{
				ownerPool.RecycleGO(base.gameObject);
			}
			break;
		}
	}
}
