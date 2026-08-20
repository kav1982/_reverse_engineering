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
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[CompilerGenerated]
internal struct Monster317System : ISystem, ISystemCompilerGenerated
{
	public struct Monster317AttackRequest : IBufferElementData
	{
		public Vector3 startPoint;

		public Vector3 requestPoint;

		public Entity masterEntity;

		public bool buffed;

		public float flyTime;

		public bool isPattern2;
	}

	[BurstCompile]
	[CompilerGenerated]
	public struct Monster317Job : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Monster317_Dots> __Monster317_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<UnitBase_Dots> __UnitBase_Dots_RW_ComponentTypeHandle;

				public ComponentTypeHandle<EndlessMonsterTag> __EndlessMonsterTag_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Monster317_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Monster317_Dots>();
					__UnitBase_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnitBase_Dots>();
					__EndlessMonsterTag_RW_ComponentTypeHandle = state.GetComponentTypeHandle<EndlessMonsterTag>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Monster317_Dots_RW_ComponentTypeHandle.Update(ref state);
					__UnitBase_Dots_RW_ComponentTypeHandle.Update(ref state);
					__EndlessMonsterTag_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster317_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
				entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<EndlessMonsterTag>();
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
			public void Run(ref Monster317Job job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref Monster317Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref Monster317Job job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref Monster317Job job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref Monster317Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref Monster317Job job, EntityManager entityManager)
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

		public SpellSpawnParams ssp;

		public Entity ShootSpellBufferEntity;

		public Entity attackRequestEntity;

		public Entity bufferClearEntity;

		public CurrentRoomEntitiesSingleton CurrentRoomEntities;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> LocalTsfLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<AnimaPlay> AnimaLookUp;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<UnitProperty_Dots> PptLookUp;

		[NativeDisableUnsafePtrRestriction]
		public RefRW<GlobalRandom> globalRandom;

		[NativeDisableParallelForRestriction]
		public BufferLookup<Monster317_Aim> aimBufferLookUp;

		public Entity SEBufferEntity;

		public Entity particleBufferEntity;

		public float deltaTime;

		public EntityCommandBuffer.ParallelWriter ecb;

		public Entity playerEntity;

