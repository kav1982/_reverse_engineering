using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_GroundHiddenHeight", -1)]
public struct Spell1023SpellMaterialOnGroundHideHeight : IComponentData, IQueryTypeParameter
{
	public float Value;
}
