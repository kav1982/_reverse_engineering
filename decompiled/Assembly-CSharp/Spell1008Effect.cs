using System.Collections.Generic;
using UnityEngine;

public class Spell1008Effect : SpellEffectBase
{
	private Spell1008ArcaneExplosion ArcaneExplosion;

	private readonly List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	private readonly Dictionary<Spell1008ArcaneExplosion.ExplosionController, ParticleSystem[]> particles = new Dictionary<Spell1008ArcaneExplosion.ExplosionController, ParticleSystem[]>();

	private readonly List<Spell1008ArcaneExplosion.ExplosionController> pausedExplosions = new List<Spell1008ArcaneExplosion.ExplosionController>();

	protected override void OnEnable()
	{
		ArcaneExplosion = (Spell1008ArcaneExplosion)base.Spell;
		_particleSystems.Clear();
		base.OnEnable();
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name != "Fall")
		{
			_particleSystems.AddRange(SpellEffectBase.GetAllParticleSystem(trans.gameObject));
		}
		switch (effect.Name)
		{
		case "Trail":
		case "Ground":
		case "Spell":
			trans.position -= new Vector3(0f, effect.AttachTarget.localPosition.y, 0f);
			break;
		}
	}

	protected override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (base.Spell.SIP.spellIsFall)
		{
			ManualCreateEffect("Fall");
			ManualCreateEffect("FallShadow");
		}
	}

	protected override void Update()
	{
		base.Update();
		foreach (KeyValuePair<Spell1008ArcaneExplosion.ExplosionController, ParticleSystem[]> particle in particles)
		{
			particle.Deconstruct(out var key, out var value);
			Spell1008ArcaneExplosion.ExplosionController item = key;
			value = value;
			for (int i = 0; i < value.Length; i++)
			{
				ParticleSystem.MainModule main = value[i].main;
				bool flag = pausedExplosions.Contains(item);
				main.simulationSpeed = Mathf.Lerp(main.simulationSpeed, flag ? 0f : 1f, 20f * Time.deltaTime);
			}
		}
	}

	public void SetPause(Spell1008ArcaneExplosion.ExplosionController ec, bool pause)
	{
		bool flag = pausedExplosions.Contains(ec);
		if (pause && !flag)
		{
			pausedExplosions.Add(ec);
		}
		if (!pause && flag)
		{
			pausedExplosions.Remove(ec);
		}
	}

	public void Play(Spell1008ArcaneExplosion.ExplosionController explosion)
	{
		CamController.Inst.SetShock(ArcaneExplosion.shock);
		ManualCreateEffect("Spell");
		ManualCreateEffect("Ground");
		ManualCreateEffect("Trail");
		ParticleSystem[] value = _particleSystems.ToArray();
		_particleSystems.Clear();
		particles[explosion] = value;
	}

	public void Stop(Spell1008ArcaneExplosion.ExplosionController explosion)
	{
		ParticleSystem[] array = particles[explosion];
		foreach (ParticleSystem particleSystem in array)
		{
			RecycleEffect(particleSystem.transform);
		}
		particles.Remove(explosion);
	}
}
