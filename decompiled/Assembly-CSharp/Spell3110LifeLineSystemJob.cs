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
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[BurstCompile]
internal struct Spell3110LifeLineSystemJob : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<Spell3110LifeLineComponent> __Spell3110LifeLineComponent_RW_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell3110LifeLineComponent_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell3110LifeLineComponent>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__Spell3110LifeLineComponent_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			DefaultQuery = entityQueryBuilder.WithAllRW<Spell3110LifeLineComponent>().Build(ref state);
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
		public void Run(ref Spell3110LifeLineSystemJob job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell3110LifeLineSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell3110LifeLineSystemJob job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell3110LifeLineSystemJob job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell3110LifeLineSystemJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell3110LifeLineSystemJob job, EntityManager entityManager)
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

	[ReadOnly]
	public PhysicsWorldSingleton physicsWorldSingleton;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> localTransformLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<UnitProperty_Dots> unitPptLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> configLookup;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<PostTransformMatrix> postTransformMatrixLookup;

	[NativeDisableContainerSafetyRestriction]
	[NativeDisableParallelForRestriction]
	public ComponentLookup<Spell3110LivingTieComponent> livingTieLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<MatOverrideOffsetFloat> matOverrideLookUp;

	public EntityCommandBuffer.ParallelWriter CMD;

	public Entity DestorySpellBufferEntity;

	public Entity HitBloodBufferEntity;

	public Entity GlobalParticle;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public void Execute([ChunkIndexInQuery] int chunkIndexInQuery, ref Spell3110LifeLineComponent spell, Entity entity)
	{
		if (localTransformLookup.HasComponent(spell.linkTarget1) && localTransformLookup.HasComponent(spell.linkTarget2))
		{
			RefRO<LocalTransform> refRO;
			RefRW<LocalTransform> refRW;
			if (spell.distancePocess > 0.001f)
			{
				spell.distancePocess = math.lerp(spell.distancePocess, 0f, 0.12f);
			}
			else
			{
				spell.damageIntervalTimer += deltaTime;
				if (spell.damageIntervalTimer > 0.2f)
				{
					NativeList<ColliderCastHit> hitList = new NativeList<ColliderCastHit>(Allocator.Temp);
					refRO = localTransformLookup.GetRefRO(spell.linkTarget1);
					ref readonly float3 position = ref refRO.ValueRO.Position;
					refRO = localTransformLookup.GetRefRO(spell.linkTarget2);
					ref readonly float3 position2 = ref refRO.ValueRO.Position;
					float width = 0.3f;
					UnitType selfCamp = UnitType.Teammate;
					SpellTools.GetAttackableEntitiesInSphereCast(in position, in position2, in width, in selfCamp, containsBrittleness: true, in unitPptLookup, in configLookup, in physicsWorldSingleton, ref hitList);
					foreach (ColliderCastHit item in hitList)
					{
						Entity target = item.Entity;
						SpellTools.HitType hitType = CMD.TryAttackEntity(chunkIndexInQuery, in target, in spell.damageInfo, in unitPptLookup, in configLookup);
						if (localTransformLookup.HasComponent(target) && hitType != SpellTools.HitType.IgnoreSpell)
						{
							ref EntityCommandBuffer.ParallelWriter cMD = ref CMD;
							Entity globalParticle = GlobalParticle;
							GlobalParticleEmitParams element = new GlobalParticleEmitParams
							{
								Name = "3110_HitBlood"
							};
							refRW = localTransformLookup.GetRefRW(target);
							element.Position = Tool2D.IgnoreZPoint(refRW.ValueRW.Position + new float3(0f, 0.3f, 0f));
							cMD.AppendToBuffer(chunkIndexInQuery, globalParticle, element);
						}
					}
					spell.damageIntervalTimer = 0f;
				}
			}
			livingTieLookUp.GetRefRW(spell.tie2).ValueRW.starting = spell.distancePocess < 0.05f;
			refRO = localTransformLookup.GetRefRO(spell.linkTarget1);
			float3 position3 = refRO.ValueRO.Position;
			refRO = localTransformLookup.GetRefRO(spell.linkTarget2);
			float3 position4 = refRO.ValueRO.Position;
			float3 @float = math.normalize(position4 - position3);
			float3 float2 = math.normalize(position4 - position3 + new float3(0f, 0.15f, 0f));
			refRW = localTransformLookup.GetRefRW(spell.line);
			refRW.ValueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(@float.y, @float.x));
			refRW = localTransformLookup.GetRefRW(spell.shadow);
			refRW.ValueRW.Rotation = quaternion.Euler(0f, 0f, math.atan2(float2.y, float2.x));
			refRW = localTransformLookup.GetRefRW(spell.line);
			ref LocalTransform valueRW = ref refRW.ValueRW;
			float3 float3 = position3 + new float3(0f, 0.1f, 0f) + (position4 - position3) / 2f;
			refRO = localTransformLookup.GetRefRO(entity);
			valueRW.Position = Tool2D.GetLayerPoint((float3)Tool2D.IgnoreZPoint(float3 - refRO.ValueRO.Position));
			refRW = localTransformLookup.GetRefRW(spell.shadow);
			ref LocalTransform valueRW2 = ref refRW.ValueRW;
			float3 float4 = position3 + (position4 - new float3(0f, 0.15f, 0f) - position3) / 2f;
			refRO = localTransformLookup.GetRefRO(entity);
			valueRW2.Position = Tool2D.GetLayerPoint((float3)Tool2D.IgnoreZPoint(float4 - refRO.ValueRO.Position), LayerCorrectType.Shadow);
			postTransformMatrixLookup.GetRefRW(spell.line).ValueRW.Value = Matrix4x4.Scale(new Vector3(math.distance(position3, position4), 1f, 1f));
			postTransformMatrixLookup.GetRefRW(spell.shadow).ValueRW.Value = Matrix4x4.Scale(new Vector3(math.distance(position3, position4), 0.4f, 1f));
			matOverrideLookUp.GetRefRW(spell.line).ValueRW.offset = spell.distancePocess;
			if (spell.fire != Entity.Null)
			{
				matOverrideLookUp.GetRefRW(spell.fire).ValueRW.offset = spell.distancePocess;
			}
			matOverrideLookUp.GetRefRW(spell.shadow).ValueRW.offset = spell.distancePocess;
		}
		else if (localTransformLookup.HasComponent(spell.linkTarget1))
		{
			if (spell.distancePocess < 0.999f)
			{
				spell.distancePocess = math.lerp(spell.distancePocess, 1f, 0.2f);
			}
			else
			{
				CMD.AppendToBuffer(chunkIndexInQuery, DestorySpellBufferEntity, new Spell3110LifeLineDestoryBuffer
				{
					spell = spell,
					spellEntity = entity
				});
			}
			livingTieLookUp.GetRefRW(spell.tie1).ValueRW.starting = false;
			livingTieLookUp.GetRefRW(spell.tie2).ValueRW.starting = false;
			matOverrideLookUp.GetRefRW(spell.line).ValueRW.offset = spell.distancePocess;
			if (spell.fire != Entity.Null)
			{
				matOverrideLookUp.GetRefRW(spell.fire).ValueRW.offset = spell.distancePocess;
			}
			matOverrideLookUp.GetRefRW(spell.shadow).ValueRW.offset = spell.distancePocess;
		}
		else
		{
			CMD.AppendToBuffer(chunkIndexInQuery, DestorySpellBufferEntity, new Spell3110LifeLineDestoryBuffer
			{
				spell = spell,
				spellEntity = entity
			});
		}
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell3110LifeLineComponent_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref Spell3110LifeLineComponent spell = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3110LifeLineComponent>(nativeArrayPtr, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
				Execute(chunkIndexInQuery, ref spell, entity);
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
					ref Spell3110LifeLineComponent spell2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3110LifeLineComponent>(nativeArrayPtr, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
					Execute(chunkIndexInQuery, ref spell2, entity2);
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
				ref Spell3110LifeLineComponent spell3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3110LifeLineComponent>(nativeArrayPtr, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
				Execute(chunkIndexInQuery, ref spell3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref Spell3110LifeLineComponent spell4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3110LifeLineComponent>(nativeArrayPtr, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
				Execute(chunkIndexInQuery, ref spell4, entity4);
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
