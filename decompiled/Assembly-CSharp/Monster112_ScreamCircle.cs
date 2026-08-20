using UnityEngine;

public class Monster112_ScreamCircle : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Spell"))
		{
			SpellBase componentInParent = other.GetComponentInParent<SpellBase>();
			if (!componentInParent.IsSameCamp(UnitType.Monster) && !(componentInParent is Spell1021MagicBreaker) && !(componentInParent is Spell4013ArcaneBlade) && !(componentInParent is Spell4019BiAnLethalBlade))
			{
				tryRecycleSpell(componentInParent);
			}
		}
		else if (other.CompareTag("RollBall"))
		{
			Spell1002RollBall componentInParent2 = other.GetComponentInParent<Spell1002RollBall>();
			if (!componentInParent2.IsSameCamp(UnitType.Monster))
			{
				tryRecycleSpell(componentInParent2);
			}
		}
	}

	private void tryRecycleSpell(SpellBase _spellBase)
	{
		if (!_spellBase.IsSameCamp(UnitType.Monster) && _spellBase.spellCfg.abilityType != SpellAbilityType.Dash)
		{
			_spellBase.CreateHitEffect();
			_spellBase.PoolRecycle();
		}
	}
}
