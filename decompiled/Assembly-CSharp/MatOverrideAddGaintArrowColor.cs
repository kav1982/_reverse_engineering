using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_AddGaintArrowColor", -1)]
public struct MatOverrideAddGaintArrowColor : IComponentData, IQueryTypeParameter
{
	public float addGaintArrowColor;
}
