using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_FuseProgress", -1)]
public struct MatOverrideFuseProgress : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float Progress;
}
