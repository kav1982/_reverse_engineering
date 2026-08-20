using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public static class DotsAnimationCurve
{
	public static BlobAssetReference<DotsAnimationCurveBlob> CreateBlobCurve(DotsKeyframeBlob[] keyframes, WrapMode preWrap = WrapMode.Once, WrapMode postWrap = WrapMode.Once)
	{
		BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		ref DotsAnimationCurveBlob reference = ref blobBuilder.ConstructRoot<DotsAnimationCurveBlob>();
		BlobBuilderArray<DotsKeyframeBlob> blobBuilderArray = blobBuilder.Allocate(ref reference.Keyframes, keyframes.Length);
		for (int i = 0; i < keyframes.Length; i++)
		{
			blobBuilderArray[i] = new DotsKeyframeBlob
			{
				Time = keyframes[i].Time,
				Value = keyframes[i].Value,
				InTangent = keyframes[i].InTangent,
				OutTangent = keyframes[i].OutTangent
			};
		}
		reference.PreWrapMode = preWrap;
		reference.PostWrapMode = postWrap;
		BlobAssetReference<DotsAnimationCurveBlob> result = blobBuilder.CreateBlobAssetReference<DotsAnimationCurveBlob>(Allocator.Persistent);
		blobBuilder.Dispose();
		return result;
	}

	public static float Evaluate(this BlobAssetReference<DotsAnimationCurveBlob> curve, float time)
	{
		ref BlobArray<DotsKeyframeBlob> keyframes = ref curve.Value.Keyframes;
		if (keyframes.Length == 0)
		{
			return 0f;
		}
		time = ApplyWrapMode(time, keyframes[0].Time, keyframes[keyframes.Length - 1].Time, curve.Value.PreWrapMode, curve.Value.PostWrapMode);
		int index = 0;
		int index2 = keyframes.Length - 1;
		for (int i = 1; i < keyframes.Length; i++)
		{
			if (time <= keyframes[i].Time)
			{
				index = i - 1;
				index2 = i;
				break;
			}
		}
		DotsKeyframeBlob dotsKeyframeBlob = keyframes[index];
		DotsKeyframeBlob dotsKeyframeBlob2 = keyframes[index2];
		float num = dotsKeyframeBlob2.Time - dotsKeyframeBlob.Time;
		float t = (time - dotsKeyframeBlob.Time) / num;
		return CubicInterpolate(dotsKeyframeBlob.Value, dotsKeyframeBlob.Value + dotsKeyframeBlob.OutTangent * num / 3f, dotsKeyframeBlob2.Value - dotsKeyframeBlob2.InTangent * num / 3f, dotsKeyframeBlob2.Value, t);
	}

	private static float CubicInterpolate(float p0, float p1, float p2, float p3, float t)
	{
		float num = t * t;
		float num2 = num * t;
		return (1f - 3f * t + 3f * num - num2) * p0 + (3f * t - 6f * num + 3f * num2) * p1 + (3f * num - 3f * num2) * p2 + num2 * p3;
	}

	private static float ApplyWrapMode(float t, float startTime, float endTime, WrapMode preWrap, WrapMode postWrap)
	{
		float num = endTime - startTime;
		if (t < startTime)
		{
			return preWrap switch
			{
				WrapMode.Loop => endTime - (startTime - t) % num, 
				WrapMode.PingPong => startTime + (startTime - t) % num, 
				_ => startTime, 
			};
		}
		if (t > endTime)
		{
			return postWrap switch
			{
				WrapMode.Loop => startTime + (t - startTime) % num, 
				WrapMode.PingPong => endTime - (t - endTime) % num, 
				_ => endTime, 
			};
		}
		return t;
	}
}
