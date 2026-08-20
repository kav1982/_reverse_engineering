using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct GetGO : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public FixedString512Bytes path;

	public float3 offset;

	public int waitFrame;

	public UnityObjectRef<GameObject> go;
}
