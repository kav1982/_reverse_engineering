using UnityEngine;

public class Spell9022FlowerBullet : SpellBase
{
	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
		SEMgr.Inst.PlaySE("SE_Spell9022Shoot");
	}

	public override void Update()
	{
		base.Update();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
			if (base.transform.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		TakeDamageInfo result = base.MakeDamageToUnit(unit);
		SEMgr.Inst.PlaySE("SE_Spell9022Hit");
		return result;
	}

	protected override bool OnHitDestructible(UnitProperty go)
	{
		SEMgr.Inst.PlaySE("SE_Spell9022Hit");
		return base.OnHitDestructible(go);
	}
}
