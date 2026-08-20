using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_MixPercent", -1)]
public struct MatOverrideMixPercent : IComponentData, IQueryTypeParameter
{
	public float mixPercent;
}
