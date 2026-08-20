using SpriteEffectSystem;
using UnityEngine;

public class Spell1020Effect : SpellEffectBase
{
	public Sprite SilverSprite;

	public Sprite CoinSprite;

	public Sprite VoidCoin;

	public SpriteRenderer SpriteRenderer;

	public SpriteEffectAnima SliverHitAnima;

	public EffectTransparencyController TransparencyController;

	protected override void OnFirstFrame()
	{
		base.OnFirstFrame();
		SpriteRenderer.sprite = ((((Spell1020ManaCoin)base.Spell).usedCoin > 0) ? CoinSprite : SilverSprite);
		if (base.Spell.ColorType == SpellColorType.Void)
		{
			SpriteRenderer.sprite = VoidCoin;
		}
		TransparencyController.ForceNoTransparent = (bool)base.Spell.ownerPpt && !base.Spell.ownerPpt.gameObject.CompareAnyTag("Player", "Teammate");
		TransparencyController.UpdateTransparent();
	}

	protected override string GetEffectPrefabColorPostfix(SpellEffectSettings settings)
	{
		if (base.Spell.ColorType == SpellColorType.Void)
		{
			return "Void";
		}
		if (base.Spell.ColorType == SpellColorType.Player && ((Spell1020ManaCoin)base.Spell).usedCoin == 0)
		{
			return "Silver";
		}
		return base.GetEffectPrefabColorPostfix(settings);
	}

	protected override SpriteEffectAnima GetRandomSpriteEffectAnima(SpellSpriteEffectSettings settings)
	{
		if (settings.Name == "Hit" && base.Spell.ColorType == SpellColorType.Player && ((Spell1020ManaCoin)base.Spell).usedCoin == 0)
		{
			return SliverHitAnima;
		}
		return base.GetRandomSpriteEffectAnima(settings);
	}

	protected override string GetFallingExplosionPrefabName()
	{
		if (base.Spell.ColorType == SpellColorType.Player && ((Spell1020ManaCoin)base.Spell).usedCoin == 0)
		{
			return "Prefabs/Spell/10201/10201_FallExplosion_Silver";
		}
		return base.GetFallingExplosionPrefabName();
	}
}
