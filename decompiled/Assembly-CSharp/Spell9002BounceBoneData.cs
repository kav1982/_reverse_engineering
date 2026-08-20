using Unity.Entities;

public struct Spell9002BounceBoneData : IComponentData, IQueryTypeParameter
{
	public bool InitOver;

	public float RotationSpeed;

	public int SEIndex;

	public Entity MeshEntity;

	public Entity ShadowEntity;

	public float RotateAngle;

	public int CurrentReboundCount;
}
