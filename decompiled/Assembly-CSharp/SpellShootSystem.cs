using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[CompilerGenerated]
public struct SpellShootSystem : ISystem, ISystemCompilerGenerated
{
	private struct TypeHandle
	{
		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellSplitComponentData> __SpellSplitComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<TeammateData> __TeammateData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RO_ComponentLookup;

		public ComponentLookup<SpellSpeedRatioValueData> __SpellSpeedRatioValueData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<MultiShootData> __MultiShootData_RO_ComponentLookup;

		public ComponentLookup<MultiShootData> __MultiShootData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellRevertDirection> __SpellRevertDirection_RO_ComponentLookup;

		public ComponentLookup<SpellRevertDirection> __SpellRevertDirection_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<ManaCostRatio> __ManaCostRatio_RO_ComponentLookup;

		public ComponentLookup<ManaCostRatio> __ManaCostRatio_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellChargeData> __SpellChargeData_RO_ComponentLookup;

		public ComponentLookup<SpellChargeData> __SpellChargeData_RW_ComponentLookup;

		public ComponentLookup<TeammateData> __TeammateData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellPlayShootSETag> __SpellPlayShootSETag_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RO_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellMoveTriggerComponentData> __SpellMoveTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellHitTriggerComponentData> __SpellHitTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellOverSplitTriggerComponentData> __SpellOverSplitTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellOverTriggerComponentData> __SpellOverTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<SpellTwineTriggerComponentData> __SpellTwineTriggerComponentData_RW_ComponentLookup;

		public ComponentLookup<Parent> __Unity_Transforms_Parent_RW_ComponentLookup;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		[ReadOnly]
		public BufferLookup<UnitKeepCastingSpells> __UnitKeepCastingSpells_RO_BufferLookup;

		[ReadOnly]
		public ComponentLookup<SpellChargingTag> __SpellChargingTag_RO_ComponentLookup;

		public BufferLookup<UnitKeepCastingSpells> __UnitKeepCastingSpells_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4004StartData> __Spell4004StartData_RO_ComponentLookup;

		public ComponentLookup<SpellChargingTag> __SpellChargingTag_RW_ComponentLookup;

		[ReadOnly]
		public EntityStorageInfoLookup __EntityStorageInfoLookup;

		[ReadOnly]
		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RO_ComponentLookup;

