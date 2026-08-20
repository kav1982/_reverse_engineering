using Unity.Entities;
using Unity.Mathematics;

public struct SpellRecheckRefractDirectionData : IBufferElementData
{
	public float3 TargetPos;

	public Entity SpellEntity;
}
