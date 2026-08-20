using UnityEngine;

public class Boss7_ExplodeGhost : EnemyEffectBullet
{
	[Header("\ufffd\ufffd\ufffd\ufffd")]
	public SpriteRenderer thisRenderer;

	public float rotateSpeed;

	private UnitProperty targetPpt;

	private Vector3 nowDirection;

	[Header("ײǽ\ufffd\ufffd\ufffdѵ\ufffd")]
	public int explodeCount;

	public void Initialize(Vector3 direction, UnitProperty ownerPpt, UnitProperty targetPpt, MiniObjPool pool)
	{
		base.direction = direction;
		nowDirection = direction;
		rigid.linearVelocity = direction.normalized * speed;
		base.ownerPpt = ownerPpt;
		this.targetPpt = targetPpt;
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
			if (targetPpt != null && Vector3.Dot(nowDirection, Tool2D.IgnoreZV2ToV1Normal(targetPpt.transform.position, base.transform.position)) > 0f)
			{
				nowDirection = Tool2D.RotateTowardsAroundZAxis(nowDirection, Tool2D.IgnoreZV2ToV1Normal(targetPpt.transform.position, base.transform.position), Time.deltaTime * speed * rotateSpeed);
			}
			base.transform.position += Time.deltaTime * speed * nowDirection;
			thisRenderer.flipX = nowDirection.x < 0f;
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

	private void Explode()
	{
		Tool2D.GetDir();
		for (int i = 0; i < explodeCount; i++)
		{
		}
	}

	public override void HitSolid(string hitTag)
	{
		hitParticle.Play();
		if (hitTag == "Wall")
		{
			base.state = BulletState.Hit;
		}
		thisTrigger.enabled = false;
	}
}
