using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Internal;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateInGroup(typeof(SpacialSpellSystemGroup))]
[CompilerGenerated]
public class Spell4005WandDataInitializeSystem : SystemBase
{
	private struct TypeHandle
	{
		public ComponentLookup<Spell4005WandSpiritData> __Spell4005WandSpiritData_RW_ComponentLookup;

		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RW_ComponentLookup;

		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RW_ComponentLookup;

		public ComponentLookup<PathFinding> __PathFinding_RW_ComponentLookup;

		public BufferLookup<TeammateOwnerInfoBuffer> __TeammateOwnerInfoBuffer_RW_BufferLookup;

		public ComponentLookup<TeammateDeadTag> __TeammateDeadTag_RW_ComponentLookup;

		public ComponentLookup<UnitBase_Dots> __UnitBase_Dots_RW_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<UnitProperty_Dots> __UnitProperty_Dots_RO_ComponentLookup;

		[ReadOnly]
		public ComponentLookup<LocalTransform> __Unity_Transforms_LocalTransform_RO_ComponentLookup;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void __AssignHandles(ref SystemState state)
		{
			__Spell4005WandSpiritData_RW_ComponentLookup = state.GetComponentLookup<Spell4005WandSpiritData>();
			__UnitProperty_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>();
			__Unity_Transforms_LocalTransform_RW_ComponentLookup = state.GetComponentLookup<LocalTransform>();
			__PathFinding_RW_ComponentLookup = state.GetComponentLookup<PathFinding>();
			__TeammateOwnerInfoBuffer_RW_BufferLookup = state.GetBufferLookup<TeammateOwnerInfoBuffer>();
			__TeammateDeadTag_RW_ComponentLookup = state.GetComponentLookup<TeammateDeadTag>();
			__UnitBase_Dots_RW_ComponentLookup = state.GetComponentLookup<UnitBase_Dots>();
			__UnitProperty_Dots_RO_ComponentLookup = state.GetComponentLookup<UnitProperty_Dots>(isReadOnly: true);
			__Unity_Transforms_LocalTransform_RO_ComponentLookup = state.GetComponentLookup<LocalTransform>(isReadOnly: true);
		}
	}

	private TypeHandle __TypeHandle;

	private EntityQuery __query_378481533_0;

	private EntityQuery __query_378481533_1;

	private EntityQuery __query_378481533_2;

	[Preserve]
	protected override void OnCreate()
	{
		RequireForUpdate<Spell4005WandSpiritData>();
	}

