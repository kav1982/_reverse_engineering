using Unity.Entities;
using Unity.Mathematics;

public struct Monster311_LaserBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public Entity attackEntity;

	public Entity targetEntity;

	public float3 targetPosition;

	public bool isPattern2;

	public bool buffed;
}