		public float3 playerMotion;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		public void Execute([ChunkIndexInQuery] int index, ref Monster317_Dots monster, ref UnitBase_Dots unitBase, ref EndlessMonsterTag endlessTag, Entity entity)
		{
			LocalTransform localTransform = LocalTsfLookUp[entity];
			ref UnitProperty_Dots valueRW = ref PptLookUp.GetRefRW(entity).ValueRW;
			ref LocalTransform valueRW2 = ref LocalTsfLookUp.GetRefRW(valueRW.ett_Motion).ValueRW;
			if (!monster.Initialized)
			{
				monster.Initialized = true;
				monster.state = Monster317State.MoveToPlayer;
				valueRW.CanTouch = false;
				monster.stateExistTime = 1f;
			}
			if (valueRW.LockMotion)
			{
				return;
			}
			if (monster.stateQuit)
			{
				monster.stateQuit = false;
				monster.changedState = true;
			}
			else
			{
				monster.changedState = false;
			}
			monster.stateExistTime += deltaTime;
			bool flag = false;
			float3 @float = default(float3);
			if (!LocalTsfLookUp.HasComponent(unitBase.targetEtt))
			{
				if (CurrentRoomEntities.FindNearestTarget(localTransform.Position, UnitType.Monster, out var target, out var _, out var _))
				{
					unitBase.targetEtt = target;
					@float = LocalTsfLookUp[target].Position;
					flag = true;
				}
				else
				{
					unitBase.targetEtt = Entity.Null;
				}
			}
			else
			{
				@float = LocalTsfLookUp[unitBase.targetEtt].Position;
				flag = true;
			}
			float num = Tool2D.IgnoreZDistanceSqr(@float, localTransform.Position);
			monster.flyTime += deltaTime * MathF.PI * 2f;
			if (monster.flyTime > MathF.PI * 2f)
			{
				monster.flyTime -= MathF.PI * 2f;
			}
			valueRW2.Position = new float3(0f, (monster.isPattern2 ? 2f : 1.6f) + math.sin(monster.flyTime) * 0.15f, 0f);
			valueRW2.Scale = 1f;
			valueRW2.Rotation = Quaternion.Euler(0f, 0f, (0f - Mathf.Clamp(Vector3.Dot(Tool2D.IgnoreZV2ToV1Normal(@float, localTransform.Position), (Vector3)unitBase.currentMotion) / valueRW.unitCfg.moveSpeed * 1.5f, -1f, 1f)) * 10f);
			switch (monster.state)
			{
			case Monster317State.MoveToPlayer:
				if (!flag)
				{
					unitBase.SetMove(float3.zero);
					break;
				}
				if (num > math.pow(monster.relativeDistance, 2f))
				{
					unitBase.SetMove(valueRW.MoveSpeed * Tool2D.IgnoreZV2ToV1Normal(@float, localTransform.Position), thisTimeShouldFlip: false);
				}
				else if (num < math.pow(monster.relativeDistance, 2f))
				{
					unitBase.SetMove(valueRW.MoveSpeed * -Tool2D.IgnoreZV2ToV1Normal(@float, localTransform.Position), thisTimeShouldFlip: false);
				}
				else
				{
					unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				}
				unitBase.SetFlip(@float.x < localTransform.Position.x);
				if (monster.stateExistTime > 1.5f && num <= math.pow(monster.relativeDistance + 1.5f, 2f) && num >= math.pow(monster.relativeDistance - 1.5f, 2f))
				{
					monster.state = Monster317State.SlowDown;
				}
				break;
			case Monster317State.SlowDown:
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				unitBase.SetFlip(@float.x < localTransform.Position.x);
				if (monster.stateExistTime > 0.3f)
				{
					monster.state = Monster317State.Attack;
				}
				break;
			case Monster317State.Attack:
			{
				if (monster.changedState)
				{
					monster.attackTargetPoint = @float;
					if (flag && unitBase.targetEtt == playerEntity)
					{
						monster.attackTargetPoint += playerMotion * 0.5f;
					}
					monster.attackTargetPoint = localTransform.Position + (float3)Tool2D.IgnoreZV2ToV1Normal(monster.attackTargetPoint, localTransform.Position) * monster.relativeDistance;
					monster.startShootPoint = localTransform.Position;
					monster.attackShootTimer = 0f;
					monster.attackShootCount = 0;
				}
				unitBase.SetMove(float3.zero, thisTimeShouldFlip: false);
				unitBase.SetFlip(monster.attackTargetPoint.x < localTransform.Position.x);
				monster.attackShootTimer += deltaTime;
				while (monster.attackShootTimer >= monster.shootInterval && monster.attackShootCount < monster.shootCount)
				{
					monster.attackShootTimer -= monster.shootInterval;
					monster.attackShootCount++;
					float3 attackPoint = GetAttackPoint(monster.startShootPoint, monster.attackTargetPoint, monster.attackShootCount, monster);
					ecb.AppendToBuffer(index, entity, new Monster317_Aim
					{
						time = 0.6f,
						point = attackPoint
					});
					ecb.AppendToBuffer(index, particleBufferEntity, new GlobalParticleEmitParams(GlobalParticleType.EF, "EF_Monster317_Aim", Tool2D.GetLayerPoint(attackPoint, LayerCorrectType.GroundEffect))
					{
						Size = (monster.isPattern2 ? 1.2f : 0.8f)
					});
				}
				DynamicBuffer<Monster317_Aim> dynamicBuffer = aimBufferLookUp[entity];
				for (int num2 = dynamicBuffer.Length - 1; num2 >= 0; num2--)
				{
					Monster317_Aim value = dynamicBuffer[num2];
					value.time -= deltaTime;
					if (value.time < 0f)
					{
						Vector3 vector = Tool2D.IgnoreZPoint(localTransform.Position + new float3(1f, 0f, 0f) * Mathf.Sign(monster.attackTargetPoint.x - localTransform.Position.x) * 0.5f, 0f - valueRW2.Position.y + 0.8f);
						Shoot(index, entity, vector, value.point, valueRW.affect_MucusSpellSpeedRatio, monster, endlessTag.has316Buff, monster.isPattern2);
						ecb.AppendToBuffer(index, bufferClearEntity, new Monster317_BufferClear
						{
							clearEntity = entity
						});
					}
					dynamicBuffer[num2] = value;
				}
				if ((double)monster.stateExistTime >= 1.2 + (double)(monster.shootInterval * (float)monster.shootCount))
				{
					monster.state = Monster317State.MoveToPlayer;
				}
				break;
			}
			}
		}

