using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PlayerLogger;
using PlayerLogger.Events;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(UnitTakeDamageGroup))]
[CompilerGenerated]
[UpdateAfter(typeof(UnitAfterTakeDamageSystem))]
[UpdateBefore(typeof(SpellTakeDamageResultSystem))]
public class UnitTakeDamageDeadSystem : SystemBase
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct IFE_1916192749_0
	{
		public struct ResolvedChunk
		{
			public IntPtr item1_IntPtr;

			public IntPtr item2_IntPtr;

			public IntPtr item3_IntPtr;

			public IntPtr Entity_IntPtr;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitDead>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Get(int index)
			{
				return new QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitDead>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>(InternalCompilerInterface.UnsafeGetUncheckedRefRW<UnitDead>(item1_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<UnitProperty_Dots>(item2_IntPtr, index), InternalCompilerInterface.UnsafeGetUncheckedRefRO<LocalTransform>(item3_IntPtr, index), InternalCompilerInterface.UnsafeGetCopyOfNativeArrayPtrElement<Entity>(Entity_IntPtr, index));
			}
		}

		public struct TypeHandle
		{
			private ComponentTypeHandle<UnitDead> item1_ComponentTypeHandle_RW;

			[ReadOnly]
			private ComponentTypeHandle<UnitProperty_Dots> item2_ComponentTypeHandle_RO;

			[ReadOnly]
			private ComponentTypeHandle<LocalTransform> item3_ComponentTypeHandle_RO;

			private EntityTypeHandle Entity_TypeHandle;

			public TypeHandle(ref SystemState systemState)
			{
				item1_ComponentTypeHandle_RW = systemState.GetComponentTypeHandle<UnitDead>();
				item2_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<UnitProperty_Dots>(isReadOnly: true);
				item3_ComponentTypeHandle_RO = systemState.GetComponentTypeHandle<LocalTransform>(isReadOnly: true);
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

		public struct Enumerator : IEnumerator<QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitDead>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>>>, IEnumerator, IDisposable
		{
			private InternalEntityQueryEnumerator _entityQueryEnumerator;

			private TypeHandle _typeHandle;

			private ResolvedChunk _resolvedChunk;

			private int _currentEntityIndex;

			private int _endEntityIndex;

			public QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitDead>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> Current => _resolvedChunk.Get(_currentEntityIndex);

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
			state.EntityManager.CompleteDependencyBeforeRW<UnitDead>();
			state.EntityManager.CompleteDependencyBeforeRO<UnitProperty_Dots>();
			state.EntityManager.CompleteDependencyBeforeRO<LocalTransform>();
		}
	}

	private struct TypeHandle
	{
		public IFE_1916192749_0.TypeHandle __IFE_1916192749_0_TypeHandle;

		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		public BufferLookup<TakeDamageInfo_Dots> __TakeDamageInfo_Dots_RW_BufferLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__IFE_1916192749_0_TypeHandle = new IFE_1916192749_0.TypeHandle(ref state);
			__TeammateDeadTag_RW_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__TakeDamageInfo_Dots_RW_BufferLookup = state.GetBufferLookup<TakeDamageInfo_Dots>();
		}
	}

	private List<UnitProperty> _deadPpts = new List<UnitProperty>();

	private List<TakeDamageInfo_Dots> _deadInfos = new List<TakeDamageInfo_Dots>();

	private List<int> _deadInfoIndex = new List<int>();

	private List<Entity> deadEntities = new List<Entity>();

	private TypeHandle __TypeHandle;

	private EntityQuery __query_1916192749_0;

	private EntityQuery __query_1916192749_1;

	private EntityQuery __query_1916192749_2;

	private EntityQuery __query_1916192749_3;

	private EntityQuery __query_1916192749_4;

	[Preserve]
	protected unsafe override void OnUpdate()
	{
		EntityCommandBuffer CMD = __query_1916192749_1.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.DefaultGameObjectInjectionWorld.Unmanaged);
		Entity singletonEntity = __query_1916192749_2.GetSingletonEntity();
		Entity singletonEntity2 = __query_1916192749_3.GetSingletonEntity();
		if (_deadPpts.Count > 0)
		{
			_deadPpts.Clear();
			_deadInfos.Clear();
			_deadInfoIndex.Clear();
		}
		if (deadEntities.Count > 0)
		{
			deadEntities.Clear();
		}
		foreach (QueryEnumerableWithEntity<InternalCompilerInterface.UncheckedRefRW<UnitDead>, InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots>, InternalCompilerInterface.UncheckedRefRO<LocalTransform>> item4 in IFE_1916192749_0.Query(__query_1916192749_0, __TypeHandle.__IFE_1916192749_0_TypeHandle, ref base.CheckedStateRef))
		{
			item4.Deconstruct(out var item, out var item2, out var item3, out var entity);
			InternalCompilerInterface.UncheckedRefRW<UnitDead> uncheckedRefRW = item;
			InternalCompilerInterface.UncheckedRefRO<UnitProperty_Dots> uncheckedRefRO = item2;
			InternalCompilerInterface.UncheckedRefRO<LocalTransform> uncheckedRefRO2 = item3;
			Entity entity2 = entity;
			if (!uncheckedRefRO.ValueRO.isDead)
			{
				continue;
			}
			UnitProperty_Dots valueRO = uncheckedRefRO.ValueRO;
			ref UnitDead valueRW = ref uncheckedRefRW.ValueRW;
			LocalTransform valueRO2 = uncheckedRefRO2.ValueRO;
			if (valueRW.playerFinallyDead)
			{
				continue;
			}
			if (valueRO.unitCfg.unitType == UnitType.Teammate || valueRO.unitCfg.unitType == UnitType.TeammateNotAttack)
			{
				InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(ref __TypeHandle.__TeammateDeadTag_RW_ComponentLookup, ref base.CheckedStateRef, entity2, value: true);
				continue;
			}
			if (valueRO.deadDropItemInfos.IsCreated && valueRO.deadDropItemInfos.Value.Length > 0)
			{
				QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, valueRO.deadDropItemInfos, Tool2D.GetNavMeshPointIngoreZ(valueRO2.Position), 0.2f);
			}
			if ((valueRO.unitCfg.triggerDeadEvent || Spell3129VoidExplosion.specialVoidExplosionTriggerableUnitIdList.Contains(valueRO.unitCfg.id)) && valueRO.voidExplosionData.InstantKillRatio > 0f)
			{
				CMD.ApplyVoidExplosion(singletonEntity, valueRO2.Position, valueRO.unitCfg.maxHP, valueRO.voidExplosionData, singletonEntity2);
			}
			UnitType unitType = valueRO.unitCfg.unitType;
			bool flag = (unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack) && DataMgr.settingData.FinalSummonTransparent <= 0.01f;
			if (!valueRW.deadlyInfo.dontCreateDeadEF && valueRO.unitCfg.deadEF != "" && !flag)
			{
				FixedString128Bytes deadEF = valueRO.unitCfg.deadEF;
				string path = "Prefabs/EF/" + deadEF.ToString();
				if (valueRO.unitCfg.deadEF == "EF_Dead_Blood" || valueRO.unitCfg.deadEF == "EF_Dead_Ghost")
				{
					float3 @float = new float3(UnityEngine.Random.Range(-0.1f, 0.1f), 0.3f + UnityEngine.Random.Range(-0.1f, 0.1f), 0f);
					FixedString32Bytes fs = default(FixedString32Bytes);
					FixedStringMethods.CopyFrom(ref fs, in valueRO.unitCfg.deadEF);
					FixedStringMethods.Append(ref fs, "G");
					float num = 0f;
					if (base.EntityManager.HasComponent(entity2, typeof(PhysicsCollider)))
					{
						PhysicsCollider componentData = base.EntityManager.GetComponentData<PhysicsCollider>(entity2);
						if (componentData.ColliderPtr->Type == ColliderType.Capsule)
						{
							num = ((Unity.Physics.CapsuleCollider*)componentData.ColliderPtr)->Geometry.Radius;
						}
					}
					CMD.AppendToBuffer(__query_1916192749_2.GetSingletonEntity(), new GlobalParticleEmitParams
					{
						Position = valueRO2.Position + @float,
						Size = Mathf.Max(1f, num * 2f + 0.3f),
						Name = fs,
						Type = GlobalParticleType.EF
					});
				}
				else
				{
					ObjPoolMgr.Inst.GetGO(path, valueRO2.Position, 5f);
				}
			}
			if (valueRO.unitCfg.deadPermanentEF.ToString() != null && valueRO.unitCfg.deadPermanentEF.ToString() != "")
			{
				ObjPoolMgr inst = ObjPoolMgr.Inst;
				FixedString128Bytes deadEF = valueRO.unitCfg.deadPermanentEF;
				inst.GetGO("Prefabs/EF/" + deadEF.ToString(), valueRO2.Position);
			}
			if (!valueRW.deadlyInfo.dontCreatebloodSplat && valueRO.unitCfg.bloodSplatSize > 0f)
			{
				DynamicBuffer<CreateBloodSplatRequest> singletonBuffer = __query_1916192749_4.GetSingletonBuffer<CreateBloodSplatRequest>();
				ref float3 knockbackForce = ref valueRW.deadlyInfo.knockbackForce;
				float3 f = Vector3.zero;
				if (DTool.IsTotallySame(in knockbackForce, in f))
				{
					singletonBuffer.Add(new CreateBloodSplatRequest
					{
						directional = false,
						point = Tool2D.IgnoreZPoint(valueRO2.Position),
						size = valueRO.unitCfg.bloodSplatSize
					});
				}
				else
				{
					singletonBuffer.Add(new CreateBloodSplatRequest
					{
						directional = true,
						point = Tool2D.IgnoreZPoint(valueRO2.Position),
						size = valueRO.unitCfg.bloodSplatSize,
						rotationZ = Tool2D.IgnoreZAngleWithSign(Vector3.up, valueRW.deadlyInfo.knockbackForce)
					});
				}
			}
			int num2 = valueRO.unitCfg.corpseCount;
			if (GameMgr.IsMobile_Static)
			{
				num2 = (int)((double)num2 * 0.5);
			}
			if (valueRO.unitCfg.corpseType != 0)
			{
				for (int i = 0; i < num2; i++)
				{
					CorpseSystem.Inst.CreateCorpse(valueRO.unitCfg.corpseType, valueRO2.Position, valueRW.deadlyInfo.knockbackForce);
				}
			}
			if (!valueRW.deadlyInfo.dontPlayDeadSE)
			{
				if (valueRO.unitCfg.isDeadSE3D)
				{
					valueRO.unitCfg.deadSEs.Value.PlaySE(valueRO2.Position);
				}
				else
				{
					valueRO.unitCfg.deadSEs.Value.PlaySE();
				}
			}
			switch (valueRO.unitCfg.unitType)
			{
			case UnitType.Player:
			{
				Debug.Log("Player Dead");
				DataMgr.selectedWorldData.deadCount++;
				TakeDamageInfo_Dots deadlyInfo = valueRW.deadlyInfo;
				if (deadlyInfo.attackerEntity != Entity.Null)
				{
					if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, deadlyInfo.attackerEntity))
					{
						UnitProperty_Dots componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, deadlyInfo.attackerEntity);
						Debug.Log("击杀者id" + componentAfterCompletingDependency.unitCfg.id);
						if (deadlyInfo.isUndifferDamage && componentAfterCompletingDependency.unitCfg.IsSameCamp(UnitType.Player))
						{
							SteamAchievementMgr.UnlockAndUpload(SteamAchievementType.KillSelf);
						}
					}
					else
					{
						Debug.Log("击杀者id已经不可考证");
					}
				}
				valueRW.playerFinallyDead = true;
				PlayerDeathLogger playerDeathLogger = new PlayerDeathLogger();
				playerDeathLogger.death_counter = DataMgr.selectedWorldData.deadCount;
				playerDeathLogger.equips = PlayerEquips.CreateAuto();
				playerDeathLogger.monsters = LevelMgr.Inst.CurrentRoomCtrller.MonsterPpts.Select((UnitProperty e) => e.unitCfg.id).ToList();
				playerDeathLogger.resources = ResourcesStatus.CreateAuto();
				playerDeathLogger.click_return_camp = valueRW.deadlyInfo.attackerType == AttackerType.FromUI;
				playerDeathLogger.Report();
				if (GameMgr.IsMobile_Static)
				{
					MobileMgr.inst.PluginActivity.UploadItemSnapshot(1);
				}
				EventMgr.PlayerDead?.Invoke();
				GameUISingletonMono<UIPlayerDead>.ShowInit();
				break;
			}
			case UnitType.Teammate:
			case UnitType.TeammateNotAttack:
				_ = valueRO.unitCfg.isHybirdUnit;
				LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(entity2);
				break;
			case UnitType.Monster:
				if (!valueRO.unitCfg.isHybirdUnit)
				{
					LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(entity2);
				}
				break;
			case UnitType.Elite:
			case UnitType.Boss:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				if (!valueRO.unitCfg.isHybirdUnit)
				{
					LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(entity2);
				}
				break;
			default:
				Debug.LogError(valueRO.unitCfg.unitType);
				break;
			}
			if (valueRO.unitCfg.isHybirdUnit)
			{
				_deadPpts.Add(base.EntityManager.GetComponentObject<UnitPptReference>(entity2).unitPpt);
				_deadInfos.Add(valueRW.deadlyInfo);
				_deadInfoIndex.Add(valueRW.deadlyInfoIndex);
			}
			else if (valueRO.unitCfg.unitType != 0 && valueRO.unitCfg.unitType != UnitType.Teammate)
			{
				CMD.DestroyEntity(entity2);
			}
			deadEntities.Add(entity2);
		}
		if (deadEntities.Count > 0)
		{
			for (int j = 0; j < deadEntities.Count; j++)
			{
				PlayerMgr.Inst.PlayerCtrller.AfterMonsterDead(deadEntities[j]);
			}
		}
		if (_deadPpts.Count <= 0)
		{
			return;
		}
		for (int k = 0; k < _deadPpts.Count; k++)
		{
			UnitProperty unitProperty = _deadPpts[k];
			Entity myEntity = unitProperty.myEntity;
			TakeDamageInfo_Dots elem = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
			ref TakeDamageInfo_Dots reference = ref elem;
			DynamicBuffer<TakeDamageInfo_Dots> bufferAfterCompletingDependency = InternalCompilerInterface.GetBufferAfterCompletingDependency(ref __TypeHandle.__TakeDamageInfo_Dots_RW_BufferLookup, ref base.CheckedStateRef, myEntity);
			if (bufferAfterCompletingDependency.Length > 0 && bufferAfterCompletingDependency.Length - 1 >= _deadInfoIndex[k])
			{
				reference = ref bufferAfterCompletingDependency.ElementAt(_deadInfoIndex[k]);
			}
			else
			{
				bufferAfterCompletingDependency.Add(elem);
				reference = ref bufferAfterCompletingDependency.ElementAt(bufferAfterCompletingDependency.Length - 1);
			}
			switch (unitProperty.unitCfg.unitType)
			{
			case UnitType.Monster:
				unitProperty.UnitBas.AfterDead(ref reference);
				unitProperty.gameObject.SetActive(value: false);
				LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(myEntity);
				break;
			case UnitType.Elite:
			case UnitType.Boss:
			case UnitType.WillAttack:
			case UnitType.NotAttack:
			case UnitType.Brittleness:
				unitProperty.UnitBas.AfterDead(ref reference);
				unitProperty.gameObject.SetActive(value: false);
				LevelMgr.Inst.CurrentRoomCtrller.UnitUnregister(myEntity);
				break;
			}
			DataMgr.selectedWorldData.GalleryUnitsDead(reference, InternalCompilerInterface.GetComponentROAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, myEntity).ValueRO);
			CMD.DestroyEntity(myEntity);
			unitProperty.AnnouncedDeath();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<LocalTransform>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAll<UnitProperty_Dots>();
		entityQueryBuilder2 = entityQueryBuilder2.WithAllRW<UnitDead>();
		__query_1916192749_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<EndSimulationEntityCommandBufferSystem.Singleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1916192749_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<GlobalParticleEmitParams>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1916192749_2 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<Spell3129DamageRequestBuffer>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1916192749_3 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAllRW<CreateBloodSplatRequest>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_1916192749_4 = entityQueryBuilder2.Build(ref state);
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
	public UnitTakeDamageDeadSystem()
	{
	}
}
