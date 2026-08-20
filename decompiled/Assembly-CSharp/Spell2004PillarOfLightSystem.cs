using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[BurstCompile]
[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
internal class Spell2004PillarOfLightSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1609050686_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr item5_IntPtr;

			public IntPtr item6_IntPtr;

			public EnabledMask item7_EnabledMask;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, EnabledRefRO<Spell2004PillarInitializeTag>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, EnabledRefRO<Spell2004PillarInitializeTag>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<Spell2004PillarOfLightData>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellMovementComponentData>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<TeammateData>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpellConfigComponentData>(item5_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitProperty_Dots>(item6_IntPtr, index), item7_EnabledMask.GetEnabledRefRO<Spell2004PillarInitializeTag>(index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<Spell2004PillarOfLightData> item1_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellMovementComponentData> item2_ComponentTypeHandle_RW;

			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<TeammateData> item4_ComponentTypeHandle_RW;

			private ComponentTypeHandle<SpellConfigComponentData> item5_ComponentTypeHandle_RW;

			private ComponentTypeHandle<UnitProperty_Dots> item6_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<Spell2004PillarInitializeTag> item7_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<Spell2004PillarOfLightData>();
				item2_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellMovementComponentData>();
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<LocalTransform>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<TeammateData>();
				item5_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpellConfigComponentData>();
				item6_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitProperty_Dots>();
				item7_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<Spell2004PillarInitializeTag>(isReadOnly: true);
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
				item7_ComponentTypeHandle_RO.Update(ref systemState);
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
				result.item7_EnabledMask = archetypeChunk.GetEnabledMask(ref item7_ComponentTypeHandle_RO);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, EnabledRefRO<Spell2004PillarInitializeTag>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, EnabledRefRO<Spell2004PillarInitializeTag>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<Spell2004PillarOfLightData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellMovementComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<TeammateData>();
			state.EntityManager.CompleteDependencyBeforeRW<SpellConfigComponentData>();
			state.EntityManager.CompleteDependencyBeforeRW<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<Spell2004PillarInitializeTag>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1609050686_0.TypeHandle __IFE_1609050686_0_TypeHandle;

		[ReadOnly]
		public ComponentLookup<EffectsCollectorData> __EffectsCollectorData_RO_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<SpellConfigComponentData> __SpellConfigComponentData_RW_ComponentLookup;

		public Spell2004Job.InternalCompilerQueryAndHandleData __Spell2004Job_WithDefaultQuery_JobEntityTypeHandle;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1609050686_0_TypeHandle = new IFE_1609050686_0.TypeHandle(ref state);
			__EffectsCollectorData_RO_ComponentLookup = state.GetComponentLookup<EffectsCollectorData>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__SpellConfigComponentData_RW_ComponentLookup = state.GetComponentLookup<SpellConfigComponentData>();
			__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.Init(ref state, assignDefaultQuery: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1609050686_0;

	private EntityQuery __query_1609050686_1;

	private EntityQuery __query_1609050686_2;

	private EntityQuery __query_1609050686_3;

	private EntityQuery __query_1609050686_4;

	private EntityQuery __query_1609050686_5;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		RequireForUpdate<PhysicsWorldSingleton>();
		RequireForUpdate<SEData>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<SpellEffectSystem.UnfollowingRequire>();
		RequireForUpdate<GlobalParticleEmitParams>();
		RequireForUpdate<SpellSingleton>();
		RequireForUpdate<Spell2004PillarOfLightData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer CMD = new EntityCommandBuffer(Allocator.TempJob);
		SpellSingleton singleton = __query_1609050686_1.GetSingleton<SpellSingleton>();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData>, InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData>, InternalCompilerInterface.UncheckedRefRW<LocalTransform>, InternalCompilerInterface.UncheckedRefRW<TeammateData>, InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData>, InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots>, EnabledRefRO<Spell2004PillarInitializeTag>> item8 in IFE_1609050686_0.Query(__query_1609050686_0, __TypeHandle.__IFE_1609050686_0_TypeHandle, ref base.CheckedStateRef))
		{
			item8.Deconstruct(out var item, out var item2, out var item3, out var item4, out var item5, out var item6, out var _, out var entity);
			InternalCompilerInterface.UncheckedRefRW<Spell2004PillarOfLightData> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRW<SpellMovementComponentData> uncheckedRefRW2 = item2;
			InternalCompilerInterface.UncheckedRefRW<LocalTransform> uncheckedRefRW3 = item3;
			InternalCompilerInterface.UncheckedRefRW<TeammateData> uncheckedRefRW4 = item4;
			InternalCompilerInterface.UncheckedRefRW<SpellConfigComponentData> uncheckedRefRW5 = item5;
			InternalCompilerInterface.UncheckedRefRW<UnitProperty_Dots> uncheckedRefRW6 = item6;
			Entity e = entity;
			int num = uncheckedRefRW4.ValueRO.TeammateCurrentFuseLevel + 1;
			uncheckedRefRW6.ValueRW.id = 700400 + uncheckedRefRW5.ValueRO.Level;
			uncheckedRefRW.ValueRW.CurrentFloatingLerpSpeed = 15f;
			if (uncheckedRefRW2.ValueRO.Type == SpellSpecialMovementType.ChaseMouse)
			{
				uncheckedRefRW2.ValueRW.ChaseMouseLerpSpeed = 0.4f;
			}
			uncheckedRefRW5.ValueRO.ColorType.ColorEnumToString(out var result);
			FixedString32Bytes fixedString32Bytes = ((uncheckedRefRW5.ValueRO.Level > 1) ? " 1" : "");
			for (int i = 0; i < num; i++)
			{
				Entity entity2 = base.EntityManager.Instantiate(singleton.Prefabs[$"2004_FusePillar_{result}{fixedString32Bytes}"]);
				base.EntityManager.SetComponentData(entity2, new LocalTransform
				{
					Position = float3.zero,
					Scale = uncheckedRefRW3.ValueRO.Scale
				});
				EffectsCollectorData componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref base.CheckedStateRef, entity2);
				if (uncheckedRefRW4.ValueRO.AdvanceSkillLevel > 0)
				{
					Entity effect = componentAfterCompletingDependency.Effect1;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, effect).ValueRW.Position = new float3(0f, 0.6f, 0f);
				}
				CMD.AppendToBuffer(e, new LinkedEntityGroup
				{
					Value = entity2
				});
				CMD.AppendToBuffer(e, new UnitMREttBED
				{
					ett = componentAfterCompletingDependency.Effect3
				});
				CMD.AddComponent<Spell2004HpRatioMaterialProperty>(componentAfterCompletingDependency.Effect3);
				CMD.SetComponent(componentAfterCompletingDependency.Effect3, new Spell2004HpRatioMaterialProperty
				{
					Value = uncheckedRefRW6.ValueRO.unitCfg.currentHP / uncheckedRefRW6.ValueRO.unitCfg.maxHP
				});
				if (uncheckedRefRW5.ValueRO.ColorType == SpellColorType.Fire || uncheckedRefRW5.ValueRO.ColorType == SpellColorType.Void)
				{
					CMD.AddComponent<Spell2004HpRatioMaterialProperty>(componentAfterCompletingDependency.Effect5);
					CMD.SetComponent(componentAfterCompletingDependency.Effect5, new Spell2004HpRatioMaterialProperty
					{
						Value = uncheckedRefRW6.ValueRO.unitCfg.currentHP / uncheckedRefRW6.ValueRO.unitCfg.maxHP
					});
				}
				CMD.AppendToBuffer(e, new Spell2004PillarBuffer
				{
					Entity = entity2
				});
				if ((num == 2 && i == 1) || num == 1)
				{
					break;
				}
				LineRenderer component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/2004/2004_FuseWallShadow", float3.zero, quaternion.identity).transform.GetComponent<LineRenderer>();
				Entity entity3 = base.EntityManager.Instantiate(singleton.Prefabs[$"2004_FuseWall_{result}"]);
				EffectsCollectorData componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__EffectsCollectorData_RO_ComponentLookup, ref base.CheckedStateRef, entity3);
				base.EntityManager.SetComponentData(entity3, new LocalTransform
				{
					Position = float3.zero,
					Scale = uncheckedRefRW3.ValueRO.Scale
				});
				CMD.AppendToBuffer(e, new LinkedEntityGroup
				{
					Value = entity3
				});
				CMD.AppendToBuffer(e, new UnitMREttBED
				{
					ett = componentAfterCompletingDependency2.Effect3
				});
				CMD.AppendToBuffer(e, new UnitMREttBED
				{
					ett = componentAfterCompletingDependency2.Effect4
				});
				CMD.AppendToBuffer(e, new Spell2004WallBuffer
				{
					Entity = entity3,
					LineRenderer = component
				});
				CMD.AddComponent(entity3, new Spell2004LineRenderCleanUpData
				{
					LineRenderer = component
				});
				if (uncheckedRefRW4.ValueRO.AdvanceSkillLevel > 0)
				{
					Entity effect2 = componentAfterCompletingDependency2.Effect1;
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, effect2).ValueRW.Position = new float3(0f, 0.6f, 0f);
				}
				CMD.AddComponent<Spell2004RotateAngleMaterialProperty>(componentAfterCompletingDependency2.Effect3);
				CMD.AddComponent<Spell2004HpRatioMaterialProperty>(componentAfterCompletingDependency2.Effect3);
				CMD.SetComponent(componentAfterCompletingDependency2.Effect3, new Spell2004HpRatioMaterialProperty
				{
					Value = uncheckedRefRW6.ValueRO.unitCfg.currentHP / uncheckedRefRW6.ValueRO.unitCfg.maxHP
				});
				CMD.AddComponent<Spell2004RotateAngleMaterialProperty>(componentAfterCompletingDependency2.Effect4);
				CMD.AddComponent<Spell2004HpRatioMaterialProperty>(componentAfterCompletingDependency2.Effect4);
				CMD.SetComponent(componentAfterCompletingDependency2.Effect4, new Spell2004HpRatioMaterialProperty
				{
					Value = uncheckedRefRW6.ValueRO.unitCfg.currentHP / uncheckedRefRW6.ValueRO.unitCfg.maxHP
				});
			}
			CreateHitBox(e, num, ref CMD, uncheckedRefRW2.ValueRO.Type == SpellSpecialMovementType.Rotation);
			CMD.SetComponentEnabled<Spell2004PillarInitializeTag>(e, value: false);
		}
		CMD.Playback(base.World.EntityManager);
		CMD.Dispose();
		base.Dependency = __ScheduleViaJobChunkExtension_0(new Spell2004Job
		{
			unitLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef),
			DeltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime,
			Singleton = __query_1609050686_2.GetSingleton<PhysicsWorldSingleton>(),
			configLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__SpellConfigComponentData_RW_ComponentLookup, ref base.CheckedStateRef),
			CMD = __query_1609050686_3.GetSingleton<EndSpellSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(base.World.Unmanaged).AsParallelWriter(),
			SEData = __query_1609050686_4.GetSingletonEntity(),
			GlobalParticle = __query_1609050686_5.GetSingletonEntity(),
			transformLookup = InternalCompilerInterface.GetComponentLookup(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef)
		}, __TypeHandle.__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.DefaultQuery, base.Dependency, ref base.CheckedStateRef, hasUserDefinedQuery: false);
	}

	private float GetPillarDistanceToCenterPoint(int totalPillarCount)
	{
		return totalPillarCount switch
		{
			1 => 0f, 
			2 => 1f, 
			_ => 1f / math.sin(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f)), 
		};
	}

	private float GetWallDistanceToCenterPoint(float toPillarDistance, int totalPillarCount)
	{
		if (totalPillarCount == 2)
		{
			return toPillarDistance;
		}
		return toPillarDistance * math.cos(360f / (float)totalPillarCount / 2f * (MathF.PI / 180f));
	}

	private void CreateHitBox(Entity e, int colliderCount, ref EntityCommandBuffer CMD, bool IsRotation)
	{
		int num = ((colliderCount <= 2) ? 2 : (colliderCount + 1));
		NativeArray<CompoundCollider.ColliderBlobInstance> children = new NativeArray<CompoundCollider.ColliderBlobInstance>(num, Allocator.Temp);
		uint belongsTo = 512u;
		uint collidesWith = 1216362496u;
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = belongsTo;
		collisionFilter.CollidesWith = collidesWith;
		collisionFilter.GroupIndex = -1;
		CollisionFilter filter = collisionFilter;
		switch (colliderCount)
		{
		case 1:
		{
			Unity.Physics.Material default2 = Unity.Physics.Material.Default;
			default2.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;
			children[0] = new CompoundCollider.ColliderBlobInstance
			{
				Collider = Unity.Physics.CapsuleCollider.Create(new CapsuleGeometry
				{
					Radius = 0.25f,
					Vertex0 = new float3(0f, 0f, -5f),
					Vertex1 = new float3(0f, 0f, 5f)
				}, filter, default2),
				CompoundFromChild = RigidTransform.identity
			};
			break;
		}
		case 2:
		{
			Unity.Physics.Material default3 = Unity.Physics.Material.Default;
			default3.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;
			float3 dir4 = DTool.GetDir(0f);
			CompoundCollider.ColliderBlobInstance value = default(CompoundCollider.ColliderBlobInstance);
			BoxGeometry geometry = new BoxGeometry
			{
				Center = float3.zero,
				Size = new float3(2.56f, 0.5f, 5f)
			};
			float2 dir3 = dir4.xy;
			geometry.Orientation = DTool.DirectionToRotation(in dir3);
			value.Collider = Unity.Physics.BoxCollider.Create(geometry, filter, default3);
			value.CompoundFromChild = RigidTransform.identity;
			children[0] = value;
			break;
		}
		default:
		{
			float pillarDistanceToCenterPoint = GetPillarDistanceToCenterPoint(colliderCount);
			float num2 = ((colliderCount > 2) ? GetWallDistanceToCenterPoint(pillarDistanceToCenterPoint, colliderCount) : 0f);
			float num3 = 360f / (float)colliderCount;
			float num4 = num3 / 2f;
			for (int i = 0; i < colliderCount; i++)
			{
				float num5 = num3 * (float)i + num4;
				float3 dir = DTool.GetDir(num5 * (MathF.PI / 180f));
				float3 dir2 = DTool.GetDir((num5 + 90f) * (MathF.PI / 180f));
				Unity.Physics.Material @default = Unity.Physics.Material.Default;
				@default.CollisionResponse = CollisionResponsePolicy.RaiseTriggerEvents;
				int index = i;
				CompoundCollider.ColliderBlobInstance value = default(CompoundCollider.ColliderBlobInstance);
				BoxGeometry geometry = new BoxGeometry
				{
					Center = dir * num2,
					Size = new float3(2.56f, 0.5f, 5f)
				};
				float2 dir3 = dir2.xy;
				geometry.Orientation = DTool.DirectionToRotation(in dir3);
				value.Collider = Unity.Physics.BoxCollider.Create(geometry, filter, @default);
				value.CompoundFromChild = RigidTransform.identity;
				children[index] = value;
			}
			break;
		}
		}
		Unity.Physics.Material default4 = Unity.Physics.Material.Default;
		default4.CollisionResponse = ((!IsRotation) ? CollisionResponsePolicy.CollideRaiseCollisionEvents : CollisionResponsePolicy.None);
		children[num - 1] = new CompoundCollider.ColliderBlobInstance
		{
			Collider = Unity.Physics.CapsuleCollider.Create(new CapsuleGeometry
			{
				Radius = 0.25f,
				Vertex0 = new float3(0f, 0f, -5f),
				Vertex1 = new float3(0f, 0f, 5f)
			}, new CollisionFilter
			{
				BelongsTo = 512u,
				CollidesWith = 65792u,
				GroupIndex = -1
			}, default4),
			CompoundFromChild = RigidTransform.identity
		};
		BlobAssetReference<Unity.Physics.Collider> value2 = CompoundCollider.Create(children);
		CMD.AddComponent(e, new PhysicsCollider
		{
			Value = value2
		});
		children.Dispose();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private JobHandle __ScheduleViaJobChunkExtension_0(Spell2004Job job, EntityQuery query, JobHandle dependency, ref SystemState state, bool hasUserDefinedQuery)
	{
		dependency = __TypeHandle.__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.UpdateBaseEntityIndexArray(ref job, query, dependency, ref state);
		__TypeHandle.__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.AssignEntityManager(ref job, state.EntityManager);
		__TypeHandle.__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.__TypeHandle.Update(ref state);
		return __TypeHandle.__Spell2004Job_WithDefaultQuery_JobEntityTypeHandle.ScheduleParallel(ref job, query, dependency);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell2004PillarInitializeTag>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<Spell2004PillarOfLightData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellMovementComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<TeammateData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpellConfigComponentData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitProperty_Dots>();
		__query_1609050686_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SpellSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1609050686_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1609050686_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSpellSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1609050686_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<SEData>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1609050686_4 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1609050686_5 = entityQueryBuilder2.Build(ref state);
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
	public Spell2004PillarOfLightSystem()
	{
	}
}
