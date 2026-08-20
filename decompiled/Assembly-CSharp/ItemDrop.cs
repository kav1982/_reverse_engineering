using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ItemDrop : IComponentData, IQueryTypeParameter
{
	public bool isInitialized;

	public BlobAssetReference<BlobArray<ItemInfo>> infoArray;

	public BlobAssetReference<BlobArray<float3>> dropPosArray;

	public Vector2Int belongRoomMapPos;

	public float dropRadius;

	public float dropInterval;

	public float dropTimer;

	public int currentDropIndex;
}
