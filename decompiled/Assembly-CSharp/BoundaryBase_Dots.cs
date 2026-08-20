using Unity.Entities;
using Unity.Mathematics;

public struct BoundaryBase_Dots : IComponentData, IQueryTypeParameter
{
	public int roomID;

	public bool shouldCreateDetail;

	public bool dontCreateIronChain;

	public float3 roomPosition;

	public Vector2Data selfPosition;

	public BlobAssetReference<BlobArray<Vector2Data>> allBoundary1Position;

	public BlobAssetReference<BlobArray<Vector2Data>> allBoundary2Position;

	public BlobAssetReference<BlobArray<Vector2Data>> allTile0Position;
}
