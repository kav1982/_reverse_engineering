using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Progress", -1)]
public struct Spell1017DeathAdderTraceProgress : IComponentData, IQueryTypeParameter
{
	public float Progress;
}
