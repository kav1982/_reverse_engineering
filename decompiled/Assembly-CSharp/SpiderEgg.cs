using UnityEngine;

public class SpiderEgg : LayerCorrect
{
	[Space(50f)]
	public Rigidbody rigid;

	public float speed;

	public float duration;

	public float knockback;

	public int damage;

	private UnitProperty ownerPpt;

	private Vector3 dir;

	private float durationTimer;

	private float upSpeed;

	private float gravity;

	private void Update()
	{
		upSpeed += gravity * Time.deltaTime;
		base.transform.position += new Vector3(0f, 0f, (0f - upSpeed) * Time.deltaTime);
		durationTimer += Time.deltaTime;
		if (durationTimer > duration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		if (base.transform.position.z >= 0f)
		{
			HitAndRecycle();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
		case "Player":
			if (other.IsPlayerTrigger())
			{
				other.GetComponent<UnitProperty>().TakeDamage(damage, ownerPpt, new TakeDamageInfo
				{
					isTrapDamage = true,
					knockbackForce = dir * knockback
				});
				HitAndRecycle();
			}
			break;
		case "Teammate":
		case "Monster":
		case "Destructible":
			other.GetComponent<UnitProperty>().TakeDamage(damage, ownerPpt, new TakeDamageInfo
			{
				isTrapDamage = true,
				knockbackForce = dir * knockback
			});
			HitAndRecycle();
			break;
		case "RollBall":
			other.GetComponentInParent<Spell1002RollBall>().TakeDamage(damage);
			HitAndRecycle();
			break;
		case "Butterfly":
			other.GetComponentInParent<Spell1003Butterfly>().HitEFAndRecycle();
			break;
		case "Wall":
			HitAndRecycle();
			break;
		case "SolidObj":
			HitAndRecycle();
			break;
		case "Brittleness":
			other.GetComponent<UnitProperty>().TakeDamage(damage, ownerPpt, new TakeDamageInfo
			{
				isTrapDamage = true,
				knockbackForce = dir * knockback
			});
			break;
		}
	}

	private void HitAndRecycle()
	{
		SEMgr.Inst.elite1WebDrop.PlaySE();
		QuickCreateSystem.Inst.CreateSpecialObj(201, Tool2D.IgnoreZPoint(base.transform));
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Ghost", base.transform.position, 1f);
		Object.Destroy(base.gameObject);
	}

	public void Initialize(UnitProperty ownerPpt, float upSpeed, float gravity, Vector3 landPoint)
	{
		this.ownerPpt = ownerPpt;
		this.upSpeed = upSpeed;
		this.gravity = gravity;
		float num = GeneralTool.CannonSpeed(upSpeed, 0f - base.transform.position.z, gravity, Tool2D.IgnoreZDistance(landPoint, base.transform.position));
		rigid.linearVelocity = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position) * num;
	}
}
