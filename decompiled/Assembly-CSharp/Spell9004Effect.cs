using UnityEngine;

public class Spell9004Effect : SpellEffectBase
{
	public float lv1Width = 1f;

	public float lv2Width = 2f;

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (!(effect.Name != "Spell"))
		{
			trans.GetComponent<Spell9004RainController>().widthRatio = ((base.Spell.spellCfg.id == 90041) ? lv1Width : lv2Width);
		}
	}
}
