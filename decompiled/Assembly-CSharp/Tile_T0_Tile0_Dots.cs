using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Tile_T0_Tile0_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	[Range(0f, 1f)]
	public float variationChance;

	public Entity ett_Tile0Base;

	[Range(0f, 1f)]
	[Header("Tile1")]
	public float tile1Chance;

	public Entity ett_Tile1;

	public int tile1CellWidth;

	public float tile1Scale;

	public float3 tile1Offset;
}
