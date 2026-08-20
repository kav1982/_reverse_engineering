using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[CompilerGenerated]
[UpdateAfter(typeof(UnitBaseSystem))]
[UpdateInGroup(typeof(UnitBaseSystemGroup))]
internal struct Monster16System : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_2053066694_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public BufferAccessor<Monster16_DotsRock> item6_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Monster16_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, DynamicBuffer<Monster16_DotsRock>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Monster16_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, DynamicBuffer<Monster16_DotsRock>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Monster16_Dots>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitBase_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PathFinding>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item5_IntPtr, index), item6_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Monster16_Dots> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitBase_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PathFinding> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitProperty_Dots> item5_ComponentTypeHandle_RW;

			private BufferTypeHandle<Monster16_DotsRock> item6_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Monster16_Dots>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitBase_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PathFinding>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item6_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<Monster16_DotsRock>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item6_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Monster16_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, DynamicBuffer<Monster16_DotsRock>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Monster16_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, DynamicBuffer<Monster16_DotsRock>> Current => _resolvedChunk.Get(_currentEntityIndex);

			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public Enumerator(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
			{
				if (!entityQuery.IsEmptyIgnoreFilter)
				{
					CompleteDependencies(ref state);
					typeHandle.Update(ref state);
				}
				_entityQueryEnumerator = new InternalEntityQueryEnumerator(entityQuery);
				_currentEntityIndex = -1;
				_endEntityIndex = -1;
				_typeHandle = typeHandle;
				_resolvedChunk = default(ResolvedChunk);
			}

			public void Dispose()
			{
				_entityQueryEnumerator.Dispose();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool MoveNext()
			{
				_currentEntityIndex++;
				if (_currentEntityIndex >= _endEntityIndex)
				{
					if (_entityQueryEnumerator.MoveNextEntityRange(out var movedToNewChunk, out var chunk, out var entityStartIndex, out var entityEndIndex))
					{
						if (movedToNewChunk)
						{
							_resolvedChunk = _typeHandle.Resolve(chunk);
						}
						_currentEntityIndex = entityStartIndex;
						_endEntityIndex = entityEndIndex;
						return true;
					}
					return false;
				}
				return true;
			}

			public Enumerator GetEnumerator()
			{
				return this;
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Enumerator Query(EntityQuery entityQuery, TypeHandle typeHandle, ref SystemState state)
		{
			return new Enumerator(entityQuery, typeHandle, ref state);
		}

		public static void CompleteDependencies(ref SystemState state)
		{
			state.EntityManager.CompleteDependencyBeforeRW<Monster16_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitBase_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PathFinding>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<Monster16_DotsRock>();
		}
	}

	private struct TypeHandle
	{
		public IFE_2053066694_0.TypeHandle __IFE_2053066694_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_2053066694_0_TypeHandle = new IFE_2053066694_0.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_000087EB_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_000087EB_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_000087EB_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnDestroy_000087ED_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_000087ED_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_000087ED_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private ComponentLookup<LocalTransform> localTsfLookUp;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_2053066694_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		localTsfLookUp = state.GetComponentLookup<LocalTransform>();
		state.RequireForUpdate<Monster16_Dots>();
	}

	public void OnUpdate(ref SystemState state)
	{
		localTsfLookUp.Update(ref state);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Monster16_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PathFinding>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, DynamicBuffer<Monster16_DotsRock>> item7 in IFE_2053066694_0.Query(__query_2053066694_0, __TypeHandle.__IFE_2053066694_0_TypeHandle, ref state))
		{
			item7.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var _);
			InternalCompilerInterface.UncheckedRefRW<Monster16_Dots> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<UnitBase_Dots> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<PathFinding> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW5 = item5;
			DynamicBuffer<Monster16_DotsRock> dynamicBuffer = item6;
			ref Monster16_Dots valueRW = ref uncheckedRefRW.ValueRW;
			ref UnitBase_Dots valueRW2 = ref uncheckedRefRW2.ValueRW;
			ref LocalTransform valueRW3 = ref uncheckedRefRW3.ValueRW;
			ref PathFinding valueRW4 = ref uncheckedRefRW4.ValueRW;
			ref UnitProperty_Dots valueRW5 = ref uncheckedRefRW5.ValueRW;
			if (valueRW.stateQuit)
			{
				valueRW.stateQuit = false;
				valueRW.changedState = true;
			}
			else
			{
				valueRW.changedState = false;
			}
			valueRW.stateExistTime += state.WorldUnmanaged.Time.DeltaTime;
			if (valueRW.AnimaNeedReset)
			{
				valueRW.AnimaReset(valueRW2.ett_AnimaRoot, state.EntityManager);
			}
			valueRW.rockAngle += valueRW.rockRotateSpeed * state.WorldUnmanaged.Time.DeltaTime;
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				LocalTransform value = localTsfLookUp[dynamicBuffer[i].entity];
				value.Position = Tool2D.GetDir(valueRW.rockAngle + 360f / (float)dynamicBuffer.Length * (float)i) * valueRW.rockDistance;
				value.Position = new float3(value.Position.x, value.Position.y, value.Position.y);
				localTsfLookUp[dynamicBuffer[i].entity] = value;
			}
			switch (valueRW.state)
			{
			case Monster16State.BornIdle:
				if (valueRW.changedState)
				{
					valueRW.AnimaPlay(valueRW2.ett_AnimaRoot, state.EntityManager, Monster16AnimaName.Idle);
				}
				if (valueRW.stateExistTime >= 0.5f)
				{
					valueRW.state = Monster16State.Idle;
				}
				break;
			case Monster16State.Idle:
				if (valueRW.changedState)
				{
					valueRW.AnimaPlay(valueRW2.ett_AnimaRoot, state.EntityManager, Monster16AnimaName.Idle);
					valueRW.idleTime.RandomResult();
				}
				valueRW2.SetMove(float3.zero, thisTimeShouldFlip: false);
				if (valueRW.stateExistTime > valueRW.idleTime.result)
				{
					valueRW.state = Monster16State.MoveRandom;
				}
				break;
			case Monster16State.MoveRandom:
				if (valueRW.changedState)
				{
					valueRW.AnimaPlay(valueRW2.ett_AnimaRoot, state.EntityManager, Monster16AnimaName.Move);
					valueRW4.UpdatePath(valueRW3.Position, Tool2D.GetNavMeshPoint(valueRW3.Position, valueRW.moveRandomRadius), 16);
				}
				if (Tool2D.IgnoreZDistanceSqr(valueRW4.endPosition, valueRW3.Position) < valueRW2.moveThreshold * valueRW2.moveThreshold)
				{
					valueRW.state = Monster16State.Idle;
				}
				else
				{
					valueRW2.SetMove(Tool2D.IgnoreZV2ToV1Normal(valueRW4.walkToPoint, valueRW3.Position) * valueRW5.unitCfg.moveSpeed);
				}
				break;
			}
		}
	}

	[BurstCompile]
	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Monster16_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitBase_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PathFinding>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Monster16_DotsRock>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_2053066694_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_000087EB_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Monster16System*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_000087ED_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Monster16System*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster16System*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((Monster16System*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
