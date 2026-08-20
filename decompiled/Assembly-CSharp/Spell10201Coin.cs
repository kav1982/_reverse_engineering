using Unity.Entities;
using UnityEngine;

public struct Spell10201Coin : IComponentData, IQueryTypeParameter
{
	public Entity ett_MR5;

	public Entity ett_MR20;

	public Entity ett_MR50;

	public Entity ett_MR100;

	public Entity ett_Anima;

	public bool isInitialized;

	public Vector2Int belongRoomMapPos;

	public bool needFlyHigh;

	public int coinCount;

	public bool onPick;
}
