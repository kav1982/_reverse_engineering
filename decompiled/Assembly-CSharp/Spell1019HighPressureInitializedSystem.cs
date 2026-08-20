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

[UpdateInGroup(typeof(SpellCreateSystemGroup))]
[CompilerGenerated]
[UpdateAfter(typeof(SpellShootSystem))]
public struct Spell1019HighPressureInitializedSystem : ISystem, ISystemCompilerGenerated
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1395995569_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public EnabledMask item2_EnabledMask;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public IntPtr item7_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData>, EnabledRefRW<Spell1019InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, SpellComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData>, EnabledRefRW<Spell1019InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, SpellComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell1019HighPressureData>(item1_IntPtr, index), item2_EnabledMask.GetEnabledRefRW<Spell1019InitializedTag>(index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<SpellComponentData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsVelocity>(item6_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item7_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell1019HighPressureData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<Spell1019InitializedTag> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item3_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<SpellComponentData> item4_ComponentTypeHandle_RO;

			private ComponentTypeHandle<LocalTransform> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<PhysicsVelocity> item6_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item7_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1019HighPressureData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell1019InitializedTag>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item4_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<SpellComponentData>(isReadOnly: true);
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsVelocity>();
				item7_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RW.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RO.Update(ref systemState);
				item5_ComponentTypeHandle_RW.Update(ref systemState);
				item6_ComponentTypeHandle_RW.Update(ref systemState);
				item7_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_EnabledMask = archetypeChunk.GetEnabledMask(ref item2_ComponentTypeHandle_RW);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RO);
				result.item5_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item5_ComponentTypeHandle_RW);
				result.item6_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item6_ComponentTypeHandle_RW);
				result.item7_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item7_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData>, EnabledRefRW<Spell1019InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, SpellComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData>, EnabledRefRW<Spell1019InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, SpellComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell1019HighPressureData>();
			state.EntityManager.CompleteDependencyBeforeRW<Spell1019InitializedTag>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRO<SpellComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsVelocity>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1395995569_0.TypeHandle __IFE_1395995569_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[ReadOnly]
		public BufferLookup<Spell1019BulletBuffer> __Spell1019BulletBuffer_RO_BufferLookup;

		public ComponentLookup<Spell1019LastShootEntityData> __Spell1019LastShootEntityData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellMovementComponentData> __SpellMovementComponentData_RO_ComponentLookup;

		public ComponentLookup<SpellComponentData> __SpellComponentData_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<SpellComponentData> __SpellComponentData_RO_ComponentLookup;

		public ComponentLookup<PhysicsVelocity> __Unity_Physics_PhysicsVelocity_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<PlayerController_Dots> __PlayerController_Dots_RO_ComponentLookup;

		public ComponentLookup<PhysicsCollider> __Unity_Physics_PhysicsCollider_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1395995569_0_TypeHandle = new IFE_1395995569_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__Spell1019BulletBuffer_RO_BufferLookup = state.GetBufferLookup<Spell1019BulletBuffer>(isReadOnly: true);
			__Spell1019LastShootEntityData_RW_ComponentLookup = state.GetComponentLookup<Spell1019LastShootEntityData>();
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
			__SpellMovementComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellMovementComponentData>(isReadOnly: true);
			__SpellComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellComponentData>();
			__SpellComponentData_RO_ComponentLookup = state.GetComponentLookup<SpellComponentData>(isReadOnly: true);
			__Unity_Physics_PhysicsVelocity_RW_ComponentLookup = state.GetComponentLookup<PhysicsVelocity>();
			__Spell4005WandSpiritData_RO_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>(isReadOnly: true);
			__PlayerController_Dots_RO_ComponentLookup = state.GetComponentLookup<PlayerController_Dots>(isReadOnly: true);
			__Unity_Physics_PhysicsCollider_RW_ComponentLookup = state.GetComponentLookup<PhysicsCollider>();
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1395995569_0;

	private EntityQuery __query_1395995569_1;

	private EntityQuery __query_1395995569_2;

	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<SpellEffectSystem.Require>();
		state.RequireForUpdate<SpellSingleton>();
		state.RequireForUpdate<Spell1019InitializedTag>();
	}

	public unsafe void OnUpdate(ref SystemState state)
	{
		ComponentLookup<LocalTransform> componentLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref state);
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		SpellSingleton singleton = __query_1395995569_1.GetSingleton<SpellSingleton>();
		Entity singletonEntity = __query_1395995569_2.GetSingletonEntity();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData>, EnabledRefRW<Spell1019InitializedTag>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, SpellComponentData, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>> item8 in IFE_1395995569_0.Query(__query_1395995569_0, __TypeHandle.__IFE_1395995569_0_TypeHandle, ref state))
		{
			item8.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var item7, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell1019HighPressureData> uncheckedRefRW = item;
			EnabledRefRW<Spell1019InitializedTag> enabledRefRW = item2;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW2 = item3;
			SpellComponentData spellComponentData = item4;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item5;
			InternalCompilerInterface.UncheckedRefRW<PhysicsVelocity> uncheckedRefRW4 = item6;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW5 = item7;
			Entity entity2 = entity;
			enabledRefRW.ValueRW = false;
			uncheckedRefRW2.ValueRO.ColorType.ColorEnumToString(out var result);
			if (uncheckedRefRW2.ValueRO.Int3 != 0)
			{
				if (!InternalCompilerInterface.HasBufferAfterCompletingDependency(ref __TypeHandle.__Spell1019BulletBuffer_RO_BufferLookup, ref state, spellComponentData.Shooter))
				{
					continue;
				}
				ref Spell1019LastShootEntityData valueRW = ref InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell1019LastShootEntityData_RW_ComponentLookup, ref state, spellComponentData.Shooter).ValueRW;
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, valueRW.lastShootEntity))
				{
					Entity e = entityCommandBuffer.Instantiate(singleton.Prefabs["1019_LineData"]);
					GameObject gO = ObjPoolMgr.Inst.GetGO(string.Format("{0}1019/1019_Line_{1}", "Prefabs/Spell/", result));
					LineRenderer component = gO.transform.Find("Line").GetComponent<LineRenderer>();
					LineRenderer component2 = gO.transform.Find("Shadow").GetComponent<LineRenderer>();
					float num3 = (component2.widthMultiplier = (component.widthMultiplier = math.max(0.0001f, math.abs(InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, spellComponentData.Shooter).Scale))));
					component.positionCount = 2;
					component2.positionCount = 2;
					component.SetPosition(0, new float3(0f, 0f, 0f));
					component.SetPosition(1, new float3(0f, 0f, 0f));
					component2.SetPosition(0, new float3(0f, 0f, 0f));
					component2.SetPosition(1, new float3(0f, 0f, 0f));
					entityCommandBuffer.SetComponent(e, new Spell1019LineData
					{
						StartEntity = valueRW.lastShootEntity,
						EndEntity = entity2,
						LineRenderer = component,
						LineShadowRenderer = component2
					});
				}
				valueRW.lastShootEntity = entity2;
				entityCommandBuffer.AppendToBuffer(spellComponentData.Shooter, new Spell1019BulletBuffer
				{
					Entity = entity2
				});
				entityCommandBuffer.SetComponent(entity2, new Spell1019BulletData
				{
					ShootEntity = spellComponentData.Shooter
				});
				entityCommandBuffer.SetComponentEnabled<Spell1019BulletData>(entity2, value: true);
				uncheckedRefRW3.ValueRW.Position = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref state, spellComponentData.Shooter).Position;
				uncheckedRefRW2.ValueRW.Damage.MulRatio *= uncheckedRefRW2.ValueRW.DamageInterval;
				uncheckedRefRW2.ValueRW.DamageInterval = 1f;
				uncheckedRefRW5.ValueRW.AroundCenter = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, spellComponentData.Shooter).AroundCenter;
				uncheckedRefRW5.ValueRW.ChaseOwnerPosition = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, spellComponentData.Shooter).ChaseOwnerPosition;
				uncheckedRefRW5.ValueRW.AroundTarget = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellMovementComponentData_RO_ComponentLookup, ref state, spellComponentData.Shooter).AroundTarget;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RW_ComponentLookup, ref state, entity2).ValueRW.Shooter = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__SpellComponentData_RO_ComponentLookup, ref state, spellComponentData.Shooter).Shooter;
				if (uncheckedRefRW5.ValueRO.Type == SpellSpecialMovementType.Rotation)
				{
					uncheckedRefRW3.ValueRW.Position = uncheckedRefRW5.ValueRW.UpdateAroundFollowAndGetAroundPositionWhenAround(componentLookup);
					if (!spellComponentData.IsSplitSpell)
					{
						uncheckedRefRW3.ValueRW.Position.z = (uncheckedRefRW5.ValueRW.IsFallSpell ? (-7f) : (-0.3f));
					}
				}
			}
			else
			{
				uncheckedRefRW4.ValueRW.Linear = float3.zero;
				uncheckedRefRW.ValueRW.StartObj = ObjPoolMgr.Inst.GetGO(string.Format("{0}1019/1019_Start_{1}", "Prefabs/Spell/", result));
				uncheckedRefRW.ValueRW.StartObj.Value.transform.localScale = new float3(1f, 1f, 1f) * uncheckedRefRW3.ValueRO.Scale * 1.5f;
				uncheckedRefRW.ValueRW.StartSpeed = uncheckedRefRW5.ValueRO.Speed;
				if (uncheckedRefRW5.ValueRO.IsFallSpell)
				{
					quaternion fallEffectRotation = DTool.GetFallEffectRotation(in uncheckedRefRW5.ValueRO);
					uncheckedRefRW.ValueRW.StartObj.Value.transform.rotation = fallEffectRotation;
					uncheckedRefRW5.ValueRW.CurrentFallSpeed = 0f;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsVelocity_RW_ComponentLookup, ref state, entity2).ValueRW.Linear = float3.zero;
				}
				entityCommandBuffer.SetComponentEnabled<Spell1019LastShootEntityData>(entity2, value: true);
				entityCommandBuffer.AddBuffer<Spell1019BulletBuffer>(entity2);
				if (uncheckedRefRW2.ValueRO.HoverDuration > 0f)
				{
					uncheckedRefRW2.ValueRW.Duration.Extra += uncheckedRefRW2.ValueRO.HoverDuration;
					uncheckedRefRW2.ValueRW.HoverDuration = 0f;
				}
				if (uncheckedRefRW5.ValueRO.Type != SpellSpecialMovementType.Rotation && (uncheckedRefRW5.ValueRO.IsFallSpell || (!InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RO_ComponentLookup, ref state, spellComponentData.Shooter) && !InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__PlayerController_Dots_RO_ComponentLookup, ref state, spellComponentData.Shooter))))
				{
					uncheckedRefRW.ValueRW.StopFollowShooter = true;
					uncheckedRefRW5.ValueRW.Speed = 0f;
				}
				CompoundCollider* colliderPtr = (CompoundCollider*)InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Physics_PhysicsCollider_RW_ComponentLookup, ref state, entity2).ValueRW.ColliderPtr;
				colliderPtr->Children[0].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
				colliderPtr->Children[1].Collider->SetCollisionResponse(CollisionResponsePolicy.None);
				entityCommandBuffer.AppendToBuffer(singletonEntity, new SpellEffectSystem.Require
				{
					Settings = singleton.Effects[1019]["LoopAudio"],
					Color = result,
					SpellId = 1019,
					Entity = entity2
				});
			}
		}
		entityCommandBuffer.Playback(state.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1019HighPressureData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsVelocity>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell1019InitializedTag>();
		__query_1395995569_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1395995569_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellEffectSystem.Require>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1395995569_2 = entityQueryBuilder2.Build(ref state);
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
		((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnCreate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnUpdate(IntPtr self, IntPtr state)
	{
		((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnUpdate(ref *(SystemState*)state.ToPointer());
	}

	[Unity.Entities.MonoPInvokeCallback(typeof(SystemBaseDelegates.Function))]
	internal unsafe static void __codegen__OnCreateForCompiler(IntPtr self, IntPtr state)
	{
		((Spell1019HighPressureInitializedSystem*)self.ToPointer())->OnCreateForCompiler(ref *(SystemState*)state.ToPointer());
	}
}
