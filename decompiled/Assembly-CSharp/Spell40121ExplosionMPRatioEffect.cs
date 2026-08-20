using UnityEngine;

public class Spell40121ExplosionMPRatioEffect : MonoBehaviour
{
	public ParticleSystem Particle;

	public Gradient Color;

	[Range(0f, 1f)]
	public float Ratio = 1f;

	private void Start()
	{
		SetRatio(Ratio);
	}

	private void SetRatio(float ratio)
	{
		ParticleSystem.MainModule main = Particle.main;
		main.startColor = Color.Evaluate(ratio);
	}
}
