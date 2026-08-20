using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[UpdateAfter(typeof(SpellEffectSystem))]
[CompilerGenerated]
public struct Spell9006TrickLongTrailEffectSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_30284174_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData>, SpellComponentData, Shadow_Dots, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData>, SpellComponentData, Shadow_Dots, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell9006TrickLongTrailData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Shadow_Dots>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell9006TrickLongTrailData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<Shadow_Dots> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell9006TrickLongTrailData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Shadow_Dots>(isReadOnly: true);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData>, SpellComponentData, Shadow_Dots, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData>, SpellComponentData, Shadow_Dots, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell9006TrickLongTrailData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<Shadow_Dots>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_30284174_0.TypeHandle __IFE_30284174_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_30284174_0_TypeHandle = new IFE_30284174_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_30284174_0;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<Spell9006TrickLongTrailData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData>, SpellComponentData, Shadow_Dots, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item5 in IFE_30284174_0.Query(__query_30284174_0, __TypeHandle.__IFE_30284174_0_TypeHandle, ref state))
		{
			item5.Deconstruct(out var item, out var item2, out var item3, out var item4, out var _);
			InternalCompilerInterface.UncheckedRefRW<Spell9006TrickLongTrailData> uncheckedRefRW = item;
			SpellComponentData spellComponentData = item2;
			Shadow_Dots shadow_Dots = item3;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW2 = item4;
			if (spellComponentData.SpellEffectEntity != Entity.Null)
			{
				uncheckedRefRW.ValueRW.RotateAngle += state.WorldUnmanaged.Time.DeltaTime * 720f;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, uncheckedRefRW.ValueRW.RotateAngle);
				uncheckedRefRW2.ValueRW.Rotation = Quaternion.identity;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, spellComponentData.SpellEffectEntity).ValueRW.Rotation = quaternion;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state, shadow_Dots.ett_Shadow).ValueRW.Rotation = quaternion;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Shadow_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell9006TrickLongTrailData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		__query_30284174_0 = entityQueryBuilder2.Build(ref state);
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
		((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell9006TrickLongTrailEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
