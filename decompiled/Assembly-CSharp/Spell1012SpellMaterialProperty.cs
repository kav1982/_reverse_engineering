using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_IsFlashing", -1)]
public struct Spell1012SpellMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
