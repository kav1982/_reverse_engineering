using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
internal class CorpseSystem : SystemBase
{
	public struct CreateCorpseRequest
	{
		public CorpseType type;

		public Vector3 knockback;

		public Vector3 position;

		public float upSpeed;
	}

	[CompilerGenerated]
	public struct CorpseJob : IJobEntity, IJobChunk
	{
		public struct InternalCompilerQueryAndHandleData
		{
			public struct TypeHandle
			{
				public ComponentTypeHandle<Corpse_Dots> __Corpse_Dots_RW_ComponentTypeHandle;

				[ReadOnly]
				public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void __AssignHandles(ref SystemState state)
				{
					__Corpse_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Corpse_Dots>();
					__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
				}

				public void Update(ref SystemState state)
				{
					__Corpse_Dots_RW_ComponentTypeHandle.Update(ref state);
					__Unity_Entities_Entity_TypeHandle.Update(ref state);
				}
			}

			public TypeHandle __TypeHandle;

			public EntityQuery DefaultQuery;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private void __AssignQueries(ref SystemState state)
			{
				EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
				DefaultQuery = entityQueryBuilder.WithAllRW<Corpse_Dots>().Build(ref state);
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
			public void Run(ref CorpseJob job, EntityQuery query)
			{
				job.__TypeHandle = __TypeHandle;
				JobChunkExtensions.RunByRef(ref job, query);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle Schedule(ref CorpseJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle ScheduleParallel(ref CorpseJob job, EntityQuery query, JobHandle dependency)
			{
				job.__TypeHandle = __TypeHandle;
				return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void UpdateBaseEntityIndexArray(ref CorpseJob job, EntityQuery query, ref SystemState state)
			{
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public JobHandle UpdateBaseEntityIndexArray(ref CorpseJob job, EntityQuery query, JobHandle dependency, ref SystemState state)
			{
				return dependency;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void AssignEntityManager(ref CorpseJob job, EntityManager entityManager)
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

		[NativeDisableParallelForRestriction]
		public EntityCommandBuffer.ParallelWriter ecb;

		[NativeDisableParallelForRestriction]
		public ComponentLookup<LocalTransform> transformLookUp;

		[NativeDisableParallelForRestriction]
		[NativeDisableUnsafePtrRestriction]
		public ComponentLookup<MatOverrideColor> colorLookUp;

		private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

		private void Execute([ChunkIndexInQuery] int index, ref Corpse_Dots corpse, Entity entity)
		{
			RefRW<LocalTransform> refRW = transformLookUp.GetRefRW(entity);
			Vector3 vector = refRW.ValueRW.Position;
			refRW = transformLookUp.GetRefRW(corpse.corpseImageEntity);
			ref LocalTransform valueRW = ref refRW.ValueRW;
			switch (corpse.state)
			{
			case Corpse_Dots.CorpseState.Fly:
			{
				if (!transformLookUp.HasComponent(corpse.corpsePhysicsEntity))
				{
					break;
				}
				corpse.currentUpSpeed += corpse.gravity * deltaTime;
				corpse.nowAngle += deltaTime * corpse.rotateSpeed;
				refRW = transformLookUp.GetRefRW(corpse.corpsePhysicsEntity);
				ref LocalTransform valueRW3 = ref refRW.ValueRW;
				valueRW3.Position += new float3(0f, 0f, 0f - corpse.currentUpSpeed) * deltaTime;
				refRW = transformLookUp.GetRefRW(corpse.shadowImageEntity);
				ref LocalTransform valueRW4 = ref refRW.ValueRW;
				valueRW.Position = Tool2D.GetLayerPoint(valueRW3.Position) - vector;
				valueRW.Rotation = quaternion.RotateZ(corpse.nowAngle * (MathF.PI / 180f));
				valueRW4.Position = Tool2D.GetLayerPoint(valueRW3.Position, LayerCorrectType.Shadow) - vector;
				valueRW4.Rotation = valueRW.Rotation;
				if (valueRW3.Position.z >= 0f && corpse.currentUpSpeed < 0f)
				{
					if (corpse.bounceTimer >= corpse.bounceTime)
					{
						corpse.state = Corpse_Dots.CorpseState.Dark;
						ecb.DestroyEntity(index, corpse.corpsePhysicsEntity);
						ecb.DestroyEntity(index, corpse.shadowImageEntity);
						corpse.corpsePhysicsEntity = Entity.Null;
						valueRW.Position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(valueRW3.Position), LayerCorrectType.Corpse) - vector;
					}
					else
					{
						corpse.bounceTimer++;
						corpse.currentUpSpeed = (0f - corpse.currentUpSpeed) * corpse.bounceRatio;
					}
				}
				break;
			}
			case Corpse_Dots.CorpseState.Dark:
			{
				corpse.currentAlpha -= deltaTime * corpse.reduceAlphaSpeed;
				ref MatOverrideColor valueRW2 = ref colorLookUp.GetRefRW(corpse.corpseImageEntity).ValueRW;
				Color color = valueRW2.color;
				color.a = corpse.currentAlpha;
				valueRW2.color = color;
				if (corpse.currentAlpha < corpse.minAlpha)
				{
					corpse.state = Corpse_Dots.CorpseState.Stay;
				}
				break;
			}
			case Corpse_Dots.CorpseState.Stay:
				if (corpse.duration > 0f)
				{
					corpse.durationTimer += deltaTime;
					if (corpse.durationTimer > corpse.duration)
					{
						ecb.DestroyEntity(index, entity);
					}
				}
				break;
			}
		}

		[CompilerGenerated]
		public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
		{
			IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Corpse_Dots_RW_ComponentTypeHandle);
			IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
			int count = chunk.Count;
			int num = 0;
			if (!useEnabledMask)
			{
				for (int i = 0; i < count; i++)
				{
					ref Corpse_Dots corpse = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Corpse_Dots>(nativeArrayPtr, i);
					Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, i);
					Execute(chunkIndexInQuery, ref corpse, entity);
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
						ref Corpse_Dots corpse2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Corpse_Dots>(nativeArrayPtr, nextRangeBegin);
						Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, nextRangeBegin);
						Execute(chunkIndexInQuery, ref corpse2, entity2);
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
					ref Corpse_Dots corpse3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Corpse_Dots>(nativeArrayPtr, j);
					Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, j);
					Execute(chunkIndexInQuery, ref corpse3, entity3);
					num++;
				}
				num2 >>= 1;
			}
			num2 = chunkEnabledMask.ULong1;
			for (int k = 64; k < count; k++)
			{
				if ((num2 & 1) != 0L)
				{
					ref Corpse_Dots corpse4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<Corpse_Dots>(nativeArrayPtr, k);
					Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr2, k);
					Execute(chunkIndexInQuery, ref corpse4, entity4);
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

		public ComponentLookup<Corpse_Dots> __Corpse_Dots_RW_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		public BufferLookup<CorpseImageEtt> __CorpseImageEtt_RW_BufferLookup;

		public ComponentLookup<MatOverrideColor> __MatOverrideColor_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Corpse_Dots> __Corpse_Dots_RO_ComponentLookup;

		public ComponentLookup<Parent> __Unity_Transforms_Parent_RW_ComponentLookup;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		public CorpseJob.InternalCompilerQueryAndHandleData __CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Corpse_Dots_RW_ComponentLookup = state.GetComponentLookup<Corpse_Dots>();
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__CorpseImageEtt_RW_BufferLookup = state.GetBufferLookup<CorpseImageEtt>();
			__MatOverrideColor_RW_ComponentLookup = state.GetComponentLookup<MatOverrideColor>();
			__Corpse_Dots_RO_ComponentLookup = state.GetComponentLookup<Corpse_Dots>(isReadOnly: true);
			__Unity_Transforms_Parent_RW_ComponentLookup = state.GetComponentLookup<Parent>();
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
			__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private EntityQuery corpseQuery;

	public List<CorpseInfo_Dots> corpseInfo = new List<CorpseInfo_Dots>();

	private List<Entity> subImageToDestroy = new List<Entity>();

	public static CorpseSystem Inst;

	public List<CreateCorpseRequest> requests = new List<CreateCorpseRequest>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_644841050_0;

	private EntityQuery __query_644841050_1;

	private EntityQuery __query_644841050_2;

	public void CreateCorpse(CorpseType type, Vector3 position, Vector3 knockback, float upspeed = -1f)
	{
		if (!GameMgr.IsHarmony_Static)
		{
			requests.Add(new CreateCorpseRequest
			{
				type = type,
				position = position,
				knockback = knockback,
				upSpeed = upspeed
			});
		}
	}

	[Preserve]
	protected override void OnCreate()
	{
		corpseQuery = base.EntityManager.CreateEntityQuery(typeof(Corpse_Dots));
		RequireForUpdate<AllMixedEtt>();
		Inst = this;
		EventMgr.DotsSystemReset = (Action)Delegate.Combine(EventMgr.DotsSystemReset, (Action)delegate
		{
			corpseInfo.Clear();
			subImageToDestroy.Clear();
		});
	}

	[Preserve]
	protected override void OnDestroy()
	{
		corpseQuery.Dispose();
		EventMgr.DotsSystemReset = (Action)Delegate.Remove(EventMgr.DotsSystemReset, (Action)delegate
		{
			corpseInfo.Clear();
			subImageToDestroy.Clear();
		});
	}

	private void Reset()
	{
		corpseInfo.Clear();
		subImageToDestroy.Clear();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (corpseInfo.Count == 0 || !base.EntityManager.Exists(corpseInfo[1].ett))
		{
			if (!__query_644841050_0.TryGetSingletonBuffer(out DynamicBuffer<CorpseInfo_Dots> value, isReadOnly: false))
			{
				return;
			}
			corpseInfo.Clear();
			for (int i = 0; i < value.Length; i++)
			{
				corpseInfo.Add(value[i]);
			}
		}
		if (requests.Count > 0)
		{
			AllMixedEtt singleton = __query_644841050_1.GetSingleton<AllMixedEtt>();
			subImageToDestroy.Clear();
			while (requests.Count > 200)
			{
				requests.RemoveAt(UnityEngine.Random.Range(0, requests.Count));
			}
			NativeList<Entity> nativeList = new NativeList<Entity>(Allocator.Temp);
			RefRW<MatOverrideColor> componentRWAfterCompletingDependency2;
			for (int j = 0; j < requests.Count; j++)
			{
				CreateCorpseRequest createCorpseRequest = requests[j];
				int type = (int)createCorpseRequest.type;
				if (type >= corpseInfo.Count)
				{
					UnityEngine.Debug.LogError("这种类型的尸体还没有创建信息");
					continue;
				}
				CorpseInfo_Dots corpseInfo_Dots = corpseInfo[type];
				Entity ett = corpseInfo_Dots.ett;
				if (ett == Entity.Null)
				{
					UnityEngine.Debug.LogError("这种类型的尸体没有对应的预制体");
					continue;
				}
				Entity value2 = base.EntityManager.Instantiate(ett);
				nativeList.Add(in value2);
				RefRW<LocalTransform> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, value2);
				componentRWAfterCompletingDependency.ValueRW.Position = createCorpseRequest.position;
				Entity entity = base.EntityManager.Instantiate(singleton.map["Corpse"]);
				componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, entity);
				componentRWAfterCompletingDependency.ValueRW.Position = createCorpseRequest.position;
				ref Corpse_Dots valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Corpse_Dots_RW_ComponentLookup, ref base.CheckedStateRef, value2).ValueRW;
				valueRW.bounceTime = corpseInfo_Dots.bounceTime.RandomResult();
				valueRW.bounceRatio = corpseInfo_Dots.bounceRemainRatio;
				valueRW.currentUpSpeed = corpseInfo_Dots.upForce.RandomResult();
				if (createCorpseRequest.upSpeed > 0f)
				{
					valueRW.currentUpSpeed = createCorpseRequest.upSpeed;
				}
				valueRW.gravity = corpseInfo_Dots.gravity;
				valueRW.rotateSpeed = corpseInfo_Dots.rotateSpeed.RandomResult() * GeneralTool.HalfChanceNPOne();
				valueRW.nowAngle = UnityEngine.Random.Range(0f, 360f);
				valueRW.reduceAlphaSpeed = corpseInfo_Dots.reduceAlphaSpeed;
				valueRW.minAlpha = corpseInfo_Dots.minAlpha;
				valueRW.currentAlpha = 1f;
				valueRW.corpsePhysicsEntity = entity;
				valueRW.duration = corpseInfo_Dots.duration;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW.MakeUnique(in entity, base.EntityManager);
				ref PhysicsVelocity valueRW2 = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref base.CheckedStateRef, entity).ValueRW;
				if (createCorpseRequest.knockback == Vector3.zero)
				{
					valueRW2.Linear = Tool2D.GetDir() * corpseInfo_Dots.forwardForceNoDirect.RandomResult();
				}
				else
				{
					valueRW2.Linear = Tool2D.GetDir(createCorpseRequest.knockback.normalized, UnityEngine.Random.Range((0f - corpseInfo_Dots.angleOffset) / 2f, corpseInfo_Dots.angleOffset / 2f)) * corpseInfo_Dots.forwardForceHaveDirect.RandomResult();
				}
				int num = UnityEngine.Random.Range(0, valueRW.imageCount);
				if (GameMgr.IsHarmony_Static && valueRW.harmonyImageCount > 0)
				{
					num = UnityEngine.Random.Range(valueRW.imageCount, valueRW.harmonyImageCount);
				}
				DynamicBuffer<CorpseImageEtt> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__CorpseImageEtt_RW_BufferLookup, ref base.CheckedStateRef, value2);
				for (int num2 = bufferAfterCompletingDependency.Length - 1; num2 >= 0; num2--)
				{
					if (num2 == num)
					{
						valueRW.corpseImageEntity = bufferAfterCompletingDependency[num2].entity;
						componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, valueRW.corpseImageEntity);
						componentRWAfterCompletingDependency.ValueRW.Scale *= corpseInfo_Dots.scale.RandomResult();
						componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref base.CheckedStateRef, bufferAfterCompletingDependency[num2].entity);
						ref MatOverrideColor valueRW3 = ref componentRWAfterCompletingDependency2.ValueRW;
						switch (UnityEngine.Random.Range(0, corpseInfo_Dots.colorCount))
						{
						case 0:
							valueRW3.color = corpseInfo_Dots.color0;
							break;
						case 1:
							valueRW3.color = corpseInfo_Dots.color1;
							break;
						case 2:
							valueRW3.color = corpseInfo_Dots.color2;
							break;
						}
					}
					else
					{
						subImageToDestroy.Add(bufferAfterCompletingDependency[num2].entity);
					}
				}
			}
			foreach (Entity item in nativeList)
			{
				Corpse_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Corpse_Dots_RO_ComponentLookup, ref base.CheckedStateRef, item);
				Entity entity2 = base.EntityManager.Instantiate(componentAfterCompletingDependency.corpseImageEntity);
				componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref base.CheckedStateRef, entity2);
				componentRWAfterCompletingDependency2.ValueRW.color = new Color(0f, 0f, 0f, 0.4f);
				componentAfterCompletingDependency.shadowImageEntity = entity2;
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Corpse_Dots_RW_ComponentLookup, ref base.CheckedStateRef, componentAfterCompletingDependency, item);
				base.EntityManager.AddComponent<Parent>(entity2);
				InternalCompilerInterface.SetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_Parent_RW_ComponentLookup, ref base.CheckedStateRef, new Parent
				{
					Value = item
				}, entity2);
				InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref base.CheckedStateRef, item).Add(new LinkedEntityGroup
				{
					Value = entity2
				});
			}
			foreach (Entity item2 in subImageToDestroy)
			{
				base.EntityManager.DestroyEntity(item2);
			}
			subImageToDestroy.Clear();
			requests.Clear();
		}
		if (!corpseQuery.IsEmpty)
		{
			EntityCommandBuffer entityCommandBuffer = __query_644841050_2.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged);
			base.Dependency = __ScheduleViaJobChunkExtension_0(new CorpseJob
			{
				deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime,
				ecb = entityCommandBuffer.AsParallelWriter(),
				transformLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef),
				colorLookUp = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__MatOverrideColor_RW_ComponentLookup, ref base.CheckedStateRef)
			}, __TypeHandle.__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(CorpseJob job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__CorpseSystem_CorpseJob_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CorpseInfo_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_644841050_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<AllMixedEtt>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_644841050_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_644841050_2 = entityQueryBuilder2.Build(ref state);
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
	public CorpseSystem()
	{
	}
}
