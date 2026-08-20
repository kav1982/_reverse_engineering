using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_RepeatCounter", -1)]
public struct MatOverrideRepeatCounter : IComponentData, IQueryTypeParameter
{
	public float RepeatCounter;
}