		private void Shoot(int index, Entity entity, float3 position, float3 targetPoint, float spellSpeedRatio, Monster317_Dots monster, bool buffed, bool isPattern2)
		{
			ssp.SetShooter(entity, entity);
			ssp.SpawnPosition = position;
			ssp.MovementComponentData.Gravity = 0f;
			ssp.MovementComponentData.Speed = monster.spellSpeed;
			ssp.MovementComponentData.Speed *= spellSpeedRatio;
			float num = math.max(Tool2D.IgnoreZDistance(position, targetPoint), 0.01f);
			ssp.MovementComponentData.Speed *= num / monster.relativeDistance;
			float num2 = 0.1f;
			ssp.MovementComponentData.CurrentFallSpeed = (0f - (position.z - targetPoint.z)) / num2;
			ssp.MovementComponentData.Direction = Tool2D.IgnoreZV2ToV1Normal(targetPoint, position);
			ecb.AppendToBuffer(index, attackRequestEntity, new Monster317AttackRequest
			{
				masterEntity = entity,
				startPoint = position,
				requestPoint = targetPoint,
				buffed = buffed,
				flyTime = num2,
				isPattern2 = isPattern2
			});
			ecb.AppendToBuffer(index, SEBufferEntity, new SEData("SE_Monster317_Attack"));
			ecb.AppendToBuffer(index, particleBufferEntity, new GlobalParticleEmitParams(GlobalParticleType.EF, "EF_EndlessBulletShoot", Tool2D.GetLayerPoint(ssp.SpawnPosition))
			{
				Velocity = -Tool2D.IgnoreZPoint(targetPoint - position + new float3(0f, 0f - targetPoint.z, 0f))
			});
		}

