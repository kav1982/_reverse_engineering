using Unity.Entities;
using UnityEngine;

internal class AllCorpseEttAuthoringBaker : Baker<AllCorpseEttAuthoring>
{
	public override void Bake(AllCorpseEttAuthoring authoring)
	{
		Entity entity = GetEntity(TransformUsageFlags.Dynamic);
		DynamicBuffer<CorpseInfo_Dots> dynamicBuffer = AddBuffer<CorpseInfo_Dots>(entity);
		for (int i = 0; i < authoring.corpsePrefabs.Count; i++)
		{
			CorpseInfo corpseInfo = authoring.corpsePrefabs[i];
			dynamicBuffer.Add(new CorpseInfo_Dots
			{
				type = corpseInfo.type,
				ett = (corpseInfo.prefab ? GetEntity(corpseInfo.prefab, TransformUsageFlags.NonUniformScale) : Entity.Null),
				forwardForceNoDirect = corpseInfo.forwardForceNoDirect,
				forwardForceHaveDirect = corpseInfo.forwardForceHaveDirect,
				upForce = corpseInfo.upForce,
				scale = corpseInfo.scale,
				bounceTime = corpseInfo.bounceTime,
				rotateSpeed = corpseInfo.rotateSpeed,
				angleOffset = corpseInfo.angleOffset,
				bounceRemainRatio = corpseInfo.bounceRemainRatio,
				gravity = corpseInfo.gravity,
				duration = corpseInfo.duration,
				reduceAlphaSpeed = corpseInfo.reduceAlphaSpeed,
				minAlpha = corpseInfo.minAlpha,
				colorCount = corpseInfo.colors.Count,
				color0 = ((corpseInfo.colors.Count > 0) ? corpseInfo.colors[0] : Color.white),
				color1 = ((corpseInfo.colors.Count > 1) ? corpseInfo.colors[1] : Color.white),
				color2 = ((corpseInfo.colors.Count > 2) ? corpseInfo.colors[2] : Color.white)
			});
		}
	}
}
