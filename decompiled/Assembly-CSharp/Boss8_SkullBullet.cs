using UnityEngine;

public class Boss8_SkullBullet : UnitBase
{
	public float waitTime;

	private float waitTimer;

	public float angleSpeed;

	public float damage;

	public void Init(Vector3 dir)
	{
		base.CurrentMotion = base.MoveSpeed * dir;
	}

	public override void EveryInitialCallback()
	{
		waitTimer = 0f;
	}

	public override void Update()
	{
		base.Update();
		waitTimer += Time.deltaTime;
		if (waitTimer > waitTime)
		{
			Vector3 b = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
			base.CurrentMotion = base.MoveSpeed * Vector3.Lerp(base.CurrentMotion, b, angleSpeed * Time.deltaTime).normalized;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Teammate") || other.CompareTag("Brittleness") || other.CompareTag("Destructible"))
		{
			other.GetComponent<UnitProperty>().TakeDamage(damage, AttackerType.NothingSpecial);
			myPpt.AnnouncedDeath();
		}
		else if (other.CompareTag("Wall"))
		{
			myPpt.AnnouncedDeath();
		}
	}
}
