using UnityEngine;

public class Boundary_T8 : BoundaryBase
{
	private MeshCollider[] groundMCs;

	private float minX;

	private float maxX;

	private float minZ;

	private float maxZ;

	public MeshCollider[] GroundMCs => groundMCs;

	public float MinX => minX;

	public float MaxX => maxX;

	public float MinZ => minZ;

	public float MaxZ => maxZ;

	public override void Correct(Vector2Data selfPoint, RoomController roomCtrller)
	{
	}
}
