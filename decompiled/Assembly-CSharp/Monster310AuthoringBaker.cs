using Unity.Collections;
using Unity.Entities;

internal class Monster310AuthoringBaker : Baker<Monster310Authoring>
{
	public override void Bake(Monster310Authoring authoring)
	{
		BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		BlobAssetReference<Monster310_Dots.Monster310_Data> blobAssetReference = default(BlobAssetReference<Monster310_Dots.Monster310_Data>);
		BlobAssetReference<Monster310_Dots.Monster310_Data_2> blobAssetReference2 = default(BlobAssetReference<Monster310_Dots.Monster310_Data_2>);
		if (authoring.pattern == AIPattern.Pattern1)
		{
			ref Monster310_Dots.Monster310_Data reference = ref blobBuilder.ConstructRoot<Monster310_Dots.Monster310_Data>();
			reference.gravity = authoring.gravity;
			reference.upSpeed = authoring.upSpeed;
			reference.maxJumpDistance = authoring.maxJumpDistance;
			blobAssetReference = blobBuilder.CreateBlobAssetReference<Monster310_Dots.Monster310_Data>(Allocator.Persistent);
			blobBuilder.Dispose();
			AddBlobAsset(ref blobAssetReference, out var _);
		}
		else
		{
			ref Monster310_Dots.Monster310_Data_2 reference2 = ref blobBuilder.ConstructRoot<Monster310_Dots.Monster310_Data_2>();
			reference2.gravity = authoring.gravity;
			reference2.upSpeed = authoring.upSpeed;
			reference2.maxJumpDistance = authoring.maxJumpDistance;
			blobAssetReference2 = blobBuilder.CreateBlobAssetReference<Monster310_Dots.Monster310_Data_2>(Allocator.Persistent);
			blobBuilder.Dispose();
			AddBlobAsset(ref blobAssetReference2, out var _);
		}
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster310_Dots component = new Monster310_Dots
		{
			data = blobAssetReference,
			data2 = blobAssetReference2,
			pattern = authoring.pattern,
			jumpOffsetRange = authoring.jumpOffsetRange
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 4
		};
		AddComponent(entity, in component2);
	}
}