		public ComponentLookup<Spell3007LightningChainEffect> __Spell3007LightningChainEffect_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__SpellMovementComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>();
			__SpellElementEffectComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellElementEffectComponentData>();
			__SpellSplitComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellSplitComponentData>();
			__SpellNeedResize_RW_ComponentLookup = state.GetComponentLookup<SpellNeedResize>();
			__TeammateData_RO_ComponentLookup = state.GetComponentLookup<TeammateData>(isReadOnly: true);
			__SpellSpeedRatioValueData_RO_ComponentLookup = state.GetComponentLookup<SpellSpeedRatioValueData>(isReadOnly: true);
			__SpellSpeedRatioValueData_RW_ComponentLookup = state.GetComponentLookup<SpellSpeedRatioValueData>();
			__MultiShootData_RO_ComponentLookup = state.GetComponentLookup<MultiShootData>(isReadOnly: true);
			__MultiShootData_RW_ComponentLookup = state.GetComponentLookup<MultiShootData>();
			__SpellRevertDirection_RO_ComponentLookup = state.GetComponentLookup<SpellRevertDirection>(isReadOnly: true);
			__SpellRevertDirection_RW_ComponentLookup = state.GetComponentLookup<SpellRevertDirection>();
			__ManaCostRatio_RO_ComponentLookup = state.GetComponentLookup<ManaCostRatio>(isReadOnly: true);
			__ManaCostRatio_RW_ComponentLookup = state.GetComponentLookup<ManaCostRatio>();
			__SpellChargeData_RO_ComponentLookup = state.GetComponentLookup<SpellChargeData>(isReadOnly: true);
			__SpellChargeData_RW_ComponentLookup = state.GetComponentLookup<SpellChargeData>();
			__TeammateData_RW_ComponentLookup = state.GetComponentLookup<TeammateData>();
			__SpellPlayShootSETag_RO_ComponentLookup = state.GetComponentLookup<SpellPlayShootSETag>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__Unity_Physics_PhysicsCollider_RO_ComponentLookup = state.GetComponentLookup<PhysicsCollider>(isReadOnly: true);
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellMoveTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellMoveTriggerComponentData>();
			__SpellHitTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellHitTriggerComponentData>();
			__SpellOverSplitTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellOverSplitTriggerComponentData>();
			__SpellOverTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellOverTriggerComponentData>();
			__SpellTwineTriggerComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellTwineTriggerComponentData>();
			__Unity_Transforms_Parent_RW_ComponentLookup = state.GetComponentLookup<Parent>();
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
			__UnitKeepCastingSpells_RO_BufferLookup = state.GetBufferLookup<UnitKeepCastingSpells>(isReadOnly: true);
			__SpellChargingTag_RO_ComponentLookup = state.GetComponentLookup<SpellChargingTag>(isReadOnly: true);
			__UnitKeepCastingSpells_RW_BufferLookup = state.GetBufferLookup<UnitKeepCastingSpells>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Spell4004StartData_RO_ComponentLookup = state.GetComponentLookup<Spell4004StartData>(isReadOnly: true);
			__SpellChargingTag_RW_ComponentLookup = state.GetComponentLookup<SpellChargingTag>();
			__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
			__SpellConfigComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>(isReadOnly: true);
			__Spell3007LightningChainEffect_RW_ComponentLookup = state.GetComponentLookup<Spell3007LightningChainEffect>();
		}
	}

	private Entity? _lastSpellEntity;

	public static int CurrentFrameMultiShootCount;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1145758856_0;

	private EntityQuery __query_1145758856_1;

	private EntityQuery __query_1145758856_2;

	private EntityQuery __query_1145758856_3;

	private EntityQuery __query_1145758856_4;

	private EntityQuery __query_1145758856_5;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<DynamicOptimizeData>();
		state.EntityManager.CreateSingletonBuffer<SpellSpawnParams>();
		state.RequireForUpdate<SpellSingleton>();
		_lastSpellEntity = null;
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSingleton singleton = __query_1145758856_0.GetSingleton<SpellSingleton>();
		foreach (SpellSpawnParams item in __query_1145758856_1.GetSingletonBuffer<SpellSpawnParams>().ToNativeArray(Allocator.Temp))
		{
			SpawnSpellEntity(ref state, item, singleton);
		}
		__query_1145758856_1.GetSingletonBuffer<SpellSpawnParams>().Clear();
		DynamicBuffer<Spell3007CreateRequest> singletonBuffer = __query_1145758856_2.GetSingletonBuffer<Spell3007CreateRequest>();
		if (singletonBuffer.Length > 0)
		{
			NativeArray<Spell3007CreateRequest> nativeArray = singletonBuffer.ToNativeArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				Spell3007CreateRequest req = nativeArray[i];
				SpawnLightningChainForRequest(ref state, in req, singleton);
			}
			__query_1145758856_2.GetSingletonBuffer<Spell3007CreateRequest>().Clear();
		}
		CurrentFrameMultiShootCount = 0;
	}

	private unsafe void SpawnSpellEntity(ref SystemState state, SpellSpawnParams spawnParams, SpellSingleton spellSingleton)
	{
		FixedString64Bytes fs = $"Spell_{spawnParams.PrefabId}";
		Entity entity = state.EntityManager.Instantiate(spellSingleton.Prefabs[fs]);
		FixedString64Bytes fixedString64Bytes = fs;
		if (spawnParams.IsSplitSpell)
		{
			fixedString64Bytes = $"{fixedString64Bytes}_Split";
		}
		state.EntityManager.SetName(entity, fixedString64Bytes);
		RefRW<SpellComponentData> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, entity);
		ref SpellComponentData valueRW = ref componentRWAfterCompletingDependency.ValueRW;
		valueRW.InShootIndex = spawnParams.InShootCountIndex;
		valueRW.IsSplitSpell = spawnParams.IsSplitSpell;
		valueRW.SubGroupEntity = spawnParams.SubGroupEntity;
		valueRW.Wand = spawnParams.Wand;
		valueRW.Shooter = spawnParams.Shooter;
		valueRW.OwnerEntity = spawnParams.OwnerUnit;
		valueRW.FromPostSlot = spawnParams.FromPostSlot;
		valueRW.PrefabId = spawnParams.PrefabId;
		valueRW.SpellEfficiency = spawnParams.SpellEfficiency;
		valueRW.EnableTriggerRedRune = spawnParams.EnableTriggerRedRune;
		valueRW.EnableConvertOverFlowCCToDamage = spawnParams.OverFlowCriticalChanceToDamage;
		SpellElementEffectComponentData elementComponentData = spawnParams.ElementComponentData;
		elementComponentData.VenomApplyCount *= spawnParams.SpellEfficiency;
		spellSingleton.SpellSpawnParamsStorage[entity] = new SpellSpawnParamsStorage(spawnParams);
		state.EntityManager.AddComponent<SpellCleanup>(entity);
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref state, spawnParams.ConfigComponentData, entity);
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state, spawnParams.MovementComponentData, entity);
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentLookup, ref state, elementComponentData, entity);
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSplitComponentData_RW_ComponentLookup, ref state, spawnParams.SplitComponentData, entity);
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RW_ComponentLookup, ref state, entity).ValueRW.ExtraSizeRatio = spawnParams.SpellExtraSizeRatio;
		SpellAbilityType abilityType;
		if (!spawnParams.MovementComponentData.IsFallSpell && spawnParams.HalfLifeTeleportCount > 0 && !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, entity))
		{
			abilityType = spawnParams.ConfigComponentData.AbilityType;
			if (abilityType != SpellAbilityType.RuneHammer && abilityType != SpellAbilityType.Laser && abilityType != SpellAbilityType.LaserBeam && abilityType != SpellAbilityType.DragonBreath && abilityType != SpellAbilityType.HighPressureWasher)
			{
				state.EntityManager.AddComponent<SpellHalfLifeTeleportData>(entity);
				state.EntityManager.SetComponentData(entity, new SpellHalfLifeTeleportData
				{
					TeleportCount = spawnParams.HalfLifeTeleportCount,
					TeleportRadius = spawnParams.HalfLifeTeleportRadius
				});
				state.EntityManager.SetComponentEnabled<SpellHalfLifeTeleportData>(entity, value: false);
			}
		}
		if (spawnParams.SpellEndTeleport && !spawnParams.IsSplitSpell)
		{
			state.EntityManager.AddComponent<SpellEndTeleportTag>(entity);
		}
		if (spawnParams.RandomPosFocusMouse)
		{
			state.EntityManager.AddComponent<SpellRemoteShootTag>(entity);
		}
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellSpeedRatioValueData_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellSpeedRatioValueData_RW_ComponentLookup, ref state, new SpellSpeedRatioValueData
			{
				Speed = spawnParams.SpeedAttribute
			}, entity);
		}
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__MultiShootData_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__MultiShootData_RW_ComponentLookup, ref state, new MultiShootData
			{
				Count = spawnParams.MultiShootAddictionCount
			}, entity);
		}
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellRevertDirection_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellRevertDirection_RW_ComponentLookup, ref state, new SpellRevertDirection
			{
				Revert = spawnParams.ReserveDirection
			}, entity);
		}
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__ManaCostRatio_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__ManaCostRatio_RW_ComponentLookup, ref state, new ManaCostRatio
			{
				ratio = spawnParams.ManaCostRatio
			}, entity);
		}
		if (spawnParams.radiuDcreaseTransIntoDamageRatio != 0f)
		{
			state.EntityManager.AddComponent<SpellradiuDcreaseTransIntoDamageData>(entity);
			state.EntityManager.SetComponentData(entity, new SpellradiuDcreaseTransIntoDamageData
			{
				radiuDecreaseRatio = spawnParams.radiuDecreaseRatio,
				radiuDcreaseTransIntoDamageRatio = spawnParams.radiuDcreaseTransIntoDamageRatio
			});
		}
		if (!Mathf.Approximately(spawnParams.radiuDecreaseRatio, 1f) && !spawnParams.MovementComponentData.IsFallSpell && (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.RuneHammer || spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.DragonBreath))
		{
			state.EntityManager.AddComponent<SpellDecreaseRadiusData>(entity);
			state.EntityManager.SetComponentData(entity, new SpellDecreaseRadiusData
			{
				RadiusMult = spawnParams.radiuDecreaseRatio,
				RadiusToDamageRatio = spawnParams.radiuDcreaseTransIntoDamageRatio
			});
		}
		if (spawnParams.FourDirectionWandAngle != 0f)
		{
			state.EntityManager.AddComponent<SpellFromFourDirectionWandData>(entity);
			state.EntityManager.SetComponentData(entity, new SpellFromFourDirectionWandData
			{
				Angle = spawnParams.FourDirectionWandAngle
			});
		}
		if (spawnParams.ChargeTimer > 0f && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargeData_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellChargeData_RW_ComponentLookup, ref state, entity).ValueRW.ChargeTimer = spawnParams.ChargeTimer;
		}
		if (spawnParams.ConfigComponentData.IsTeammate && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__TeammateData_RW_ComponentLookup, ref state, spawnParams.TeammateComponentData, entity);
		}
		if (spawnParams.MovementComponentData.IsFallSpell)
		{
			state.EntityManager.AddComponent<SpellFallTag>(entity);
		}
		if (spawnParams.DisableResize)
		{
			state.EntityManager.RemoveComponent<SpellNeedResize>(entity);
		}
		FixedString32Bytes seName;
		if (!spawnParams.DisableShootSound && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellPlayShootSETag_RO_ComponentLookup, ref state, entity) && (spawnParams.PrefabId != 1031 || !spawnParams.IsSplitSpell))
		{
			DynamicBuffer<SEData> singletonBuffer = __query_1145758856_3.GetSingletonBuffer<SEData>();
			int prefabId = spawnParams.PrefabId;
			seName = "Shoot";
			singletonBuffer.Add(new SEData(DTool.GetSpellSEName(prefabId, in seName)));
		}
		if (spawnParams.DropCoinRatioOnKill > 0f)
		{
			state.EntityManager.AddComponent<SpellOnKillDropCoin>(entity);
			state.EntityManager.SetComponentData(entity, new SpellOnKillDropCoin
			{
				DropRatio = spawnParams.DropCoinRatioOnKill
			});
		}
		if (spawnParams.DropCrystalRatioOnKill > 0f)
		{
			state.EntityManager.AddComponent<SpellOnKillDropCrystal>(entity);
			state.EntityManager.SetComponentData(entity, new SpellOnKillDropCrystal
			{
				DropRatio = spawnParams.DropCrystalRatioOnKill
			});
		}
		if (spawnParams.RefractionData.RemainCount > 0)
		{
			state.EntityManager.AddComponent<SpellRefractionData>(entity);
			state.EntityManager.SetComponentData(entity, new SpellRefractionData
			{
				RemainCount = spawnParams.RefractionData.RemainCount
			});
			state.EntityManager.AddBuffer<SpellRefractionHitEntities>(entity);
		}
		ref LocalTransform valueRW2 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, entity).ValueRW;
		ref SpellMovementComponentData valueRW3 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RW_ComponentLookup, ref state, entity).ValueRW;
		switch (valueRW3.Type)
		{
		case SpellSpecialMovementType.Rotation:
		{
			if (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.ShotGun && !spawnParams.IsSplitSpell)
			{
				valueRW3.AroundAngle = (float)spawnParams.InShootCountIndex * 360f / (float)(spawnParams.ConfigComponentData.Level + 2);
			}
			float3 @float = spawnParams.SpawnPosition;
			if (spawnParams.RandomPosFocusMouse)
			{
				valueRW3.AroundTarget = Entity.Null;
			}
			if (state.EntityManager.Exists(valueRW3.AroundTarget) && state.EntityManager.HasComponent<LocalTransform>(valueRW3.AroundTarget))
			{
				@float = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, valueRW3.AroundTarget).Position;
			}
			if (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.MagicBreaker && !valueRW3.IsFallSpell && spawnParams.IsSplitSpell)
			{
				@float = valueRW3.AroundCenter;
			}
			valueRW2.Position = valueRW3.UpdateAroundFollowAndGetAroundPositionWhenAround(@float);
			if (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.Boomerang)
			{
				valueRW2.Position = @float;
			}
			if (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.Summon3 && spawnParams.IsFuseTeammate)
			{
				valueRW2.Position = spawnParams.SpawnPosition;
			}
			valueRW2.Position.z = spawnParams.SpawnPosition.z;
			valueRW3.Direction = math.normalize(Tool2D.GetDir(valueRW3.AroundAngle + 90f));
			break;
		}
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.ChaseMouse:
		case SpellSpecialMovementType.ChaseOwner:
			valueRW2.Position = spawnParams.SpawnPosition;
			break;
		}
		InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref state, entity).ValueRW.Linear = spawnParams.MovementComponentData.Direction * spawnParams.MovementComponentData.Speed;
		if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state, entity))
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, entity).ValueRW.MakeUnique(in entity, state.EntityManager);
			PhysicsCollider collider = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RO_ComponentLookup, ref state, entity).ValueRO;
			if (spawnParams.MovementComponentData.Type == SpellSpecialMovementType.Rotation || spawnParams.MovementComponentData.IsIgnoreWall)
			{
				DTool.ChangeCollisionFilter(in collider, 0u, BitOperator.Or, 4294967039u, BitOperator.And, changeTrigger: true, changeCollider: true);
			}
			int id = spawnParams.ConfigComponentData.Id;
			if (id < 20000 || id >= 30000)
			{
				if (DTool.IsSameCamp(spawnParams.ConfigComponentData.ShooterType, UnitType.Monster))
				{
					DTool.ChangeCollisionFilter(in collider, 0u, BitOperator.Or, 16777728u, BitOperator.Or, changeTrigger: true, changeCollider: false);
					DTool.ChangeCollisionFilter(in collider, 0u, BitOperator.Or, 4294952959u, BitOperator.And, changeTrigger: true, changeCollider: false);
					DTool.ChangeCollisionFilter(in collider, 8388608u, BitOperator.Set, 0u, BitOperator.Or, changeTrigger: true, changeCollider: true);
					DTool.ChangeCollisionFilter(in collider, 0u, BitOperator.Or, 16777216u, BitOperator.Or, changeTrigger: true, changeCollider: false);
				}
				else
				{
					DTool.ChangeCollisionFilter(in collider, 16777216u, BitOperator.Set, 0u, BitOperator.Or, changeTrigger: true, changeCollider: true);
					DTool.ChangeCollisionFilter(in collider, 0u, BitOperator.Or, 8388608u, BitOperator.Or, changeTrigger: true, changeCollider: false);
				}
			}
			if (spawnParams.MovementComponentData.ReboundCount <= 0 || spawnParams.MovementComponentData.IsFallSpell)
			{
				SpellTools.DisableSpellReboundCollider(in collider);
			}
			if (spawnParams.MovementComponentData.IsFallSpell)
			{
				DTool.ChangeCollisionFilter(collider.ColliderPtr, 0u, BitOperator.Or, 0u, BitOperator.And, changeTrigger: true, changeCollider: false);
			}
			if (spawnParams.ConfigComponentData.IsTeammate && spawnParams.MovementComponentData.Type == SpellSpecialMovementType.Rotation)
			{
				DTool.ChangeCollisionFilter(collider.ColliderPtr, 0u, BitOperator.Or, 4294966783u, BitOperator.And, changeTrigger: false, changeCollider: true);
			}
		}
		if (spawnParams.ConfigComponentData.IsTeammate)
		{
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state, entity).ValueRW.id = spawnParams.TeammateComponentData.TeammateId;
		}
		if (spawnParams.MoveTriggerComponentData.TriggerDistanceRatio > 0f)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellMoveTriggerComponentData_RW_ComponentLookup, ref state, entity, value: true);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMoveTriggerComponentData_RW_ComponentLookup, ref state, spawnParams.MoveTriggerComponentData, entity);
		}
		if (spawnParams.HitTriggerComponentData.SubGroupMp > 0f)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellHitTriggerComponentData_RW_ComponentLookup, ref state, entity, value: true);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellHitTriggerComponentData_RW_ComponentLookup, ref state, spawnParams.HitTriggerComponentData, entity);
		}
		if (spawnParams.OverSplitTriggerBufferEntity != Entity.Null)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellOverSplitTriggerComponentData_RW_ComponentLookup, ref state, entity, value: true);
			InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellOverSplitTriggerComponentData_RW_ComponentLookup, ref state, entity).ValueRW.TriggerBufferEntity = spawnParams.OverSplitTriggerBufferEntity;
		}
		if (spawnParams.OverTriggerComponentData.Count > 0)
		{
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellOverTriggerComponentData_RW_ComponentLookup, ref state, entity, value: true);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellOverTriggerComponentData_RW_ComponentLookup, ref state, spawnParams.OverTriggerComponentData, entity);
		}
		if (spawnParams.TwineTriggerComponentData.Count > 0)
		{
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__SpellTwineTriggerComponentData_RW_ComponentLookup, ref state, spawnParams.TwineTriggerComponentData, entity);
			InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellTwineTriggerComponentData_RW_ComponentLookup, ref state, entity, value: true);
		}
		DynamicBuffer<LinkedEntityGroup> dynamicBuffer = state.EntityManager.AddBuffer<LinkedEntityGroup>(entity);
		if (spawnParams.SubGroupEntity != Entity.Null)
		{
			dynamicBuffer.Add(spawnParams.SubGroupEntity);
		}
		if (spawnParams.OverSplitTriggerBufferEntity != Entity.Null)
		{
			dynamicBuffer.Add(spawnParams.OverSplitTriggerBufferEntity);
		}
		int prefabId2 = spawnParams.PrefabId;
		seName = "Spell";
		if (spellSingleton.TryGetSpellEffectEntity(prefabId2, in seName, spawnParams.ConfigComponentData.ColorType, out var entity2))
		{
			Entity entity3 = state.EntityManager.Instantiate(entity2);
			componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, entity);
			componentRWAfterCompletingDependency.ValueRW.SpellEffectEntity = entity3;
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, LocalTransform.Identity, entity3);
			state.EntityManager.AddComponent<Parent>(entity3);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_Parent_RW_ComponentLookup, ref state, new Parent
			{
				Value = entity
			}, entity3);
			InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref state, entity).Add(new LinkedEntityGroup
			{
				Value = entity3
			});
		}
		int prefabId3 = spawnParams.PrefabId;
		seName = "Trail";
		if (spellSingleton.TryGetSpellEffectEntity(prefabId3, in seName, spawnParams.ConfigComponentData.ColorType, out var entity4))
		{
			Entity entity5 = state.EntityManager.Instantiate(entity4);
			componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, entity);
			componentRWAfterCompletingDependency.ValueRW.TrailEffectEntity = entity5;
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, LocalTransform.Identity, entity5);
			state.EntityManager.AddComponent<Parent>(entity5);
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_Parent_RW_ComponentLookup, ref state, new Parent
			{
				Value = entity
			}, entity5);
			InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref state, entity).Add(new LinkedEntityGroup
			{
				Value = entity5
			});
		}
		if (spellSingleton.Effects.TryGetValue(spawnParams.PrefabId, out var item))
		{
			DynamicBuffer<SpellEffectSystem.Require> singletonBuffer2 = __query_1145758856_4.GetSingletonBuffer<SpellEffectSystem.Require>();
			foreach (SpellEffect item2 in item.GetValueArray(Allocator.Temp))
			{
				spawnParams.ConfigComponentData.ColorType.ColorEnumToString(out var result);
				if (item2.AutoCreate)
				{
					singletonBuffer2.Add(new SpellEffectSystem.Require
					{
						Settings = item2,
						Entity = entity,
						Color = result,
						SpellId = spawnParams.PrefabId
					});
				}
			}
		}
		abilityType = spawnParams.ConfigComponentData.AbilityType;
		if (((abilityType == SpellAbilityType.DragonBreath || abilityType == SpellAbilityType.DisintegrationRay || (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.HighPressureWasher && spawnParams.ConfigComponentData.Int3 == 0)) && !spawnParams.IsSplitSpell && !spawnParams.MovementComponentData.IsFallSpell && !spawnParams.RandomPosFocusMouse) || InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargeData_RO_ComponentLookup, ref state, entity))
		{
			Entity entity6 = default(Entity);
			if (InternalCompilerInterface.HasBufferAfterCompletingDependency(ref __TypeHandle.__UnitKeepCastingSpells_RO_BufferLookup, ref state, spawnParams.Shooter))
			{
				entity6 = spawnParams.Shooter;
			}
			else if (spawnParams.ChargeStarEntity != Entity.Null && !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargingTag_RO_ComponentLookup, ref state, entity))
			{
				entity6 = spawnParams.OwnerUnit;
			}
			if (entity6 != default(Entity) && !spawnParams.FromEcho && (!spawnParams.FromPostSlot || spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.Dash))
			{
				InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__UnitKeepCastingSpells_RW_BufferLookup, ref state, entity6).Add(new UnitKeepCastingSpells
				{
					Spell = entity,
					ReduceMoveSpeed = (spawnParams.ConfigComponentData.AbilityType != SpellAbilityType.DragonBreath)
				});
			}
			state.EntityManager.AddComponent<SpellKeepCastingCleanup>(entity);
			state.EntityManager.SetComponentData(entity, new SpellKeepCastingCleanup
			{
				OwnerUnit = entity6
			});
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref state, spawnParams.Shooter) || (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, spawnParams.Shooter) && spawnParams.MovementComponentData.Type == SpellSpecialMovementType.Rotation) || InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4004StartData_RO_ComponentLookup, ref state, spawnParams.Shooter))
			{
				state.EntityManager.AddComponent<SpellKeepCastingAttach>(entity);
				EntityManager entityManager = state.EntityManager;
				Entity entity7 = entity;
				SpellKeepCastingAttach componentData = new SpellKeepCastingAttach
				{
					Offset = spawnParams.Offset
				};
				float2 a = spawnParams.MovementComponentData.Direction.xy;
				componentData.DirOffset = DTool.GetDirOffset(in a, in spawnParams.SourceShootDir);
				componentData.FallPositionOffset = spawnParams.MovementComponentData.FallTargetPosition.xy - spawnParams.SourceShootTargetPosition;
				entityManager.SetComponentData(entity7, componentData);
			}
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellChargingTag_RO_ComponentLookup, ref state, entity) && !spawnParams.FromPostSlot)
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__SpellChargingTag_RW_ComponentLookup, ref state, entity, value: true);
			}
			if ((bool)spawnParams.ChargeStar)
			{
				state.EntityManager.AddComponent<SpellFromChargeModeStar>(entity);
				state.EntityManager.SetComponentData(entity, new SpellFromChargeModeStar
				{
					Star = spawnParams.ChargeStar,
					StarEntity = spawnParams.ChargeStarEntity
				});
			}
		}
		if (spawnParams.ConfigComponentData.AbilityType == SpellAbilityType.DaveHarpoons)
		{
			state.EntityManager.AddComponent<Spell4024DaveHarpoonMultiShootData>(entity);
			state.EntityManager.SetComponentData(entity, new Spell4024DaveHarpoonMultiShootData
			{
				Offset = spawnParams.Offset,
				FallPositionOffset = spawnParams.MovementComponentData.FallTargetPosition.xy - spawnParams.SourceShootTargetPosition
			});
		}
		if (spawnParams.ConfigComponentData.LightningChainDamage > 0f && !spawnParams.IgnoreSpawnLightningChain && spawnParams.ConfigComponentData.AbilityType != SpellAbilityType.BiAnLethalBlade && spawnParams.ConfigComponentData.AbilityType != SpellAbilityType.Umbrella && spawnParams.ConfigComponentData.AbilityType != SpellAbilityType.RuneHammer && spawnParams.ConfigComponentData.AbilityType != SpellAbilityType.LaserBeam)
		{
			TrySpawnLightningChain(ref state, spawnParams, spellSingleton, entity);
			if (DTool.IsSameCamp(spawnParams.ConfigComponentData.ShooterType, UnitType.Player))
			{
				_lastSpellEntity = entity;
			}
		}
	}

	private void TrySpawnLightningChain(ref SystemState state, SpellSpawnParams spawnParams, SpellSingleton spellSingleton, Entity spell)
	{
		if (!_lastSpellEntity.HasValue || !InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref state, _lastSpellEntity.Value) || !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, _lastSpellEntity.Value))
		{
			return;
		}
		float num = (GameMgr.IsMobile_Static ? 15f : 30f);
		DynamicOptimizeData singleton = __query_1145758856_5.GetSingleton<DynamicOptimizeData>();
		float num2 = 1f;
		if (singleton.IsLowFpsOptimizeActive(num))
		{
			if (!(UnityEngine.Random.Range(0f, 1f) < singleton.CurrentFPS / num))
			{
				return;
			}
			num2 = num / math.max(1f, singleton.CurrentFPS);
		}
		FixedString64Bytes fs = "Spell_3007";
		Entity entity = state.EntityManager.Instantiate(spellSingleton.Prefabs[fs]);
		spawnParams.ConfigComponentData.ColorType.ColorEnumToString(out var result);
		fs = $"Spell3007Chain{result}";
		Entity entity2 = state.EntityManager.Instantiate(spellSingleton.Prefabs[fs]);
		state.EntityManager.AddComponent<Parent>(entity2);
		state.EntityManager.SetComponentData(entity2, new Parent
		{
			Value = entity
		});
		state.EntityManager.AddBuffer<LinkedEntityGroup>(entity).Add(new LinkedEntityGroup
		{
			Value = entity2
		});
		float num3 = (spawnParams.IsSplitSpell ? (spawnParams.ConfigComponentData.LightningChainDamage * 0.35f) : spawnParams.ConfigComponentData.LightningChainDamage);
		num3 *= num2;
		InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Spell3007LightningChainEffect_RW_ComponentLookup, ref state, new Spell3007LightningChainEffect
		{
			SourceEntity = spell,
			TargetEntity = _lastSpellEntity.Value,
			Damage = num3,
			PenetrateCount = 1 + spawnParams.BonusPenetrate
		}, entity);
	}

	[BurstCompile]
	private void SpawnLightningChainForRequest(ref SystemState state, in Spell3007CreateRequest req, SpellSingleton spellSingleton)
	{
		if (req.MarkAsFired && (!_lastSpellEntity.HasValue || !InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref state, _lastSpellEntity.Value)))
		{
			_lastSpellEntity = req.Source;
		}
		else if (!_lastSpellEntity.HasValue || !InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref state, _lastSpellEntity.Value))
		{
			if (req.MarkAsFired)
			{
				_lastSpellEntity = req.Source;
			}
		}
		else
		{
			if (!InternalCompilerInterface.DoesEntityExist(ref __TypeHandle.__EntityStorageInfoLookup, ref state, req.Source))
			{
				return;
			}
			FixedString32Bytes result = req.ColorName;
			if (result.Length == 0 && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, req.Source))
			{
				InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellConfigComponentData_RO_ComponentLookup, ref state, req.Source).ColorType.ColorEnumToString(out result);
			}
			if (result.Length == 0)
			{
				result = "Player";
			}
			float damage = req.Damage;
			if (damage <= 0f)
			{
				if (req.MarkAsFired)
				{
					_lastSpellEntity = req.Source;
				}
				return;
			}
			FixedString64Bytes fs = "Spell_3007";
			Entity entity = state.EntityManager.Instantiate(spellSingleton.Prefabs[fs]);
			FixedString64Bytes fs2 = "Spell3007Chain";
			FixedStringMethods.Append(ref fs2, in result);
			Entity entity2 = state.EntityManager.Instantiate(spellSingleton.Prefabs[fs2]);
			state.EntityManager.AddComponent<Parent>(entity2);
			state.EntityManager.SetComponentData(entity2, new Parent
			{
				Value = entity
			});
			state.EntityManager.AddBuffer<LinkedEntityGroup>(entity).Add(new LinkedEntityGroup
			{
				Value = entity2
			});
			InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Spell3007LightningChainEffect_RW_ComponentLookup, ref state, new Spell3007LightningChainEffect
			{
				SourceEntity = req.Source,
				TargetEntity = _lastSpellEntity.Value,
				Damage = damage,
				PenetrateCount = 1 + req.Penetrate
			}, entity);
			if (req.MarkAsFired)
			{
				_lastSpellEntity = req.Source;
			}
		}
	}

	public static void Shoot(IEnumerable<SpellSpawnParams> spawnParams)
	{
		NativeArray<SpellSpawnParams> spawnParams2 = new NativeArray<SpellSpawnParams>(spawnParams.ToArray(), Allocator.Temp);
		Shoot(spawnParams2);
		spawnParams2.Dispose();
	}

	public static void Shoot(NativeArray<SpellSpawnParams> spawnParams)
	{
		using EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SpellSpawnParams));
		DynamicBuffer<SpellSpawnParams> singletonBuffer = entityQuery.GetSingletonBuffer<SpellSpawnParams>();
		foreach (SpellSpawnParams item in spawnParams)
		{
			singletonBuffer.Add(item);
		}
	}

	public static void Shoot(SpellSpawnParams spawnParams)
	{
		using EntityQuery entityQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SpellSpawnParams));
		entityQuery.GetSingletonBuffer<SpellSpawnParams>().Add(spawnParams);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3007CreateRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1145758856_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		((SpellShootSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((SpellShootSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((SpellShootSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
