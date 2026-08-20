using UnityEngine;

public class Spell9026Bullet : SpellBase
{
	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
	}

	public override void Update()
	{
		base.Update();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (!(base.DurationTimer > base.spellCfg.duration))
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		TakeDamageInfo result = base.MakeDamageToUnit(unit);
		if (base.spellCfg.float1 > 0f)
		{
			unit.BonusTakeDamageRatioRegister(base.spellCfg.float1, base.spellCfg.float2);
		}
		return result;
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (!base.SIP.spellIsFall)
		{
			base.CreateHitEffect(position, rotation);
		}
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (ownerPpt.unitCfg.unitType == UnitType.NotAttack || (ownerPpt.unitCfg.unitType == UnitType.Brittleness && unit.unitCfg.unitType != 0 && unit.unitCfg.unitType != UnitType.Teammate && unit.unitCfg.unitType != UnitType.TeammateNotAttack))
		{
			return false;
		}
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		MakeDamageToUnit(unit);
		CreateHitEffect();
		TryRefractOrPenetrateOrRecycleOnHitTarget(unit.gameObject);
		return true;
	}
}
