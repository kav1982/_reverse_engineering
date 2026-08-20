using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_ShadowSize", -1)]
public struct Spell1023ShadowMaterialOverride : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float Value;
}
