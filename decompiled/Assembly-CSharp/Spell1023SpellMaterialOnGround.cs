using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_EnableHiddenUnderGround", -1)]
public struct Spell1023SpellMaterialOnGround : IComponentData, IQueryTypeParameter
{
	public float Value;
}
