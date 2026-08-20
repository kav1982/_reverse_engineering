using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_RotateAngle", -1)]
public struct Spell2004RotateAngleMaterialProperty : IComponentData, IQueryTypeParameter
{
	public float Value;
}
