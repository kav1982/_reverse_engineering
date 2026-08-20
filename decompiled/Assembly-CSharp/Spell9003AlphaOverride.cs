using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_GroundHiddenHeight", -1)]
public struct Spell9003AlphaOverride : IComponentData, IQueryTypeParameter
{
	public float value;
}