		private float3 GetAttackPoint(float3 shootPosition, float3 targetPoint, int shootIndex, Monster317_Dots monster)
		{
			float3 @float = Tool2D.IgnoreZV2ToV1Normal(targetPoint, shootPosition);
			return (float3)Tool2D.IgnoreZPoint(shootPosition) + @float * (1f + (float)shootIndex * monster.shootDistanceInterval);
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Monster317_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__UnitBase_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__EndlessMonsterTag_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Monster317_Dots monster = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster317_Dots>(nativeArrayPtr, i);
					ref UnitBase_Dots unitBase = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, i);
					ref EndlessMonsterTag endlessTag = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, i);
					Execute(chunkIndexInQuery, ref monster, ref unitBase, ref endlessTag, entity);
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
						ref Monster317_Dots monster2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster317_Dots>(nativeArrayPtr, nextRangeBegin);
						ref UnitBase_Dots unitBase2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, nextRangeBegin);
						ref EndlessMonsterTag endlessTag2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, nextRangeBegin);
						Execute(chunkIndexInQuery, ref monster2, ref unitBase2, ref endlessTag2, entity2);
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
					ref Monster317_Dots monster3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster317_Dots>(nativeArrayPtr, j);
					ref UnitBase_Dots unitBase3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, j);
					ref EndlessMonsterTag endlessTag3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, j);
					Execute(chunkIndexInQuery, ref monster3, ref unitBase3, ref endlessTag3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Monster317_Dots monster4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Monster317_Dots>(nativeArrayPtr, k);
					ref UnitBase_Dots unitBase4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<UnitBase_Dots>(nativeArrayPtr2, k);
					ref EndlessMonsterTag endlessTag4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<EndlessMonsterTag>(nativeArrayPtr3, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr4, k);
					Execute(chunkIndexInQuery, ref monster4, ref unitBase4, ref endlessTag4, entity4);
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
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<AnimaPlay> __AnimaPlay_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public BufferLookup<Monster317_Aim> __Monster317_Aim_RW_BufferLookup;

		public Monster317Job.InternalCompilerQueryAndHandleData __Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__AnimaPlay_RW_ComponentLookup = state.GetComponentLookup<AnimaPlay>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Monster317_Aim_RW_BufferLookup = state.GetBufferLookup<Monster317_Aim>();
			__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00008CD0_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00008CD0_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00008CD0_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnCreate_0024BurstManaged(self, state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnDestroy_00008CD2_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00008CD2_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00008CD2_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
				{
					Invoke(self, state);
				}).Value;
			}
			P_0 = Pointer;
		}

		private static IntPtr GetFunctionPointer()
		{
			nint result = 0;
			GetFunctionPointerDiscard(ref result);
			return result;
		}

		public unsafe static void Invoke(IntPtr self, IntPtr state)
		{
			if (BurstCompiler.IsEnabled)
			{
				IntPtr functionPointer = GetFunctionPointer();
				if (functionPointer != (IntPtr)0)
				{
					((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)functionPointer)(self, state);
					return;
				}
			}
			__codegen__OnDestroy_0024BurstManaged(self, state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_232013272_0;

	private EntityQuery __query_232013272_1;

	private EntityQuery __query_232013272_2;

	private EntityQuery __query_232013272_3;

	private EntityQuery __query_232013272_4;

	private EntityQuery __query_232013272_5;

	private EntityQuery __query_232013272_6;

	private EntityQuery __query_232013272_7;

	private EntityQuery __query_232013272_8;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Monster317_Dots>();
		state.EntityManager.CreateSingletonBuffer<Monster317AttackRequest>();
		state.EntityManager.CreateSingletonBuffer<Monster317_BufferClear>();
	}

	public void OnUpdate(ref SystemState state)
	{
		SpellSpawnParams ssp = UnitDotsSyncSystem.GetSpellPrototype(90481);
		UnitBase.UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = 10f;
		sSPModifier.Damage = 10f;
		sSPModifier.Speed = 8f;
		sSPModifier.ApplyToSSP(ref ssp);
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
		state.Dependency = __ScheduleViaJobChunkExtension_0(new Monster317Job
		{
			ssp = ssp,
			CurrentRoomEntities = __query_232013272_0.GetSingleton<CurrentRoomEntitiesSingleton>(),
			LocalTsfLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state),
			AnimaLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__AnimaPlay_RW_ComponentLookup, ref state),
			deltaTime = state.WorldUnmanaged.Time.DeltaTime,
			ecb = entityCommandBuffer.AsParallelWriter(),
			PptLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref state),
			aimBufferLookUp = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__Monster317_Aim_RW_BufferLookup, ref state),
			ShootSpellBufferEntity = __query_232013272_1.GetSingletonEntity(),
			attackRequestEntity = __query_232013272_2.GetSingletonEntity(),
			SEBufferEntity = __query_232013272_3.GetSingletonEntity(),
			bufferClearEntity = __query_232013272_4.GetSingletonEntity(),
			particleBufferEntity = __query_232013272_5.GetSingletonEntity(),
			globalRandom = __query_232013272_6.GetSingletonRW<GlobalRandom>(),
			playerEntity = PlayerMgr.Inst.PlayerEtt,
			playerMotion = PlayerMgr.Inst.PlayerCtrller.CurrentMotion
		}, __TypeHandle.__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, state.Dependency, ref state, hasUserDefinedQuery: false);
		state.Dependency.Complete();
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
		NativeArray<Monster317_BufferClear> nativeArray = __query_232013272_7.GetSingletonBuffer<Monster317_BufferClear>().ToNativeArray(Allocator.Temp);
		if (nativeArray.Length > 0)
		{
			for (int i = 0; i < nativeArray.Length; i++)
			{
				DynamicBuffer<Monster317_Aim> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Monster317_Aim_RW_BufferLookup, ref state, nativeArray[i].clearEntity);
				for (int num = bufferAfterCompletingDependency.Length - 1; num >= 0; num--)
				{
					if (bufferAfterCompletingDependency[num].time < 0f)
					{
						bufferAfterCompletingDependency.RemoveAt(num);
					}
				}
			}
			__query_232013272_7.GetSingletonBuffer<Monster317_BufferClear>().Clear();
		}
		nativeArray.Dispose();
		DynamicBuffer<Monster317AttackRequest> singletonBuffer = __query_232013272_8.GetSingletonBuffer<Monster317AttackRequest>();
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		foreach (Monster317AttackRequest item in singletonBuffer)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster317_Gun" + (item.isPattern2 ? "Big" : ""), item.startPoint).GetComponent<Monster317_Gun>().InitializeGun(item.startPoint, item.requestPoint, item.flyTime, item.masterEntity, item.buffed);
		}
		singletonBuffer.Clear();
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Monster317Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Monster317System_Monster317Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSpawnParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster317AttackRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Monster317_BufferClear>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster317_BufferClear>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_7 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster317AttackRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_232013272_8 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnCreate(IntPtr self, IntPtr state)
	{
		__codegen__OnCreate_00008CD0_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster317System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00008CD2_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster317System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster317System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster317System*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
