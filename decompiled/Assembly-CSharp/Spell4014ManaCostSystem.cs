using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine.Scripting;

[UpdateAfter(typeof(Spell4014LaserCrystalSystem))]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public class Spell4014ManaCostSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1128555481_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1128555481_1
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1128555481_2
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Get(int index)
			{
				return (InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item2_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item2_ComponentTypeHandle_RO;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<(InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>)>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public (InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>) Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell4014LaserCrystalData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1128555481_0.TypeHandle __IFE_1128555481_0_TypeHandle;

		public IFE_1128555481_1.TypeHandle __IFE_1128555481_1_TypeHandle;

		public IFE_1128555481_2.TypeHandle __IFE_1128555481_2_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1128555481_0_TypeHandle = new IFE_1128555481_0.TypeHandle(ref state);
			__IFE_1128555481_1_TypeHandle = new IFE_1128555481_1.TypeHandle(ref state);
			__IFE_1128555481_2_TypeHandle = new IFE_1128555481_2.TypeHandle(ref state);
		}
	}

	private NativeHashMap<int, (float costThisFrameTotal, bool isGettedCurrentMana, bool isCostMana)> WandManaCosts;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1128555481_0;

	[Preserve]
	protected override void OnCreate()
	{
		WandManaCosts = new NativeHashMap<int, (float, bool, bool)>(10, Allocator.Persistent);
		RequireForUpdate<Spell4014LaserCrystalData>();
	}

	[Preserve]
	protected override void OnDestroy()
	{
		if (WandManaCosts.IsCreated)
		{
			WandManaCosts.Dispose();
		}
	}

	[Preserve]
	protected override void OnUpdate()
	{
		if (!WandManaCosts.IsCreated)
		{
			return;
		}
		foreach (var (uncheckedRefRW, uncheckedRefRO) in IFE_1128555481_0.Query(__query_1128555481_0, __TypeHandle.__IFE_1128555481_0_TypeHandle, ref base.CheckedStateRef))
		{
			if (uncheckedRefRO.ValueRO.Wand.Value == null)
			{
				continue;
			}
			if (!uncheckedRefRW.ValueRO.IsInitializedWandCost)
			{
				uncheckedRefRW.ValueRW.IsInitializedWandCost = true;
				if (!WandManaCosts.ContainsKey(uncheckedRefRO.ValueRO.Wand.Value.WandIndex))
				{
					WandManaCosts.Add(uncheckedRefRO.ValueRO.Wand.Value.WandIndex, (0f, false, false));
				}
				else
				{
					SetWandManaCost(uncheckedRefRO.ValueRO.Wand.Value.WandIndex, WandManaCosts[uncheckedRefRO.ValueRO.Wand.Value.WandIndex].costThisFrameTotal, isGettedCurrentMana: false, isCostMana: false);
				}
			}
			else if (WandManaCosts.ContainsKey(uncheckedRefRO.ValueRO.Wand.Value.WandIndex) && !WandManaCosts[uncheckedRefRO.ValueRO.Wand.Value.WandIndex].isGettedCurrentMana)
			{
				SetWandManaCost(uncheckedRefRO.ValueRO.Wand.Value.WandIndex, 0f, isGettedCurrentMana: true, isCostMana: false);
			}
		}
		foreach (var (uncheckedRefRW2, uncheckedRefRO2) in IFE_1128555481_1.Query(__query_1128555481_0, __TypeHandle.__IFE_1128555481_1_TypeHandle, ref base.CheckedStateRef))
		{
			if (uncheckedRefRW2.ValueRO.IsHit && uncheckedRefRW2.ValueRO.IsHitFrame)
			{
				SetWandManaCost(uncheckedRefRO2.ValueRO.Wand.Value.WandIndex, WandManaCosts[uncheckedRefRO2.ValueRO.Wand.Value.WandIndex].costThisFrameTotal + uncheckedRefRW2.ValueRO.ManaCostThisFrame, isGettedCurrentMana: false, isCostMana: false);
			}
		}
		foreach (var (uncheckedRefRW3, uncheckedRefRO3) in IFE_1128555481_2.Query(__query_1128555481_0, __TypeHandle.__IFE_1128555481_2_TypeHandle, ref base.CheckedStateRef))
		{
			if (!uncheckedRefRW3.ValueRO.IsHit || !uncheckedRefRW3.ValueRO.IsHitFrame)
			{
				continue;
			}
			if (uncheckedRefRO3.ValueRO.Wand.Value.CheckCurrentMpEnough(WandManaCosts[uncheckedRefRO3.ValueRO.Wand.Value.WandIndex].costThisFrameTotal))
			{
				if (!WandManaCosts[uncheckedRefRO3.ValueRO.Wand.Value.WandIndex].isCostMana)
				{
					uncheckedRefRO3.ValueRO.Wand.Value.CostMp(WandManaCosts[uncheckedRefRO3.ValueRO.Wand.Value.WandIndex].costThisFrameTotal);
					(float, bool, bool) value = WandManaCosts[uncheckedRefRO3.ValueRO.Wand.Value.WandIndex];
					value.Item3 = true;
					WandManaCosts[uncheckedRefRO3.ValueRO.Wand.Value.WandIndex] = value;
				}
			}
			else
			{
				uncheckedRefRW3.ValueRW.ForceCooling = true;
			}
		}
		foreach (KVPair<int, (float, bool, bool)> wandManaCost in WandManaCosts)
		{
			wandManaCost.Value.Item3 = false;
			wandManaCost.Value.Item2 = false;
		}
	}

	private void SetWandManaCost(int index, float costThisFrameTotal, bool isGettedCurrentMana, bool isCostMana)
	{
		(float, bool, bool) tuple = WandManaCosts[index];
		tuple = (costThisFrameTotal, isGettedCurrentMana, isCostMana);
		WandManaCosts[index] = tuple;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
		__query_1128555481_0 = entityQueryBuilder2.Build(ref state);
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
	public Spell4014ManaCostSystem()
	{
	}
}
