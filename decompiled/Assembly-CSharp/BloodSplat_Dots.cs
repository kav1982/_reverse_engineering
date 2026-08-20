using Unity.Entities;

public struct BloodSplat_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float startAlphaPercent;

	public float endAlphaPercent;

	public float baseAlpha;

	public float fadeTime;

	public float baseScale;

	public float startScalePercent;

	public float scaleTime;

	public float existTime;

	public Entity bloodEntity;
}
