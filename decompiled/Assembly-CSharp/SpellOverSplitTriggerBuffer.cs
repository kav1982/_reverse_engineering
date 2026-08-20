using Unity.Entities;

[InternalBufferCapacity(8)]
public struct SpellOverSplitTriggerBuffer : IBufferElementData
{
	public float DamageRatio;

	public int Count;
}
