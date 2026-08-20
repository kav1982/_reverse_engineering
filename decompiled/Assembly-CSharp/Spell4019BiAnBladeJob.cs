using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
public struct Spell4019BiAnBladeJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellMovementComponentData> __SpellMovementComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellComponentData> __SpellComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<SpellElementEffectComponentData> __SpellElementEffectComponentData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell4019BiAnBladeData> __Spell4019BiAnBladeData_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

			public BufferTypeHandle<StatefulCollisionEvent> __Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle;

			public BufferTypeHandle<StatefulTriggerEvent> __Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle;

			public ComponentTypeHandle<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle;

			public BufferTypeHandle<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__SpellConfigComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellConfigComponentData>();
				__SpellMovementComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellMovementComponentData>();
				__SpellComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellComponentData>();
				__SpellElementEffectComponentData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpellElementEffectComponentData>();
				__Spell4019BiAnBladeData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell4019BiAnBladeData>();
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
				__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulCollisionEvent>();
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle = state.GetBufferTypeHandle<StatefulTriggerEvent>();
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsVelocity>();
				__SpellGameObjectEffectLink_RW_BufferTypeHandle = state.GetBufferTypeHandle<SpellGameObjectEffectLink>();
			}

			public void Update(ref SystemState state)
			{
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__SpellConfigComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellMovementComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellComponentData_RW_ComponentTypeHandle.Update(ref state);
				__SpellElementEffectComponentData_RW_ComponentTypeHandle.Update(ref state);
				__Spell4019BiAnBladeData_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle.Update(ref state);
				__SpellGameObjectEffectLink_RW_BufferTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellElementEffectComponentData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4019BiAnBladeData>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulCollisionEvent>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellGameObjectEffectLink>();
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
		public void Run(ref Spell4019BiAnBladeJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell4019BiAnBladeJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell4019BiAnBladeJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return InternalCompilerInterface.JobChunkInterface.ScheduleParallelByRef(ref job, query, dependency, job.__ChunkBaseEntityIndices);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell4019BiAnBladeJob job, EntityQuery query, ref SystemState state)
		{
			NativeArray<int> nativeArray = (job.__ChunkBaseEntityIndices = query.CalculateBaseEntityIndexArray(state.WorldUpdateAllocator));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell4019BiAnBladeJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			JobHandle outJobHandle;
			NativeArray<int> nativeArray = (job.__ChunkBaseEntityIndices = query.CalculateBaseEntityIndexArrayAsync(state.WorldUpdateAllocator, dependency, out outJobHandle));
			return outJobHandle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell4019BiAnBladeJob job, EntityManager entityManager)
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

	public EntityCommandBuffer.ParallelWriter CMD;

	public GlobalRandom Random;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<EffectsCollectorData> EffectCtrlLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> TransformLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell4019BladeHideUnderGroundMat> BladeUnderGroundMatDataLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<Spell4019BladeColorChangeMat> BladeGlowMatDataLookUp;

	[ReadOnly]
	public PlayerController_Dots PlayerController;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public NativeArray<BladeShootListenerData> ShootRequests;

	public Entity ChainReqEntity;

	[ReadOnly]
	public NativeArray<BladeWandSingletonData> BladeWands;

	[ReadOnly]
	public float DeltaTime;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellNeedResize> NeedResizeLookup;

	public Entity EffectRequireBufferEntity;

	public Entity EffectRecycleBufferEntity;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[ReadOnly]
	public PhysicsWorldSingleton PhysicsWorld;

	public bool Hide;

	public DynamicOptimizeData OptimizeData;

	public Entity SEPlayerSingleton;

	public uint GlobalSeed;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<BreakLightningChain> LightningChainDestroyLookup;

	public bool IsIgnoreWallRelic;

	public Entity Spell3101Buffer;

	public Entity GlobalParticleSystemBufferEntity;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<GlobalParticle.Emitter> GlobalParticleEmitterLookUp;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<GlobalParticle.EmitDistanceCounter> GlobalParticleMoveEmitterLookUp;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[ReadOnly]
	public NativeArray<int> __ChunkBaseEntityIndices;

	[BurstCompile]
	private void Execute(Entity entity, ref LocalTransform transform, ref SpellConfigComponentData config, ref SpellMovementComponentData movement, ref SpellComponentData componentData, ref SpellElementEffectComponentData elementEffect, ref Spell4019BiAnBladeData bladeData, ref PhysicsCollider collider, DynamicBuffer<StatefulCollisionEvent> collisionBuffer, DynamicBuffer<StatefulTriggerEvent> triggerBuffer, ref PhysicsVelocity velocity, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, [ChunkIndexInQuery] int chunkIndex, [EntityIndexInQuery] int entityInQueryIndex)
	{
		if (!bladeData.IsInitialized)
		{
			if (componentData.SpellEffectEntity == Entity.Null || !EffectCtrlLookUp.TryGetComponent(componentData.SpellEffectEntity, out var componentData2))
			{
				return;
			}
			bladeData.Head = componentData2.Effect1;
			bladeData.Tail = componentData2.Effect2;
			bladeData.Material = componentData2.Effect3;
			RefRW<LocalTransform> refRW = TransformLookup.GetRefRW(bladeData.Material);
			refRW.ValueRW.Position = new float3(refRW.ValueRW.Position.xy, (0f - TransformLookup.GetRefRW(entity).ValueRW.Position.z) * 0.01f);
			bladeData.IsInitialized = true;
			bladeData.StateCurrent = -1;
			bladeData.StateNext = 0;
			componentData.CanThroughWall = true;
			uint num = math.hash(new uint3(GlobalSeed, (uint)chunkIndex, (uint)entityInQueryIndex));
			Unity.Mathematics.Random random = Unity.Mathematics.Random.CreateFromIndex((num == 0) ? 1u : num);
			float num2 = random.NextFloat(0.3f);
			float2 @float = random.NextFloat2Direction();
			float2 followRandomDiff = num2 * @float;
			followRandomDiff.y *= 0.5f;
			bladeData.FollowRandomDiff = followRandomDiff;
			RadiusAttributeValue radius = config.Radius;
			radius.Base = 0f;
			config.Radius = radius;
			bladeData.RebounceTimeTotal = movement.ReboundCount;
			bladeData.ResizeForHideShader = 1.3f;
			if (NeedResizeLookup.TryGetComponent(entity, out var componentData3))
			{
				bladeData.ResizeForHideShader += componentData3.ExtraSizeRatio;
			}
			bladeData.Speed = movement.Speed;
			bladeData.OriginSpeed = movement.OriginalSpellHorizontalSpeed;
			bladeData.FallGravity = movement.Gravity;
			velocity.Linear = Unity.Mathematics.float3.zero;
			if (!TransformLookup.TryGetComponent(componentData.OwnerEntity, out var componentData4))
			{
				return;
			}
			float2 float2 = -math.sign(PlayerController.mousePosition.xy - componentData4.Position.xy);
			bladeData.FollowPosition = new float3(float2 * 0.5f, 0f) + componentData4.Position;
			transform.Position = bladeData.FollowPosition + new float3(bladeData.FollowRandomDiff, 0f);
		}
		if (!TransformLookup.TryGetComponent(componentData.OwnerEntity, out var componentData5))
		{
			return;
		}
		if (Hide)
		{
			componentData5.Position = Unity.Mathematics.float3.zero;
		}
		float2 float3 = -math.sign(PlayerController.mousePosition.xy - componentData5.Position.xy);
		bladeData.FollowPosition = new float3(float3 * 0.5f, -0.5f) + componentData5.Position;
		RefRW<LocalTransform> refRW2 = TransformLookup.GetRefRW(bladeData.Head);
		RefRW<LocalTransform> refRW3 = TransformLookup.GetRefRW(bladeData.Tail);
		RefRW<LocalTransform> refRW4 = TransformLookup.GetRefRW(bladeData.Material);
		RefRW<Spell4019BladeColorChangeMat> refRW5 = BladeGlowMatDataLookUp.GetRefRW(bladeData.Material);
		RefRW<Spell4019BladeHideUnderGroundMat> refRW6 = BladeUnderGroundMatDataLookUp.GetRefRW(bladeData.Material);
		config.ColorType.ColorEnumToString(out var result);
		switch (bladeData.StateNext)
		{
		case 0:
			DisableCollision(collider);
			ResetGlow(ref refRW5.ValueRW);
			movement.Direction = new float3(0f, -1f, 0f);
			BladeNoFallRotationSet(in componentData, in movement);
			bladeData.StateCurrent = 0;
			bladeData.StateNext = -1;
			bladeData.CanShoot = true;
			bladeData.CanReturn = false;
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			movement.CurrentFallSpeed = 0f;
			movement.OriginalSpellHorizontalSpeed = 0f;
			bladeData.ShowTrail = false;
			if (!LightningChainDestroyLookup.HasComponent(entity))
			{
				CMD.AddComponent<BreakLightningChain>(chunkIndex, entity);
			}
			break;
		case 1:
		{
			PlayAudio(chunkIndex, "StartShoot");
			EnableCollision(collider);
			bladeData.ShowTrail = TryShowTrail();
			if (!IsIgnoreWallRelic)
			{
				componentData.CanThroughWall = false;
			}
			if (movement.IsFallSpell)
			{
				PlayGlobalEffectDirectionScale(chunkIndex, "FallFlyUp", 4019, new float3(transform.Position.xy, -0.3f), new float3(1f, 0f, 0f), 1f, in config);
			}
			bladeData.CanReturn = true;
			config.DurationTimer = 0f;
			bladeData.StateCurrent = 1;
			bladeData.StateNext = -1;
			movement.ReboundCount = 99999;
			bladeData.RebounceTimeCurrent = bladeData.RebounceTimeTotal;
			config.Penetrate.Base = 99999;
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			transform.Position = componentData5.Position + new float3(0f, 0f, -0.3f);
			float degree = Random.random.NextFloat(0f - config.Scatter, config.Scatter) / 2f;
			movement.Direction = DTool.RotateDir(math.normalizesafe(new float3(PlayerController.mousePosition.xy - transform.Position.xy, 0f)), degree);
			movement.Speed = bladeData.Speed;
			velocity.Linear = movement.Speed * movement.Direction;
			float num4 = config.Duration.Calculate();
			bladeData.RandomDuration = Random.random.NextFloat(num4 - 0.05f, num4 + 0.05f);
			if (movement.Type == SpellSpecialMovementType.Rotation)
			{
				movement.AroundAngle = math.atan2(transform.Position.y - componentData5.Position.y, transform.Position.x - componentData5.Position.x) * 57.29578f;
				bladeData.RotateRadius = 0.5f;
			}
			if (movement.IsFallSpell)
			{
				movement.OriginalSpellHorizontalSpeed = bladeData.OriginSpeed;
				movement.CurrentFallSpeed = movement.OriginalSpellHorizontalSpeed;
				float z = -6f * movement.OriginalSpellHorizontalSpeed / bladeData.Speed;
				switch (movement.Type)
				{
				case SpellSpecialMovementType.Normal:
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseMouse:
				case SpellSpecialMovementType.ChaseOwner:
					transform.Position = PlayerController.mousePosition + new float3(math.normalizesafe(componentData5.Position.xy - PlayerController.mousePosition.xy) * 6f, z);
					break;
				case SpellSpecialMovementType.Rotation:
				{
					movement.AroundAngle = Random.random.NextFloat(360f);
					float3 dir = DTool.GetDir(movement.AroundAngle * (MathF.PI / 180f));
					transform.Position = componentData5.Position + new float3((movement.AroundRadius * dir).xy, z);
					bladeData.RotateRadius = movement.AroundRadius;
					break;
				}
				}
			}
			if (LightningChainDestroyLookup.HasComponent(entity))
			{
				CMD.RemoveComponent<BreakLightningChain>(chunkIndex, entity);
			}
			float num5 = 0f;
			int penetrate = 0;
			for (int i = 0; i < BladeWands.Length; i++)
			{
				if (BladeWands[i].WandId == bladeData.WandIndex)
				{
					num5 = BladeWands[i].LightningChainDamage;
					penetrate = BladeWands[i].LightningChainPenetrate;
					break;
				}
			}
			if (!(num5 <= 0f))
			{
				CMD.AppendToBuffer(chunkIndex, ChainReqEntity, new Spell3007CreateRequest
				{
					Source = entity,
					Damage = num5,
					Penetrate = penetrate,
					ColorName = result,
					MarkAsFired = true
				});
			}
			break;
		}
		case 2:
			DisableCollision(collider);
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			bladeData.ShowTrail = false;
			bladeData.StateCurrent = 2;
			bladeData.StateNext = -1;
			movement.Direction = new float3(0f, -1f, 0f);
			BladeNoFallRotationSet(in componentData, in movement);
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			movement.CurrentFallSpeed = 0f;
			movement.OriginalSpellHorizontalSpeed = 0f;
			bladeData.OnGroundRatio = 1f;
			bladeData.OnGroundShakeCurrent = 0f;
			bladeData.OnGroundShakeResult = Random.random.NextFloat(-40f, 40f);
			config.DurationTimer = 0f;
			transform.Position.z = -0.38f;
			SetHideSwordRateWithScale(0.1833f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			bladeData.CanReturn = true;
			break;
		case 3:
			bladeData.StateCurrent = 3;
			bladeData.StateNext = -1;
			SetHideSwordRateWithScale(0.1833f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			BladeNoFallRotationSet(in componentData, in movement);
			bladeData.CanReturn = true;
			bladeData.ShowTrail = false;
			break;
		case 4:
		{
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			bladeData.StateCurrent = 4;
			bladeData.StateNext = -1;
			bladeData.ShowTrail = false;
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			float degree2 = Random.random.NextFloat(-20f, 20f);
			bladeData.WallRebounceLength = Random.random.NextFloat(1f, 4f);
			float3 float5 = DTool.RotateDir(new float3(math.normalizesafe(componentData5.Position.xy - bladeData.WallHitPos.xy), 0f), degree2);
			bladeData.WallRebounceRandomPosition = float5 * bladeData.WallRebounceLength + bladeData.WallHitPos;
			config.DurationTimer = 0f;
			bladeData.IsRotClockwise = float5.x >= 0f;
			SpellTools.DisableSpellTrigger(in collider);
			BladeNoFallRotationSet(in componentData, in movement);
			bladeData.CanReturn = true;
			break;
		}
		case 5:
			DisableCollision(collider);
			PlayAudio(chunkIndex, "PickUp");
			bladeData.ShowTrail = TryShowTrail();
			bladeData.StateCurrent = 7;
			bladeData.StateNext = -1;
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			config.DurationTimer = 0f;
			SetHideSwordRateWithScale(1f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			bladeData.CanReturn = false;
			if (!LightningChainDestroyLookup.HasComponent(entity))
			{
				CMD.AddComponent<BreakLightningChain>(chunkIndex, entity);
			}
			break;
		case 6:
		{
			movement.Speed = 0f;
			movement.CurrentFallSpeed = 0f;
			movement.OriginalSpellHorizontalSpeed = 0f;
			movement.Gravity = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			config.DurationTimer = 0f;
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			SpellTools.EnableSpellTrigger(in collider);
			SpellTools.DisableSpellReboundCollider(in collider);
			bladeData.CanReturn = false;
			float3 float4 = componentData5.Position - transform.Position;
			bladeData.ShowTrail = TryShowTrail();
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Rotation:
				movement.AroundAngle = math.atan2(transform.Position.y - componentData5.Position.y, transform.Position.x - componentData5.Position.x) * 57.29578f - 90f;
				bladeData.RotateRadius = math.length(transform.Position.xy - componentData5.Position.xy);
				break;
			case SpellSpecialMovementType.ChaseEnemy:
			case SpellSpecialMovementType.ChaseOwner:
			{
				movement.Direction = math.normalizesafe(-float4);
				int num3 = (Random.random.NextBool() ? 1 : (-1));
				movement.Direction = DTool.RotateDir(movement.Direction, 30 * num3);
				break;
			}
			case SpellSpecialMovementType.ChaseMouse:
				movement.Direction = math.normalizesafe(float4);
				break;
			}
			bladeData.FallReturnRandom = Random.random.NextFloat(0f, 0.3f);
			bladeData.FallReturnIsGenerate = false;
			if (movement.IsFallSpell)
			{
				movement.Direction = new float3(0f, 1f, 0f);
				int stateCurrent = bladeData.StateCurrent;
				if (stateCurrent == 2 || stateCurrent == 3)
				{
					PlayGlobalEffectDirectionScale(chunkIndex, "FallFlyUp", 4019, new float3(transform.Position.xy, 0f), new float3(1f, 0f, 0f), 1f, in config);
					float3 touchGroundPos = new float3(transform.Position.xy, 0f);
					TouchGroundExplosionHit(entity, in config, in transform, in componentData, in movement, in elementEffect, in touchGroundPos, chunkIndex);
				}
			}
			bladeData.StateCurrent = 6;
			bladeData.StateNext = -1;
			break;
		}
		case 7:
			DisableCollision(collider);
			bladeData.StateCurrent = 5;
			bladeData.StateNext = -1;
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			transform.Position = bladeData.FollowPosition + new float3(bladeData.FollowRandomDiff, 0f);
			movement.Direction = new float3(0f, -1f, 0f);
			BladeNoFallRotationSet(in componentData, in movement);
			config.DurationTimer = 0f;
			SetHideSwordRateWithScale(1f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			ResetHeadTailTransform(ref refRW2.ValueRW, ref refRW3.ValueRW, ref refRW4.ValueRW);
			break;
		}
		switch (bladeData.StateCurrent)
		{
		case 0:
			movement.Speed = 0f;
			velocity.Linear = Unity.Mathematics.float3.zero;
			movement.CurrentFallSpeed = 0f;
			movement.Gravity = 0f;
			SetBladeFollowPosAndRot(ref refRW3.ValueRW, in bladeData, ref transform);
			SetHideSwordRateWithScale(0f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			break;
		case 1:
		{
			float2 vXY = velocity.Linear.xy;
			switch (movement.Type)
			{
			case SpellSpecialMovementType.Rotation:
			{
				bladeData.RotateRadius += movement.AroundRadius * DeltaTime;
				bladeData.RotateRadius = math.min(bladeData.RotateRadius, movement.AroundRadius);
				float num17 = 360f / (MathF.PI * 2f * movement.AroundRadius / bladeData.Speed) * DeltaTime;
				movement.AroundAngle += num17;
				movement.Direction = Tool2D.GetDir(movement.AroundAngle + 90f);
				float3 float11 = (float3)Tool2D.GetDir(movement.AroundAngle) * bladeData.RotateRadius;
				vXY = (float11.xy + componentData5.Position.xy - transform.Position.xy) / DeltaTime;
				transform.Position.xy = float11.xy + componentData5.Position.xy;
				velocity.Linear = Unity.Mathematics.float3.zero;
				break;
			}
			case SpellSpecialMovementType.ChaseMouse:
			{
				float3 float10 = DTool.IgnoreZDir(in PlayerController.mousePosition, in transform.Position);
				ref float3 linear2 = ref velocity.Linear;
				float3 touchGroundPos = float10 * movement.Speed;
				velocity.Linear = DTool.Lerp(in linear2, in touchGroundPos, movement.Speed * DeltaTime * movement.ChaseMouseLerpSpeed * 0.5f);
				vXY = velocity.Linear.xy;
				movement.Direction = math.normalize(velocity.Linear);
				break;
			}
			}
			if (!movement.IsFallSpell)
			{
				foreach (StatefulCollisionEvent item in collisionBuffer)
				{
					if (bladeData.RebounceTimeCurrent <= 0)
					{
						bladeData.StateNext = 4;
						bladeData.WallHitPos = item.CollisionDetails.AverageContactPointPosition;
						CheckThunderStone(chunkIndex, entity, in transform, in componentData, in movement, in config, in elementEffect);
						break;
					}
					bladeData.RebounceTimeCurrent--;
				}
				TriggerHitEffect(triggerBuffer, in config, entity, in movement, chunkIndex);
				if (config.DurationTimer >= bladeData.RandomDuration)
				{
					bladeData.StateNext = 2;
					float3 float12 = (bladeData.RandomDuration - config.DurationTimer) * velocity.Linear;
					transform.Position += float12;
					CheckThunderStone(chunkIndex, entity, in transform, in componentData, in movement, in config, in elementEffect);
				}
				BladeNoFallRotationSet(in componentData, in movement);
			}
			else
			{
				BladeNormalFallRotationSet(in componentData, in movement, vXY);
			}
			SetHideSwordRateWithScale(0f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			SetGlowSwordLerp(1f, ref refRW5.ValueRW);
			break;
		}
		case 2:
			if (bladeData.OnGroundRatio >= 10f)
			{
				bladeData.StateNext = 3;
			}
			bladeData.OnGroundShakeCurrent = 50f / bladeData.OnGroundRatio * math.sin(config.DurationTimer * 100f);
			bladeData.OnGroundRatio += DeltaTime * 50f;
			refRW2.ValueRW.Rotation = quaternion.RotateZ((bladeData.OnGroundShakeCurrent + bladeData.OnGroundShakeResult) * (MathF.PI / 180f));
			SetGlowSwordLerp(0f, ref refRW5.ValueRW);
			TryDistanceReturn(ref bladeData, in transform, in config, in componentData5);
			break;
		case 3:
			SetGlowSwordLerp(0f, ref refRW5.ValueRW);
			TryDistanceReturn(ref bladeData, in transform, in config, in componentData5);
			break;
		case 4:
		{
			SetHideSwordRateWithScale(0f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			SetGlowSwordLerp(0f, ref refRW5.ValueRW);
			float num7 = 20f;
			float num8 = 10f;
			float num9 = bladeData.WallRebounceLength / num8;
			float num10 = num7 * num9 / 2f;
			config.DurationTimer += DeltaTime;
			float num11 = num10 * config.DurationTimer - 0.5f * num7 * config.DurationTimer * config.DurationTimer;
			float num12 = config.DurationTimer * num8;
			float2 float6 = math.normalizesafe(bladeData.WallRebounceRandomPosition.xy - bladeData.WallHitPos.xy);
			transform.Position = bladeData.WallHitPos + new float3(float6 * num12, 0f - num11);
			float num13 = 1800f * config.DurationTimer;
			if (bladeData.IsRotClockwise)
			{
				num13 *= -1f;
			}
			refRW4.ValueRW.Rotation = quaternion.Euler(new float3(0f, 0f, num13));
			if (num12 >= bladeData.WallRebounceLength)
			{
				transform.Position = bladeData.WallRebounceRandomPosition;
				bladeData.StateNext = 2;
			}
			break;
		}
		case 5:
			SetBladeFollowPosAndRot(ref refRW3.ValueRW, in bladeData, ref transform);
			if (config.DurationTimer > 0.2f)
			{
				bladeData.ShowTrail = false;
			}
			if (config.DurationTimer < 0.3f)
			{
				SetGlowSwordLerp(1f, ref refRW5.ValueRW);
				SetHideSwordRateWithScale(1f - config.DurationTimer / 0.3f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			}
			else if (config.DurationTimer < 0.6f)
			{
				SetGlowSwordLerp(0f, ref refRW5.ValueRW);
				SetHideSwordRateWithScale(0f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			}
			else
			{
				bladeData.StateNext = 0;
			}
			break;
		case 6:
			if (!movement.IsFallSpell)
			{
				float3 target = componentData5.Position - transform.Position;
				float num14 = 80f * DeltaTime;
				TriggerHitEffect(triggerBuffer, in config, entity, in movement, chunkIndex);
				if (math.length(target) <= num14 * 2f + 0.2f)
				{
					bladeData.StateNext = 7;
					bladeData.CanShoot = true;
					bladeData.ShowTrail = false;
					break;
				}
				switch (movement.Type)
				{
				case SpellSpecialMovementType.Normal:
					movement.Direction = math.normalizesafe(target);
					velocity.Linear = movement.Direction * 80f;
					break;
				case SpellSpecialMovementType.ChaseEnemy:
				case SpellSpecialMovementType.ChaseOwner:
				{
					float3 touchGroundPos = movement.Direction;
					movement.Direction = DTool.DirMoveTowardsIgnoreZ(in touchGroundPos, in target, bladeData.Speed * movement.ChaseRotateSpeed * DeltaTime * (1f + config.DurationTimer));
					velocity.Linear = movement.Direction * 80f;
					break;
				}
				case SpellSpecialMovementType.Rotation:
				{
					float num15 = math.max(math.length(componentData5.Position.xy - transform.Position.xy), bladeData.RotateRadius) * DeltaTime;
					bladeData.RotateRadius -= num15;
					float num16 = 360f / (MathF.PI * 2f * movement.AroundRadius / bladeData.Speed) * DeltaTime;
					movement.AroundAngle += num16;
					float3 float8 = (float3)Tool2D.GetDir(movement.AroundAngle) * bladeData.RotateRadius;
					float3 float9 = math.normalizesafe(new float3(float8.xy - componentData5.Position.xy, 0f));
					transform.Position.xy = float8.xy + componentData5.Position.xy;
					velocity.Linear = Unity.Mathematics.float3.zero;
					float3 dir2 = DTool.GetDir((movement.AroundAngle + 90f) * (MathF.PI / 180f));
					movement.Direction = math.normalizesafe(dir2 * bladeData.Speed * DeltaTime + new float3(-float9.xy * num15, 0f));
					break;
				}
				case SpellSpecialMovementType.ChaseMouse:
				{
					float3 float7 = ((!(config.DurationTimer < config.Duration.Calculate() / 2f)) ? DTool.IgnoreZDir(in componentData5.Position, in transform.Position) : DTool.IgnoreZDir(in PlayerController.mousePosition, in transform.Position));
					ref float3 linear = ref velocity.Linear;
					float3 touchGroundPos = float7 * bladeData.Speed;
					velocity.Linear = DTool.Lerp(in linear, in touchGroundPos, bladeData.Speed * DeltaTime * movement.ChaseMouseLerpSpeed * 0.5f);
					movement.Direction = math.normalize(velocity.Linear);
					break;
				}
				}
			}
			else if (config.DurationTimer < 0.3f)
			{
				transform.Position.z += (0f - DeltaTime) * 50f;
			}
			else if (config.DurationTimer < 0.6f + bladeData.FallReturnRandom)
			{
				if (!bladeData.FallReturnIsGenerate)
				{
					bladeData.FallReturnIsGenerate = true;
					transform.Position = bladeData.FollowPosition + new float3(bladeData.FollowRandomDiff, 0f);
					movement.Direction = new float3(0f, -1f, 0f);
				}
				transform.Position.z = (0f - (0.6f + bladeData.FallReturnRandom - config.DurationTimer)) * 50f;
				movement.CurrentFallSpeed = 0f;
			}
			else
			{
				bladeData.StateNext = 0;
				bladeData.CanShoot = true;
				PlayAudio(chunkIndex, "PickUp");
			}
			BladeNoFallRotationSet(in componentData, in movement);
			SetGlowSwordLerp(1f, ref refRW5.ValueRW, 20f);
			SetHideSwordRateWithScale(0f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			break;
		case 7:
		{
			float3 x = bladeData.FollowPosition + new float3(bladeData.FollowRandomDiff, 0f) - transform.Position;
			float num6 = 80f * DeltaTime;
			if (math.length(x) <= num6)
			{
				bladeData.StateNext = 7;
				bladeData.CanShoot = true;
				bladeData.ShowTrail = false;
			}
			else
			{
				transform.Position += math.normalizesafe(x) * num6;
				SetHideSwordRateWithScale(1f, ref refRW6.ValueRW, in transform, bladeData.ResizeForHideShader);
			}
			break;
		}
		}
		if (bladeData.RequiredTrail)
		{
			if (TryGetLinkEffect(linkBuffer, out bladeData.TrailEffect, "Trail"))
			{
				bladeData.RequiredTrail = false;
			}
			return;
		}
		if (!bladeData.ShowTrail)
		{
			TryRecycleEffect(bladeData.TrailEffect, "Trail", entity, chunkIndex);
			bladeData.TrailEffect = default(UnityObjectRef<GameObject>);
		}
		else if (!bladeData.TrailEffect.IsValid())
		{
			TrailEffectRequire(result, entity, chunkIndex);
			bladeData.RequiredTrail = true;
		}
		EnableGlobalParticleEmitter(componentData.SpellEffectEntity, bladeData.ShowTrail, chunkIndex);
	}

	private bool TryShowTrail()
	{
		return Random.random.NextFloat(0f, 1f) < OptimizeData.PoolEffectShowRatio;
	}

	private void CheckThunderStone(int chunkIndex, Entity entity, in LocalTransform transform, in SpellComponentData componentData, in SpellMovementComponentData movement, in SpellConfigComponentData config, in SpellElementEffectComponentData elementEffect)
	{
		if (config.ColorType == SpellColorType.Thunder)
		{
			CMD.CheckFallThunderDamage(chunkIndex, Spell3101Buffer, transform.Position, UnitPropertyLookup, PhysicsWorld, in config, in movement, in transform, in elementEffect, in componentData, entity);
		}
	}

	private bool TryGetLinkEffect(DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out UnityObjectRef<GameObject> linkedObject, string name)
	{
		foreach (SpellGameObjectEffectLink item in linkBuffer)
		{
			SpellGameObjectEffectLink current = item;
			if (current.EffectName == name)
			{
				linkedObject = current.GameObject;
				return true;
			}
		}
		linkedObject = default(UnityObjectRef<GameObject>);
		return false;
	}

	private void EnableGlobalParticleEmitter(Entity entity, bool enable, [ChunkIndexInQuery] int chunkIndex)
	{
		if (enable != GlobalParticleEmitterLookUp.IsComponentEnabled(entity))
		{
			if (GlobalParticleMoveEmitterLookUp.TryGetComponent(entity, out var componentData))
			{
				componentData.startCounter = false;
			}
			CMD.SetComponentEnabled<GlobalParticle.Emitter>(chunkIndex, entity, enable);
		}
	}

	private void TrailEffectRequire(FixedString32Bytes colorName, Entity spellEntity, [ChunkIndexInQuery] int chunkIndex)
	{
		CMD.AppendToBuffer(chunkIndex, EffectRequireBufferEntity, new SpellEffectSystem.Require
		{
			Entity = spellEntity,
			SpellId = 4019,
			Color = colorName,
			Settings = 
			{
				Name = "Trail",
				Layer = LayerCorrectType.Coordinate,
				IgnoreColor = false,
				ClearParticle = true,
				ClearTrail = true,
				ScaleMode = SpellEffectSystem.ScaleMode.Ignore
			}
		});
	}

	private void TryRecycleEffect(UnityObjectRef<GameObject> goRef, string name, Entity spellEntity, [ChunkIndexInQuery] int chunkIndex)
	{
		if (goRef.IsValid())
		{
			CMD.AppendToBuffer(chunkIndex, EffectRecycleBufferEntity, new SpellEffectSystem.Destroy
			{
				Entity = spellEntity,
				Name = name
			});
		}
	}

	private void SetBladeFollowPosAndRot(ref LocalTransform rotCenterTail, in Spell4019BiAnBladeData bladeData, ref LocalTransform transform)
	{
		float3 end = bladeData.FollowPosition + new float3(bladeData.FollowRandomDiff, 0f);
		transform.Position = DTool.Lerp(in transform.Position, in end, 0.1f);
		float num = 1f - 1f / (math.abs(end.x - transform.Position.x) + 1f);
		float num2 = math.sign(end.x - transform.Position.x);
		float num3 = -90f * num * num2 * (0.5f - num2 * bladeData.FollowRandomDiff.x) / 1f;
		float num4 = 1f - 1f / (math.abs(end.y - transform.Position.y) + 1f);
		float x = bladeData.FollowRandomDiff.x;
		float num5 = ((end.y >= transform.Position.y) ? 40f : (-10f));
		num3 += num5 * x * num4;
		quaternion q = quaternion.Euler(new float3(0f, 0f, num3 * (MathF.PI / 180f)));
		rotCenterTail.Rotation = math.slerp(rotCenterTail.Rotation, q, 0.2f);
	}

	private void TryDistanceReturn(ref Spell4019BiAnBladeData bladeData, in LocalTransform spellTrans, in SpellConfigComponentData config, in LocalTransform ownerT)
	{
		float num = 2f * (config.Radius.AddRatio + 1f) * config.Radius.MulRatio;
		if (math.length(ownerT.Position - spellTrans.Position) <= num)
		{
			bladeData.StateNext = 6;
			bladeData.ShowTrail = TryShowTrail();
		}
	}

	private void BladeNoFallRotationSet(in SpellComponentData componentData, in SpellMovementComponentData movement)
	{
		TransformLookup.GetRefRW(componentData.SpellEffectEntity).ValueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(movement.Direction.y, movement.Direction.x));
	}

	private void BladeNormalFallRotationSet(in SpellComponentData componentData, in SpellMovementComponentData movement, float2 vXY)
	{
		float3 rootPosition = new float3(vXY, movement.CurrentFallSpeed);
		float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
		rootPosition = math.normalizesafe(new float3(rootPosition.xy + layerPosition.xy, 0f));
		TransformLookup.GetRefRW(componentData.SpellEffectEntity).ValueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(rootPosition.y, rootPosition.x));
	}

	private void SetHideSwordRateWithScale(float rate, ref Spell4019BladeHideUnderGroundMat bladeHide, in LocalTransform spellTrans, float scale)
	{
		float3 layerPosition = DTool.GetLayerPosition(in spellTrans.Position, LayerCorrectType.Coordinate);
		bladeHide.Value = spellTrans.Position.y + layerPosition.y + (rate - 0.5f) * scale;
	}

	private void SetGlowSwordLerp(float targetGlow, ref Spell4019BladeColorChangeMat bladeGlow, float lerpRatio = 10f)
	{
		bladeGlow.Value = DTool.Lerp(bladeGlow.Value, targetGlow, DeltaTime * lerpRatio);
	}

	private void ResetGlow(ref Spell4019BladeColorChangeMat bladeGlow)
	{
		bladeGlow.Value = 0f;
	}

	private void ResetHeadTailTransform(ref LocalTransform headT, ref LocalTransform tailT, ref LocalTransform centerT)
	{
		headT.Rotation = quaternion.identity;
		tailT.Rotation = quaternion.identity;
		centerT.Rotation = quaternion.identity;
	}

	private void DisableCollision(PhysicsCollider collider)
	{
		SpellTools.DisableSpellReboundCollider(in collider);
		SpellTools.DisableSpellTrigger(in collider);
	}

	private void EnableCollision(PhysicsCollider collider)
	{
		SpellTools.EnableSpellTrigger(in collider);
		if (!IsIgnoreWallRelic)
		{
			SpellTools.EnableSpellReboundCollider(in collider);
		}
	}

	private void TriggerHitEffect(DynamicBuffer<StatefulTriggerEvent> triggerBuffer, in SpellConfigComponentData config, Entity entity, in SpellMovementComponentData movement, [ChunkIndexInQuery] int chunkIndex)
	{
		foreach (StatefulTriggerEvent item in triggerBuffer)
		{
			if (item.State == StatefulEventState.Enter)
			{
				Entity otherEntity = item.GetOtherEntity(entity);
				if (UnitPropertyLookup.HasComponent(otherEntity) && TransformLookup.HasComponent(otherEntity))
				{
					PlayGlobalEffectDirectionScale(chunkIndex, "Hit", 4019, TransformLookup.GetRefRO(otherEntity).ValueRO.Position + new float3(0f, 0.3f, 0f), movement.Direction, 1f, in config);
					PlayAudio(chunkIndex, "Hit");
				}
			}
		}
	}

	[BurstCompile]
	private void PlayGlobalEffectDirectionScale([ChunkIndexInQuery] int chunkIndex, FixedString32Bytes name, int id, float3 position, float3 direction, float scale, in SpellConfigComponentData config)
	{
		config.ColorType.ColorEnumToString(out var result);
		float3 layerPosition = DTool.GetLayerPosition(in position, LayerCorrectType.Coordinate);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"{id}_{name}_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(position) + layerPosition;
		globalParticleEmitParams.Velocity = direction;
		globalParticleEmitParams.Size = scale;
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
	}

	[BurstCompile]
	private void HitDamageApplyExplosion(Entity spellEntity, in NativeList<Entity> hitList, in float3 explosionPos, in SpellComponentData componentData, in SpellConfigComponentData config, in SpellMovementComponentData movement, in LocalTransform transform, in SpellElementEffectComponentData elementEffect, [ChunkIndexInQuery] int chunkIndex)
	{
		for (int i = 0; i < hitList.Length; i++)
		{
			TakeDamageInfo_Dots.NewInfo(spellEntity, CostPenetrate: false, in config, in movement, in transform, in elementEffect, in componentData, out var info, CostRefraction: false);
			info.spell.HitPosition = TransformLookup[hitList[i]].Position;
			info.SetKnockbackForceIgnoreZBySpell(math.normalizesafe(info.spell.HitPosition - explosionPos));
			ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
			Entity target = hitList[i];
			cMD.TryAttackEntity(chunkIndex, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
		}
	}

	[BurstCompile]
	private void TouchGroundExplosionHit(Entity spellEntity, in SpellConfigComponentData config, in LocalTransform trans, in SpellComponentData data, in SpellMovementComponentData movement, in SpellElementEffectComponentData elementEffect, in float3 touchGroundPos, [ChunkIndexInQuery] int chunkIndex)
	{
		config.ColorType.ColorEnumToString(out var result);
		GlobalParticleEmitParams globalParticleEmitParams = default(GlobalParticleEmitParams);
		globalParticleEmitParams.Name = $"3119_Fall_{result}";
		globalParticleEmitParams.Alpha = 1f;
		globalParticleEmitParams.Position = new float3(touchGroundPos.xy, 1.08f);
		globalParticleEmitParams.Size = config.Radius.Calculate();
		GlobalParticleEmitParams element = globalParticleEmitParams;
		CMD.AppendToBuffer(chunkIndex, GlobalParticleSystemBufferEntity, element);
		NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
		float radius = config.Radius.Calculate();
		SpellTools.GetAttackableEntitiesInRange(in touchGroundPos, in radius, in config.ShooterType, containsBrittleness: true, in UnitPropertyLookup, in SpellConfigLookup, in PhysicsWorld, ref entities);
		HitDamageApplyExplosion(spellEntity, in entities, in touchGroundPos, in data, in config, in movement, in trans, in elementEffect, chunkIndex);
	}

	[BurstCompile]
	private void PlayAudio([ChunkIndexInQuery] int chunkIndex, string name)
	{
		ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
		Entity sEPlayerSingleton = SEPlayerSingleton;
		FixedString32Bytes seName = name;
		cMD.AppendToBuffer(chunkIndex, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(4019, in seName)));
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellConfigComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellMovementComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr6 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpellElementEffectComponentData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr7 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell4019BiAnBladeData_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr8 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
		BufferAccessor<StatefulCollisionEvent> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulCollisionEvent_RW_BufferTypeHandle);
		BufferAccessor<StatefulTriggerEvent> bufferAccessor2 = chunk.GetBufferAccessor(ref __TypeHandle.__Unity_Physics_Stateful_StatefulTriggerEvent_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr9 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentTypeHandle);
		BufferAccessor<SpellGameObjectEffectLink> bufferAccessor3 = chunk.GetBufferAccessor(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				ref LocalTransform transform = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i);
				ref SpellConfigComponentData config = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, i);
				ref SpellMovementComponentData movement = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, i);
				ref SpellComponentData componentData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, i);
				ref SpellElementEffectComponentData elementEffect = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, i);
				ref Spell4019BiAnBladeData bladeData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4019BiAnBladeData>(nativeArrayPtr7, i);
				ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, i);
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer = bufferAccessor[i];
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer = bufferAccessor2[i];
				ref PhysicsVelocity velocity = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, i);
				DynamicBuffer<SpellGameObjectEffectLink> linkBuffer = bufferAccessor3[i];
				int entityInQueryIndex = __ChunkBaseEntityIndices[chunkIndexInQuery] + num;
				Execute(entity, ref transform, ref config, ref movement, ref componentData, ref elementEffect, ref bladeData, ref collider, collisionBuffer, triggerBuffer, ref velocity, linkBuffer, chunkIndexInQuery, entityInQueryIndex);
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
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, nextRangeBegin);
					ref LocalTransform transform2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin);
					ref SpellConfigComponentData config2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, nextRangeBegin);
					ref SpellMovementComponentData movement2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, nextRangeBegin);
					ref SpellComponentData componentData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, nextRangeBegin);
					ref SpellElementEffectComponentData elementEffect2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, nextRangeBegin);
					ref Spell4019BiAnBladeData bladeData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4019BiAnBladeData>(nativeArrayPtr7, nextRangeBegin);
					ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, nextRangeBegin);
					DynamicBuffer<StatefulCollisionEvent> collisionBuffer2 = bufferAccessor[nextRangeBegin];
					DynamicBuffer<StatefulTriggerEvent> triggerBuffer2 = bufferAccessor2[nextRangeBegin];
					ref PhysicsVelocity velocity2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, nextRangeBegin);
					DynamicBuffer<SpellGameObjectEffectLink> linkBuffer2 = bufferAccessor3[nextRangeBegin];
					int entityInQueryIndex2 = __ChunkBaseEntityIndices[chunkIndexInQuery] + num;
					Execute(entity2, ref transform2, ref config2, ref movement2, ref componentData2, ref elementEffect2, ref bladeData2, ref collider2, collisionBuffer2, triggerBuffer2, ref velocity2, linkBuffer2, chunkIndexInQuery, entityInQueryIndex2);
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
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, j);
				ref LocalTransform transform3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j);
				ref SpellConfigComponentData config3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, j);
				ref SpellMovementComponentData movement3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, j);
				ref SpellComponentData componentData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, j);
				ref SpellElementEffectComponentData elementEffect3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, j);
				ref Spell4019BiAnBladeData bladeData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4019BiAnBladeData>(nativeArrayPtr7, j);
				ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, j);
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer3 = bufferAccessor[j];
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer3 = bufferAccessor2[j];
				ref PhysicsVelocity velocity3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, j);
				DynamicBuffer<SpellGameObjectEffectLink> linkBuffer3 = bufferAccessor3[j];
				int entityInQueryIndex3 = __ChunkBaseEntityIndices[chunkIndexInQuery] + num;
				Execute(entity3, ref transform3, ref config3, ref movement3, ref componentData3, ref elementEffect3, ref bladeData3, ref collider3, collisionBuffer3, triggerBuffer3, ref velocity3, linkBuffer3, chunkIndexInQuery, entityInQueryIndex3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, k);
				ref LocalTransform transform4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k);
				ref SpellConfigComponentData config4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellConfigComponentData>(nativeArrayPtr3, k);
				ref SpellMovementComponentData movement4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellMovementComponentData>(nativeArrayPtr4, k);
				ref SpellComponentData componentData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellComponentData>(nativeArrayPtr5, k);
				ref SpellElementEffectComponentData elementEffect4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpellElementEffectComponentData>(nativeArrayPtr6, k);
				ref Spell4019BiAnBladeData bladeData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell4019BiAnBladeData>(nativeArrayPtr7, k);
				ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr8, k);
				DynamicBuffer<StatefulCollisionEvent> collisionBuffer4 = bufferAccessor[k];
				DynamicBuffer<StatefulTriggerEvent> triggerBuffer4 = bufferAccessor2[k];
				ref PhysicsVelocity velocity4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsVelocity>(nativeArrayPtr9, k);
				DynamicBuffer<SpellGameObjectEffectLink> linkBuffer4 = bufferAccessor3[k];
				int entityInQueryIndex4 = __ChunkBaseEntityIndices[chunkIndexInQuery] + num;
				Execute(entity4, ref transform4, ref config4, ref movement4, ref componentData4, ref elementEffect4, ref bladeData4, ref collider4, collisionBuffer4, triggerBuffer4, ref velocity4, linkBuffer4, chunkIndexInQuery, entityInQueryIndex4);
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
