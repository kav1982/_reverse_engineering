using Unity.Entities;
using UnityEngine;

public struct SpecialObj101MoreInOne_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Effect;

	public bool isInitialized;

	public UnityObjectRef<GameObject> go_EF;

	public bool isUse;

	public bool isHandleUse;
}
