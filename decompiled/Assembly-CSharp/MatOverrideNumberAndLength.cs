using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

[MaterialProperty("_NumberAndLength", -1)]
public struct MatOverrideNumberAndLength : IComponentData, IQueryTypeParameter
{
	public float3 numberAndLength;
}
