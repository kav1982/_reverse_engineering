using UnityEngine;

public class Spell1031Effect : SpellEffectBase
{
	private Spell1031Shotgun shotgunScript;

	private Transform spellTrans;

	protected override void Awake()
	{
		base.Awake();
		shotgunScript = (Spell1031Shotgun)base.Spell;
	}

	private void OnDisable()
	{
		spellTrans = null;
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name == "Spell")
		{
			spellTrans = trans;
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateSpellEffectDirection();
	}

	private void UpdateSpellEffectDirection()
	{
		if ((bool)spellTrans)
		{
			if (shotgunScript.SIP.spellIsFall)
			{
				Vector2 vector = new Vector2(base.Spell.Direction.x * base.Spell.CurrentSpeed, base.Spell.CurrentUpSpeed + base.Spell.Direction.y * base.Spell.CurrentSpeed);
				spellTrans.transform.right = vector;
			}
			else
			{
				spellTrans.transform.right = shotgunScript.Direction;
			}
		}
	}
}
