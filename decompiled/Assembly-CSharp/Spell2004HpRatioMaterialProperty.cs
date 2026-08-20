using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_HpPercent", -1)]
public struct Spell2004HpRatioMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
