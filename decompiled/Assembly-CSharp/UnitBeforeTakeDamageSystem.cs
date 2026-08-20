using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;

[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[BurstCompile]
[UpdateBefore(typeof(UnitPropertySystem))]
internal struct UnitBeforeTakeDamageSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_704884004_0
	{
		public struct ResolvedChunk
		{
			public ManagedComponentAccessor<UnitPptReference> item1_ManagedComponentAccessor;

			public BufferAccessor<TakeDamageInfo_Dots> item2_BufferAccessor;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (UnitPptReference, DynamicBuffer<TakeDamageInfo_Dots>) Get(int index)
			{
				return (item1_ManagedComponentAccessor[index], item2_BufferAccessor[index]);
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<UnitPptReference> item1_ManagedComponentTypeHandle_RO;

			private BufferTypeHandle<TakeDamageInfo_Dots> item2_BufferTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<UnitPptReference>(isReadOnly: false);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<TakeDamageInfo_Dots>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item1_ManagedComponentTypeHandle_RO, _entityManager);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(UnitPptReference, DynamicBuffer<TakeDamageInfo_Dots>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (UnitPptReference, DynamicBuffer<TakeDamageInfo_Dots>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitPptReference>();
			state.EntityManager.CompleteDependencyBeforeRW<TakeDamageInfo_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_704884004_0.TypeHandle __IFE_704884004_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_704884004_0_TypeHandle = new IFE_704884004_0.TypeHandle(ref state);
		}
	}

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	internal delegate void __codegen__OnCreate_00009301_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnCreate_00009301_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnCreate_00009301_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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
	internal delegate void __codegen__OnDestroy_00009303_0024PostfixBurstDelegate(IntPtr self, IntPtr state);

	internal static class __codegen__OnDestroy_00009303_0024BurstDirectCall
	{
		private static IntPtr Pointer;

		[BurstDiscard]
		private static void GetFunctionPointerDiscard(ref IntPtr P_0)
		{
			if (Pointer == (IntPtr)0)
			{
				Pointer = BurstCompiler.CompileFunctionPointer<__codegen__OnDestroy_00009303_0024PostfixBurstDelegate>(delegate(IntPtr self, IntPtr state)
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

	private EntityQuery __query_704884004_0;

	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
	}

	public void OnUpdate(ref SystemState state)
	{
		bool flag = false;
		if (Boss6_Stage2.Inst != null && !Boss6_Stage2.Inst.myPpt.AlreadyDead && UnitDotsSyncSystem.EntityIsValid(Boss6_Stage2.Inst.myPpt.myEntity))
		{
			flag = true;
			Boss6_Stage2.Inst.takeDamageInfoBuffer = state.EntityManager.GetBuffer<TakeDamageInfo_Dots>(Boss6_Stage2.Inst.myPpt.myEntity);
		}
		foreach (var (unitPptReference, dynamicBuffer) in IFE_704884004_0.Query(__query_704884004_0, __TypeHandle.__IFE_704884004_0_TypeHandle, ref state))
		{
			if (dynamicBuffer.Length <= 0)
			{
				continue;
			}
			for (int i = 0; i < dynamicBuffer.Length; i++)
			{
				if (unitPptReference.unitPpt.unitCfg.id != 500621)
				{
					unitPptReference.unitPpt.UnitBas.BeforeTakeDamage_Dots(ref dynamicBuffer.ElementAt(i));
				}
			}
		}
		if (flag)
		{
			DynamicBuffer<TakeDamageInfo_Dots> buffer = state.EntityManager.GetBuffer<TakeDamageInfo_Dots>(Boss6_Stage2.Inst.myPpt.myEntity);
			for (int j = 0; j < buffer.Length; j++)
			{
				Boss6_Stage2.Inst.BeforeTakeDamage_Dots(ref buffer.ElementAt(j));
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
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitPptReference>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TakeDamageInfo_Dots>();
		__query_704884004_0 = entityQueryBuilder2.Build(ref state);
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
		__codegen__OnCreate_00009301_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		__codegen__OnDestroy_00009303_0024BurstDirectCall.Invoke(self, state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[BurstCompile]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreate_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	[BurstCompile]
	internal unsafe static void __codegen__OnDestroy_0024BurstManaged(IntPtr self, IntPtr state)
	{
		((UnitBeforeTakeDamageSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}
}
