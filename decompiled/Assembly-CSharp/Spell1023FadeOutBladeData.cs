using Unity.Entities;
using Unity.Mathematics;

public struct Spell1023FadeOutBladeData : IComponentData, IQueryTypeParameter
{
	public float3 Direction;

	public float Timer;

	public float MoveSpeed;

	public bool ShouldSpawnObject;
}
