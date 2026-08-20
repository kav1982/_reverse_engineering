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
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[UpdateAfter(typeof(SpellShootSystem))]
[CompilerGenerated]
public class SpellTwineTriggerSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_454781983_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellComponentData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<SpellTwineTriggerComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item1_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<SpellTwineTriggerComponentData> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellTwineTriggerComponentData>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RO.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RO.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RO);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellTwineTriggerComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_454781983_0.TypeHandle __IFE_454781983_0_TypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_454781983_0_TypeHandle = new IFE_454781983_0.TypeHandle(ref state);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_454781983_0;

	private EntityQuery __query_454781983_1;

	private EntityQuery __query_454781983_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<GlobalRandom>();
		RequireForUpdate<SpellTwineTriggerComponentData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		GlobalRandom singleton = __query_454781983_1.GetSingleton<GlobalRandom>();
		ShootSpellBuffer shootSpellBuffer = new ShootSpellBuffer();
		int num = 0;
		DynamicOptimizeData singleton2 = __query_454781983_2.GetSingleton<DynamicOptimizeData>();
		int num2 = (singleton2.IsMobilePlatform ? 100 : 150);
		int threshold = (singleton2.IsMobilePlatform ? 120 : 180);
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRO<SpellComponentData>, InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item4 in IFE_454781983_0.Query(__query_454781983_0, __TypeHandle.__IFE_454781983_0_TypeHandle, ref base.CheckedStateRef))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRO<SpellComponentData> data = item;
			InternalCompilerInterface.UncheckedRefRO<SpellTwineTriggerComponentData> twine = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO = item3;
			Entity entity2 = entity;
			SpellShootGroup subGroup = base.EntityManager.GetComponentObject<SpellSubGroupComponentData>(data.ValueRO.SubGroupEntity).SubGroup;
			int num3 = 0;
			SpellShootData[] shoots = subGroup.Shoots;
			foreach (SpellShootData spellShootData in shoots)
			{
				num3 += SpellTools.CalculateSpellComplexity(spellShootData.Spell.GetFinalConfig().abilityType);
			}
			float num4 = singleton.random.NextFloat(0f, 360f);
			int num5 = twine.ValueRO.Count;
			float spellEfficiency = 1f;
			if (num + num3 >= num2)
			{
				int finalSpawnCountWithLimitCount = SpellTools.GetFinalSpawnCountWithLimitCount(num2, 2, threshold, 1, num, num5);
				spellEfficiency = (float)num5 / (float)finalSpawnCountWithLimitCount;
				num5 = finalSpawnCountWithLimitCount;
			}
			for (int j = 0; j < num5; j++)
			{
				float angle = num4 + (float)j * (360f / (float)num5);
				SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
				builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
				{
					parameter.OverwriteRotationStartAngle = angle;
					parameter.tags.Add(SpellTag.Twine);
					parameter.RotationMovementInfo = (2f, 0f, twine.ValueRO.Radius);
					parameter.shootFromPostSlots = data.ValueRO.FromPostSlot;
					parameter.spellEfficiency *= spellEfficiency;
					parameter.finalDamageRatio *= spellEfficiency;
				};
				num += num3;
				shootSpellBuffer.ShootByTrigger(entity2, data.ValueRO, subGroup, ShootSpellSpatialInfo.ToPoint(uncheckedRefRO.ValueRO.Position, uncheckedRefRO.ValueRO.Position + new float3(0f, 0.1f, 0f)), builder);
			}
			base.EntityManager.SetComponentEnabled<SpellTwineTriggerComponentData>(entity2, value: false);
		}
		shootSpellBuffer.Playback();
		shootSpellBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<SpellTwineTriggerComponentData>();
		__query_454781983_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalRandom>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_454781983_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_454781983_2 = entityQueryBuilder2.Build(ref state);
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
	public SpellTwineTriggerSystem()
	{
	}
}
