using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[CompilerGenerated]
[UpdateAfter(typeof(SpellShootSystem))]
[UpdateInGroup(typeof(SpellCreateSystemGroup))]
public struct Spell4027ProcessSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1539333093_0
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
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell4027BlueRuneData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell4027BlueRuneData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsCollider> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item6_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell4027BlueRuneData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellComponentData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1539333093_0.TypeHandle __IFE_1539333093_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RO_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1539333093_0_TypeHandle = new IFE_1539333093_0.TypeHandle(ref state);
			__Unity_Physics_PhysicsVelocity_RO_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>(isReadOnly: true);
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1539333093_0;

	private EntityQuery __query_1539333093_1;

	private EntityQuery __query_1539333093_2;

	private EntityQuery __query_1539333093_3;

	private EntityQuery __query_1539333093_4;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<DynamicOptimizeData>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell4027BlueRuneData>();
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		int item = PlayerMgr.Inst.GetPlayerRuneCount().BlueRune;
		DynamicOptimizeData singleton = __query_1539333093_1.GetSingleton<DynamicOptimizeData>();
		float num = state.WorldUnmanaged.Time.DeltaTime * singleton.LastFrameTimeScale;
		SpellSingleton singleton2 = __query_1539333093_2.GetSingleton<SpellSingleton>();
		Entity singletonEntity = __query_1539333093_3.GetSingletonEntity();
		EntityCommandBuffer.ParallelWriter parallelWriter = __query_1539333093_4.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData>, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellComponentData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>> item8 in IFE_1539333093_0.Query(__query_1539333093_0, __TypeHandle.__IFE_1539333093_0_TypeHandle, ref state))
		{
			item8.Deconstruct(out var item2, out var item3, out var item4, out var item5, out var item6, out var item7, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell4027BlueRuneData> uncheckedRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW2 = item3;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW3 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellComponentData> uncheckedRefRW4 = item5;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW5 = item6;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW6 = item7;
			Entity entity2 = entity;
			uncheckedRefRW.ValueRW.NormalChasePower = math.min(math.pow(uncheckedRefRW.ValueRW.DisableColliderTimer, 2.8f) / 1f * 7f, 1f);
			uncheckedRefRW.ValueRW.DisableColliderTimer += num;
			if (uncheckedRefRW.ValueRO.IsInitialized)
			{
				continue;
			}
			uncheckedRefRW.ValueRW.IsInitialized = true;
			int runeEffectLevel = PlayerMgr.Inst.GetRuneEffectLevel(item);
			if (runeEffectLevel >= 2 && !uncheckedRefRW4.ValueRO.IsSplitSpell)
			{
				uncheckedRefRW.ValueRW.MpRefillAmount = uncheckedRefRW3.ValueRO.Float3;
			}
			float num2 = uncheckedRefRW3.ValueRO.Float1 * (float)item;
			if (runeEffectLevel >= 3 && uncheckedRefRW4.ValueRO.Wand.Value.WandCfg != null)
			{
				num2 += uncheckedRefRW4.ValueRO.Wand.Value.MaxMP * 0.2f;
			}
			if (uncheckedRefRW3.ValueRO.Int3 > 0)
			{
				uncheckedRefRW6.ValueRW.Scale *= 1.5f;
				uncheckedRefRW.ValueRW.IsSuperBlueRune = true;
			}
			uncheckedRefRW3.ValueRW.Damage.Base = num2;
			uncheckedRefRW3.ValueRW.Duration.Base = 1.3f;
			if (!uncheckedRefRW5.ValueRO.IsFallSpell)
			{
				uncheckedRefRW.ValueRW.InitialShootDirection = uncheckedRefRW5.ValueRW.Direction;
				uncheckedRefRW.ValueRW.NoTargetPosition = uncheckedRefRW6.ValueRO.Position + uncheckedRefRW5.ValueRO.Direction * UnityEngine.Random.Range(8f, 16f);
				if (uncheckedRefRW5.ValueRW.Type == SpellSpecialMovementType.Rotation)
				{
					uncheckedRefRW.ValueRW.MaxRotationRadius = uncheckedRefRW5.ValueRW.AroundRadius;
					uncheckedRefRW5.ValueRW.AroundRadius *= 0.1f;
					uncheckedRefRW.ValueRW.CurrentRotationRadius *= 0.1f;
					uncheckedRefRW6.ValueRW.Position.xy = uncheckedRefRW5.ValueRW.AroundCenter.xy;
				}
				float num3 = (uncheckedRefRW.ValueRO.IsSuperBlueRune ? 90f : 60f);
				float num4 = UnityEngine.Random.Range(180f - num3, 180f + num3);
				uncheckedRefRW.ValueRW.NormalIgnoreChaseDuration = 0.2f + (1f - math.abs(num4 - 180f) / num3) * 0.15f * UnityEngine.Random.Range(0.5f, 1.5f);
				float3 oldDir = uncheckedRefRW5.ValueRW.Direction;
				float3 dir = DTool.GetDir(in oldDir, num4);
				uncheckedRefRW5.ValueRW.Direction = dir;
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RO_ComponentLookup, ref state, entity2) && uncheckedRefRW5.ValueRW.Type != SpellSpecialMovementType.Rotation)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref state, entity2).ValueRW.Linear = dir * uncheckedRefRW5.ValueRW.Speed;
				}
			}
			else
			{
				uncheckedRefRW6.ValueRW.Position += (float3)(UnityEngine.Random.insideUnitSphere.IgnoreZ() * 0.75f);
			}
			if (uncheckedRefRW.ValueRW.IsSuperBlueRune)
			{
				SpellSingleton singleton3 = __query_1539333093_2.GetSingleton<SpellSingleton>();
				FixedString32Bytes effectName = "SSpell";
				if (singleton3.TryGetSpellEffectEntity(4027, in effectName, uncheckedRefRW3.ValueRO.ColorType, out var entity3))
				{
					Entity entity4 = parallelWriter.Instantiate(0, entity3);
					Entity spellEffectEntity = uncheckedRefRW4.ValueRO.SpellEffectEntity;
					parallelWriter.AddComponent<Parent>(0, entity4);
					parallelWriter.SetComponent(0, entity4, new Parent
					{
						Value = spellEffectEntity
					});
					LocalTransform identity = LocalTransform.Identity;
					identity.Position.yz = new float2(0.15f, -0.15f);
					identity.Rotation = quaternion.Euler(new float3(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
					parallelWriter.SetComponent(0, entity4, identity);
					parallelWriter.AppendToBuffer(0, spellEffectEntity, new LinkedEntityGroup
					{
						Value = entity4
					});
				}
				parallelWriter.AppendToBuffer(0, singletonEntity, new SpellEffectSystem.Require
				{
					Settings = singleton2.Effects[4027]["STTrail"],
					Entity = entity2,
					Color = uncheckedRefRW3.ValueRO.ColorType.ToString(),
					SpellId = 4027
				});
			}
			uncheckedRefRW.ValueRW.RecordColliderType = true;
			CompoundCollider* colliderPtr = (CompoundCollider*)uncheckedRefRW2.ValueRW.ColliderPtr;
			uncheckedRefRW.ValueRW.Collider1Type = colliderPtr->Children[0].Collider->GetCollisionResponse();
			uncheckedRefRW.ValueRW.Collider2Type = colliderPtr->Children[1].Collider->GetCollisionResponse();
			colliderPtr->Children[0].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
			colliderPtr->Children[1].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAllRW<Spell4027BlueRuneData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		__query_1539333093_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<DynamicOptimizeData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1539333093_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1539333093_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1539333093_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1539333093_4 = entityQueryBuilder2.Build(ref state);
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
		((Spell4027ProcessSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell4027ProcessSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell4027ProcessSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
