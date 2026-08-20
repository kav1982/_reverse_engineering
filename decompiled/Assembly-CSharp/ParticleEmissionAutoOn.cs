using System.Collections.Generic;
using UnityEngine;

public class ParticleEmissionAutoOn : MonoBehaviour
{
	public List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

	private void OnEnable()
	{
		ParticleEmissionOn();
	}

	public void ParticleEmissionOn()
	{
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			if (!(particleSystem == null))
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				emission.enabled = true;
			}
		}
	}

	public void ParticleEmissionOff()
	{
		foreach (ParticleSystem particleSystem in _particleSystems)
		{
			if (!(particleSystem == null))
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				emission.enabled = false;
			}
		}
	}
}
