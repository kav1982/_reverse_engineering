using Unity.Entities;
using Unity.Mathematics;

public struct SpellKeepCastingAttach : IComponentData, IQueryTypeParameter, IEnableableComponent
{
	public float Offset;

	public float DirOffset;

	public float2 FallPositionOffset;
}
