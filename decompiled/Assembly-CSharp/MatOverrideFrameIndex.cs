using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_FrameIndex", -1)]
public struct MatOverrideFrameIndex : IComponentData, IQueryTypeParameter
{
	public float FrameIndex;
}
