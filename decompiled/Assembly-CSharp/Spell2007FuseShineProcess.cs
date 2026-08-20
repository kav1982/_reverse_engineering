using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_FuseShineProcess", -1)]
public struct Spell2007FuseShineProcess : IComponentData, IQueryTypeParameter
{
	public int Value;
}
