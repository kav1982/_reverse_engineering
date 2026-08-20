using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_BeHit", -1)]
public struct Spell1002BeHitMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
