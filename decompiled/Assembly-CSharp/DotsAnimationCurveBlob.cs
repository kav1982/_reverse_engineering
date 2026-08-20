using Unity.Entities;
using UnityEngine;

public struct DotsAnimationCurveBlob
{
	public BlobArray<DotsKeyframeBlob> Keyframes;

	public WrapMode PreWrapMode;

	public WrapMode PostWrapMode;
}
