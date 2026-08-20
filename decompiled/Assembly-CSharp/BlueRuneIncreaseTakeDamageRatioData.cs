using Unity.Entities;

public struct BlueRuneIncreaseTakeDamageRatioData : IBufferElementData
{
	public Entity TargetEntity;

	public float EffectDuration;

	public float EffectRatio;
}
