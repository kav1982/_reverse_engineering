using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_ProgressAlpha", -1)]
public struct Spell1008SpellMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