	[Preserve]
	protected override void OnUpdate()
	{
		float deltaTime = base.CheckedStateRef.WorldUnmanaged.Time.DeltaTime;
		CurrentRoomEntitiesSingleton currentRoomEntities = __query_378481533_0.GetSingleton<CurrentRoomEntitiesSingleton>();
		using (EntityQuery entityQuery = base.EntityManager.CreateEntityQuery(typeof(LocalTransform), typeof(Spell4005WandSpiritData), typeof(UnitProperty_Dots)))
		{
			NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			for (int i = 0; i < nativeArray.Length; i++)
			{
				RefRW<Spell4005WandSpiritData> wandSpiritData = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Spell4005WandSpiritData_RW_ComponentLookup, ref base.CheckedStateRef, nativeArray[i]);
				RefRW<UnitProperty_Dots> componentRWAfterCompletingDependency = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RW_ComponentLookup, ref base.CheckedStateRef, nativeArray[i]);
				RefRW<LocalTransform> trans = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RW_ComponentLookup, ref base.CheckedStateRef, nativeArray[i]);
				RefRW<PathFinding> componentRWAfterCompletingDependency2 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__PathFinding_RW_ComponentLookup, ref base.CheckedStateRef, nativeArray[i]);
				Entity entity = nativeArray[i];
				Wand wand = wandSpiritData.ValueRW.Wand.Value;
				if (wand == null || wand.passiveAutoWandShooterData == null || wand.passiveAutoWandShooterData.wandObjectScript == null)
				{
					continue;
				}
				if (wand.passiveAutoWandShooterData == null)
				{
					using (NativeArray<TeammateOwnerInfoBuffer>.Enumerator enumerator = InternalCompilerInterface.GetBufferLookup(ref __TypeHandle.__TeammateOwnerInfoBuffer_RW_BufferLookup, ref base.CheckedStateRef)[entity].GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							InternalCompilerInterface.SetComponentEnabledAfterCompletingDependency(entity: enumerator.Current.TeammateEntity, componentLookup: ref __TypeHandle.__TeammateDeadTag_RW_ComponentLookup, state: ref base.CheckedStateRef, value: true);
						}
					}
					continue;
				}
				RefRW<UnitBase_Dots> componentRWAfterCompletingDependency3 = InternalCompilerInterface.GetComponentRWAfterCompletingDependency(ref __TypeHandle.__UnitBase_Dots_RW_ComponentLookup, ref base.CheckedStateRef, entity);
				if (!wandSpiritData.ValueRW.IsInitialized)
				{
					componentRWAfterCompletingDependency.ValueRW.id = 705201;
					wandSpiritData.ValueRW.IsInitialized = true;
					componentRWAfterCompletingDependency.ValueRW.InvincibleRegister();
					wandSpiritData.ValueRW.LastFramePosition = trans.ValueRW.Position;
				}
				if (!GeneralTool.IsPlayerCanMotion())
				{
					componentRWAfterCompletingDependency3.ValueRW.SetMove(float3.zero);
					continue;
				}
				Teammate52 wandSpiritScript = wand.passiveAutoWandShooterData.wandObjectScript;
				wandSpiritData.ValueRW.AttackDurationTimer -= deltaTime;
				float num = 1f + wand.passiveOwnerSpeedUpRatio;
				float num2 = componentRWAfterCompletingDependency.ValueRW.unitCfg.moveSpeed * num;
				RecheckValidTarget();
				float num3 = ((wand.shootGroups.Count <= 0 && wand.passiveRuneHammerEnable) ? 10f : 4f);
				HammerAttackRangeInWandSingleton comp = __query_378481533_1.GetSingleton<HammerAttackRangeInWandSingleton>();
				comp.TryGetWandHammerAttackRange(wand, out var range);
				float num4 = 0f;
				float num5 = 0f;
				if (wand.passiveRuneHammerEnable)
				{
					if (wand.shootGroups.Count > 0)
					{
						num4 = wandSpiritScript.attackDistance;
						num5 = math.min(wandSpiritScript.attackDistance, range);
					}
					else
					{
						num4 = range;
						num5 = range;
					}
				}
				else
				{
					num4 = wandSpiritScript.attackDistance;
					num5 = wandSpiritScript.attackDistance;
				}
				if (InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRO.ChaseTarget))
				{
					num4 += InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__UnitProperty_Dots_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRO.ChaseTarget).size / 2f;
				}
				if (IsSpecificUIOpen())
				{
					componentRWAfterCompletingDependency3.ValueRW.SetMove(float3.zero);
				}
				switch (wandSpiritData.ValueRW.State)
				{
				case WandSpiritState.Idle:
					EnterState(WandSpiritState.rotateAroundPlayer);
					wandSpiritData.ValueRW.WandLookDirection = new float3(0f, 1f, 0f);
					break;
				case WandSpiritState.rotateAroundPlayer:
				{
					if (PlayerMgr.Inst.PlayerCtrller.CanMotion)
					{
						componentRWAfterCompletingDependency2.ValueRW.UpdatePath(trans.ValueRO.Position, GetRotatePoint(wandSpiritScript), componentRWAfterCompletingDependency.ValueRO.navAreaMask);
						componentRWAfterCompletingDependency3.ValueRW.SetMove(Tool2D.IgnoreZPoint(componentRWAfterCompletingDependency2.ValueRO.walkToPoint - trans.ValueRO.Position).normalized * num2 * math.min(deltaTime, 0.04f) * 15f * wandSpiritData.ValueRW.SpiritRotateAroundPlayerLerpRatio);
					}
					wandSpiritData.ValueRW.RotateAroundPlayerFloatingTimer += deltaTime * 225f;
					wandSpiritData.ValueRW.RotateAroundPlayerFloatingAngle = math.sin(wandSpiritData.ValueRW.RotateAroundPlayerFloatingTimer * (MathF.PI / 180f)) * 12f;
					float3 point = new float3(0f, 1f, 0f);
					float3 target = DTool.GetDir(in point, wandSpiritData.ValueRW.RotateAroundPlayerFloatingAngle);
					wandSpiritData.ValueRW.WandLookDirection = DTool.DirMoveTowardsIgnoreZ(in wandSpiritData.ValueRW.WandLookDirection, in target, 100f * deltaTime);
					ref readonly float3 position = ref trans.ValueRO.Position;
					point = GetRotatePoint(wandSpiritScript);
					if (DTool.IgnoreZDistance(in position, in point) >= 2f)
					{
						wandSpiritData.ValueRW.SpiritRotateAroundPlayerLerpRatio += 0.5f * deltaTime;
					}
					else
					{
						wandSpiritData.ValueRW.SpiritRotateAroundPlayerLerpRatio -= 1.5f * deltaTime;
					}
					wandSpiritData.ValueRW.SpiritRotateAroundPlayerLerpRatio = math.clamp(wandSpiritData.ValueRW.SpiritRotateAroundPlayerLerpRatio, 1f, 2f);
					if ((wand.shootGroups.Count > 0 || wand.postSlotShootGroups.Count > 0 || (wand.passiveRuneHammerEnable && wand.shootGroups.Count <= 0)) && wandSpiritData.ValueRW.ChaseTarget != Entity.Null && base.EntityManager.HasComponent<LocalTransform>(wandSpiritData.ValueRW.ChaseTarget))
					{
						point = PlayerMgr.Inst.PlayerPoint;
						LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget);
						if (DTool.IgnoreZDistance(in point, in componentAfterCompletingDependency.Position) <= num4 + num3)
						{
							EnterState(WandSpiritState.BattleLockTarget);
						}
					}
					break;
				}
				case WandSpiritState.BattleLockTarget:
				{
					PhysicsWorldSingleton physicsWorld = __query_378481533_2.GetSingleton<PhysicsWorldSingleton>();
					if (!(wandSpiritData.ValueRW.ChaseTarget == Entity.Null) && (wand.shootGroups.Count > 0 || wand.postSlotShootGroups.Count > 0 || wand.passiveRuneHammerEnable) && base.EntityManager.HasComponent<LocalTransform>(wandSpiritData.ValueRW.ChaseTarget))
					{
						float3 point = PlayerMgr.Inst.PlayerPoint;
						LocalTransform componentAfterCompletingDependency = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget);
						if (!(DTool.IgnoreZDistance(in point, in componentAfterCompletingDependency.Position) > num4 + num3))
						{
							float3 point2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget).Position;
							float num6 = DTool.IgnoreZDistance(in point2, in trans.ValueRW.Position);
							point = PlayerMgr.Inst.PlayerPoint;
							Unity.Physics.RaycastHit hitResult;
							if (DTool.IgnoreZDistance(in point, in trans.ValueRW.Position) > num3 || (wand.passiveRuneHammerEnable && wand.shootGroups.Count == 0 && num6 < num5 - 0.3f) || (wandSpiritData.ValueRO.CurrentShootGroupMovementType == SpellSpecialMovementType.Rotation && num6 < num4 - 0.5f))
							{
								if (PlayerMgr.Inst.PlayerCtrller.CanMotion)
								{
									componentRWAfterCompletingDependency2.ValueRW.UpdatePath(trans.ValueRW.Position, PlayerMgr.Inst.PlayerPoint, componentRWAfterCompletingDependency.ValueRW.navAreaMask);
									componentRWAfterCompletingDependency3.ValueRW.SetMove(Tool2D.IgnoreZPoint(componentRWAfterCompletingDependency2.ValueRW.walkToPoint - trans.ValueRW.Position).normalized * num2 * 0.5f);
								}
							}
							else if (num6 > num5 || (wand.passiveRuneHammerEnable && wand.shootGroups.Count == 0 && num6 > num5 + 0.3f) || (DTool.RaycastWallHit(trans.ValueRW.Position, point2, out hitResult, in physicsWorld) && !wandSpiritScript.ignoreWall))
							{
								componentRWAfterCompletingDependency2.ValueRW.UpdatePath(trans.ValueRW.Position, point2, componentRWAfterCompletingDependency.ValueRW.navAreaMask);
								componentRWAfterCompletingDependency3.ValueRW.SetMove(Tool2D.IgnoreZPoint(componentRWAfterCompletingDependency2.ValueRW.walkToPoint - trans.ValueRW.Position).normalized * num2 * 0.5f);
							}
							else
							{
								componentRWAfterCompletingDependency3.ValueRW.SetMove(float3.zero);
							}
							Vector3 normalized = Tool2D.IgnoreZPoint(point2 - trans.ValueRW.Position).normalized;
							ref Spell4005WandSpiritData valueRW = ref wandSpiritData.ValueRW;
							ref float3 wandLookDirection = ref wandSpiritData.ValueRW.WandLookDirection;
							point = normalized;
							valueRW.WandLookDirection = DTool.DirMoveTowardsIgnoreZ(in wandLookDirection, in point, 200f * deltaTime);
							if (IsSpecificUIOpen() || !GeneralTool.IsPlayerCanMotion())
							{
								continue;
							}
							if (wand.currentShootGroup != null && num6 <= num4 && wand.CoolingTimer <= 0f && wand.ShootIntervalTimer <= 0f && wand.CheckCurrentMpEnough(wand.currentShootGroup.GetGroupManaCost_FinalPlayerValue(wand)) && wandSpiritData.ValueRW.AttackDurationTimer <= 0f && GetAngleBetweenTwoDirection(wandSpiritData.ValueRW.WandLookDirection, normalized) < 20f && (!DTool.RaycastWallHit(trans.ValueRW.Position, point2, out var _, in physicsWorld) || wandSpiritScript.ignoreWall) && (!DataMgr.settingData.AiSummon || wand.currentShootGroup.Shoots.Select((SpellShootData e) => e.Spell.GetFinalConfig()).All((SpellConfig e) => e.useType != SpellType.Summon) || (from e in wand.currentShootGroup.Shoots
								select e.Spell.GetFinalConfig() into e
								where e.useType == SpellType.Summon
								select e).Any(delegate(SpellConfig e)
							{
								EntityManager entityMgr = base.EntityManager;
								return SpellTools.CheckIsTargetTeammateReachSummonLimit(in entityMgr, SpellTools.GetSpellTeammateType(e.abilityType), in entity);
							})))
							{
								wandSpiritData.ValueRW.ReadyToAttack = true;
							}
							break;
						}
					}
					EnterState(WandSpiritState.rotateAroundPlayer);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException();
				}
				Quaternion quaternion = trans.ValueRW.Rotation;
				wandSpiritScript.lastFrameTargetPosition = (base.EntityManager.HasComponent<LocalTransform>(wandSpiritData.ValueRW.ChaseTarget) ? ((Vector3)InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget).Position) : PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint);
				wandSpiritScript.lastFrameTargetDirection = (base.EntityManager.HasComponent<LocalTransform>(wandSpiritData.ValueRW.ChaseTarget) ? quaternion.eulerAngles : PlayerMgr.Inst.PlayerDir);
				UpdatePostSlotCharge();
				trans.ValueRW.Position = Tool2D.GetNavMeshPointIngoreZ(trans.ValueRW.Position, 32);
				wandSpiritScript.transform.position = trans.ValueRW.Position;
				wandSpiritScript.wandRotateTransform.transform.right = wandSpiritData.ValueRW.WandLookDirection;
				float3 position2 = trans.ValueRW.Position;
				position2.z = 0f;
				wand.passiveAutoWandShooterData.shootPosition = position2 + new float3(0f, 0f, -0.3f) + wandSpiritData.ValueRW.WandLookDirection * 0.3f;
				wand.passiveAutoWandShooterData.shootDirection = wandSpiritData.ValueRW.WandLookDirection;
				trans.ValueRW.Rotation = Unity.Mathematics.quaternion.Euler(0f, 0f, DTool.GetDegree(wandSpiritData.ValueRW.WandLookDirection) * (MathF.PI / 180f));
				if (wand.ChargeStars.Count > 0 && (wand.currentShootGroup == null || wandSpiritData.ValueRW.ChaseTarget == Entity.Null))
				{
					wand.ReleaseCharge();
				}
				void EnterState(WandSpiritState state)
				{
					wandSpiritData.ValueRW.State = state;
					switch (wandSpiritData.ValueRW.State)
					{
					case WandSpiritState.rotateAroundPlayer:
						wandSpiritData.ValueRW.ChaseTarget = Entity.Null;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					case WandSpiritState.Idle:
					case WandSpiritState.BattleLockTarget:
						break;
					}
				}
				void RecheckValidTarget()
				{
					if (wand.WandCfg != null && (wand.shootGroups.Count > 0 || wand.postSlotShootGroups.Count > 0 || wand.passiveRuneHammerEnable))
					{
						wandSpiritData.ValueRW.RecheckTargetTimer += deltaTime;
						if (!(wandSpiritData.ValueRW.RecheckTargetTimer < 0.1f))
						{
							wandSpiritData.ValueRW.RecheckTargetTimer = 0f;
							currentRoomEntities.FindNearestTarget(trans.ValueRO.Position, UnitType.Teammate, out wandSpiritData.ValueRW.ChaseTarget, out var targetPosition, out var _);
							if (wandSpiritData.ValueRW.ChaseTarget != Entity.Null && InternalCompilerInterface.HasComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget))
							{
								LocalTransform componentAfterCompletingDependency2 = InternalCompilerInterface.GetComponentAfterCompletingDependency(ref __TypeHandle.__Unity_Transforms_LocalTransform_RO_ComponentLookup, ref base.CheckedStateRef, wandSpiritData.ValueRW.ChaseTarget);
								ref float3 position3 = ref componentAfterCompletingDependency2.Position;
								targetPosition = PlayerMgr.Inst.PlayerPoint;
								if (DTool.IgnoreZDistance(in position3, in targetPosition) > wandSpiritScript.attackDistance + 4f)
								{
									wandSpiritData.ValueRW.ChaseTarget = Entity.Null;
								}
							}
						}
					}
				}
				void UpdatePostSlotCharge()
				{
					float num7 = Vector3.Distance((Vector3)wandSpiritData.ValueRW.LastFramePosition, (Vector3)trans.ValueRW.Position);
					if (wand.WandCfg.PostslotStandChargeRatio > 0f && num7 <= 0.02f)
					{
						wand.ChargePostSlots(wand.WandCfg.PostslotStandChargeRatio * PlayerMgr.Inst.PlayerDeltaTime);
					}
					else if (wand.WandCfg.PostslotMoveChargeRatio > 0f && num7 <= 5f)
					{
						wand.ChargePostSlots(wand.WandCfg.PostslotMoveChargeRatio * num7);
					}
					wandSpiritData.ValueRW.LastFramePosition = trans.ValueRW.Position;
				}
			}
		}
		static float GetAngleBetweenTwoDirection(float3 dir1, float3 dir2)
		{
			return math.acos(math.clamp(math.dot(math.normalize(dir1), math.normalize(dir2)), -1f, 1f)) * 57.29578f;
		}
		static bool IsSpecificUIOpen()
		{
			if (!GameUISingletonMono<UIReroll>.StaticIsOpen && !GameUISingletonMono<UICompound>.StaticIsOpen && !GameUISingletonMono<UIMoreInOne>.StaticIsOpen)
			{
				return GameUISingletonMono<UITraining>.StaticIsOpen;
			}
			return true;
		}
	}

	private float3 GetRotatePoint(Teammate52 targetWandSpirit)
	{
		return PlayerMgr.Inst.PlayerPoint + 1.2f * Tool2D.GetDir(Teammate52.wandBaseAngle + 360f / (float)Teammate52.autoWandList.Count * (float)Teammate52.autoWandList.IndexOf(targetWandSpirit) + 90f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void __AssignQueries(ref SystemState state)
	{
		EntityQueryBuilder entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp);
		EntityQueryBuilder entityQueryBuilder2 = entityQueryBuilder.WithAll<CurrentRoomEntitiesSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_378481533_0 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<HammerAttackRangeInWandSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_378481533_1 = entityQueryBuilder2.Build(ref state);
		entityQueryBuilder.Reset();
		entityQueryBuilder2 = entityQueryBuilder.WithAll<PhysicsWorldSingleton>();
		entityQueryBuilder2 = entityQueryBuilder2.WithOptions(EntityQueryOptions.IncludeSystems);
		__query_378481533_2 = entityQueryBuilder2.Build(ref state);
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
	public Spell4005WandDataInitializeSystem()
	{
	}
}
