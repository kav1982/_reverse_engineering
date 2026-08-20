using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(UnitBeforeTakeDamageSystem))]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
internal class UnitEnvironmentSystem : SystemBase
{
	public struct ContinueEffectRef : IComponentData, IQueryTypeParameter
	{
		public UnityObjectRef<ParticleSystem> obj;

		public Entity entity;

		public float beforeFadeTime;

		public bool particlePlaying;
	}

	public enum DamageEffectType
	{
		Weaken,
		Frozen,
		Mucus,
		Reverse,
		Void,
		WeakenBlueRune,
		Venom,
		Spike,
		Touch,
		Burn,
		TrunIntoGold
	}

	public struct DamageEffectInfo : IBufferElementData
	{
		public DamageEffectType type;

		public float3 point;

		public Entity entity;
	}

	public struct CheckStuckInfo : IBufferElementData
	{
		public Entity entity;

		public bool isPlayer;

		public Vector3 point;
	}

	[BurstCompile]
	[CompilerGenerated]
	public struct UnitEnvironmentJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitDead> __UnitDead_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
					__UnitDead_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitDead>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
					__UnitDead_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<PhysicsCollider>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitDead>();
				DefaultQuery = entityQueryBuilder2.Build(ref state);
				entityQueryBuilder.Reset();
				entityQueryBuilder.Dispose();
			}

			public void Init(ref SystemState state, bool assignDefaultQuery)
			{
				if (assignDefaultQuery)
				{
					__AssignQueries(ref state);
				}
				__TypeHandle.__AssignHandles(ref state);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Run(ref UnitEnvironmentJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref UnitEnvironmentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref UnitEnvironmentJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref UnitEnvironmentJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref UnitEnvironmentJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref UnitEnvironmentJob job, EntityManager entityManager)
			{
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		public struct InternalCompiler
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
			public static void CheckForErrors(int scheduleType)
			{
			}
		}

		public float deltaTime;

		public CollisionFilter checkRadiusFilter;

		public Entity effectEntity;

		public Entity checkStuckEntity;

		public bool TurningEnemyIntoGold;

		public Entity DamageRecordBufferEntity;

		public bool inEndlessMode;

		public float endlessDamageRatio;

		public float backpackSetVenomStack;

		[NativeDisableParallelForRestriction]
		public NativeList<Entity> toucherList;

		[ReadOnly]
		public PhysicsWorldSingleton physicsWorld;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AbyssTag> abyssLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<VenomTag> venomLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<MucusTag> mucusLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<SpecialObj3_Dots> trapLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> tsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> pptLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PhysicsMassOverride> massOverrideLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<PhysicsVelocity> physicsVelocityLookUp;

		[NativeDisableParallelForRestriction]
		public EntityCommandBuffer.ParallelWriter ecb;

		[NativeDisableParallelForRestriction]
		public NativeReference<bool> backPackSetFind;

		[NativeDisableParallelForRestriction]
		public BufferLookup<TakeDamageInfo_Dots> damageBufferLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<EndlessMonsterTag> endlessTagLookUp;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public unsafe void Execute([ChunkIndexInQuery] int index, ref PhysicsCollider collider, ref UnitDead unitDead, Entity entity)
		{
			DynamicBuffer<TakeDamageInfo_Dots> damageBuffer = damageBufferLookUp[entity];
			RefRW<UnitProperty_Dots> refRW = pptLookUp.GetRefRW(entity);
			ref UnitProperty_Dots valueRW = ref refRW.ValueRW;
			if (!valueRW.isInitialed || valueRW.unitCfg.unitType == UnitType.Brittleness || valueRW.isDead || valueRW.disabled)
			{
				return;
			}
			ref LocalTransform valueRW2 = ref tsfLookUp.GetRefRW(entity).ValueRW;
			if (valueRW.unitCfg.isCheckStuck && !valueRW.LockMotion)
			{
				valueRW.stuckCheckIntervalTimer += deltaTime;
				if (valueRW.stuckCheckIntervalTimer >= 2.5f || (valueRW.unitCfg.id == 800001 && valueRW.stuckCheckIntervalTimer >= 1f))
				{
					valueRW.stuckCheckIntervalTimer = 0f;
					ecb.AppendToBuffer(index, checkStuckEntity, new CheckStuckInfo
					{
						point = valueRW2.Position,
						isPlayer = (valueRW.unitCfg.id == 800001),
						entity = entity
					});
				}
			}
			bool flag = true;
			float num = 1f;
			bool flag2 = collider.ColliderPtr->Type == ColliderType.Capsule;
			bool flag3 = collider.ColliderPtr->Type == ColliderType.Sphere;
			bool flag4 = collider.ColliderPtr->Type == ColliderType.Compound;
			if (!flag2 && !flag3 && flag4 && valueRW.unitCfg.id != 800001)
			{
				valueRW.SetSize(valueRW2.Scale);
				flag = false;
			}
			if (flag)
			{
				if (flag4)
				{
					CompoundCollider* colliderPtr = (CompoundCollider*)collider.ColliderPtr;
					if (colliderPtr->Children[0].Collider->GetCollisionResponse() == CollisionResponsePolicy.Collide)
					{
						Unity.Physics.CapsuleCollider* collider2 = (Unity.Physics.CapsuleCollider*)colliderPtr->Children[0].Collider;
						num = collider2->Radius * valueRW2.Scale;
					}
					else
					{
						Unity.Physics.CapsuleCollider* collider3 = (Unity.Physics.CapsuleCollider*)colliderPtr->Children[1].Collider;
						num = collider3->Radius * valueRW2.Scale;
					}
				}
				else if (flag2)
				{
					Unity.Physics.CapsuleCollider* colliderPtr2 = (Unity.Physics.CapsuleCollider*)collider.ColliderPtr;
					num = colliderPtr2->Radius * valueRW2.Scale;
				}
				else if (flag3)
				{
					Unity.Physics.SphereCollider* colliderPtr3 = (Unity.Physics.SphereCollider*)collider.ColliderPtr;
					num = colliderPtr3->Radius * valueRW2.Scale;
				}
			}
			if (!valueRW.Affect_InAbyss)
			{
				valueRW.SetSize(valueRW2.Scale * num * 2f);
			}
			bool flag5 = !valueRW.IsFly && !valueRW.IsImmuneMucus && !valueRW.IsImmuneGroundAffect;
			bool flag6 = !valueRW.IsFly && !valueRW.IsImmuneVenom && !valueRW.IsImmuneGroundAffect;
			bool flag7 = !valueRW.IsFly && !valueRW.Affect_InAbyss && !valueRW.unitCfg.immuneAbyss;
			bool flag8 = flag && !valueRW.IsFly && !valueRW.unitCfg.immuneSpike && !valueRW.IsImmuneGroundAffect && valueRW.affect_SpikesTimer >= 0.33f;
			bool flag9 = flag && valueRW.unitCfg.id / 10 != 70040 && valueRW.unitCfg.IsSameCamp(UnitType.Player) && valueRW.monsterTouchTimer >= 0.33f;
			flag = flag && (flag5 || flag6 || flag7 || flag8 || flag9);
			NativeList<DistanceHit> outHits = new NativeList<DistanceHit>(Allocator.Temp);
			float radius = num;
			bool flag10 = num < 0.35f;
			if (flag10)
			{
				radius = 0.35f;
			}
			if (flag)
			{
				physicsWorld.OverlapSphere(valueRW2.Position, radius, ref outHits, checkRadiusFilter);
			}
			for (int num2 = outHits.Length - 1; num2 >= 0; num2--)
			{
				if (!tsfLookUp.HasComponent(outHits[num2].Entity))
				{
					outHits.RemoveAt(num2);
				}
			}
			if (flag7 && !valueRW.Affect_InAbyss && !valueRW.LockMotion)
			{
				for (int i = 0; i < outHits.Length; i++)
				{
					if (outHits[i].Distance < 0.35f && abyssLookUp.HasComponent(outHits[i].Entity))
					{
						valueRW.FallinAbyss(tsfLookUp[outHits[i].Entity].Position);
						break;
					}
				}
			}
			if (valueRW.Affect_InAbyss)
			{
				if (!DTool.IsTotallySame(in valueRW2.Position, in valueRW.affect_AbyssPoint))
				{
					valueRW2.Position = Vector3.MoveTowards((Vector3)valueRW2.Position, (Vector3)valueRW.affect_AbyssPoint, 4f * deltaTime);
				}
				float num3 = valueRW2.Scale - deltaTime;
				if (num3 < 0f)
				{
					if (valueRW.unitCfg.unitType == UnitType.Player)
					{
						valueRW.Affect_InAbyss = false;
					}
					else
					{
						valueRW.AnnouncedDeath(ecb, index, damageBuffer, ref unitDead, entity);
					}
				}
				else
				{
					valueRW2.Scale = num3;
				}
			}
			for (int num4 = outHits.Length - 1; num4 >= 0; num4--)
			{
				if (flag10 && outHits[num4].Distance > num)
				{
					outHits.RemoveAt(num4);
				}
			}
			valueRW.damageReciveIncresePercentTimer -= deltaTime;
			if (valueRW.damageReciveIncresePercentTimer <= 0f)
			{
				valueRW.DamageReciveIncresePercent = 0f;
			}
			valueRW.affect_IsMucusDecelerate = false;
			if (valueRW.affect_IsMucusHit)
			{
				valueRW.affect_IsMucusDecelerate = true;
				valueRW.affect_MucusHitTimer -= deltaTime;
				if (valueRW.affect_MucusHitTimer <= 0f)
				{
					valueRW.ClearMucusState();
					valueRW.UpdateBodyColor();
				}
			}
			if (flag5 && !valueRW.affect_IsMucusHit && flag)
			{
				for (int j = 0; j < outHits.Length; j++)
				{
					if (mucusLookUp.HasComponent(outHits[j].Entity))
					{
						valueRW.affect_IsMucusDecelerate = true;
						valueRW.affect_MucusMoveSpeedRatio = Mathf.Min(0.6f, valueRW.affect_MucusMoveSpeedRatio);
						break;
					}
				}
			}
			switch (valueRW.FronzenState)
			{
			case UnitProperty.Affect_FrozenState.Frozening:
				valueRW.affect_FrozenTimer += deltaTime;
				if (valueRW.affect_FrozenTimer >= valueRW.affect_FrozenTime)
				{
					valueRW.ClearFrozenState();
					valueRW.FronzenState = UnitProperty.Affect_FrozenState.FrozenImmune;
					valueRW.UpdateBodyColor();
				}
				break;
			case UnitProperty.Affect_FrozenState.FrozenImmune:
				valueRW.affect_FrozenTimer += deltaTime;
				if (valueRW.affect_FrozenTimer >= 2f)
				{
					valueRW.affect_FrozenTimer = 0f;
					valueRW.FronzenState = UnitProperty.Affect_FrozenState.Normal;
				}
				break;
			}
			if (valueRW.frozenStateChanged && !valueRW.unitCfg.isHybirdUnit && valueRW.unitCfg.id != 800001)
			{
				if (valueRW.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
				{
					if (massOverrideLookUp.TryGetRefRW(entity, out var outRef))
					{
						valueRW.frozenBeforeKinematic = outRef.ValueRW.IsKinematic != 0;
						outRef.ValueRW.IsKinematic = 1;
						outRef.ValueRW.SetVelocityToZero = 1;
					}
					if (physicsVelocityLookUp.TryGetRefRW(entity, out var outRef2))
					{
						valueRW.frozenBeforeSpeed = outRef2.ValueRW.Linear;
						outRef2.ValueRW.Linear = float3.zero;
					}
				}
				else
				{
					if (massOverrideLookUp.TryGetRefRW(entity, out var outRef3))
					{
						byte b = (byte)(valueRW.frozenBeforeKinematic ? 1u : 0u);
						outRef3.ValueRW.IsKinematic = b;
						outRef3.ValueRW.SetVelocityToZero = b;
					}
					if (physicsVelocityLookUp.TryGetRefRW(entity, out var outRef4))
					{
						outRef4.ValueRW.Linear = valueRW.frozenBeforeSpeed;
					}
				}
				valueRW.frozenStateChanged = false;
			}
			if (valueRW.affect_burnDurationTimer > 0f)
			{
				valueRW.affect_burnDurationTimer -= deltaTime;
				if (valueRW.affect_burnDurationTimer <= 0f)
				{
					valueRW.ClearBurnState();
					valueRW.UpdateBodyColor();
				}
			}
			if (valueRW.affect_burnDurationTimer > 0f)
			{
				valueRW.affect_burnAttackIntervalTimer += deltaTime;
				if (valueRW.affect_burnAttackIntervalTimer >= 0.33f)
				{
					valueRW.affect_burnAttackIntervalTimer = 0f;
					TakeDamageInfo_Dots element = TakeDamageInfo_Dots.NewInfo(AttackerType.Burn);
					element.damage = valueRW.unitCfg.maxHP * valueRW.affect_burnHPRatioPerHit * 0.33f;
					element.ignoreBeHitColor = true;
					element.damageRecordId = 3111;
					element.isPercentageDamage = true;
					ecb.AppendToBuffer(index, entity, element);
					if (valueRW.showAffect)
					{
						ecb.AppendToBuffer(index, effectEntity, new DamageEffectInfo
						{
							point = valueRW2.Position,
							type = DamageEffectType.Burn
						});
					}
				}
			}
			bool flag11 = false;
			if (flag6 && !(valueRW.affect_VenomDurationTimer > 0f) && flag)
			{
				for (int k = 0; k < outHits.Length; k++)
				{
					if (venomLookUp.HasComponent(outHits[k].Entity))
					{
						flag11 = true;
						valueRW.SetVenom(2f, 2f);
						break;
					}
				}
			}
			if (valueRW.affect_VenomDurationTimer > 0f)
			{
				valueRW.affect_VenomDurationTimer -= deltaTime;
				if (valueRW.affect_VenomDurationTimer <= 0f)
				{
					valueRW.ClearVenomState();
					valueRW.UpdateBodyColor();
				}
			}
			valueRW.affect_VenomInjureTimer += deltaTime;
			if (valueRW.affect_VenomDurationTimer > 0f && valueRW.affect_VenomInjureTimer >= 1f)
			{
				valueRW.affect_VenomInjureTimer = 0f;
				if (!flag11 && flag && flag6)
				{
					for (int l = 0; l < outHits.Length; l++)
					{
						if (venomLookUp.HasComponent(outHits[l].Entity))
						{
							flag11 = true;
							valueRW.SetVenom(2f, 2f);
							break;
						}
					}
				}
				if (valueRW.affect_VenomCurrentStack >= backpackSetVenomStack && valueRW.unitCfg.id != 10501 && valueRW.unitCfg.IsSameCamp(UnitType.Monster))
				{
					backPackSetFind.Value = true;
				}
				TakeDamageInfo_Dots element2 = TakeDamageInfo_Dots.NewInfo(AttackerType.Venom);
				element2.damage = valueRW.affect_VenomCurrentStack;
				element2.ignoreBeHitColor = true;
				element2.damageRecordId = 3005;
				ecb.AppendToBuffer(index, entity, element2);
				if (valueRW.showAffect)
				{
					ecb.AppendToBuffer(index, effectEntity, new DamageEffectInfo
					{
						point = valueRW2.Position,
						type = DamageEffectType.Venom
					});
				}
			}
			if (valueRW.voidEffectTimer > 0f || valueRW.voidExplosionData.ConstVoidEffect)
			{
				valueRW.voidEffectTimer -= deltaTime;
				if (!valueRW.voidExplosionData.ConstVoidEffect)
				{
					valueRW.ChangeColor(GameConst.color_BodyVoid);
				}
				if (valueRW.voidEffectTimer <= 0f && !valueRW.voidExplosionData.ConstVoidEffect)
				{
					valueRW.voidEffectTimer = 0f;
					valueRW.ClearVoidState();
					valueRW.UpdateBodyColor();
				}
				if (valueRW.unitCfg.unitType != 0)
				{
					float num5 = valueRW.voidExplosionData.InstantKillRatio;
					if (valueRW.unitCfg.unitType == UnitType.Elite || valueRW.unitCfg.unitType == UnitType.Boss)
					{
						num5 /= 2f;
					}
					if (valueRW.unitCfg.currentHP / valueRW.unitCfg.maxHP < num5 && !valueRW.IsInvincible)
					{
						if (valueRW.unitCfg.currentHP > 0f)
						{
							ecb.AppendToBuffer(index, DamageRecordBufferEntity, new DamageRecordBuffer
							{
								Damage = valueRW.unitCfg.currentHP,
								HitUnitId = valueRW.unitCfg.id,
								SpellOrRelicId = 3129
							});
						}
						valueRW.unitCfg.currentHP = 0f;
						valueRW.AnnouncedDeath(ecb, index, damageBuffer, ref unitDead, entity);
					}
				}
			}
			if (valueRW.affect_IsReverseMove)
			{
				valueRW.affect_ReverseMoveTimer -= deltaTime;
				if (valueRW.affect_ReverseMoveTimer <= 0f)
				{
					valueRW.affect_ReverseMoveTimer = 0f;
				}
			}
			if (valueRW.affect_SpikesTimer < 0.33f)
			{
				valueRW.affect_SpikesTimer += deltaTime;
			}
			if (flag && flag8)
			{
				for (int m = 0; m < outHits.Length; m++)
				{
					Entity entity2 = outHits[m].Entity;
					if (trapLookUp.HasComponent(entity2))
					{
						valueRW.affect_SpikesTimer = 0f;
						TakeDamageInfo_Dots element3 = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						element3.damage = 3f;
						element3.isTrapDamage = true;
						ecb.AppendToBuffer(index, entity, element3);
						if (valueRW.showAffect)
						{
							ecb.AppendToBuffer(index, effectEntity, new DamageEffectInfo
							{
								point = valueRW2.Position,
								type = DamageEffectType.Spike
							});
						}
						break;
					}
				}
			}
			if (!valueRW.unitCfg.IsSameCamp(UnitType.Player))
			{
				return;
			}
			if (valueRW.monsterTouchTimer < 0.33f)
			{
				valueRW.monsterTouchTimer += deltaTime;
			}
			if (!flag || !flag9)
			{
				return;
			}
			for (int n = 0; n < outHits.Length; n++)
			{
				if (!toucherList.Contains(outHits[n].Entity))
				{
					continue;
				}
				valueRW.monsterTouchTimer = 0f;
				TakeDamageInfo_Dots element4 = TakeDamageInfo_Dots.NewInfo(outHits[n].Entity);
				if (valueRW.unitCfg.unitType == UnitType.Player || !inEndlessMode)
				{
					element4.damage = 6f;
				}
				else
				{
					element4.damage = 3f;
				}
				if (inEndlessMode && endlessTagLookUp.TryGetComponent(outHits[n].Entity, out var componentData))
				{
					element4.damage *= endlessDamageRatio;
					if (componentData.has316Buff)
					{
						element4.damage *= 1f;
					}
				}
				ecb.AppendToBuffer(index, entity, element4);
				if (valueRW.unitCfg.unitType == UnitType.Player)
				{
					ecb.AppendToBuffer(index, effectEntity, new DamageEffectInfo
					{
						point = valueRW2.Position,
						type = DamageEffectType.Touch
					});
				}
				if (valueRW.unitCfg.unitType == UnitType.Player && TurningEnemyIntoGold)
				{
					refRW = pptLookUp.GetRefRW(outHits[n].Entity);
					ref UnitProperty_Dots valueRW3 = ref refRW.ValueRW;
					UnitType unitType = valueRW3.unitCfg.unitType;
					if (unitType != UnitType.Boss && unitType != UnitType.Elite && valueRW3.unitCfg.id != 199901 && valueRW3.unitCfg.id != 300921 && valueRW3.unitCfg.id != 500622 && valueRW3.unitCfg.id != 501021)
					{
						ecb.AppendToBuffer(index, effectEntity, new DamageEffectInfo
						{
							point = valueRW2.Position,
							type = DamageEffectType.TrunIntoGold,
							entity = outHits[n].Entity
						});
					}
					continue;
				}
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitDead_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr, i);
					ref UnitDead unitDead = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr2, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, i);
					Execute(chunkIndexInQuery, ref collider, ref unitDead, entity);
					num++;
				}
				return;
			}
			if (math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) + math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1 <= 4)
			{
				int nextRangeBegin = 0;
				int nextRangeEnd = 0;
				while (InternalCompilerInterface.UnsafeTryGetNextEnabledBitRange(chunkEnabledMask, nextRangeEnd, out nextRangeBegin, out nextRangeEnd))
				{
					while (nextRangeBegin < nextRangeEnd)
					{
						ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr, nextRangeBegin);
						ref UnitDead unitDead2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr2, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, nextRangeBegin);
						Execute(chunkIndexInQuery, ref collider2, ref unitDead2, entity2);
						nextRangeBegin++;
						num++;
					}
				}
				return;
			}
			ulong num2 = chunkEnabledMask.ULong0;
			int num3 = math.min(64, count);
			for (int j = 0; j < num3; j++)
			{
				if ((num2 & 1) != 0L)
				{
					ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr, j);
					ref UnitDead unitDead3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr2, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, j);
					Execute(chunkIndexInQuery, ref collider3, ref unitDead3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr, k);
					ref UnitDead unitDead4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitDead>(nativeArrayPtr2, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr3, k);
					Execute(chunkIndexInQuery, ref collider4, ref unitDead4, entity4);
					num++;
				}
				num2 >>= 1;
			}
		}

		private JobHandle __ThrowCodeGenException()
		{
			throw new Exception("This method should have been replaced by source gen.");
		}

		public void Run()
		{
			__ThrowCodeGenException();
		}

		public void RunByRef()
		{
			__ThrowCodeGenException();
		}

		public void Run(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void RunByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public JobHandle Schedule(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleByRef(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle Schedule(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleByRef(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public void Schedule()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleByRef()
		{
			__ThrowCodeGenException();
		}

		public void Schedule(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void ScheduleByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallel(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return __ThrowCodeGenException();
		}

		public JobHandle ScheduleParallelByRef(EntityQuery query, JobHandle dependsOn, NativeArray<int> chunkBaseEntityIndices)
		{
			return __ThrowCodeGenException();
		}

		public void ScheduleParallel()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallelByRef()
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallel(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		public void ScheduleParallelByRef(EntityQuery query)
		{
			__ThrowCodeGenException();
		}

		void IJobChunk.Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
		}
	}

	private struct TypeHandle
	{
		public ComponentLookup<AbyssTag> __AbyssTag_RW_ComponentLookup;

		public ComponentLookup<SpecialObj3_Dots> __SpecialObj3_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<VenomTag> __VenomTag_RW_ComponentLookup;

		public ComponentLookup<MucusTag> __MucusTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<PhysicsMassOverride> __Unity_Physics_PhysicsMassOverride_RW_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		public ComponentLookup<EndlessMonsterTag> __EndlessMonsterTag_RW_ComponentLookup;

		public UnitEnvironmentJob.InternalCompilerQueryAndHandleData __UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__AbyssTag_RW_ComponentLookup = state.GetComponentLookup<AbyssTag>();
			__SpecialObj3_Dots_RW_ComponentLookup = state.GetComponentLookup<SpecialObj3_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__VenomTag_RW_ComponentLookup = state.GetComponentLookup<VenomTag>();
			__MucusTag_RW_ComponentLookup = state.GetComponentLookup<MucusTag>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Unity_Physics_PhysicsMassOverride_RW_ComponentLookup = state.GetComponentLookup<PhysicsMassOverride>();
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
			__EndlessMonsterTag_RW_ComponentLookup = state.GetComponentLookup<EndlessMonsterTag>();
			__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	public static string path_Void = "Prefabs/EF/EF_VoidEffect_Dots";

	public static string path_Weaken = "Prefabs/EF/EF_BonusTakeDamage_Dots";

	public static string path_Weaken_H = "Prefabs/EF/EF_BonusTakeDamage_Dots_H";

	public static string path_WeakenBlueRune = "Prefabs/EF/EF_BonusTakeDamageBlueRune_Dots";

	public static string path_Reverse = "Prefabs/EF/EF_ReverseMove_Dots";

	public static string path_Frozen = "Prefabs/EF/EF_Frozen_Dots";

	public static string path_Mucus = "Prefabs/EF/EF_Deceleration_Dots";

	private NativeList<Entity> touchersList;

	private ComponentLookup<SpecialObj3_Dots> trapLookUp;

	private ComponentLookup<LocalTransform> tsfLookUp;

	private ComponentLookup<UnitProperty_Dots> pptLookUp;

	private ComponentLookup<VenomTag> venomLookUp;

	private ComponentLookup<MucusTag> mucusLookUp;

	private ComponentLookup<AbyssTag> abyssLookUp;

	private CollisionFilter checkRadiusFilter;

	private EntityQuery effectRefQuery;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1311607441_0;

	private EntityQuery __query_1311607441_1;

	private EntityQuery __query_1311607441_2;

	private EntityQuery __query_1311607441_3;

	private EntityQuery __query_1311607441_4;

	private EntityQuery __query_1311607441_5;

	private EntityQuery __query_1311607441_6;

	[Preserve]
	protected override void OnCreate()
	{
		touchersList = new NativeList<Entity>(Allocator.Persistent);
		base.EntityManager.CreateSingletonBuffer<DamageEffectInfo>();
		base.EntityManager.CreateSingletonBuffer<CheckStuckInfo>();
		RequireForUpdate<DamageRecordBuffer>();
		RequireForUpdate<PhysicsWorldSingleton>();
		abyssLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AbyssTag_RW_ComponentLookup, ref base.CheckedStateRef);
		trapLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpecialObj3_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
		tsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef);
		pptLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef);
		venomLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__VenomTag_RW_ComponentLookup, ref base.CheckedStateRef);
		mucusLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MucusTag_RW_ComponentLookup, ref base.CheckedStateRef);
		effectRefQuery = base.EntityManager.CreateEntityQuery(typeof(ContinueEffectRef));
		checkRadiusFilter = new CollisionFilter
		{
			BelongsTo = uint.MaxValue,
			CollidesWith = 15488u,
			GroupIndex = 0
		};
	}

	[Preserve]
	protected override void OnDestroy()
	{
		touchersList.Dispose();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (LevelMgr.Inst == null || LevelMgr.Inst.RoomCtrllers == null || LevelMgr.Inst.RoomCtrllers.Count <= 0)
		{
			return;
		}
		if (touchersList.Length > 0)
		{
			touchersList.Clear();
		}
		for (int i = 0; i < LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Count; i++)
		{
			Entity value = LevelMgr.Inst.CurrentRoomCtrller.targetableEttList[i];
			if (base.EntityManager.HasComponent<UnitProperty_Dots>(value))
			{
				UnitProperty_Dots valueRO = InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, value).ValueRO;
				if (valueRO.CanTouch && valueRO.unitCfg.IsSameCamp(UnitType.Monster))
				{
					touchersList.Add(in value);
				}
			}
		}
		mucusLookUp.Update(this);
		venomLookUp.Update(this);
		abyssLookUp.Update(this);
		pptLookUp.Update(this);
		tsfLookUp.Update(this);
		trapLookUp.Update(this);
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		NativeReference<bool> backPackSetFind = new NativeReference<bool>(Allocator.TempJob);
		float endlessDamageRatio = (GameMgr.InEndlessMode ? GameConstManaged.endlessMonsterDamageRatio : 1f);
		UnitEnvironmentJob unitEnvironmentJob = default(UnitEnvironmentJob);
		unitEnvironmentJob.deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
		unitEnvironmentJob.toucherList = touchersList;
		unitEnvironmentJob.TurningEnemyIntoGold = PlayerMgr.Inst.ItemCtrller.relicGroupConfigs.TryGetValue(5, out var value2);
		unitEnvironmentJob.physicsWorld = __query_1311607441_0.GetSingleton<PhysicsWorldSingleton>();
		unitEnvironmentJob.abyssLookUp = abyssLookUp;
		unitEnvironmentJob.venomLookUp = venomLookUp;
		unitEnvironmentJob.mucusLookUp = mucusLookUp;
		unitEnvironmentJob.trapLookUp = trapLookUp;
		unitEnvironmentJob.tsfLookUp = tsfLookUp;
		unitEnvironmentJob.pptLookUp = pptLookUp;
		unitEnvironmentJob.massOverrideLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsMassOverride_RW_ComponentLookup, ref base.CheckedStateRef);
		unitEnvironmentJob.physicsVelocityLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref base.CheckedStateRef);
		unitEnvironmentJob.checkRadiusFilter = checkRadiusFilter;
		unitEnvironmentJob.effectEntity = __query_1311607441_1.GetSingletonEntity();
		unitEnvironmentJob.checkStuckEntity = __query_1311607441_2.GetSingletonEntity();
		unitEnvironmentJob.ecb = entityCommandBuffer.AsParallelWriter();
		unitEnvironmentJob.DamageRecordBufferEntity = __query_1311607441_3.GetSingletonEntity();
		unitEnvironmentJob.backPackSetFind = backPackSetFind;
		unitEnvironmentJob.backpackSetVenomStack = SetConfig.dic[8].unlockInt1;
		unitEnvironmentJob.damageBufferLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef);
		unitEnvironmentJob.endlessTagLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__EndlessMonsterTag_RW_ComponentLookup, ref base.CheckedStateRef);
		unitEnvironmentJob.inEndlessMode = GameMgr.InEndlessMode;
		unitEnvironmentJob.endlessDamageRatio = endlessDamageRatio;
		UnitEnvironmentJob job = unitEnvironmentJob;
		base.Dependency = __ScheduleViaJobChunkExtension_0(job, __TypeHandle.__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
		base.Dependency.Complete();
		entityCommandBuffer.Playback(base.EntityManager);
		entityCommandBuffer.Dispose();
		if (backPackSetFind.Value && !DataMgr.selectedWorldData.FindSet8)
		{
			DataMgr.selectedWorldData.SetFindSet8();
		}
		backPackSetFind.Dispose();
		DynamicBuffer<CheckStuckInfo> singletonBuffer = __query_1311607441_4.GetSingletonBuffer<CheckStuckInfo>();
		foreach (CheckStuckInfo item2 in singletonBuffer)
		{
			if (item2.isPlayer && PlayerMgr.Inst.inDashSpell)
			{
				continue;
			}
			RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, item2.entity);
			if (!NavMesh.SamplePosition(Tool2D.IgnoreZPoint(item2.point, 4.35f), out var _, 0.5f, 8))
			{
				if (!componentRWAfterCompletingDependency.ValueRW.isStuck)
				{
					componentRWAfterCompletingDependency.ValueRW.isStuck = true;
					continue;
				}
				componentRWAfterCompletingDependency.ValueRW.isStuck = false;
				float3 @float = float3.zero;
				bool flag = true;
				if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.accessDown == Vector2Data.Up1000)
				{
					@float = Tool2D.GetNavMeshPointIngoreZ(item2.point);
				}
				else
				{
					NavMeshPath navMeshPath = Tool2D.GetNavMeshPath(LevelMgr.Inst.CurrentRoomCtrller.GetAccessCenterPoint(FourDir.Down), item2.point);
					if (navMeshPath.corners.Length != 0)
					{
						@float = Tool2D.IgnoreZPoint(navMeshPath.corners[navMeshPath.corners.Length - 1]);
					}
					else
					{
						flag = false;
					}
				}
				if (flag)
				{
					if (item2.isPlayer)
					{
						PlayerMgr.Inst.SetPlayerPoint(@float);
						continue;
					}
					LocalTransform componentData = base.EntityManager.GetComponentData<LocalTransform>(item2.entity);
					componentData.Position = @float;
					base.EntityManager.SetComponentData(item2.entity, componentData);
				}
			}
			else
			{
				componentRWAfterCompletingDependency.ValueRW.isStuck = false;
			}
		}
		singletonBuffer.Clear();
		SpellSingleton singleton = __query_1311607441_5.GetSingleton<SpellSingleton>();
		DynamicBuffer<DamageEffectInfo> singletonBuffer2 = __query_1311607441_6.GetSingletonBuffer<DamageEffectInfo>();
		if (singletonBuffer2.Length <= 0)
		{
			return;
		}
		for (int j = 0; j < singletonBuffer2.Length; j++)
		{
			DamageEffectInfo damageEffectInfo = singletonBuffer2[j];
			Entity item;
			switch (damageEffectInfo.type)
			{
			case DamageEffectType.Spike:
				if (!GameMgr.IsHarmony_Static)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", damageEffectInfo.point, 1f);
				}
				break;
			case DamageEffectType.Venom:
				if (singleton.Prefabs.TryGetValue("3005_Poison", out item))
				{
					Entity entity = base.EntityManager.Instantiate(item);
					LocalTransform componentData2 = base.EntityManager.GetComponentData<LocalTransform>(entity);
					componentData2.Position = damageEffectInfo.point;
					base.EntityManager.SetComponentData(entity, componentData2);
				}
				break;
			case DamageEffectType.Touch:
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch", PlayerMgr.Inst.PlayerPoint, 0.5f);
				break;
			case DamageEffectType.Burn:
				if (singleton.Prefabs.TryGetValue("3111_Burn", out item))
				{
					Entity entity2 = base.EntityManager.Instantiate(item);
					LocalTransform componentData3 = base.EntityManager.GetComponentData<LocalTransform>(entity2);
					componentData3.Position = damageEffectInfo.point;
					base.EntityManager.SetComponentData(entity2, componentData3);
				}
				break;
			case DamageEffectType.TrunIntoGold:
			{
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, damageEffectInfo.entity))
				{
					UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, damageEffectInfo.entity);
					int id = componentAfterCompletingDependency.unitCfg.id;
					if (id == 104201 || id == 104221 || id == 104202 || id == 104222 || id == 104121 || id == 104122 || id == 103121)
					{
						TakeDamageInfo_Dots info_Dots = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
						componentAfterCompletingDependency.isDead = true;
						info_Dots.isTargetDead = true;
						info_Dots.damage = 100000000f;
						info_Dots.ignoreFloatText = true;
						if (componentAfterCompletingDependency.unitCfg.triggerDeadEvent)
						{
							info_Dots.isTriggerDeadEvent = true;
						}
						componentAfterCompletingDependency.AnnouncedDeath(info_Dots, damageEffectInfo.entity);
					}
					else
					{
						componentAfterCompletingDependency.AnnouncedDeath(damageEffectInfo.entity);
					}
					InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, damageEffectInfo.entity);
				}
				for (int k = 0; k < value2.int1.result; k++)
				{
					Vector3 navMeshPointIngoreZ = Tool2D.GetNavMeshPointIngoreZ(damageEffectInfo.point);
					QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Resource, 11), navMeshPointIngoreZ);
				}
				break;
			}
			}
		}
		singletonBuffer2.Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(UnitEnvironmentJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__UnitEnvironmentSystem_UnitEnvironmentJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DamageEffectInfo>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<CheckStuckInfo>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DamageRecordBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CheckStuckInfo>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<DamageEffectInfo>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1311607441_6 = entityQueryBuilder2.Build(ref state);
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
	public UnitEnvironmentSystem()
	{
	}
}
