using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss13DashDamageCheck : MonoBehaviour, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	public UnityEngine.CapsuleCollider CC;

	public float dashDamage;

	public Vector3 dashDir;

	public float knockbackForce;

	public List<Entity> hitEntities = new List<Entity>();

	public bool damageCheck;

	public bool canCollideEnvironment;

	public Entity thisEntity { get; set; }

	private void OnEnable()
	{
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 1073741824u;
		collisionFilter.CollidesWith = 2228736u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, CC);
	}

	private void OnDisable()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (hitEntities.Contains(other))
		{
			return;
		}
		uint layer = UnitDotsSyncSystem.GetLayer(other);
		if ((layer == 512 || layer == 131072 || layer == 2097152) && (damageCheck || (layer != 512 && layer != 2097152)) && (canCollideEnvironment || layer != 131072))
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss13.Inst.myPpt.myEntity);
			info.damage = dashDamage;
			info.teammateTakeDamageRatio = 4f;
			info.knockbackForce = dashDir * knockbackForce;
			info.ignorePlayerInvincibleFrame = true;
			if (layer == 131072)
			{
				info.damage = 99999f;
				info.ignoreFloatText = true;
			}
			UnitDotsSyncSystem.AddTakeDamageRequest(other, info);
			hitEntities.Add(other);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", UnitDotsSyncSystem.GetComponentData<LocalTransform>(other).Position, 1f);
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
