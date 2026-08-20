using SpriteEffectSystem;

public class Spell1014Effect : SpellEffectBase
{
	public Spell1014Rainbow SpellScript;

	public SpriteEffectAnima HitRed;

	public SpriteEffectAnima HitOrange;

	public SpriteEffectAnima HitYellow;

	public SpriteEffectAnima HitGreen;

	public SpriteEffectAnima HitCyan;

	public SpriteEffectAnima HitBlue;

	public SpriteEffectAnima HitPurple;

	public SpriteEffectAnima HitVoid;

	protected override string GetEffectPrefabColorPostfix(SpellEffectSettings settings)
	{
		if (settings.Name != "Trail")
		{
			return base.GetEffectPrefabColorPostfix(settings);
		}
		if (settings.Name == "Trail" && SpellScript.ColorType == SpellColorType.Void)
		{
			return base.GetEffectPrefabColorPostfix(settings);
		}
		return Spell1014Rainbow.Colors[((Spell1014Rainbow)base.Spell).InitialParameter.inSpellShootIndex];
	}

	protected override string GetFallingExplosionPrefabName()
	{
		int num = ((base.Spell.ColorType == SpellColorType.Void) ? 7 : ((Spell1014Rainbow)base.Spell).InitialParameter.inSpellShootIndex);
		return "Prefabs/Spell/" + EffectID + "/" + EffectID + "_FallExplosion_" + Spell1014Rainbow.Colors[num];
	}

	protected override SpriteEffectAnima GetRandomSpriteEffectAnima(SpellSpriteEffectSettings settings)
	{
		if (base.Spell.ColorType == SpellColorType.Void)
		{
			return HitVoid;
		}
		if (base.Spell.ColorType == SpellColorType.Player)
		{
			return ((Spell1014Rainbow)base.Spell).InitialParameter.inSpellShootIndex switch
			{
				0 => HitRed, 
				1 => HitOrange, 
				2 => HitYellow, 
				3 => HitGreen, 
				4 => HitCyan, 
				5 => HitBlue, 
				6 => HitPurple, 
				_ => HitRed, 
			};
		}
		return base.GetRandomSpriteEffectAnima(settings);
	}
}
