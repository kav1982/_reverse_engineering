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
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
public class SpecialObj46System : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1830818084_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr item4_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj46>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj46>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<SpecialObj46>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<LocalTransform>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<PhysicsCollider>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRW<InteractiveObj_Dots>(item4_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<SpecialObj46> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item2_ComponentTypeHandle_RO;

			private ComponentTypeHandle<PhysicsCollider> item3_ComponentTypeHandle_RW;

			private ComponentTypeHandle<InteractiveObj_Dots> item4_ComponentTypeHandle_RW;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<SpecialObj46>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
				item3_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<PhysicsCollider>();
				item4_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<InteractiveObj_Dots>();
				Entity_TypeHandle = systemState.GetEntityTypeHandle();
			}

			public void Update(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW.Update(ref systemState);
				item2_ComponentTypeHandle_RO.Update(ref systemState);
				item3_ComponentTypeHandle_RW.Update(ref systemState);
				item4_ComponentTypeHandle_RW.Update(ref systemState);
				Entity_TypeHandle.Update(ref systemState);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ResolvedChunk Resolve(ArchetypeChunk archetypeChunk)
			{
				ResolvedChunk result = default(ResolvedChunk);
				result.item1_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item1_ComponentTypeHandle_RW);
				result.item2_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayReadOnlyIntPtrWithoutChecks(in archetypeChunk, ref item2_ComponentTypeHandle_RO);
				result.item3_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item3_ComponentTypeHandle_RW);
				result.item4_IntPtr = InternalCompilerInterface.UnsafeGetChunkNativeArrayIntPtrWithoutChecks(in archetypeChunk, ref item4_ComponentTypeHandle_RW);
				result.Entity_IntPtr = InternalCompilerInterface.UnsafeGetChunkEntityArrayIntPtr(archetypeChunk, Entity_TypeHandle);
				return result;
			}
		}

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj46>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj46>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<SpecialObj46>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
			state.EntityManager.CompleteDependencyBeforeRW<PhysicsCollider>();
			state.EntityManager.CompleteDependencyBeforeRW<InteractiveObj_Dots>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1830818084_0.TypeHandle __IFE_1830818084_0_TypeHandle;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1830818084_0_TypeHandle = new IFE_1830818084_0.TypeHandle(ref state);
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
		}
	}

	public const string DaveVideoPrefix = "Dave";

	public const int DaveVideoCount = 3;

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1830818084_0;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<SpecialObj46>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer ecb = base.World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>().CreateCommandBuffer();
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<SpecialObj46>, LocalTransform, InternalCompilerInterface.UncheckedRefRW<PhysicsCollider>, InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots>> item6 in IFE_1830818084_0.Query(__query_1830818084_0, __TypeHandle.__IFE_1830818084_0_TypeHandle, ref base.CheckedStateRef))
		{
			item6.Deconstruct(out var item, out var item2, out var item3, out var item4, out var entity);
			InternalCompilerInterface.UncheckedRefRW<SpecialObj46> so46 = item;
			LocalTransform localTransform = item2;
			InternalCompilerInterface.UncheckedRefRW<PhysicsCollider> uncheckedRefRW = item3;
			InternalCompilerInterface.UncheckedRefRW<InteractiveObj_Dots> uncheckedRefRW2 = item4;
			Entity entity2 = entity;
			if (!so46.ValueRW.isInitialized)
			{
				so46.ValueRW.isInitialized = true;
				uncheckedRefRW.ValueRW.MakeUnique(in entity2, ecb);
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter1).ValueRW.Scale = 0f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter2).ValueRW.Scale = 0f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter3).ValueRW.Scale = 0f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter4).ValueRW.Scale = 0f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter5).ValueRW.Scale = 0f;
				if (BattleMgr.Inst.CurrentStage <= 2)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter1).ValueRW.Scale = 1f;
				}
				else if (BattleMgr.Inst.CurrentStage <= 4)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter2).ValueRW.Scale = 1f;
				}
				else if (BattleMgr.Inst.CurrentStage <= 6)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter3).ValueRW.Scale = 1f;
				}
				else if (BattleMgr.Inst.CurrentStage <= 8)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter4).ValueRW.Scale = 1f;
				}
				else
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Appearance_Chapter5).ValueRW.Scale = 1f;
				}
				so46.ValueRW.so46Mono = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/SO46Mono", localTransform.Position).GetComponent<SO46Mono>();
				Transform transform = so46.ValueRW.so46Mono.Value.transform;
				float3 @float = localTransform.Position + so46.ValueRW.npc8Offset;
				float3 rootPosition = localTransform.Position + so46.ValueRW.npc8Offset;
				transform.position = @float + DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
			}
			if (so46.ValueRW.forcePlayMusicWaitFrameTimer < 5)
			{
				so46.ValueRW.forcePlayMusicWaitFrameTimer++;
				if (so46.ValueRW.forcePlayMusicWaitFrameTimer == 4)
				{
					LevelMgr.Inst.CurrentRoomCtrller.roomCfg.overrideThemeMusic = "BGM_Spring_Dave";
					MusicMgr.Inst.ForcePlayMusic("BGM_Spring_Dave");
				}
			}
			if (uncheckedRefRW2.ValueRW.onSelect)
			{
				uncheckedRefRW2.ValueRW.onSelect = false;
				if (so46.ValueRW.isShowSusui)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_SushiOutline).ValueRW.Scale = 1f;
				}
				else
				{
					so46.ValueRW.so46Mono.Value.Selected();
				}
			}
			if (uncheckedRefRW2.ValueRW.onDeselect)
			{
				uncheckedRefRW2.ValueRW.onDeselect = false;
				if (so46.ValueRW.isShowSusui)
				{
					InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_SushiOutline).ValueRW.Scale = 0f;
				}
				else
				{
					so46.ValueRW.so46Mono.Value.UnSelected();
				}
			}
			if (!uncheckedRefRW2.ValueRW.onInteract)
			{
				continue;
			}
			uncheckedRefRW2.ValueRW.onInteract = false;
			if (so46.ValueRW.isShowSusui)
			{
				PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
				float num = (float)DataMgr.selectedWorldData.GetResearchValueConsiderActive(ResearchAbilityType.Spring) / 100f;
				float recoveryHP = playerPpt.unitCfg.maxHP * num;
				UnitDotsSyncSystem.UnitRecoveryHP(PlayerMgr.Inst.PlayerEtt, recoveryHP, base.EntityManager);
				SEMgr.Inst.so17Drink.PlaySE();
				DTool.SetCollider(in uncheckedRefRW.ValueRO, 512u);
				ecb.DestroyEntity(so46.ValueRW.ett_Sushi);
				continue;
			}
			if (DataMgr.selectedWorldData.IsDave)
			{
				if (!DataMgr.selectedWorldData.daveSpringTalk)
				{
					DataMgr.selectedWorldData.daveSpringTalk = true;
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShowCommon(302, UseSpring);
				}
				else
				{
					GameUISingletonMono<UIDialogueMgr>.Inst.HDShowCommon(303, UseSpring);
				}
				break;
			}
			UseSpring();
			void ShowSushi()
			{
				so46.ValueRW.isShowSusui = true;
				so46.ValueRW.so46Mono.Value.UnSelected();
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_Sushi).ValueRW.Scale = 1f;
				InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, so46.ValueRW.ett_SushiOutline).ValueRW.Scale = 1f;
			}
			void UseSpring()
			{
				string item5 = "Videos/Dave" + UnityEngine.Random.Range(1, 4);
				GameUISingletonMono<UIWhiteScreen>.ShowInit((0.2f, item5, 0.2f, true, "BGM_Spring_Dave"));
				GameUISingletonMono<UIWhiteScreen>.Inst.actionOnClose = ShowSushi;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<SpecialObj46>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<PhysicsCollider>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<InteractiveObj_Dots>();
		__query_1830818084_0 = entityQueryBuilder2.Build(ref state);
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
	public SpecialObj46System()
	{
	}
}
