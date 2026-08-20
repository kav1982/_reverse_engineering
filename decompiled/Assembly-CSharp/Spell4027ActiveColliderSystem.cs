using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;

[CompilerGenerated]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
public struct Spell4027ActiveColliderSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1539333223_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4027BlueRuneData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4027BlueRuneData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4027BlueRuneData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4027BlueRuneData>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1539333223_0.TypeHandle __IFE_1539333223_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1539333223_0_TypeHandle = new IFE_1539333223_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1539333223_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell4027BlueRuneData>();
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>> item4 in IFE_1539333223_0.Query(__query_1539333223_0, __TypeHandle.__IFE_1539333223_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var _, out var _);
			InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW2 = item2;
			Spell4027BlueRuneData valueRO = uncheckedRefRW.ValueRO;
			if (valueRO.RecordColliderType && valueRO.IsInitialized && uncheckedRefRW.ValueRW.DisableColliderTimer >= uncheckedRefRW.ValueRO.NormalIgnoreChaseDuration)
			{
				uncheckedRefRW.ValueRW.RecordColliderType = false;
				CompoundCollider* colliderPtr = (CompoundCollider*)uncheckedRefRW2.ValueRW.ColliderPtr;
				colliderPtr->Children[0].Collider->SetCollisionResponse(uncheckedRefRW.ValueRO.Collider1Type);
				colliderPtr->Children[1].Collider->SetCollisionResponse(uncheckedRefRW.ValueRO.Collider2Type);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4027BlueRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		__query_1539333223_0 = entityQueryBuilder2.Build(ref state);
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
		((Spell4027ActiveColliderSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4027ActiveColliderSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4027ActiveColliderSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
