using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

internal class Monster311AuthoringBaker : Baker<Monster311Authoring>
{
	public override void Bake(Monster311Authoring authoring)
	{
		BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);
		ref Monster311_Data reference = ref blobBuilder.ConstructRoot<Monster311_Data>();
		reference.idleDuration = authoring.idleDuration;
		reference.attackDistance = authoring.attackDistance;
		reference.shootDuration = authoring.shootDuration;
		reference.shootDuration = authoring.avoidDistance;
		reference.moveDuration = authoring.moveDuration;
		BlobAssetReference<Monster311_Data> blobAssetReference = blobBuilder.CreateBlobAssetReference<Monster311_Data>(Allocator.Persistent);
		blobBuilder.Dispose();
		AddBlobAsset(ref blobAssetReference, out var _);
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Monster311_Dots component = new Monster311_Dots
		{
			data = blobAssetReference,
			isPattern2 = authoring.isPattern2,
			laserOffset = authoring.laserOffset
		};
		AddComponent(entity, in component);
		EndlessMonsterTag component2 = new EndlessMonsterTag
		{
			dropCount = 1
		};
		AddComponent(entity, in component2);
		PhysicsMassOverride component3 = default(PhysicsMassOverride);
		AddComponent(entity, in component3);
	}
}
