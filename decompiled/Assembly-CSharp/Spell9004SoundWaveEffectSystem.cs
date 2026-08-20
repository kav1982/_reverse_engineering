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
[UpdateInGroup(typeof(SpellEffectSystemGroup))]
[UpdateAfter(typeof(SpellEffectSystem))]
[CompilerGenerated]
public struct Spell9004SoundWaveEffectSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1795156274_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell9004SoundWaveData>, SpellComponentData, LocalTransform) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell9004SoundWaveData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item3_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell9004SoundWaveData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell9004SoundWaveData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell9004SoundWaveData>, SpellComponentData, LocalTransform)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell9004SoundWaveData>, SpellComponentData, LocalTransform) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell9004SoundWaveData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1795156274_0.TypeHandle __IFE_1795156274_0_TypeHandle;

		public BufferLookup<LinkedEntityGroup> __Unity_Entities_LinkedEntityGroup_RW_BufferLookup;

		public ComponentLookup<Spell9004MatRingWidthData> __Spell9004MatRingWidthData_RW_ComponentLookup;

		public ComponentLookup<Spell9004MatOutlineWidth1Data> __Spell9004MatOutlineWidth1Data_RW_ComponentLookup;

		public ComponentLookup<Spell9004MatOutlineWidth2Data> __Spell9004MatOutlineWidth2Data_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1795156274_0_TypeHandle = new IFE_1795156274_0.TypeHandle(ref state);
			__Unity_Entities_LinkedEntityGroup_RW_BufferLookup = state.GetBufferLookup<LinkedEntityGroup>();
			__Spell9004MatRingWidthData_RW_ComponentLookup = state.GetComponentLookup<Spell9004MatRingWidthData>();
			__Spell9004MatOutlineWidth1Data_RW_ComponentLookup = state.GetComponentLookup<Spell9004MatOutlineWidth1Data>();
			__Spell9004MatOutlineWidth2Data_RW_ComponentLookup = state.GetComponentLookup<Spell9004MatOutlineWidth2Data>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1795156274_0;

	public void OnUpdate(ref SystemState state)
	{
		foreach (var (uncheckedRefRW, spellComponentData, localTransform) in IFE_1795156274_0.Query(__query_1795156274_0, __TypeHandle.__IFE_1795156274_0_TypeHandle, ref state))
		{
			if (spellComponentData.SpellEffectEntity != Entity.Null)
			{
				float scale = localTransform.Scale;
				scale = math.max(scale, 0.01f);
				Entity value = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__Unity_Entities_LinkedEntityGroup_RW_BufferLookup, ref state, spellComponentData.SpellEffectEntity)[1].Value;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell9004MatRingWidthData_RW_ComponentLookup, ref state, value).ValueRW.Value = 0.06f / scale * uncheckedRefRW.ValueRO.width;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell9004MatOutlineWidth1Data_RW_ComponentLookup, ref state, value).ValueRW.Value = 0.12f / scale * uncheckedRefRW.ValueRO.width;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell9004MatOutlineWidth2Data_RW_ComponentLookup, ref state, value).ValueRW.Value = 0.24f / scale * uncheckedRefRW.ValueRO.width;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell9004SoundWaveData>();
		__query_1795156274_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder.Dispose();
	}

	public void OnCreateForCompiler(ref SystemState state)
	{
		__AssignQueries(ref state);
		__TypeHandle.__AssignHandles(ref state);
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell9004SoundWaveEffectSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell9004SoundWaveEffectSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
