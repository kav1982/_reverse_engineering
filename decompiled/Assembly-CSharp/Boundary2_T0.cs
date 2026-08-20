using Unity.Entities;

public struct Boundary2_T0 : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float iconChainChance;

	public int ironChainPerMeter;

	public float offset;

	public Entity ettIronChain;
}
