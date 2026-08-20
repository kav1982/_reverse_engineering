using Unity.Collections;
using Unity.Entities;

internal class UnitFrameAnimaAuthoring1Baker : Baker<UnitFrameAnimaAuthoring>
{
	public override void Bake(UnitFrameAnimaAuthoring authoring)
	{
		BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		BlobBuilderArray<UnitFrameAnimaClip> blobBuilderArray = blobBuilder.Allocate(ref blobBuilder.ConstructRoot<UnitFrameAnimaData>().clips, authoring.animaClipList.Count);
		for (int i = 0; i < authoring.animaClipList.Count; i++)
		{
			blobBuilderArray[i] = authoring.animaClipList[i];
		}
		BlobAssetReference<UnitFrameAnimaData> blobAssetReference = blobBuilder.CreateBlobAssetReference<UnitFrameAnimaData>(Allocator.Persistent);
		blobBuilder.Dispose();
		AddBlobAsset(ref blobAssetReference, out var _);
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		UnitFrameAnima component = new UnitFrameAnima
		{
			indexEntity = GetEntity(authoring.frameIndexGO, TransformUsageFlags.Dynamic),
			data = blobAssetReference
		};
		AddComponent(entity, in component);
	}
}
