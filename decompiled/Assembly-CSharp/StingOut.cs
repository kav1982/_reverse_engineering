using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class StingOut : LayerCorrect, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public Rigidbody rigid;

	public float speed;

	public float duration;

	public float knockback;

	public int damage;

	public UnityEngine.Collider thisCollider;

	private SpecialObj6 owner;

	private Vector3 dir;

	private float durationTimer;

	public Entity thisEntity { get; set; }

	private void Update()
	{
		durationTimer += Time.deltaTime;
		if (durationTimer > duration)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void Initialize(SpecialObj6 ownerGO, Vector3 dir)
	{
		durationTimer = 0f;
		owner = ownerGO;
		this.dir = dir;
		rigid.linearVelocity = dir * speed;
		CollisionFilter filter_MonsterAoeUndiffer = GameConst.Filter_MonsterAoeUndiffer;
		filter_MonsterAoeUndiffer.CollidesWith |= 256u;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter_MonsterAoeUndiffer, thisCollider);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 8388608u:
		case 16777216u:
		{
			UnitDotsSyncSystem.ProcessHitSpell(other, damage, out var hitRollBall);
			if (hitRollBall)
			{
				HitAndRecycle();
			}
			break;
		}
		case 512u:
		case 2048u:
		case 4096u:
		case 8192u:
		case 32768u:
		case 131072u:
		case 2097152u:
		{
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(other, out var result))
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
				info.damage = damage;
				info.knockbackForce = dir * knockback;
				info.isTrapDamage = true;
				if (result.unitCfg.unitType != UnitType.Brittleness)
				{
					HitAndRecycle();
					UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
				}
			}
			break;
		}
		case 256u:
			if (other != ((IDotsPhysicsReciever)owner).thisEntity)
			{
				HitAndRecycle();
			}
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}

	private void HitAndRecycle()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_StingOutHit", base.transform.position, 1f);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}
}
