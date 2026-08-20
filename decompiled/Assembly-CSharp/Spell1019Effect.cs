using System.Linq;
using UnityEngine;

public class Spell1019Effect : SpellEffectBase
{
	public void StopStartEffect()
	{
		(Transform, SpellEffectSettings)[] array = CurrentEffects.Where(((Transform trans, SpellEffectSettings effect) e) => e.effect.Name == "Start").ToArray();
		if (array.Length != 0)
		{
			ParticleSystem[] allParticleSystem = SpellEffectBase.GetAllParticleSystem(array[0].Item1.gameObject);
			for (int i = 0; i < allParticleSystem.Length; i++)
			{
				allParticleSystem[i].Stop();
			}
		}
	}
}
