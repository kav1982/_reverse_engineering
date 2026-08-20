using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class AnimationCurveAuthoring : MonoBehaviour
{
	public class AnimationCurveAuthoringBaker : Baker<AnimationCurveAuthoring>
	{
		public override void Bake(AnimationCurveAuthoring authoring)
		{
			BlobAssetReference<DotsAnimationCurveBlob> blobRefByCurve = GetBlobRefByCurve(authoring.curve1);
			BlobAssetReference<DotsAnimationCurveBlob> blobRefByCurve2 = GetBlobRefByCurve(authoring.curve2);
			BlobAssetReference<DotsAnimationCurveBlob> blobRefByCurve3 = GetBlobRefByCurve(authoring.curve3);
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AnimationCurveData component = new AnimationCurveData
			{
				Curve1 = blobRefByCurve,
				Curve2 = blobRefByCurve2,
				Curve3 = blobRefByCurve3
			};
			AddComponent(entity, in component);
		}

		private BlobAssetReference<DotsAnimationCurveBlob> GetBlobRefByCurve(AnimationCurve curve)
		{
			Keyframe[] keys = curve.keys;
			BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
			ref DotsAnimationCurveBlob reference = ref blobBuilder.ConstructRoot<DotsAnimationCurveBlob>();
			BlobBuilderArray<DotsKeyframeBlob> blobBuilderArray = blobBuilder.Allocate(ref reference.Keyframes, keys.Length);
			for (int i = 0; i < keys.Length; i++)
			{
				Keyframe keyframe = keys[i];
				blobBuilderArray[i] = new DotsKeyframeBlob
				{
					Time = keyframe.time,
					Value = keyframe.value,
					InTangent = keyframe.inTangent,
					OutTangent = keyframe.outTangent
				};
			}
			reference.PreWrapMode = curve.preWrapMode;
			reference.PostWrapMode = curve.postWrapMode;
			BlobAssetReference<DotsAnimationCurveBlob> result = blobBuilder.CreateBlobAssetReference<DotsAnimationCurveBlob>(Allocator.Persistent);
			blobBuilder.Dispose();
			return result;
		}
	}

	public AnimationCurve curve1;

	public AnimationCurve curve2;

	public AnimationCurve curve3;
}
