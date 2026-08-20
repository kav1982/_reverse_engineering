using System.Collections.Generic;
using UnityEngine;

public class Spell1017Effect : SpellEffectBase
{
	private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	private float _timeScale = 1f;

	protected override void OnEnable()
	{
		_particleSystems.Clear();
		_timeScale = 1f;
		base.OnEnable();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		ParticleSystem[] allParticleSystem = SpellEffectBase.GetAllParticleSystem(trans.gameObject);
		ParticleSystem[] array = allParticleSystem;
		for (int i = 0; i < array.Length; i++)
		{
			ParticleSystem.MainModule main = array[i].main;
			main.simulationSpeed = 1f;
		}
		_particleSystems.AddRange(allParticleSystem);
	}

	protected override void Update()
	{
		base.Update();
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, _timeScale, 10f * Time.deltaTime);
		}
	}

	public void PauseEffect()
	{
		_timeScale = 0f;
	}

	public void ResumeEffect()
	{
		_timeScale = 1f;
	}
}
