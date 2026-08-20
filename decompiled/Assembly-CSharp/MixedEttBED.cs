using Unity.Collections;
using Unity.Entities;

public struct MixedEttBED : IBufferElementData
{
	public FixedString128Bytes name;

	public Entity ett;
}
