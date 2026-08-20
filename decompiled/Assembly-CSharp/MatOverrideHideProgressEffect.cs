using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_HideProgress", -1)]
public struct MatOverrideHideProgressEffect : IComponentData, IQueryTypeParameter
{
	public float Progress;
}
