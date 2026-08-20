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
public struct Spell3007Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public BufferTypeHandle<Spell3007DamageCoolDownBuffer> __Spell3007DamageCoolDownBuffer_RW_BufferTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			public ComponentTypeHandle<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentTypeHandle;

			public ComponentTypeHandle<Spell3007LightningChainEffect> __Spell3007LightningChainEffect_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PostTransformMatrix> __Unity_Transforms_PostTransformMatrix_RW_ComponentTypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__Spell3007DamageCoolDownBuffer_RW_BufferTypeHandle = state.GetBufferTypeHandle<Spell3007DamageCoolDownBuffer>();
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LocalTransform>();
				__Spell3007LightningChainEffect_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Spell3007LightningChainEffect>();
				__Unity_Transforms_PostTransformMatrix_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PostTransformMatrix>();
			}

			public void Update(ref SystemState state)
			{
				__Spell3007DamageCoolDownBuffer_RW_BufferTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
				__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle.Update(ref state);
				__Spell3007LightningChainEffect_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Transforms_PostTransformMatrix_RW_ComponentTypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell3007DamageCoolDownBuffer>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell3007LightningChainEffect>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PostTransformMatrix>();
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
		public void Run(ref Spell3007Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref Spell3007Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref Spell3007Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref Spell3007Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref Spell3007Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref Spell3007Job job, EntityManager entityManager)
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

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<LocalTransform> LocalTransformLookUp;

	[NativeDisableParallelForRestriction]
	[NativeDisableContainerSafetyRestriction]
	public ComponentLookup<BreakLightningChain> BreakChainLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<UnitProperty_Dots> UnitPropertyLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellComponentData> SpellLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellConfigComponentData> SpellConfigLookup;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<SpellElementEffectComponentData> SpellElementLookup;

	public EntityCommandBuffer.ParallelWriter Cmd;

	public Entity globalParticleBuffer;

	public Entity SEPlayerSingleton;

	[ReadOnly]
	public PhysicsWorldSingleton Physics;

	public float DeltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	[BurstCompile]
	private void Execute(DynamicBuffer<Spell3007DamageCoolDownBuffer> damageCoolDownBuffer, Entity chainSpell, ref LocalTransform chainTrans, ref Spell3007LightningChainEffect singleton, [ChunkIndexInQuery] int chunkIndexInQuery, ref PostTransformMatrix freeScale)
	{
		LocalTransform componentData;
		bool entityExists;
		bool flag = LocalTransformLookUp.TryGetComponent(singleton.SourceEntity, out componentData, out entityExists);
		LocalTransform componentData2;
		bool entityExists2;
		bool flag2 = LocalTransformLookUp.TryGetComponent(singleton.TargetEntity, out componentData2, out entityExists2);
		if (entityExists && entityExists2 && flag && flag2 && !math.isnan(componentData.Position.x) && !math.isnan(componentData2.Position.x))
		{
			if (BreakChainLookup.HasComponent(singleton.SourceEntity) && BreakChainLookup.HasComponent(singleton.TargetEntity))
			{
				Cmd.DestroyEntity(chunkIndexInQuery, chainSpell);
				return;
			}
			float3 rayStart = componentData.Position;
			float3 position = componentData2.Position;
			float3 @float = ((math.distance(rayStart, position) <= 0.01f) ? new float3(1f, 0f, 0f) : math.normalize(position - rayStart));
			float num = math.distance(rayStart, position);
			float3 rootPosition = rayStart + (position - rayStart) / 2f;
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			rootPosition += layerPosition;
			chainTrans.Position = rootPosition;
			float3 xyz = new float3(0f, 0f, math.atan2(@float.y, @float.x));
			chainTrans.Rotation = quaternion.Euler(xyz);
			freeScale.Value = Matrix4x4.Scale(new Vector3(num, 1f, 1f));
			singleton.DamageTimer += DeltaTime;
			if (singleton.DamageTimer <= 0.06f)
			{
				return;
			}
			singleton.DamageTimer -= 0.06f;
			NativeList<Entity> list = new NativeList<Entity>(Allocator.Temp);
			NativeList<ColliderCastHit> hitList = new NativeList<ColliderCastHit>(Allocator.Temp);
			float3 rayEnd = rayStart + @float * num;
			float width = 0.4f;
			UnitType selfCamp = UnitType.Teammate;
			SpellTools.GetAttackableEntitiesInSphereCast(in rayStart, in rayEnd, in width, in selfCamp, containsBrittleness: false, in UnitPropertyLookup, in SpellConfigLookup, in Physics, ref hitList);
			bool flag3 = false;
			foreach (ColliderCastHit item in hitList)
			{
				Entity target = item.Entity;
				if (SpellConfigLookup.HasComponent(target))
				{
					TakeDamageInfo_Dots.NewInfo(singleton.Damage, out var info);
					Cmd.TryAttackEntity(chunkIndexInQuery, in target, in info, in UnitPropertyLookup, in SpellConfigLookup);
					continue;
				}
				list.Add(in target);
				if (!HaveEntityInBuffer(damageCoolDownBuffer, target))
				{
					Cmd.AppendToBuffer(chunkIndexInQuery, chainSpell, new Spell3007DamageCoolDownBuffer
					{
						EnemyEntity = target,
						CoolDownTimer = 0f
					});
				}
			}
			hitList.Dispose();
			for (int num2 = damageCoolDownBuffer.Length - 1; num2 >= 0; num2--)
			{
				Spell3007DamageCoolDownBuffer value = damageCoolDownBuffer[num2];
				value.CoolDownTimer -= DeltaTime;
				if (!LocalTransformLookUp.HasComponent(damageCoolDownBuffer[num2].EnemyEntity))
				{
					damageCoolDownBuffer.RemoveAt(num2);
				}
				else
				{
					if (value.CoolDownTimer <= 0f && list.Contains(damageCoolDownBuffer[num2].EnemyEntity))
					{
						value.CoolDownTimer = 0.1f;
						TakeDamageInfo_Dots.NewInfo(singleton.Damage, out var info2);
						info2.spell = new TakeDamageInfo_Dots.SpellData
						{
							Entity = chainSpell,
							Transform = chainTrans,
							Config = SpellConfigLookup[singleton.SourceEntity],
							ElementEffect = SpellElementLookup[singleton.SourceEntity],
							CostPenetrate = false,
							CostRefraction = false
						};
						info2.attackerEntity = SpellLookup[singleton.SourceEntity].OwnerEntity;
						flag3 = true;
						info2.spell.Config.AbilityType = SpellAbilityType.LightningChain;
						Cmd.AppendToBuffer(chunkIndexInQuery, damageCoolDownBuffer[num2].EnemyEntity, info2);
						SpellConfigLookup[singleton.SourceEntity].ColorType.ColorEnumToString(out var result);
						GlobalParticleEmitParams element = new GlobalParticleEmitParams(GlobalParticleType.Spell, $"3007_Hit_{result}", LocalTransformLookUp[damageCoolDownBuffer[num2].EnemyEntity].Position)
						{
							Size = 1f
						};
						Cmd.AppendToBuffer(chunkIndexInQuery, globalParticleBuffer, element);
						singleton.PenetrateCount--;
						if (singleton.PenetrateCount <= 0)
						{
							Cmd.DestroyEntity(chunkIndexInQuery, chainSpell);
							list.Dispose();
							return;
						}
					}
					damageCoolDownBuffer[num2] = value;
				}
			}
			list.Dispose();
			if (flag3)
			{
				ref EntityCommandBuffer.ParallelWriter cmd = ref Cmd;
				Entity sEPlayerSingleton = SEPlayerSingleton;
				FixedString32Bytes seName = "Hit";
				cmd.AppendToBuffer(chunkIndexInQuery, sEPlayerSingleton, new SEData(DTool.GetSpellSEName(3007, in seName)));
			}
		}
		else
		{
			Cmd.DestroyEntity(chunkIndexInQuery, chainSpell);
		}
	}

	[BurstCompile]
	private bool HaveEntityInBuffer(DynamicBuffer<Spell3007DamageCoolDownBuffer> buffer, Entity target)
	{
		for (int i = 0; i < buffer.Length; i++)
		{
			if (buffer[i].EnemyEntity == target)
			{
				return true;
			}
		}
		return false;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		BufferAccessor<Spell3007DamageCoolDownBuffer> bufferAccessor = chunk.GetBufferAccessor(ref __TypeHandle.__Spell3007DamageCoolDownBuffer_RW_BufferTypeHandle);
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Spell3007LightningChainEffect_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Transforms_PostTransformMatrix_RW_ComponentTypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				DynamicBuffer<Spell3007DamageCoolDownBuffer> damageCoolDownBuffer = bufferAccessor[i];
				Entity chainSpell = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, i);
				Execute(damageCoolDownBuffer, chainSpell, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, i), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3007LightningChainEffect>(nativeArrayPtr3, i), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PostTransformMatrix>(nativeArrayPtr4, i));
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
					DynamicBuffer<Spell3007DamageCoolDownBuffer> damageCoolDownBuffer2 = bufferAccessor[nextRangeBegin];
					Entity chainSpell2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, nextRangeBegin);
					Execute(damageCoolDownBuffer2, chainSpell2, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, nextRangeBegin), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3007LightningChainEffect>(nativeArrayPtr3, nextRangeBegin), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PostTransformMatrix>(nativeArrayPtr4, nextRangeBegin));
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
				DynamicBuffer<Spell3007DamageCoolDownBuffer> damageCoolDownBuffer3 = bufferAccessor[j];
				Entity chainSpell3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, j);
				Execute(damageCoolDownBuffer3, chainSpell3, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, j), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3007LightningChainEffect>(nativeArrayPtr3, j), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PostTransformMatrix>(nativeArrayPtr4, j));
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				DynamicBuffer<Spell3007DamageCoolDownBuffer> damageCoolDownBuffer4 = bufferAccessor[k];
				Entity chainSpell4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr, k);
				Execute(damageCoolDownBuffer4, chainSpell4, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<LocalTransform>(nativeArrayPtr2, k), ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Spell3007LightningChainEffect>(nativeArrayPtr3, k), chunkIndexInQuery, ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PostTransformMatrix>(nativeArrayPtr4, k));
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
