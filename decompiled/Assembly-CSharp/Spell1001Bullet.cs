using UnityEngine;

public class Spell1001Bullet : SpellBase
{
	[Header("怪物重写音效")]
	public bool useSoundOverride;

	public int soundPrefix;

	public float SEInterval;

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
	}

	public override AudioSource PlaySE(string suffix, float SEInterval = 0.05f)
	{
		if (useSoundOverride)
		{
			return SEMgr.Inst.PlaySE("SE_Spell" + soundPrefix + suffix, SEPlayMode.Replay, 3, this.SEInterval);
		}
		return base.PlaySE(suffix, SEInterval);
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
}
