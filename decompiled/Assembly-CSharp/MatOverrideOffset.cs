using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[MaterialProperty("_Offset", -1)]
public struct MatOverrideOffset : IComponentData, IQueryTypeParameter
{
	public float2 offset;
}
