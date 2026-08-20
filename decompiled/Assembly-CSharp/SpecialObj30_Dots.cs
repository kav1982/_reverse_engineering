using Unity.Entities;
using UnityEngine;

public struct SpecialObj30_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Center;

	public bool isInitialized;

	public bool hasFollowGO;

	public UnityObjectRef<GameObject> followGO;
}
