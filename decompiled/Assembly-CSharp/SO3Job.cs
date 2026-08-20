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

[BurstCompile]
[CompilerGenerated]
public struct SO3Job : IJobEntity, IJobChunk
{
	public struct InternalCompilerQueryAndHandleData
	{
		public struct TypeHandle
		{
			public ComponentTypeHandle<SpecialObj3_Dots> __SpecialObj3_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<IRoomObjExtraData_Dots> __IRoomObjExtraData_Dots_RW_ComponentTypeHandle;

			public ComponentTypeHandle<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle;

			[ReadOnly]
			public ComponentTypeHandle<ITrap_Dots> __ITrap_Dots_RO_ComponentTypeHandle;

			[ReadOnly]
			public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void __AssignHandles(ref SystemState state)
			{
				__SpecialObj3_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<SpecialObj3_Dots>();
				__IRoomObjExtraData_Dots_RW_ComponentTypeHandle = state.GetComponentTypeHandle<IRoomObjExtraData_Dots>();
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PhysicsCollider>();
				__ITrap_Dots_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ITrap_Dots>(isReadOnly: true);
				__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
			}

			public void Update(ref SystemState state)
			{
				__SpecialObj3_Dots_RW_ComponentTypeHandle.Update(ref state);
				__IRoomObjExtraData_Dots_RW_ComponentTypeHandle.Update(ref state);
				__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle.Update(ref state);
				__ITrap_Dots_RO_ComponentTypeHandle.Update(ref state);
				__Unity_Entities_Entity_TypeHandle.Update(ref state);
			}
		}

		public TypeHandle __TypeHandle;

