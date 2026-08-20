using UnityEngine;

public class Spell1011Effect : SpellEffectBase
{
	public LineRenderer Line;

	public LineRenderer Shadow;

	public MaterialsByColorType Materials;

	public GameObjectByColorType Nodes;

	protected override void OnFirstFrame()
	{
		base.OnFirstFrame();
		Line.sharedMaterial = Materials.Get(base.Spell.ColorType);
		Line.enabled = true;
		GameObject[] all = Nodes.GetAll();
		for (int i = 0; i < all.Length; i++)
		{
			all[i].SetActive(value: false);
		}
		Line.loop = base.Spell.currentSpellMovement == SpellSpecialMovementType.Rotation && !base.Spell.SIP.spellIsFall;
		Shadow.loop = base.Spell.currentSpellMovement == SpellSpecialMovementType.Rotation && !base.Spell.SIP.spellIsFall;
		if (base.Spell.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			Nodes.Get(base.Spell.ColorType).SetActive(value: true);
		}
	}

	private void OnDisable()
	{
		Line.enabled = false;
		Nodes.Get(base.Spell.ColorType).SetActive(value: false);
	}
}
