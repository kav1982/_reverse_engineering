using Unity.Entities;

internal class BloodSplatAuthoringBaker : Baker<BloodSplatAuthoring>
{
	public override void Bake(BloodSplatAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.NonUniformScale);
		BloodSplat_Dots component = new BloodSplat_Dots
		{
			startAlphaPercent = authoring.startAlphaPercent,
			endAlphaPercent = authoring.endAlphaPercent,
			baseAlpha = authoring.baseAlpha,
			fadeTime = authoring.fadeTime,
			startScalePercent = authoring.startScalePercent,
			scaleTime = authoring.scaleTime
		};
		AddComponent(entity, in component);
		DynamicBuffer<BloodSplatElement> dynamicBuffer = AddBuffer<BloodSplatElement>(entity);
		for (int i = 0; i < authoring.bloodObjs.Count; i++)
		{
			dynamicBuffer.Add(new BloodSplatElement
			{
				entity = GetEntity(authoring.bloodObjs[i], TransformUsageFlags.Dynamic)
			});
		}
	}
}
