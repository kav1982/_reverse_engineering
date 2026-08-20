using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Progress", -1)]
public struct Spell1023SpellMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
