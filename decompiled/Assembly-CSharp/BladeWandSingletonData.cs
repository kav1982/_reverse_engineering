using Unity.Entities;

public struct BladeWandSingletonData : IBufferElementData
{
	public int WandId;

	public int ShootCount;

	public float LightningChainDamage;

	public int LightningChainPenetrate;
}
