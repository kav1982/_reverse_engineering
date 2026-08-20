using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;

[UpdateAfter(typeof(UnitAfterTakeDamageSystem))]
[UpdateBefore(typeof(UnitTakeDamageDeadSystem))]
[CompilerGenerated]
[UpdateInGroup(typeof(UnitTakeDamageGroup))]
internal struct UnitBeforeAnnounceDeathSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_720133308_0
	{
		public struct ResolvedChunk
		{
			public ManagedComponentAccessor<UnitPptReference> item1_ManagedComponentAccessor;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (UnitPptReference, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitDead>) Get(int index)
			{
				return (item1_ManagedComponentAccessor[index], InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitDead>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<UnitPptReference> item1_ManagedComponentTypeHandle_RO;

			private ComponentTypeHandle<UnitProperty_Dots> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitDead> item3_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<UnitPptReference>(isReadOnly: false);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitDead>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item1_ManagedComponentTypeHandle_RO, _entityManager);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(UnitPptReference, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitDead>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (UnitPptReference, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRW<UnitDead>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitDead>();
		}
	}

	private struct TypeHandle
	{
		public IFE_720133308_0.TypeHandle __IFE_720133308_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_720133308_0_TypeHandle = new IFE_720133308_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_720133308_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<UnitPptReference>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (var (unitPptReference, uncheckedRefRW, uncheckedRefRW2) in IFE_720133308_0.Query(__query_720133308_0, __TypeHandle.__IFE_720133308_0_TypeHandle, ref state))
		{
			if (uncheckedRefRW.ValueRO.isDead)
			{
				ref TakeDamageInfo_Dots deadlyInfo = ref uncheckedRefRW2.ValueRW.deadlyInfo;
				if (!uncheckedRefRW2.ValueRO.ignoreBeforeAnnouncedDeath)
				{
					unitPptReference.unitPpt.UnitBas.BeforeAnnouncedDeath_Dots(ref deadlyInfo);
				}
				if (deadlyInfo.stopAnnouncedDeath)
				{
					uncheckedRefRW.ValueRW.isDead = false;
					deadlyInfo.isTargetDead = false;
				}
			}
		}
	}

	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitPptReference>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitDead>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_720133308_0 = entityQueryBuilder2.Build(ref state);
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
		((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((UnitBeforeAnnounceDeathSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
