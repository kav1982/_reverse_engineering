using Unity.Transforms;
using UnityEngine;

public class Monster42_Head : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle
	}

	private MonsterState state;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	public Monster42 Master;

	private bool canDie;

	public override void EveryInitialCallback()
	{
		canDie = false;
	}

	public void SetPosition(Vector3 position)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			base.transform.position = position;
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = position;
			SetComponentData(componentData);
		}
	}

	public void SetCanTouch(bool canTouch)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
		}
	}

	public override void Update()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(Master.myPpt.myEntity, out var result))
		{
			if (componentData.affect_VenomCurrentStack > 0f)
			{
				result.SetVenom(componentData.affect_VenomDurationTimer, componentData.affect_VenomCurrentStack);
			}
			if (componentData.affect_burnDurationTimer > 0f)
			{
				result.SetBurn(componentData.affect_burnDurationTimer, componentData.affect_burnHPRatioPerHit);
			}
			if (componentData.voidEffectTimer > 0f)
			{
				result.SetVoid(componentData.voidExplosionData);
			}
			SetComponentData(result, Master.myPpt.myEntity);
		}
		componentData.ClearVenomState();
		componentData.ClearBurnState();
		componentData.ClearVoidState();
		SetComponentData(componentData);
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		base.Update();
		if (!base.IsLocked)
		{
			_ = changedState;
			_ = state;
		}
	}

	public void ManualDie()
	{
		canDie = true;
		DotsAnnouncedDeath();
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.attackerType == AttackerType.Venom || info.attackerType == AttackerType.Burn)
		{
			info.immuneDamage = true;
		}
		else
		{
			Master.ShareDamage(info);
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (!canDie)
		{
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
			SetComponentData(componentData);
		}
	}

	public override void AnimaAction(string animaName)
	{
	}
}
