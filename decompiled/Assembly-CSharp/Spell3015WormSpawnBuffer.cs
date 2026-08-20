using Unity.Entities;
using Unity.Mathematics;

public struct Spell3015WormSpawnBuffer : IBufferElementData
{
	public float3 spawnPosition;

	public SpellComponentData data;

	public SpellConfigComponentData config;

	public SpellElementEffectComponentData element;

	public float moveSpeed;

	public float radius;

	public SpellColorType wormColorType;
}
