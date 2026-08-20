using Unity.Entities;
using Unity.Mathematics;

public struct GroundCoinsBuffer : IBufferElementData
{
	public bool IsInitialized;

	public int CoinAmount;

	public float3 Position;

	public Entity SourceEntity;
}
