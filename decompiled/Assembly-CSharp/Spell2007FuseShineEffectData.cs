using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_UseFuseShineEffect", -1)]
public struct Spell2007FuseShineEffectData : IComponentData, IQueryTypeParameter
{
	public int Value;
}
