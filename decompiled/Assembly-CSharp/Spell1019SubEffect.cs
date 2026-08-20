using UnityEngine;

public class Spell1019SubEffect : SpellEffectBase
{
	public MaterialsByColorType Materials;

	public LineRenderer Line;

	public GameObject ThunderEffect;

	private Spell1019WaterEffectRemaster spell;

	protected override void OnEnable()
	{
		base.OnEnable();
		spell = (Spell1019WaterEffectRemaster)base.Spell;
		Line.enabled = false;
		ThunderEffect.SetActive(value: false);
	}

	protected override void OnFirstFrame()
	{
		Line.sharedMaterial = Materials.Get(base.Spell.ColorType);
		Line.enabled = true;
		ThunderEffect.SetActive(base.Spell.ColorType == SpellColorType.Thunder);
	}

	public override void PlayFallingGroundSound()
	{
		spell.PlaySoundEffect();
	}
}
