using Unity.Entities;

public struct AnimationCurveData : IComponentData, IQueryTypeParameter
{
	public BlobAssetReference<DotsAnimationCurveBlob> Curve1;

	public BlobAssetReference<DotsAnimationCurveBlob> Curve2;

	public BlobAssetReference<DotsAnimationCurveBlob> Curve3;
}
