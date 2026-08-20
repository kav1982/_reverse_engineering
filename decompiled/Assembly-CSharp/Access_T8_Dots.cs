using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Access_T8_Dots : IComponentData, IQueryTypeParameter
{
	public Entity ett_Layer;

	public Entity ett_Access;

	public Entity ett_AccessNotNeedKey;

	public Entity ett_Offset;

	public Entity ett_OffsetNotNeedKey;

	public Entity ett_AccessLight;

	public Entity ett_AccessLightNotNeedKey;

	public float3 torch1Offset;

	public float3 torch2Offset;

	public float openFinalYOffset;

	public float openYOffsetSpeed;

	public Entity ett_Trigger;

	public UnityObjectRef<GameObject> go_Torch1;

	public UnityObjectRef<GameObject> go_Torch2;

	public bool isInitialized;

	public bool isOpening;

	public bool isClosing;

	public float openCurrentYOffset;
}
