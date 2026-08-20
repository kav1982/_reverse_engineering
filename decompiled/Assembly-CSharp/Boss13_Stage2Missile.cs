using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13_Stage2Missile : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	[Header("爆炸参数")]
	public ShockParam shockParam;

	public float knockBack;

	public float explosionRadius;

	public int boomDamage;

	[Header("移动相关")]
	public Vector3 currentDir;

	public Vector3 targetDir;

	public float moveSpeed;

	public float turnSpeed;

	public Transform motion;

	public bool followMissile;

	public float aimInterval;

	public float aimIntervalTimer;

	public float followDuration;

	public float followDurationTimer;

	public bool canFollow;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228992u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
		canFollow = true;
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		base.transform.position += currentDir * moveSpeed * Time.deltaTime;
		currentDir = Vector3.Slerp(currentDir, targetDir, turnSpeed * Time.deltaTime);
		if (followMissile)
		{
			if (!canFollow)
			{
				aimIntervalTimer += Time.deltaTime;
				if (aimIntervalTimer > aimInterval)
				{
					followDurationTimer = 0f;
					canFollow = true;
				}
			}
			else
			{
				followDurationTimer += Time.deltaTime;
				if (followDurationTimer > followDuration)
				{
					aimIntervalTimer = 0f;
					canFollow = false;
				}
				targetDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
			}
		}
		motion.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, currentDir) + 90f);
		LocalTransform componentData = UnitDotsSyncSystem.GetComponentData<LocalTransform>(thisEntity);
		componentData.Position = base.transform.position;
		UnitDotsSyncSystem.SetComponentData(componentData, thisEntity);
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", explodePoint, 6f);
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster34_Trace", explodePoint, 10f);
		CamController.Inst.SetShock(shockParam);
		SEMgr.Inst.monster34Explosion.PlaySE();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, explosionRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, boomDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, explodePoint) * knockBack;
				info.damage = boomDamage;
				info.isUndifferDamage = true;
				if (layer == 131072)
				{
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		switch (UnitDotsSyncSystem.GetLayer(other))
		{
		case 131072u:
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
			info.damage = Boss10.Inst.ramDamage;
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			break;
		}
		case 256u:
		case 512u:
		case 2097152u:
			ExplodeOnce(base.transform.position);
			break;
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
