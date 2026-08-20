using System;
using UnityEngine;

public class EffectDisableWhenZeroTransparency : MonoBehaviour
{
	public EffectTransparencyController.ControlMode mode;

	public Renderer[] needDisableRenderers = Array.Empty<Renderer>();

	public ParticleSystem[] needDisableParticles = Array.Empty<ParticleSystem>();

	private bool _effectEnable = true;

	public void Update()
	{
		float transparency = mode.GetTransparency();
		if (transparency > 0.01f && !_effectEnable)
		{
			EnableComponents();
			_effectEnable = true;
		}
		if (transparency <= 0.01f && _effectEnable)
		{
			DisableComponents();
			_effectEnable = false;
		}
	}

	private void DisableComponents()
	{
		Renderer[] array = needDisableRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		ParticleSystem[] array2 = needDisableParticles;
		for (int i = 0; i < array2.Length; i++)
		{
			ParticleSystem.EmissionModule emission = array2[i].emission;
			emission.enabled = false;
		}
	}

	private void EnableComponents()
	{
		Renderer[] array = needDisableRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		ParticleSystem[] array2 = needDisableParticles;
		for (int i = 0; i < array2.Length; i++)
		{
			ParticleSystem.EmissionModule emission = array2[i].emission;
			emission.enabled = true;
		}
	}
}
