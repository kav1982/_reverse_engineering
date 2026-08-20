using Unity.Entities;
using UnityEngine;

public struct Tile_T1_Tile0_Dots : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	[Range(0f, 1f)]
	public float variationChance;

	public Entity ett_Tile0Base;

	[Header("Tile1")]
	[Range(0f, 1f)]
	public float tile1Chance;

	public Entity ett_Tile1;

	public int tile1CellWidth;
}
