using UnityEngine;

public class GhostHarmonious : MonoBehaviour
{
	public Gradient gradientHarmonious;

	public Material ghostMaterialHarmonious;

	public TrailRenderer TrailRenderer;

	private void Start()
	{
		if (GameMgr.IsHarmony_Static)
		{
			TrailRenderer.material = ghostMaterialHarmonious;
			TrailRenderer.colorGradient = gradientHarmonious;
		}
	}
}
