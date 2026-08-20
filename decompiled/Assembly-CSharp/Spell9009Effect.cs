using UnityEngine;

public class Spell9009Effect : SpellEffectBase
{
	public Sprite s1;

	public Sprite s2;

	private SpriteRenderer sr;

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (!(effect.Name != "Spell"))
		{
			sr = trans.GetComponentInChildren<SpriteRenderer>();
			sr.sprite = (((Spell9009BladeWave)base.Spell).isSprite1 ? s1 : s2);
		}
	}

	protected override void Update()
	{
		base.Update();
		sr.sprite = (((Spell9009BladeWave)base.Spell).isSprite1 ? s1 : s2);
	}
}
