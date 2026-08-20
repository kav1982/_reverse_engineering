using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.GraphicsIntegration;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Scripting;

[UpdateAfter(typeof(LocalToWorldSystem))]
[UpdateInGroup(typeof(TransformSystemGroup))]
public class UnitLateSyncSystem : SystemBase
{
	[Preserve]
	protected override void OnUpdate()
	{
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		for (int num = UnitDotsSyncSystem.existingUnit.Count - 1; num >= 0; num--)
		{
			UnitProperty unitProperty = UnitDotsSyncSystem.existingUnit[num];
			if (unitProperty == null)
			{
				UnitDotsSyncSystem.existingUnit.Remove(unitProperty);
			}
			else if (!UnitDotsSyncSystem.EntityIsValid(unitProperty.myEntity))
			{
				unitProperty.gameObject.SetActive(value: false);
				UnitDotsSyncSystem.existingUnit.Remove(unitProperty);
				if (LevelMgr.Inst.CurrentRoomCtrller != null)
				{
					if (LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Contains(unitProperty.myEntity))
					{
						LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(unitProperty.myEntity);
					}
					if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Contains(unitProperty.myEntity))
					{
						LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(unitProperty.myEntity);
					}
				}
			}
			else
			{
				UnitProperty_Dots componentData = UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(unitProperty.myEntity);
				if (!componentData.isDead && !unitProperty.gameObject.activeSelf && componentData.unitCfg.unitType != 0)
				{
					if (LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Contains(unitProperty.myEntity))
					{
						LevelMgr.Inst.CurrentRoomCtrller.monsterEttList.Remove(unitProperty.myEntity);
					}
					if (LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Contains(unitProperty.myEntity))
					{
						LevelMgr.Inst.CurrentRoomCtrller.targetableEttList.Remove(unitProperty.myEntity);
					}
					entityCommandBuffer.DestroyEntity(unitProperty.myEntity);
				}
				else if (componentData.unitCfg.unitType != UnitType.Brittleness)
				{
					if (componentData.frozenStateChanged)
					{
						componentData.frozenStateChanged = false;
						UnitDotsSyncSystem.SetComponentData(componentData, unitProperty.myEntity);
						if (componentData.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
						{
							unitProperty.SetFrozen(componentData.affect_FrozenTime / componentData.unitCfg.frozenTimeRatio);
						}
						else if (componentData.FronzenState == UnitProperty.Affect_FrozenState.FrozenImmune)
						{
							unitProperty.ClearFrozenState(tempImmume: true);
						}
					}
					if (componentData.affect_IsMucusHit && unitProperty.affect_MucusHitTimer <= 0f)
					{
						unitProperty.SetMucus(componentData.affect_MucusHitTimer, componentData.affect_MucusMoveSpeedRatio, componentData.affect_MucusSpellSpeedRatio);
					}
					else if (!componentData.affect_IsMucusDecelerate && unitProperty.affect_MucusHitTimer > 0f)
					{
						unitProperty.ClearMucusState();
					}
					if (componentData.affect_IsMucusDecelerate)
					{
						unitProperty.affect_IsMucusDecelerate = true;
						unitProperty.affect_MucusMoveSpeedRatio = componentData.affect_MucusMoveSpeedRatio;
						unitProperty.affect_MucusSpellSpeedRatio = componentData.affect_MucusSpellSpeedRatio;
					}
					else
					{
						unitProperty.affect_IsMucusDecelerate = false;
						unitProperty.affect_MucusMoveSpeedRatio = 1f;
						unitProperty.affect_MucusSpellSpeedRatio = 1f;
					}
					if (componentData.Affect_InAbyss && !unitProperty.Affect_InAbyss)
					{
						unitProperty.FallinAbyss(componentData.affect_AbyssPoint);
					}
					if (componentData.disabled)
					{
						unitProperty.ChangeColor(Color.white);
					}
					else
					{
						unitProperty.Tsf_BeHit.localPosition = componentData.beHitDir * componentData.beHitCurrentOffsetAmount;
						if (componentData.needSyncColor)
						{
							unitProperty.ChangeColor(componentData.baseColor);
							unitProperty.ChangeAlpha(componentData.baseColor.a);
						}
						if (unitProperty.unitCfg.unitType != 0 && unitProperty.unitCfg.unitType != UnitType.Brittleness && unitProperty.unitCfg.unitType != UnitType.NotAttack)
						{
							if (unitProperty.Rigid != null && unitProperty.isRigidLerp_Dots)
							{
								if (UnitDotsSyncSystem.GetComponentData<PhysicsGraphicalSmoothing>(unitProperty.myEntity).ApplySmoothing != 0)
								{
									LocalToWorld componentData2 = UnitDotsSyncSystem.GetComponentData<LocalToWorld>(unitProperty.myEntity);
									unitProperty.transform.position = componentData2.Position;
								}
							}
							else
							{
								LocalTransform componentData3 = UnitDotsSyncSystem.GetComponentData<LocalTransform>(unitProperty.myEntity);
								unitProperty.transform.position = componentData3.Position;
							}
						}
						if (componentData.affect_IsReverseMove)
						{
							unitProperty.SetReverseMove(componentData.affect_ReverseMoveTimer);
						}
						if (!(unitProperty.Rigid == null) && !unitProperty.Rigid.isKinematic)
						{
							PhysicsVelocity componentData4 = UnitDotsSyncSystem.GetComponentData<PhysicsVelocity>(unitProperty.myEntity);
							unitProperty.Rigid.linearVelocity = componentData4.Linear;
						}
					}
				}
			}
		}
		entityCommandBuffer.Playback(base.EntityManager);
		entityCommandBuffer.Dispose();
	}

	[Preserve]
	public UnitLateSyncSystem()
	{
	}
}
