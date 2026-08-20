using System.Collections.Generic;
using Unity.Entities;

public class Elite9_BodyInvisible : UnitBase
{
	public List<Entity> hitList;

	public Elite9 master;

	public Elite9_Body target;

	public override void Update()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(master.myPpt.myEntity, out var result))
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
			SetComponentData(result, master.myPpt.myEntity);
		}
		componentData.ClearVenomState();
		componentData.ClearBurnState();
		componentData.ClearVoidState();
		SetComponentData(componentData);
		base.Update();
		if (target != null)
		{
			base.transform.position = target.transform.position;
			SyncDotsPosition();
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.attackerType == AttackerType.Venom || info.attackerType == AttackerType.Burn)
		{
			info.immuneDamage = true;
		}
		else if (info.spell.Entity != Entity.Null)
		{
			if (!hitList.Contains(info.spell.Entity))
			{
				UnitDotsSyncSystem.AddTakeDamageRequest(master.myPpt.myEntity, info);
			}
			else
			{
				info.immuneDamage = true;
			}
		}
		else
		{
			UnitDotsSyncSystem.AddTakeDamageRequest(master.myPpt.myEntity, info);
		}
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (!master.myPpt.AlreadyDead)
		{
			info.stopAnnouncedDeath = true;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.unitCfg.currentHP = componentData.unitCfg.maxHP;
			SetComponentData(componentData);
		}
	}
}
