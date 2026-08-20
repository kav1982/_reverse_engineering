using UnityEngine;

public class Spell1009BackMP : SpellBase
{
	public interface ICanManaBack
	{
		void ManaBackFromSpell1009(float mana);
	}

	private bool drainOnce;

	private int drainSplitCount;

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
		drainOnce = false;
	}

	public override void Update()
	{
		base.Update();
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration) || base.SIP.spellIsFall)
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
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		TakeDamageInfo takeDamageInfo = base.OutputDamage(unitPpt, info);
		if (takeDamageInfo.beHitPpt.unitCfg.IsSameCamp(UnitType.Monster))
		{
			ManaBack();
		}
		return takeDamageInfo;
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		((Spell1009BackMP)spellBase).drainSplitCount = base.spellSplitCount;
		return spellBase;
	}

	private float GetMpGenAmount()
	{
		float num = base.ShootData.GetSpellManaCost_FinalPlayerValue(base.shooterWand).Result;
		if (base.spellCfg.isSplitSpell || base.spellSplitCount > 0)
		{
			num /= 2f;
		}
		if (base.SIP.multiShootCount > 1 && !base.spellCfg.isSplitSpell)
		{
			num /= (float)Mathf.Max(1, base.SIP.multiShootCount);
		}
		if (base.spellCfg.isSplitSpell)
		{
			num /= (float)Mathf.Max(1, drainSplitCount);
		}
		return num;
	}

	private void ManaBack()
	{
		if (drainOnce)
		{
			return;
		}
		if (ownerPpt.UnitBas is ICanManaBack canManaBack)
		{
			canManaBack.ManaBackFromSpell1009(GetMpGenAmount());
			drainOnce = true;
			return;
		}
		UnitType unitType = ownerPpt.unitCfg.unitType;
		if ((unitType == UnitType.Player || unitType == UnitType.TeammateNotAttack) && !(base.shooterWand == null) && base.shooterWand.WandCfg != null && base.shooterWand.gameObject.activeInHierarchy)
		{
			base.shooterWand.GainMP(GetMpGenAmount());
			drainOnce = true;
		}
	}
}
