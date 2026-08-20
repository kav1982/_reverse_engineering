using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Fill", -1)]
public struct MatOverrideFill : IComponentData, IQueryTypeParameter
{
	public float fill;
}
