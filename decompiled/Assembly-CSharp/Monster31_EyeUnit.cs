public class Monster31_EyeUnit : UnitBase
{
	public Monster31 Master;

	public bool closeImmume;

	public void Initialize(Monster31 master)
	{
		Master = master;
		closeImmume = true;
	}

	public override void EveryInitialCallback()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
	}

	public override void Update()
	{
		if (closeImmume && base.CC_Self.enabled)
		{
			base.CC_Self.enabled = false;
			SetDotsCCEnable(isOpen: false);
		}
		else if (!closeImmume && !base.CC_Self.enabled)
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
		}
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
		base.Update();
		_ = base.IsLocked;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.attackerType == AttackerType.Venom || info.attackerType == AttackerType.Burn || closeImmume)
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
		if (!Master.myPpt.AlreadyDead)
		{
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
			SetComponentData(componentData);
		}
	}
}
