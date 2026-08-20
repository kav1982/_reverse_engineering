using Unity.Entities;
using Unity.Mathematics;

public struct TileBase_Dots : IComponentData, IQueryTypeParameter
{
	public float3 roomPosition;

	public Vector2Data selfPosition;

	public BlobAssetReference<BlobArray<Vector2Data>> allTilePosition;
}
