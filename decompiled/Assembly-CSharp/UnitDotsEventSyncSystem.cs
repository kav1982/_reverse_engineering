using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(AfterPhysicsSystemGroup))]
[CompilerGenerated]
public class UnitDotsEventSyncSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_631585324_0
	{
		public struct ResolvedChunk
		{
			public ManagedComponentAccessor<UnitTriggerReference> item1_ManagedComponentAccessor;

			public BufferAccessor<StatefulTriggerEvent> item2_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<UnitTriggerReference, DynamicBuffer<StatefulTriggerEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<UnitTriggerReference, DynamicBuffer<StatefulTriggerEvent>>(item1_ManagedComponentAccessor[index], item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<UnitTriggerReference> item1_ManagedComponentTypeHandle_RO;

			private BufferTypeHandle<StatefulTriggerEvent> item2_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<UnitTriggerReference>(isReadOnly: false);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulTriggerEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item1_ManagedComponentTypeHandle_RO, _entityManager);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<UnitTriggerReference, DynamicBuffer<StatefulTriggerEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<UnitTriggerReference, DynamicBuffer<StatefulTriggerEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitTriggerReference>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulTriggerEvent>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_631585324_1
	{
		public struct ResolvedChunk
		{
			public ManagedComponentAccessor<UnitCollisionReference> item1_ManagedComponentAccessor;

			public BufferAccessor<StatefulCollisionEvent> item2_BufferAccessor;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<UnitCollisionReference, DynamicBuffer<StatefulCollisionEvent>> Get(int index)
			{
				return new QueryEnumerableWithEntity<UnitCollisionReference, DynamicBuffer<StatefulCollisionEvent>>(item1_ManagedComponentAccessor[index], item2_BufferAccessor[index], InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			public EntityManager _entityManager;

			[ReadOnly]
			private ComponentTypeHandle<UnitCollisionReference> item1_ManagedComponentTypeHandle_RO;

			private BufferTypeHandle<StatefulCollisionEvent> item2_BufferTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				_entityManager = systemState.EntityManager;
				item1_ManagedComponentTypeHandle_RO = systemState.EntityManager.GetComponentTypeHandle<UnitCollisionReference>(isReadOnly: false);
				item2_BufferTypeHandle_RW = systemState.GetBufferTypeHandle<StatefulCollisionEvent>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ManagedComponentTypeHandle_RO.Update(ref systemState);
				item2_BufferTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_ManagedComponentAccessor = archetypeChunk.GetManagedComponentAccessor(ref item1_ManagedComponentTypeHandle_RO, _entityManager);
				result.item2_BufferAccessor = archetypeChunk.GetBufferAccessor(ref item2_BufferTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<UnitCollisionReference, DynamicBuffer<StatefulCollisionEvent>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<UnitCollisionReference, DynamicBuffer<StatefulCollisionEvent>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitCollisionReference>();
			state.EntityManager.CompleteDependencyBeforeRW<StatefulCollisionEvent>();
		}
	}

	private struct TypeHandle
	{
		public IFE_631585324_0.TypeHandle __IFE_631585324_0_TypeHandle;

		public IFE_631585324_1.TypeHandle __IFE_631585324_1_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_631585324_0_TypeHandle = new IFE_631585324_0.TypeHandle(ref state);
			__IFE_631585324_1_TypeHandle = new IFE_631585324_1.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_631585324_0;

	private EntityQuery __query_631585324_1;

	private EntityQuery __query_631585324_2;

	[Preserve]
	protected override void OnUpdate()
	{
		UnitDotsSyncSystem.pws = __query_631585324_2.GetSingleton<PhysicsWorldSingleton>();
		Entity entity;
		foreach (QueryEnumerableWithEntity<UnitTriggerReference, DynamicBuffer<StatefulTriggerEvent>> item5 in IFE_631585324_0.Query(__query_631585324_0, __TypeHandle.__IFE_631585324_0_TypeHandle, ref base.CheckedStateRef))
		{
			item5.Deconstruct(out var item, out var item2, out entity);
			UnitTriggerReference unitTriggerReference = item;
			DynamicBuffer<StatefulTriggerEvent> dynamicBuffer = item2;
			Entity self = entity;
			if (dynamicBuffer.Length <= 0 || unitTriggerReference.reference as MonoBehaviour == null)
			{
				continue;
			}
			foreach (StatefulTriggerEvent item6 in dynamicBuffer)
			{
				Entity otherEntity = item6.GetOtherEntity(self);
				switch (item6.State)
				{
				case StatefulEventState.Enter:
					unitTriggerReference.reference.OnTriggerEnter_Dots(otherEntity);
					break;
				case StatefulEventState.Stay:
					unitTriggerReference.reference.OnTriggerStay_Dots(otherEntity);
					break;
				case StatefulEventState.Exit:
					unitTriggerReference.reference.OnTriggerExit_Dots(otherEntity);
					break;
				}
			}
		}
		foreach (QueryEnumerableWithEntity<UnitCollisionReference, DynamicBuffer<StatefulCollisionEvent>> item7 in IFE_631585324_1.Query(__query_631585324_1, __TypeHandle.__IFE_631585324_1_TypeHandle, ref base.CheckedStateRef))
		{
			item7.Deconstruct(out var item3, out var item4, out entity);
			UnitCollisionReference unitCollisionReference = item3;
			DynamicBuffer<StatefulCollisionEvent> dynamicBuffer2 = item4;
			if (dynamicBuffer2.Length <= 0 || unitCollisionReference.reference as MonoBehaviour == null)
			{
				continue;
			}
			foreach (StatefulCollisionEvent item8 in dynamicBuffer2)
			{
				switch (item8.State)
				{
				case StatefulEventState.Enter:
					unitCollisionReference.reference.OnCollisionEnter_Dots(item8);
					break;
				case StatefulEventState.Stay:
					unitCollisionReference.reference.OnCollisionStay_Dots(item8);
					break;
				case StatefulEventState.Exit:
					unitCollisionReference.reference.OnCollisionExit_Dots(item8);
					break;
				}
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitTriggerReference>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulTriggerEvent>();
		__query_631585324_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<UnitCollisionReference>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<StatefulCollisionEvent>();
		__query_631585324_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_631585324_2 = entityQueryBuilder2.Build(ref state);
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
	public UnitDotsEventSyncSystem()
	{
	}
}
