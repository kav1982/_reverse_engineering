using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Relic_DruidRing : MonoBehaviour
{
	public Transform outerRing;

	public Transform innerRing;

	public float outerRingRotateSpeed;

	public float innerRingRotateSpeed;

	private RelicConfig relicCfg;

	private float attackTimer;

	private float attackInterval = 0.5f;

	private EntityManager ettMgr;

	private NativeList<Entity> targetEttList = new NativeList<Entity>(Allocator.Persistent);

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
	}

	public void Initialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
	}

	private void LateUpdate()
	{
		float num = relicCfg.float1.result * (1f + PlayerMgr.Inst.ExtraRadiusOfInfluence(isSpell: false));
		base.transform.position = PlayerMgr.Inst.PlayerPoint;
		base.transform.localScale = Vector3.one * num;
		outerRing.Rotate(0f, 0f, outerRingRotateSpeed * Time.deltaTime);
		innerRing.Rotate(0f, 0f, innerRingRotateSpeed * Time.deltaTime);
		attackTimer -= Time.deltaTime;
		if (attackTimer > 0f)
		{
			return;
		}
		attackTimer = attackInterval;
		if (!PlayerMgr.Inst.TryGetPlayerPpt(out var _))
		{
			return;
		}
		UnitDotsSyncSystem.GetAttackableEntitiesInRange(PlayerMgr.Inst.PlayerPoint, num, UnitType.Player, containsBrittleness: true, ref targetEttList);
		foreach (Entity targetEtt in targetEttList)
		{
			if (UnitDotsSyncSystem.entityMgr.HasComponent<UnitProperty_Dots>(targetEtt))
			{
				UnitProperty_Dots componentData = UnitDotsSyncSystem.entityMgr.GetComponentData<UnitProperty_Dots>(targetEtt);
				componentData.SetMucus(attackInterval + 0.1f, (float)relicCfg.int1.result / 100f, 1f, changeColor: false);
				UnitDotsSyncSystem.entityMgr.SetComponentData(targetEtt, componentData);
			}
		}
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		CollisionFilter @default = CollisionFilter.Default;
		@default.CollidesWith = 131072u;
		UnitDotsSyncSystem.GetCollidersInRange(PlayerMgr.Inst.PlayerPoint, num, @default, list);
		TakeDamageInfo_Dots damageInfo = TakeDamageInfo_Dots.NewInfo(PlayerMgr.Inst.PlayerEtt);
		damageInfo.damage = 10000f;
		damageInfo.ignoreFloatText = true;
		foreach (UnitDotsSyncSystem.DistanceHitResult item in list)
		{
			UnitDotsSyncSystem.DistanceHitResult current2 = item;
			UnitDotsSyncSystem.TryAttackEntity(in current2.entity, in damageInfo, ettMgr);
		}
		float num2 = (float)relicCfg.int2.result * attackInterval;
		GeneralTool.TryHealTargetTeammates(PlayerMgr.Inst.PlayerPoint, (int)num2, 0f, num, LevelMgr.Inst.CurrentRoomCtrller.TeammateEttList);
		GeneralTool.TryHealTargetTeammates(PlayerMgr.Inst.PlayerPoint, (int)num2, 0f, num, LevelMgr.Inst.CurrentRoomCtrller.TeammateNotAttackEttList);
	}

	private void OnDestroy()
	{
		if (targetEttList.IsCreated)
		{
			targetEttList.Dispose();
		}
	}
}
