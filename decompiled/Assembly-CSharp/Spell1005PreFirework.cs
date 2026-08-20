using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1005PreFirework : LauncherSpellBase
{
	public override void InitializeCallback()
	{
		base.InitializeCallback();
		if (!base.SIP.spellIsFall)
		{
			base.rebounceTime = 100;
		}
		ApplySpeedToVelocity();
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
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	public override void PoolRecycle()
	{
		EffectBase.ManualCreateEffect("End");
		Launch();
		base.PoolRecycle();
	}

	protected override IEnumerable<SpellBase> CreateLaunchSpells()
	{
		PlaySE("Trigger");
		EffectBase.CreateSpriteEffect("Trigger");
		SpellBase[] array = base.CreateLaunchSpells().ToArray();
		Spell3007ChainSystem.CreateChains(array, base.shooterWand);
		return array;
	}
}
