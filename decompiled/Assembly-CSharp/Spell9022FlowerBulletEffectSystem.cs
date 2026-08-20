using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
public struct Spell9022FlowerBulletEffectSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1539175539_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9022FlowerBulletData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9022FlowerBulletData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell9022FlowerBulletData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell9022FlowerBulletData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item4_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell9022FlowerBulletData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9022FlowerBulletData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9022FlowerBulletData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell9022FlowerBulletData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1539175539_0.TypeHandle __IFE_1539175539_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1539175539_0_TypeHandle = new IFE_1539175539_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1539175539_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell9022FlowerBulletData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9022FlowerBulletData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>, InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>> item5 in IFE_1539175539_0.Query(__query_1539175539_0, __TypeHandle.__IFE_1539175539_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var _, out var item2, out var item3, out var _, out var _);
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			if (uncheckedRefRW.ValueRW.SpellEffectGameObject.IsValid())
			{
				uncheckedRefRW.ValueRW.SpellEffectGameObject.Value.transform.localScale = uncheckedRefRO.ValueRO.Scale * Vector3.one;
			}
		}
	}

	private bool TryGetLinkEffect(FixedString32Bytes name, DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out GameObject linkedObject)
	{
		for (int i = 0; i < linkBuffer.Length; i++)
		{
			SpellGameObjectEffectLink spellGameObjectEffectLink = linkBuffer[i];
			if (spellGameObjectEffectLink.EffectName == name)
			{
				linkedObject = linkBuffer[i].GameObject.Value;
				linkBuffer.RemoveAt(i);
				return true;
			}
		}
		linkedObject = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell9022FlowerBulletData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1539175539_0 = entityQueryBuilder2.Build(ref state);
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
		((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell9022FlowerBulletEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
