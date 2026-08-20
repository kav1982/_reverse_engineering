using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Progress", -1)]
public struct ItemFlashProgressMaterialOverride : IComponentData, IQueryTypeParameter
{
	public float Progress;
}
