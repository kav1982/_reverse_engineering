using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct Spell9011RotateArrowData : IComponentData, IQueryTypeParameter
{
	public UnityObjectRef<GameObject> ShadowObj;

	public bool InitOver;

	public float rotateSpeed;

	public bool Aligned;

	public float3 TargetPos;
}
