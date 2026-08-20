using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Offset", -1)]
public struct MatOverrideOffsetFloat : IComponentData, IQueryTypeParameter
{
	public float offset;
}
