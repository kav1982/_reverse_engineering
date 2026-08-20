using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
[UpdateBefore(typeof(Spell1026ShiningStarEndChargingSystem))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
public class Spell1026ShiningStarChargingSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1180813553_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<Spell1026ShiningStarData, SpellComponentData, SpellConfigComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Spell1026ShiningStarInitializeTag> Get(int index)
			{
				return new QueryEnumerableWithEntity<Spell1026ShiningStarData, SpellComponentData, SpellConfigComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Spell1026ShiningStarInitializeTag>(InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Spell1026ShiningStarData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), default(Spell1026ShiningStarInitializeTag), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<Spell1026ShiningStarData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1026ShiningStarData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<Spell1026ShiningStarData, SpellComponentData, SpellConfigComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Spell1026ShiningStarInitializeTag>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<Spell1026ShiningStarData, SpellComponentData, SpellConfigComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Spell1026ShiningStarInitializeTag> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<Spell1026ShiningStarData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1180813553_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1026ShiningStarData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellChargeData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1026ShiningStarData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellChargeData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item6_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1026ShiningStarData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellChargeData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
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
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellChargeData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1180813553_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public EnabledMask item6_EnabledMask;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRO<Spell1026ShiningStarIsChargingTag>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRO<Spell1026ShiningStarIsChargingTag>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellMovementComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1026ShiningStarData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item5_IntPtr, index), item6_EnabledMask.GetEnabledRefRO<Spell1026ShiningStarIsChargingTag>(index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<SpellMovementComponentData> item1_ComponentTypeHandle_RO;

			private ComponentTypeHandle<Spell1026ShiningStarData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item5_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<Spell1026ShiningStarIsChargingTag> item6_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellMovementComponentData>(isReadOnly: true);
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1026ShiningStarData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item6_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell1026ShiningStarIsChargingTag>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_EnabledMask = archetypeChunk.GetEnabledMask(ref item6_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRO<Spell1026ShiningStarIsChargingTag>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRO<Spell1026ShiningStarIsChargingTag>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1026ShiningStarData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<Spell1026ShiningStarIsChargingTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1180813553_0.TypeHandle __IFE_1180813553_0_TypeHandle;

		public IFE_1180813553_1.TypeHandle __IFE_1180813553_1_TypeHandle;

		public IFE_1180813553_2.TypeHandle __IFE_1180813553_2_TypeHandle;

		[ReadOnly]
		public ComponentLookup<SpellNeedResize> __SpellNeedResize_RO_ComponentLookup;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1180813553_0_TypeHandle = new IFE_1180813553_0.TypeHandle(ref state);
			__IFE_1180813553_1_TypeHandle = new IFE_1180813553_1.TypeHandle(ref state);
			__IFE_1180813553_2_TypeHandle = new IFE_1180813553_2.TypeHandle(ref state);
			__SpellNeedResize_RO_ComponentLookup = state.GetComponentLookup<SpellNeedResize>(isReadOnly: true);
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1180813553_0;

	private EntityQuery __query_1180813553_1;

	private EntityQuery __query_1180813553_2;

	private EntityQuery __query_1180813553_3;

	private EntityQuery __query_1180813553_4;

	private EntityQuery __query_1180813553_5;

	private EntityQuery __query_1180813553_6;

	private EntityQuery __query_1180813553_7;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<SEData>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<Spell1026ShiningStarData>();
		RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		RequireForUpdate<SpellEffectSystem.Require>();
		RequireForUpdate<SpellEffectSystem.Destroy>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		SpellSingleton singleton = __query_1180813553_3.GetSingleton<SpellSingleton>();
		Entity singletonEntity = __query_1180813553_4.GetSingletonEntity();
		EntityCommandBuffer cMD = new EntityCommandBuffer(Allocator.Temp);
		Entity singletonEntity2 = __query_1180813553_5.GetSingletonEntity();
		Entity singletonEntity3 = __query_1180813553_6.GetSingletonEntity();
		Entity singletonEntity4 = __query_1180813553_7.GetSingletonEntity();
		NativeHashMap<FixedString64Bytes, SpellEffect> effects = singleton.Effects[1026];
		NativeHashSet<Entity> nativeHashSet = new NativeHashSet<Entity>(4, Allocator.Temp);
		InternalCompilerInterface.UncheckedRefRW<LocalTransform> item4;
		Entity entity;
		foreach (QueryEnumerableWithEntity<Spell1026ShiningStarData, SpellComponentData, SpellConfigComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, Spell1026ShiningStarInitializeTag> item13 in IFE_1180813553_0.Query(__query_1180813553_0, __TypeHandle.__IFE_1180813553_0_TypeHandle, ref base.CheckedStateRef))
		{
			item13.Deconstruct(out var item, out var item2, out var item3, out item4, out var _, out entity);
			Spell1026ShiningStarData spell1026ShiningStarData = item;
			SpellComponentData spellComponentData = item2;
			SpellConfigComponentData spellConfigComponentData = item3;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW = item4;
			Entity entity2 = entity;
			cMD.SetComponentEnabled<Spell1026ShiningStarInitializeTag>(entity2, value: false);
			float sePitch = ((spell1026ShiningStarData.CurStage == 4) ? 1f : (0.9f + (float)(spell1026ShiningStarData.CurStage - 1) * 0.1f));
			FixedString32Bytes seName = "Flash";
			cMD.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(1026, in seName), SEPlayMode.Replay, 3, 0.05f, sePitch));
			float damage = singleton.Configs[spellConfigComponentData.Id].damage;
			if (damage > 0f)
			{
				uncheckedRefRW.ValueRW.Scale = SpellTools.CallulateSpellScaleByDamage(damage, spellConfigComponentData.Damage.Calculate());
			}
			float extraSizeRatio = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity2).ExtraSizeRatio;
			if (extraSizeRatio > 0f)
			{
				uncheckedRefRW.ValueRW.Scale += uncheckedRefRW.ValueRO.Scale * extraSizeRatio;
			}
			spellConfigComponentData.ColorType.ColorEnumToString(out var result);
			if (!spellComponentData.IsSplitSpell)
			{
				cMD.AppendToBuffer(singletonEntity2, new SpellEffectSystem.Require
				{
					SpellId = 1026,
					Color = result,
					Entity = entity2,
					Settings = effects["Charge_Charge"]
				});
			}
			for (int i = 1; i <= spell1026ShiningStarData.CurStage; i++)
			{
				cMD.AppendToBuffer(singletonEntity2, new SpellEffectSystem.Require
				{
					SpellId = 1026,
					Color = result,
					Entity = entity2,
					Settings = effects[$"Charge_Shine_{i}"]
				});
			}
		}
		InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData> item6;
		InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> item9;
		InternalCompilerInterface.UncheckedRefRW<SpellComponentData> item10;
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellChargeData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>> item14 in IFE_1180813553_1.Query(__query_1180813553_1, __TypeHandle.__IFE_1180813553_1_TypeHandle, ref base.CheckedStateRef))
		{
			item14.Deconstruct(out item6, out var item7, out var item8, out item4, out item9, out item10, out entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData> uncheckedRefRW2 = item6;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW3 = item7;
			InternalCompilerInterface.UncheckedRefRW<SpellChargeData> uncheckedRefRW4 = item8;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW5 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW6 = item9;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW7 = item10;
			Entity entity3 = entity;
			DynamicBuffer<SpellGameObjectEffectLink> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref base.CheckedStateRef, entity3);
			float extraSizeRatio2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity3).ExtraSizeRatio;
			extraSizeRatio2 = ((extraSizeRatio2 <= 1f) ? 1f : extraSizeRatio2);
			if (!uncheckedRefRW2.ValueRW.ChargePrefab)
			{
				foreach (SpellGameObjectEffectLink item15 in bufferAfterCompletingDependency)
				{
					SpellGameObjectEffectLink current = item15;
					if (current.EffectName == "Charge_Charge")
					{
						uncheckedRefRW2.ValueRW.ChargePrefab = current.GameObject;
						uncheckedRefRW2.ValueRW.ChargePrefab.Value.gameObject.SetActive(uncheckedRefRW2.ValueRO.CurStage != 4);
						CreateFlashEffect(uncheckedRefRW2.ValueRW.CurStage, in uncheckedRefRW5.ValueRO, in uncheckedRefRW6.ValueRO, ref uncheckedRefRW7.ValueRW, singletonEntity, cMD, singletonEntity2, entity3, effects);
						break;
					}
				}
			}
			float chargeTimer = uncheckedRefRW4.ValueRW.ChargeTimer;
			uncheckedRefRW6.ValueRW.CriticalChance = uncheckedRefRW2.ValueRW.baseCritical + chargeTimer * 0.25f;
			int num = Mathf.Clamp(Mathf.CeilToInt(uncheckedRefRW6.ValueRW.CriticalChance / 0.33f), 1, 4);
			if (!uncheckedRefRW2.ValueRO.ResizeSpellEffect && (bool)uncheckedRefRW3.ValueRO.SpellEffectGameObject.Value)
			{
				uncheckedRefRW2.ValueRW.ResizeSpellEffect = true;
				uncheckedRefRW3.ValueRW.SpellEffectGameObject.Value.transform.Find("Scale").localScale = Vector3.one * Mathf.Pow(1.1f, uncheckedRefRW2.ValueRO.CurStage) / uncheckedRefRW5.ValueRO.Scale * extraSizeRatio2;
			}
			if (uncheckedRefRW2.ValueRW.CurStage != num)
			{
				uncheckedRefRW6.ValueRO.ColorType.ColorEnumToString(out var result2);
				for (int j = uncheckedRefRW2.ValueRW.CurStage; j <= num; j++)
				{
					cMD.AppendToBuffer(singletonEntity2, new SpellEffectSystem.Require
					{
						SpellId = 1026,
						Color = result2,
						Entity = entity3,
						Settings = effects[$"Charge_Shine_{j}"]
					});
				}
				uncheckedRefRW2.ValueRW.CurStage = num;
				CreateFlashEffect(num, in uncheckedRefRW5.ValueRO, in uncheckedRefRW6.ValueRO, ref uncheckedRefRW7.ValueRW, singletonEntity, cMD, singletonEntity2, entity3, effects);
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW3.ValueRW.SpellEffectEntity))
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW3.ValueRW.SpellEffectEntity).ValueRW.Scale = Mathf.Pow(1.1f, uncheckedRefRW2.ValueRO.CurStage) * extraSizeRatio2;
				}
				if (num == 4 && (bool)uncheckedRefRW2.ValueRO.ChargePrefab)
				{
					uncheckedRefRW2.ValueRO.ChargePrefab.Value.gameObject.SetActive(value: false);
				}
			}
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, EnabledRefRO<Spell1026ShiningStarIsChargingTag>> item16 in IFE_1180813553_2.Query(__query_1180813553_2, __TypeHandle.__IFE_1180813553_2_TypeHandle, ref base.CheckedStateRef))
		{
			item16.Deconstruct(out var item11, out item6, out item9, out item4, out item10, out var _, out entity);
			InternalCompilerInterface.UncheckedRefRO<SpellMovementComponentData> uncheckedRefRO = item11;
			InternalCompilerInterface.UncheckedRefRW<Spell1026ShiningStarData> uncheckedRefRW8 = item6;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW9 = item9;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW10 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW11 = item10;
			Entity entity4 = entity;
			cMD.SetComponentEnabled<Spell1026ShiningStarIsChargingTag>(entity4, value: false);
			float extraSizeRatio3 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellNeedResize_RO_ComponentLookup, ref base.CheckedStateRef, entity4).ExtraSizeRatio;
			extraSizeRatio3 = ((extraSizeRatio3 <= 1f) ? 1f : extraSizeRatio3);
			if ((bool)uncheckedRefRW8.ValueRO.ChargePrefab)
			{
				cMD.AppendToBuffer(singletonEntity4, new SpellEffectSystem.Destroy
				{
					Entity = entity4,
					Name = "Charge_Charge"
				});
			}
			uncheckedRefRW8.ValueRW.ReadyToShoot = true;
			uncheckedRefRW9.ValueRW.ColorType.ColorEnumToString(out var result3);
			for (int k = 1; k <= uncheckedRefRW8.ValueRW.CurStage; k++)
			{
				cMD.AppendToBuffer(singletonEntity2, new SpellEffectSystem.Require
				{
					SpellId = 1026,
					Color = result3,
					Entity = entity4,
					Settings = new SpellEffect
					{
						Name = $"Trail_{k}",
						DestroyDelay = 0f,
						Layer = LayerCorrectType.Coordinate
					}
				});
			}
			if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW11.ValueRO.Shooter) && !uncheckedRefRO.ValueRO.IsFallSpell && !nativeHashSet.Contains(uncheckedRefRW11.ValueRO.Shooter))
			{
				nativeHashSet.Add(uncheckedRefRW11.ValueRO.Shooter);
				ref UnitProperty_Dots valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, uncheckedRefRW11.ValueRO.Shooter).ValueRW;
				float num2 = 1f;
				if (valueRW.unitCfg.IsSameCamp(UnitType.Player))
				{
					num2 = SpellShootGroupExtend.GetRecoilRatio(uncheckedRefRW11.ValueRO.Wand).CurrentMulRatio;
				}
				valueRW.TakeKnockback(-uncheckedRefRO.ValueRO.Direction * 2f * ((uncheckedRefRW8.ValueRO.CurStage < 1) ? 1 : uncheckedRefRW8.ValueRO.CurStage) * num2);
			}
			uncheckedRefRW9.ValueRW.CriticalChance = uncheckedRefRW8.ValueRO.baseCritical;
			FixedString32Bytes seName = "ShootStar";
			cMD.AppendToBuffer(singletonEntity, new SEData(DTool.GetSpellSEName(1026, in seName)));
			CreateShootEffect(cMD, singletonEntity3, uncheckedRefRW8, in uncheckedRefRW10.ValueRO, uncheckedRefRW9, uncheckedRefRW11, uncheckedRefRO);
			if ((bool)uncheckedRefRW11.ValueRW.SpellEffectGameObject.Value)
			{
				uncheckedRefRW11.ValueRW.SpellEffectGameObject.Value.transform.Find("Scale").localScale = Vector3.one * Mathf.Pow(1.1f, uncheckedRefRW8.ValueRO.CurStage) / uncheckedRefRW10.ValueRO.Scale * extraSizeRatio3;
			}
		}
		nativeHashSet.Dispose();
		cMD.Playback(base.World.EntityManager);
		cMD.Dispose();
	}

	private void CreateFlashEffect(int stage, in LocalTransform transform, in SpellConfigComponentData config, ref SpellComponentData spell, Entity SEData, EntityCommandBuffer CMD, Entity Require, Entity spellEntity, NativeHashMap<FixedString64Bytes, SpellEffect> effects)
	{
		config.ColorType.ColorEnumToString(out var result);
		CMD.AppendToBuffer(Require, new SpellEffectSystem.Require
		{
			SpellId = 1026,
			Color = result,
			Entity = spellEntity,
			Settings = effects[$"Flash_{stage}"]
		});
		FixedString32Bytes seName = ((stage == 4) ? "FinalFlash" : "Flash");
		float sePitch = ((stage == 4) ? 1f : (0.9f + (float)(stage - 1) * 0.1f));
		CMD.AppendToBuffer(SEData, new SEData(DTool.GetSpellSEName(1026, in seName), SEPlayMode.Replay, 3, 0.05f, sePitch));
	}

	private void CreateShootEffect(EntityCommandBuffer CMD, Entity UnfollowingRequire, RefRW<Spell1026ShiningStarData> spell, in LocalTransform transform, RefRW<SpellConfigComponentData> config, RefRW<SpellComponentData> data, RefRO<SpellMovementComponentData> movement)
	{
		if (movement.ValueRO.Type == SpellSpecialMovementType.Rotation || data.ValueRW.IsSplitSpell)
		{
			return;
		}
		config.ValueRW.ColorType.ColorEnumToString(out var result);
		Quaternion quaternion;
		if (!movement.ValueRO.IsFallSpell)
		{
			float3 direction = movement.ValueRO.Direction;
			direction.z = 0f;
			float z = math.atan2(direction.y, direction.x);
			quaternion = Unity.Mathematics.quaternion.Euler(0f, 0f, z);
		}
		else
		{
			float3 @float = movement.ValueRO.Speed * movement.ValueRO.Direction;
			if (movement.ValueRO.Type == SpellSpecialMovementType.Rotation)
			{
				@float = movement.ValueRO.Speed * movement.ValueRO.Direction;
			}
			float3 rootPosition = @float + new float3(0f, 0f, movement.ValueRO.CurrentFallSpeed);
			float3 layerPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			rootPosition += layerPosition;
			quaternion = Unity.Mathematics.quaternion.Euler(0f, 0f, math.atan2(rootPosition.y, rootPosition.x));
		}
		float3 layerPosition2 = DTool.GetLayerPosition(in transform.Position, LayerCorrectType.Coordinate);
		CMD.AppendToBuffer(UnfollowingRequire, new SpellEffectSystem.UnfollowingRequire
		{
			SpellId = 1026,
			Color = result,
			Scale = transform.Scale,
			StartPosition = transform.Position + layerPosition2,
			StartRotation = quaternion,
			Settings = new SpellEffect
			{
				Name = $"Shoot_{math.max(spell.ValueRW.CurStage, 1)}",
				Layer = LayerCorrectType.Coordinate,
				DestroyDelay = 2f
			}
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell1026ShiningStarData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1026ShiningStarInitializeTag>();
		__query_1180813553_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithDisabled<Spell1026ShiningStarInitializeTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellChargingTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1026ShiningStarData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellChargeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		__query_1180813553_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithDisabled<SpellChargingTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithDisabled<Spell1026ShiningStarInitializeTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<Spell1026ShiningStarIsChargingTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1026ShiningStarData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		__query_1180813553_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1180813553_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1180813553_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1180813553_5 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.UnfollowingRequire>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1180813553_6 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1180813553_7 = entityQueryBuilder2.Build(ref state);
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
	public Spell1026ShiningStarChargingSystem()
	{
	}
}
