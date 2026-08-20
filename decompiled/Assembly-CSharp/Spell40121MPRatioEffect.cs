using UnityEngine;

public class Spell40121MPRatioEffect : MonoBehaviour
{
	public MeshRenderer ShieldMeshRenderer;

	public ParticleSystemRenderer ShieldParticle;

	public ParticleSystemRenderer HitParticle;

	[Header("Color By MP Ratio")]
	public Gradient MeshColor;

	public Gradient ParticleColor;

	[Range(0f, 1f)]
	public float Ratio = 1f;

	private static readonly int FINAL_COLOR = Shader.PropertyToID("_FinalColor");

	private static readonly int BORDER_COLOR = Shader.PropertyToID("_BorderColor");

	private static readonly int COLOR = Shader.PropertyToID("_Color");

	private void Update()
	{
		SetRatio(Ratio);
	}

	private void SetRatio(float ratio)
	{
		Color value = MeshColor.Evaluate(ratio);
		ShieldMeshRenderer.material.SetColor(COLOR, value);
		value.a = 1f;
		ShieldMeshRenderer.material.SetColor(BORDER_COLOR, value);
		Color value2 = ParticleColor.Evaluate(ratio);
		ShieldParticle.material.SetColor(FINAL_COLOR, value2);
		HitParticle.material.SetColor(FINAL_COLOR, value2);
	}
}
