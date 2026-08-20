using Unity.Collections;
using Unity.Entities;

public struct SEData : IBufferElementData
{
	public FixedString32Bytes SEName;

	public readonly SEPlayMode SEMode;

	public readonly int SEMaxCount;

	public readonly float SEMinInterval;

	public readonly float SEPitch;

	public SEData(FixedString32Bytes seName, SEPlayMode seMode = SEPlayMode.Replay, int seMaxCount = 3, float seMinInterval = 0.05f, float sePitch = 1f)
	{
		SEName = seName;
		SEMode = seMode;
		SEMaxCount = seMaxCount;
		SEMinInterval = seMinInterval;
		SEPitch = sePitch;
	}
}
