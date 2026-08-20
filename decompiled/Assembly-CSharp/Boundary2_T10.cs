using Unity.Entities;
using UnityEngine;

public struct Boundary2_T10 : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	[Header("Potion")]
	[Range(0f, 1f)]
	public float detailChancePotion;

	public int intervalPotion;

	public float offsetPotion;

	public Entity ett_DetailPotion;

	[Header("Store")]
	[Range(0f, 1f)]
	public float detailChanceStore;

	public int intervalStore;

	public float offsetStore;

	public Entity ett_DetailStore;
}
