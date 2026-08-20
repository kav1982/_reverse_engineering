using Unity.Entities;

internal class CorpseImageAuthoringBaker : Baker<CorpseAuthoring>
{
	public override void Bake(CorpseAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		Corpse_Dots component = new Corpse_Dots
		{
			imageCount = authoring.images.Count,
			harmonyImageCount = authoring.imagesHarmony.Count
		};
		AddComponent(entity, in component);
		DynamicBuffer<CorpseImageEtt> dynamicBuffer = AddBuffer<CorpseImageEtt>(entity);
		for (int i = 0; i < authoring.images.Count; i++)
		{
			dynamicBuffer.Add(new CorpseImageEtt
			{
				entity = GetEntity(authoring.images[i], TransformUsageFlags.Dynamic)
			});
		}
		for (int j = 0; j < authoring.imagesHarmony.Count; j++)
		{
			dynamicBuffer.Add(new CorpseImageEtt
			{
				entity = GetEntity(authoring.imagesHarmony[j], TransformUsageFlags.Dynamic)
			});
		}
	}
}
