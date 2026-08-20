using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_FrameIndex", -1)]
public struct Spell1025ChangeFrameAnim : IComponentData, IQueryTypeParameter
{
	public float FrameIndex;
}
