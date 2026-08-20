using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_ApplyGhostEffect", -1)]
public struct MatOverrideGhostEffect : IComponentData, IQueryTypeParameter
{
	public float ApplyGhostEffect;
}
