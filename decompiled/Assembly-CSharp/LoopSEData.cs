using Unity.Collections;
using Unity.Entities;

public struct LoopSEData : IBufferElementData
{
	public FixedString32Bytes SEName;

	public readonly float Duration;

	public float Volume;

	public LoopSEData(FixedString32Bytes seName, float duration, float volume = 1f)
	{
		SEName = seName;
		Duration = duration;
		Volume = volume;
	}
}
