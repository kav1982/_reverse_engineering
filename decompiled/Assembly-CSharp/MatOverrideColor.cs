using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

[MaterialProperty("_Color", -1)]
public struct MatOverrideColor : IComponentData, IQueryTypeParameter
{
	public Color color;
}
