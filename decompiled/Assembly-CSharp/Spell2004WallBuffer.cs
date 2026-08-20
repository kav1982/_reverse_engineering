using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell2004WallBuffer : IBufferElementData
{
	public Entity Entity;

	public UnityObjectRef<LineRenderer> LineRenderer;

	public float WallDistance;

	public float3 WallDir;
}
