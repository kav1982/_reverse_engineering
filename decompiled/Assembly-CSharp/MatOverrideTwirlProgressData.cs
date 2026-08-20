using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_TwirlProgress", -1)]
public struct MatOverrideTwirlProgressData : IComponentData, IQueryTypeParameter
{
	public float Progress;
}
