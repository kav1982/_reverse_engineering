using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
[UpdateAfter(typeof(Spell4014LaserDamageApplySystem))]
internal struct Spell4014CrystalChangeSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1349209344_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4014LaserCrystalData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellConfigComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4014LaserCrystalData> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellConfigComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4014LaserCrystalData>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellConfigComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1349209344_0.TypeHandle __IFE_1349209344_0_TypeHandle;

		public BufferLookup<SpellGameObjectEffectLink> __SpellGameObjectEffectLink_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1349209344_0_TypeHandle = new IFE_1349209344_0.TypeHandle(ref state);
			__SpellGameObjectEffectLink_RW_BufferLookup = state.GetBufferLookup<SpellGameObjectEffectLink>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1349209344_0;

	private EntityQuery __query_1349209344_1;

	private EntityQuery __query_1349209344_2;

	private EntityQuery __query_1349209344_3;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellEffectSystem.Destroy>();
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<Spell4014LaserCrystalData>();
	}

	public void OnUpdate(ref SystemState state)
	{
		EntityCommandBuffer cmd = __query_1349209344_1.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
		Entity singletonEntity = __query_1349209344_2.GetSingletonEntity();
		Entity singletonEntity2 = __query_1349209344_3.GetSingletonEntity();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData>, InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellComponentData>> item4 in IFE_1349209344_0.Query(__query_1349209344_0, __TypeHandle.__IFE_1349209344_0_TypeHandle, ref state))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell4014LaserCrystalData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<SpellConfigComponentData> uncheckedRefRO = item2;
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> uncheckedRefRO2 = item3;
			Entity entity2 = entity;
			float rBase = (uncheckedRefRO2.ValueRO.IsSplitSpell ? 0.6f : 1f);
			if (uncheckedRefRW.ValueRO.IsHit)
			{
				TryGenerateEffect(ref uncheckedRefRW.ValueRW.IsRequiredPowerUp, ref uncheckedRefRW.ValueRW.LaserCrystalPowerUpGO, "PowerUp", cmd, singletonEntity, singletonEntity2, InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), in uncheckedRefRO.ValueRO, rBase, entity2);
			}
			else
			{
				TryRecycleEffect(ref uncheckedRefRW.ValueRW.LaserCrystalPowerUpGO, ref uncheckedRefRW.ValueRW.IsRequiredPowerUp, "PowerUp", cmd, singletonEntity2, entity2);
			}
			if (uncheckedRefRW.ValueRO.KeepHittingRate > 2f)
			{
				TryGenerateEffect(ref uncheckedRefRW.ValueRW.IsRequired2Sec, ref uncheckedRefRW.ValueRW.LaserCrystal2SecGO, "2Sec", cmd, singletonEntity, singletonEntity2, InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), in uncheckedRefRO.ValueRO, rBase, entity2);
			}
			else
			{
				TryRecycleEffect(ref uncheckedRefRW.ValueRW.LaserCrystal2SecGO, ref uncheckedRefRW.ValueRW.IsRequired2Sec, "2Sec", cmd, singletonEntity2, entity2);
			}
			if (uncheckedRefRW.ValueRO.KeepHittingRate > 5f)
			{
				TryGenerateEffect(ref uncheckedRefRW.ValueRW.IsRequired5Sec, ref uncheckedRefRW.ValueRW.LaserCrystal5SecGO, "5Sec", cmd, singletonEntity, singletonEntity2, InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), in uncheckedRefRO.ValueRO, rBase, entity2);
			}
			else
			{
				TryRecycleEffect(ref uncheckedRefRW.ValueRW.LaserCrystal5SecGO, ref uncheckedRefRW.ValueRW.IsRequired5Sec, "5Sec", cmd, singletonEntity2, entity2);
			}
			if (uncheckedRefRW.ValueRO.KeepHittingRate > 10f)
			{
				TryGenerateEffect(ref uncheckedRefRW.ValueRW.IsRequired10Sec, ref uncheckedRefRW.ValueRW.LaserCrystal10SecGO, "10Sec", cmd, singletonEntity, singletonEntity2, InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__SpellGameObjectEffectLink_RW_BufferLookup, ref state, entity2), in uncheckedRefRO.ValueRO, rBase, entity2);
			}
			else
			{
				TryRecycleEffect(ref uncheckedRefRW.ValueRW.LaserCrystal10SecGO, ref uncheckedRefRW.ValueRW.IsRequired10Sec, "10Sec", cmd, singletonEntity2, entity2);
			}
		}
	}

	private bool TryGenerateEffect(ref bool requireSign, ref UnityObjectRef<GameObject> goRef, string name, EntityCommandBuffer cmd, Entity crystalEffectRequire, Entity effectDestroyRequire, DynamicBuffer<SpellGameObjectEffectLink> effectLink, in SpellConfigComponentData config, float rBase, Entity entity)
	{
		if (!requireSign)
		{
			requireSign = true;
			TryRecycleEffect(ref goRef, name, cmd, effectDestroyRequire, entity);
			GenerateCrystalAndLineEffectRequire(name, cmd, crystalEffectRequire, entity);
		}
		return EffectLinkGetAndInitEffect(name, ref goRef, in config, in effectLink, rBase);
	}

	private void GenerateCrystalAndLineEffectRequire(string name, EntityCommandBuffer cmd, Entity crystalEffectRequire, Entity spellEntity)
	{
		cmd.AppendToBuffer(crystalEffectRequire, new SpellEffectSystem.Require
		{
			Entity = spellEntity,
			SpellId = 4014,
			Settings = 
			{
				Name = name,
				Layer = LayerCorrectType.Coordinate,
				IgnoreColor = true,
				ClearParticle = true,
				ClearTrail = true,
				ScaleMode = SpellEffectSystem.ScaleMode.Ignore
			}
		});
	}

	private bool EffectLinkGetAndInitEffect(string name, ref UnityObjectRef<GameObject> goRef, in SpellConfigComponentData config, in DynamicBuffer<SpellGameObjectEffectLink> effectLink, float rBase)
	{
		return TryGetEffGO(ref goRef, in config, in effectLink);
		static float GetR(float baseR, in RadiusAttributeValue rAttValue)
		{
			RadiusAttributeValue radiusAttributeValue = rAttValue;
			radiusAttributeValue.Base = baseR;
			return radiusAttributeValue.CalculateIgnoreFall();
		}
		bool TryGetEffGO(ref UnityObjectRef<GameObject> goRef, in SpellConfigComponentData config, in DynamicBuffer<SpellGameObjectEffectLink> effectLink)
		{
			if (!goRef.Value)
			{
				if (TryGetLinkEffect(effectLink, out var linkedObject2))
				{
					linkedObject2.Value.transform.localScale = Vector3.one * GetR(rBase, in config.Radius);
					goRef = linkedObject2;
					return true;
				}
				return false;
			}
			return true;
		}
		bool TryGetLinkEffect(DynamicBuffer<SpellGameObjectEffectLink> linkBuffer, out UnityObjectRef<GameObject> linkedObject)
		{
			foreach (SpellGameObjectEffectLink item in linkBuffer)
			{
				SpellGameObjectEffectLink current = item;
				if (current.EffectName == name)
				{
					linkedObject = current.GameObject;
					return true;
				}
			}
			linkedObject = null;
			return false;
		}
	}

	private void TryRecycleEffect(ref UnityObjectRef<GameObject> goRef, ref bool requirSign, string name, EntityCommandBuffer cmd, Entity effectDestroyRequire, Entity spellEntity)
	{
		if ((bool)goRef)
		{
			cmd.AppendToBuffer(effectDestroyRequire, new SpellEffectSystem.Destroy
			{
				Entity = spellEntity,
				Name = name
			});
			requirSign = false;
			goRef = default(UnityObjectRef<GameObject>);
		}
	}

	private void TryRecycleEffect(ref UnityObjectRef<GameObject> goRef, string name, EntityCommandBuffer cmd, Entity effectDestroyRequire, Entity spellEntity)
	{
		if ((bool)goRef)
		{
			cmd.AppendToBuffer(effectDestroyRequire, new SpellEffectSystem.Destroy
			{
				Entity = spellEntity,
				Name = name
			});
		}
	}

	public void OnDestroy(ref SystemState state)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell4014LaserCrystalData>();
		__query_1349209344_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1349209344_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1349209344_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Destroy>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1349209344_3 = entityQueryBuilder2.Build(ref state);
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
		((Spell4014CrystalChangeSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4014CrystalChangeSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnDestroy(IntPtr self, IntPtr state)
	{
		((Spell4014CrystalChangeSystem*)self.ToPointer())->OnDestroy(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4014CrystalChangeSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
