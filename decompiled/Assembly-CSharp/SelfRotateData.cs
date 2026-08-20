using Unity.Entities;

public struct SelfRotateData : IComponentData, IQueryTypeParameter
{
	public float RotateSpeed;
}
