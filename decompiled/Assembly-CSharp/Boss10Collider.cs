using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Boss10Collider : UnitBase
{
	public List<Entity> hitList = new List<Entity>();

	public Boss10 master;

	public float clearListTime = 0.07f;

	public float clearListTimer;

	public GameObject ramCheckObj;

	public Boss10_AttackZone boss10_AttackZone;

	public Entity thisEntity { get; set; }

	public void SetCanBeTarget(bool value)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanBeTarget = value;
			componentData.CanTouch = value;
			SetComponentData(componentData);
		}
	}

	public unsafe void Init(UnityEngine.BoxCollider boxCollider)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		base.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
		Unity.Physics.BoxCollider* colliderPtr = (Unity.Physics.BoxCollider*)componentData2.ColliderPtr;
		BoxGeometry geometry = colliderPtr->Geometry;
		geometry.Size = boxCollider.size;
		geometry.Center = boxCollider.center;
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData2);
		componentData.CanTouch = false;
		componentData.CanBeTarget = true;
		SetComponentData(componentData);
		LocalTransform componentData3 = GetComponentData<LocalTransform>();
		componentData3.Scale = base.transform.localScale.x;
		SetComponentData(componentData3);
	}

	public void SyncPosition(Vector3 position, Vector3 eulerAngle)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			base.transform.position = position;
			base.transform.eulerAngles = eulerAngle;
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			componentData.Rotation = base.transform.rotation;
			SetComponentData(componentData);
		}
	}

	public override void Update()
	{
		if (!EntityIsValid(master.myPpt.myEntity))
		{
			return;
		}
		if (clearListTimer < clearListTime)
		{
			clearListTimer += Time.deltaTime;
		}
		else
		{
			clearListTimer = 0f;
			hitList.Clear();
		}
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(master.myPpt.myEntity, out var result))
		{
			if (componentData.affect_VenomCurrentStack > 0f)
			{
				result.SetVenom(componentData.affect_VenomDurationTimer, componentData.affect_VenomCurrentStack);
			}
			if (componentData.affect_burnDurationTimer > 0f)
			{
				result.SetBurn(componentData.affect_burnDurationTimer, componentData.affect_burnHPRatioPerHit);
			}
			if (componentData.voidEffectTimer > 0f)
			{
				result.SetVoid(componentData.voidExplosionData);
			}
			SetComponentData(result, master.myPpt.myEntity);
		}
		componentData.ClearVenomState();
		componentData.ClearBurnState();
		componentData.ClearVoidState();
		SetComponentData(componentData);
		base.Update();
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.spell.Entity != Entity.Null)
		{
			GetComponentData<UnitProperty_Dots>();
			if (!hitList.Contains(info.spell.Entity))
			{
				myPpt.TakeBeHit(info.spell.Config.Knockback * info.spell.Movement.Direction);
				info.beHitShakeDir = Vector3.zero;
				UnitDotsSyncSystem.AddTakeDamageRequest(master.myPpt.myEntity, info);
				hitList.Add(info.spell.Entity);
			}
			else
			{
				info.immuneDamage = true;
			}
		}
		else if (info.attackerEntity != Entity.Null)
		{
			UnitDotsSyncSystem.AddTakeDamageRequest(master.myPpt.myEntity, info);
		}
		else
		{
			UnitDotsSyncSystem.AddTakeDamageRequest(master.myPpt.myEntity, info);
		}
		info.immuneDamage = true;
		info.ignoreFloatText = true;
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (!master.myPpt.AlreadyDead)
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			info.stopAnnouncedDeath = true;
			componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
			SetComponentData(componentData);
		}
	}
}
