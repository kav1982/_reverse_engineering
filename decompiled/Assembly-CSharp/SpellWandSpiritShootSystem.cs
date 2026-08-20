using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateBefore(typeof(SpellShootSystem))]
[CompilerGenerated]
public class SpellWandSpiritShootSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell4005WandSpiritData_RW_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_378481905_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4005WandSpiritData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (!GeneralTool.IsPlayerCanMotion())
		{
			return;
		}
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		using EntityQuery entityQuery = base.EntityManager.CreateEntityQuery(typeof(LocalTransform), typeof(Spell4005WandSpiritData), typeof(UnitProperty_Dots));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		SpellDashDriverSingleton singleton = __query_378481905_0.GetSingleton<SpellDashDriverSingleton>();
		for (int j = 0; j < nativeArray.Length; j++)
		{
			Entity entity = nativeArray[j];
			RefRW<Spell4005WandSpiritData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref base.CheckedStateRef, entity);
			RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity);
			RefRW<LocalTransform> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
			if (singleton.IsShooterDriving(entity))
			{
				continue;
			}
			Wand value = componentRWAfterCompletingDependency.ValueRW.Wand.Value;
			if (value == null || value.passiveAutoWandShooterData == null || value.passiveAutoWandShooterData.wandObjectScript == null)
			{
				continue;
			}
			SpellShootGroup currentShootGroup = value.currentShootGroup;
			float groupManaCost_FinalPlayerValue = value.currentShootGroup.GetGroupManaCost_FinalPlayerValue(value);
			if (currentShootGroup == null || !componentRWAfterCompletingDependency.ValueRW.ReadyToAttack || !value.CheckCurrentMpEnough(groupManaCost_FinalPlayerValue))
			{
				componentRWAfterCompletingDependency.ValueRW.ReadyToAttack = false;
				continue;
			}
			componentRWAfterCompletingDependency.ValueRW.ReadyToAttack = false;
			if (value.passiveChargeEnable)
			{
				if (value.ChargeStars.Count < value.passiveChargeCountLimit)
				{
					if (value.ChargeStars.Count == 0)
					{
						value.StartCharge();
					}
					value.TryChargeOnce();
				}
				else
				{
					value.ReleaseCharge();
				}
				continue;
			}
			SpellInitialParameter.Builder builder = value.CreateSIPBuilder(fromPostSlots: false);
			float3 @float = DTool.IgnoreZPosition(in componentRWAfterCompletingDependency3.ValueRW.Position);
			float3 wandLookDirection = componentRWAfterCompletingDependency.ValueRW.WandLookDirection;
			float3 float2 = @float + wandLookDirection * 0.3f;
			float3 float3 = new float3(0f, 0f, -0.4f);
			ShootSpellSpatialInfo shootSpellSpatialInfo = ShootSpellSpatialInfo.ToPoint(target: (float3)value.passiveAutoWandShooterData.wandObjectScript.lastFrameTargetPosition + float3, start: float2 + float3);
			float reverseCopyShootRate = ((PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy != null) ? ((float)PlayerMgr.Inst.ItemCtrller.relicCfg_SpellCopy.int1.result / 100f) : 0f);
			float groupManaCost_FinalPlayerValue2 = value.currentShootGroup.GetGroupManaCost_FinalPlayerValue(value);
			if (!value.CheckCurrentMpEnough(groupManaCost_FinalPlayerValue))
			{
				continue;
			}
			value.CostMp(groupManaCost_FinalPlayerValue2);
			if (value.WandCfg.specialAbility == WandAbility.FourDirShoot)
			{
				int i;
				for (i = 0; i < 4; i++)
				{
					ShootSpellSpatialInfo shootSpellSpatialInfo2 = shootSpellSpatialInfo.Copy();
					SpellInitialParameter.Builder builder2 = builder.Copy();
					shootSpellSpatialInfo2.Direction = Tool2D.GetDir(shootSpellSpatialInfo2.Direction, 90 * i);
					builder2.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
					{
						parameter.FourDirWandAngle = 90 * i;
					};
					ShootSpellUtils.ShootSpellGroup(currentShootGroup.Copy(), shootSpellSpatialInfo2, builder2, reverseCopyShootRate);
				}
			}
			else
			{
				ShootSpellUtils.ShootSpellGroup(currentShootGroup.Copy(), shootSpellSpatialInfo, builder, reverseCopyShootRate);
			}
			WandExtend.TryTriggerEchoEffect(value);
			componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref base.CheckedStateRef, entity);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.TakeKnockback(-wandLookDirection * value.currentShootGroup.GetGroupHighestRecoil() * UnityEngine.Random.Range(0.9f, 1.1f));
			componentRWAfterCompletingDependency.ValueRW.AttackDurationTimer = SpellGroupAttackDistanceCalculator.SpellGroupAttackDuration(currentShootGroup, value);
			componentRWAfterCompletingDependency.ValueRW.CurrentShootGroupMovementType = SpellGroupAttackDistanceCalculator.GetShootGroupMovementType(value.currentShootGroup, value);
			if (value.passiveBiAnBladeEnable)
			{
				value.PassiveTryShootBiAnBlade_Dots();
			}
			value.TryUseWandAbility_ChanceInstentCoolDownAndFullMana_FullMana();
			if (value.WandCfg.PostslotCastSpellChargeRatio > 0f)
			{
				value.ChargePostSlots(value.WandCfg.PostslotCastSpellChargeRatio);
			}
			value.EnterNextGroup(setCoolDownOrInterval: true);
		}
		shootSpellBuffer.Playback();
		shootSpellBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellDashDriverSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_378481905_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	protected override void OnCreateForCompiler()
	{
		base.OnCreateForCompiler();
		__AssignQueries(ref base.CheckedStateRef);
		__TypeHandle.__AssignHandles(ref base.CheckedStateRef);
	}

	[Preserve]
	public SpellWandSpiritShootSystem()
	{
	}
}