		public EntityQuery DefaultQuery;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void __AssignQueries(ref SystemState state)
		{
			EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
			EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<ITrap_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj3_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<IRoomObjExtraData_Dots>();
			entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
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
		public void Run(ref SO3Job job, EntityQuery query)
		{
			job.__TypeHandle = __TypeHandle;
			JobChunkExtensions.RunByRef(ref job, query);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle Schedule(ref SO3Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle ScheduleParallel(ref SO3Job job, EntityQuery query, JobHandle dependency)
		{
			job.__TypeHandle = __TypeHandle;
			return JobChunkExtensions.ScheduleParallelByRef(ref job, query, dependency);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateBaseEntityIndexArray(ref SO3Job job, EntityQuery query, ref SystemState state)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public JobHandle UpdateBaseEntityIndexArray(ref SO3Job job, EntityQuery query, JobHandle dependency, ref SystemState state)
		{
			return dependency;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AssignEntityManager(ref SO3Job job, EntityManager entityManager)
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
	public NativeReference<bool> DaveDead;

	[NativeDisableParallelForRestriction]
	public ComponentLookup<MatOverrideFrameIndex> matLookUp;

	public Entity SEBufferEntity;

	public EntityCommandBuffer.ParallelWriter ecb;

	public float deltaTime;

	private InternalCompilerQueryAndHandleData.TypeHandle __TypeHandle;

	public unsafe void Execute([ChunkIndexInQuery] int index, ref SpecialObj3_Dots so3, ref IRoomObjExtraData_Dots extraData, ref PhysicsCollider collider, in ITrap_Dots iTrap, Entity entity)
	{
		if (so3.state == SO3State.Invalid)
		{
			return;
		}
		if (iTrap.onInvalid)
		{
			so3.state = SO3State.Invalid;
		}
		ref float stateExistTime = ref so3.stateExistTime;
		ref bool changedState = ref so3.changedState;
		ref bool stateQuit = ref so3.stateQuit;
		ref float animaDelayTime = ref so3.animaDelayTime;
		ref MatOverrideFrameIndex valueRW = ref matLookUp.GetRefRW(so3.matEntity).ValueRW;
		ref StateVariableMgr_Dots varMgr = ref so3.varMgr;
		ref float nowAnimaIndex = ref so3.nowAnimaIndex;
		SO3Pattern pattern = so3.pattern;
		float num = 0.05f;
		int num2 = ((!so3.mode1) ? 6 : 0);
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += deltaTime;
		switch (so3.state)
		{
		case SO3State.BornHide:
			if (changedState)
			{
				if (pattern == SO3Pattern.Dave)
				{
					so3.state = SO3State.Show;
					break;
				}
				valueRW.FrameIndex = (float)num2 + nowAnimaIndex;
				if (pattern == SO3Pattern.Always)
				{
					so3.state = SO3State.Show;
				}
				if (pattern == SO3Pattern.Interval)
				{
					animaDelayTime = extraData.data1;
				}
				BoxCollider* colliderPtr2 = (BoxCollider*)collider.ColliderPtr;
				BoxGeometry geometry2 = colliderPtr2->Geometry;
				if (pattern == SO3Pattern.Always)
				{
					geometry2.Center = new float3(0f, 0f, 0f);
				}
				else
				{
					geometry2.Center = new float3(0f, 0f, 10000f);
				}
				colliderPtr2->Geometry = geometry2;
			}
			if (stateExistTime > animaDelayTime && pattern == SO3Pattern.Interval)
			{
				so3.state = SO3State.Wait;
			}
			if (so3.triggered)
			{
				so3.state = SO3State.Wait;
			}
			break;
		case SO3State.Wait:
			if (stateExistTime > 0.15f)
			{
				so3.state = SO3State.Out;
			}
			break;
		case SO3State.Out:
		{
			ref float @float = ref varMgr.float1;
			if (changedState)
			{
				nowAnimaIndex = 0f;
			}
			@float += deltaTime;
			if (@float > num)
			{
				@float -= num;
				nowAnimaIndex += 1f;
				valueRW.FrameIndex = (float)num2 + nowAnimaIndex;
				if (nowAnimaIndex == 3f)
				{
					ecb.AppendToBuffer(index, SEBufferEntity, new SEData("SE_SO3"));
					BoxCollider* colliderPtr3 = (BoxCollider*)collider.ColliderPtr;
					BoxGeometry geometry3 = colliderPtr3->Geometry;
					geometry3.Center = new float3(0f, 0f, 0f);
					colliderPtr3->Geometry = geometry3;
				}
				if (nowAnimaIndex >= 5f)
				{
					so3.state = SO3State.Show;
				}
			}
			break;
		}
		case SO3State.Show:
			if (changedState && pattern != SO3Pattern.Dave)
			{
				nowAnimaIndex = 5f;
				valueRW.FrameIndex = (float)num2 + nowAnimaIndex;
			}
			if (pattern != SO3Pattern.Dave && stateExistTime > 1.75f && so3.pattern != 0)
			{
				so3.state = SO3State.In;
			}
			break;
		case SO3State.In:
		{
			ref float float2 = ref varMgr.float1;
			if (changedState)
			{
				nowAnimaIndex = 5f;
			}
			float2 += deltaTime;
			if (float2 > num)
			{
				float2 -= num;
				nowAnimaIndex -= 1f;
				valueRW.FrameIndex = (float)num2 + nowAnimaIndex;
				if (nowAnimaIndex == 3f)
				{
					BoxCollider* colliderPtr5 = (BoxCollider*)collider.ColliderPtr;
					BoxGeometry geometry5 = colliderPtr5->Geometry;
					geometry5.Center = new float3(0f, 0f, 10000f);
					colliderPtr5->Geometry = geometry5;
				}
				if (nowAnimaIndex <= 0f)
				{
					so3.state = SO3State.Hide;
				}
			}
			break;
		}
		case SO3State.Hide:
			if (changedState)
			{
				BoxCollider* colliderPtr4 = (BoxCollider*)collider.ColliderPtr;
				BoxGeometry geometry4 = colliderPtr4->Geometry;
				geometry4.Center = new float3(0f, 0f, 10000f);
				colliderPtr4->Geometry = geometry4;
			}
			if (pattern == SO3Pattern.Interval && stateExistTime > 1.75f)
			{
				so3.state = SO3State.Out;
			}
			if (so3.triggered)
			{
				so3.state = SO3State.Wait;
			}
			break;
		case SO3State.Invalid:
			if (changedState)
			{
				if (pattern != SO3Pattern.Dave)
				{
					nowAnimaIndex = 0f;
					valueRW.FrameIndex = (float)num2 + nowAnimaIndex;
				}
				else
				{
					valueRW.FrameIndex = 2f;
					DaveDead.Value = true;
				}
				BoxCollider* colliderPtr = (BoxCollider*)collider.ColliderPtr;
				BoxGeometry geometry = colliderPtr->Geometry;
				geometry.Center = new float3(0f, 0f, 10000f);
				colliderPtr->Geometry = geometry;
			}
			break;
		}
		so3.triggered = false;
	}

	[CompilerGenerated]
	public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in v128 chunkEnabledMask)
	{
		IntPtr nativeArrayPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__SpecialObj3_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr2 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__IRoomObjExtraData_Dots_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr3 = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtr(chunk, ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentTypeHandle);
		IntPtr nativeArrayPtr4 = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtr(chunk, ref __TypeHandle.__ITrap_Dots_RO_ComponentTypeHandle);
		IntPtr nativeArrayPtr5 = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(chunk, __TypeHandle.__Unity_Entities_Entity_TypeHandle);
		int count = chunk.Count;
		int num = 0;
		if (!useEnabledMask)
		{
			for (int i = 0; i < count; i++)
			{
				ref SpecialObj3_Dots so = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpecialObj3_Dots>(nativeArrayPtr, i);
				ref IRoomObjExtraData_Dots extraData = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<IRoomObjExtraData_Dots>(nativeArrayPtr2, i);
				ref PhysicsCollider collider = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, i);
				ref ITrap_Dots iTrap = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ITrap_Dots>(nativeArrayPtr4, i);
				Entity entity = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, i);
				Execute(chunkIndexInQuery, ref so, ref extraData, ref collider, in iTrap, entity);
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
					ref SpecialObj3_Dots so2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpecialObj3_Dots>(nativeArrayPtr, nextRangeBegin);
					ref IRoomObjExtraData_Dots extraData2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<IRoomObjExtraData_Dots>(nativeArrayPtr2, nextRangeBegin);
					ref PhysicsCollider collider2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, nextRangeBegin);
					ref ITrap_Dots iTrap2 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ITrap_Dots>(nativeArrayPtr4, nextRangeBegin);
					Entity entity2 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, nextRangeBegin);
					Execute(chunkIndexInQuery, ref so2, ref extraData2, ref collider2, in iTrap2, entity2);
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
				ref SpecialObj3_Dots so3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpecialObj3_Dots>(nativeArrayPtr, j);
				ref IRoomObjExtraData_Dots extraData3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<IRoomObjExtraData_Dots>(nativeArrayPtr2, j);
				ref PhysicsCollider collider3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, j);
				ref ITrap_Dots iTrap3 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ITrap_Dots>(nativeArrayPtr4, j);
				Entity entity3 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, j);
				Execute(chunkIndexInQuery, ref so3, ref extraData3, ref collider3, in iTrap3, entity3);
				num++;
			}
			num2 >>= 1;
		}
		num2 = chunkEnabledMask.ULong1;
		for (int k = 64; k < count; k++)
		{
			if ((num2 & 1) != 0L)
			{
				ref SpecialObj3_Dots so4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<SpecialObj3_Dots>(nativeArrayPtr, k);
				ref IRoomObjExtraData_Dots extraData4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<IRoomObjExtraData_Dots>(nativeArrayPtr2, k);
				ref PhysicsCollider collider4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<PhysicsCollider>(nativeArrayPtr3, k);
				ref ITrap_Dots iTrap4 = ref InternalCompilerInterface.UnsafeGetRefToNativeArrayPtrElement<ITrap_Dots>(nativeArrayPtr4, k);
				Entity entity4 = InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(nativeArrayPtr5, k);
				Execute(chunkIndexInQuery, ref so4, ref extraData4, ref collider4, in iTrap4, entity4);
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
