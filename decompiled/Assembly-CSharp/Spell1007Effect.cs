using DG.Tweening;
using UnityEngine;

public class Spell1007Effect : SpellEffectBase
{
	protected override void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnWillRecycleEffect(effect, trans);
		trans.DOScale(0f, 0.2f);
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name.Contains("Trail"))
		{
			trans.localScale = Vector3.one;
		}
	}

	protected override void OnFirstFrame()
	{
		if (base.Spell.SIP.spellIsFall)
		{
			ManualCreateEffect("Shadow");
			ManualCreateEffect("FallSpell");
		}
		else
		{
			ManualCreateEffect("Spell");
		}
		base.OnFirstFrame();
	}
}
