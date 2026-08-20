using Unity.Entities;
using Unity.Mathematics;

public struct Monster327MissileLaunch_Dots : IComponentData, IQueryTypeParameter
{
	public float3 initialDirection;

	public Entity target;

	public Entity shooter;
}
