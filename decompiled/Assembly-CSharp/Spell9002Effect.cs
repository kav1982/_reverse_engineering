using UnityEngine;

public class Spell9002Effect : SpellEffectBase
{
	private Transform spellTrans;

	private Transform shadowTrans;

	public void Rotate(float angle)
	{
		if ((bool)spellTrans)
		{
			spellTrans.Rotate(0f, 0f, angle);
		}
		if ((bool)shadowTrans)
		{
			shadowTrans.Rotate(0f, 0f, angle);
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		if (effect.Name.StartsWith("Spell"))
		{
			spellTrans = trans;
		}
		if (effect.Name.StartsWith("Shadow"))
		{
			shadowTrans = trans;
		}
	}

	protected override void OnWillRecycleEffect(SpellEffectSettings effect, Transform trans)
	{
		if (effect.Name.StartsWith("Spell"))
		{
			spellTrans = null;
		}
		if (effect.Name.StartsWith("Shadow"))
		{
			shadowTrans = null;
		}
	}

	protected override void OnChangeColor(SpellEffectSettings effect, Transform newColorTrans)
	{
		if (effect.Name.StartsWith("Spell"))
		{
			spellTrans = newColorTrans;
		}
		if (effect.Name.StartsWith("Shadow"))
		{
			shadowTrans = newColorTrans;
		}
	}
}
