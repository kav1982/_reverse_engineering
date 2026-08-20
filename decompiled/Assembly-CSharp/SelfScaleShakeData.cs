using Unity.Entities;

public struct SelfScaleShakeData : IComponentData, IQueryTypeParameter
{
	public float BaseScale;

	public float BonusScale;

	public float ShakeSpeed;
}
