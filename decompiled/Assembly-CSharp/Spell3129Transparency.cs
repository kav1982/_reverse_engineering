using UnityEngine;

public class Spell3129Transparency : MonoBehaviour
{
	private static readonly int Alpha = Shader.PropertyToID("_Alpha");

	public ParticleSystemRenderer TrailParticle;

	private void Update()
	{
		TrailParticle.material.SetFloat(Alpha, DataMgr.settingData.FinalSpellTransparent);
	}
}
