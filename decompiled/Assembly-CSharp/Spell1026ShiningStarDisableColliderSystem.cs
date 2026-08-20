using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Physics;
using UnityEngine;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateAfter(typeof(SpellShootSystem))]
[CompilerGenerated]
public struct Spell1026ShiningStarDisableColliderSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1180813501_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1026ShiningStarData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellChargeData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item4_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1026ShiningStarData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellChargeData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item4_ComponentTypeHandle_RW;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1026ShiningStarData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellChargeData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1026ShiningStarData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellChargeData>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1180813501_0.TypeHandle __IFE_1180813501_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1180813501_0_TypeHandle = new IFE_1180813501_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1180813501_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell1026ShiningStarData>();
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		foreach (var (uncheckedRefRW, uncheckedRefRW2, uncheckedRefRW3, uncheckedRefRW4) in IFE_1180813501_0.Query(__query_1180813501_0, __TypeHandle.__IFE_1180813501_0_TypeHandle, ref state))
		{
			if (!uncheckedRefRW.ValueRO.RecordColliderType)
			{
				uncheckedRefRW.ValueRW.baseCritical = uncheckedRefRW4.ValueRW.CriticalChance;
				int num = Mathf.FloorToInt(uncheckedRefRW2.ValueRW.ChargeTimer);
				uncheckedRefRW4.ValueRW.CriticalChance = uncheckedRefRW.ValueRW.baseCritical + (float)num * 0.25f;
				if (uncheckedRefRW4.ValueRW.CriticalChance <= 0.33f)
				{
					uncheckedRefRW.ValueRW.CurStage = 1;
				}
				else if (uncheckedRefRW4.ValueRW.CriticalChance <= 0.66f)
				{
					uncheckedRefRW.ValueRW.CurStage = 2;
				}
				else if (uncheckedRefRW4.ValueRW.CriticalChance < 1f)
				{
					uncheckedRefRW.ValueRW.CurStage = 3;
				}
				else
				{
					uncheckedRefRW.ValueRW.CurStage = 4;
				}
				uncheckedRefRW.ValueRW.RecordColliderType = true;
				CompoundCollider* colliderPtr = (CompoundCollider*)uncheckedRefRW3.ValueRW.ColliderPtr;
				uncheckedRefRW.ValueRW.Collider1Type = colliderPtr->Children[0].Collider->GetCollisionResponse();
				uncheckedRefRW.ValueRW.Collider2Type = colliderPtr->Children[1].Collider->GetCollisionResponse();
				colliderPtr->Children[0].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
				colliderPtr->Children[1].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell1026ShiningStarData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellChargeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		__query_1180813501_0 = entityQueryBuilder2.Build(ref state);
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
		((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1026ShiningStarDisableColliderSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
