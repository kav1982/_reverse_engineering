using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class HideBoundary_Theme29 : HideBoundaryBase
{
	public string lightPath;

	public float3 lightOffsetLeft;

	public float3 lightOffsetRight;

	public override void Disappear()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpecialObj45));
		SpecialObj45 singleton = entityQuery.GetSingleton<SpecialObj45>();
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(singleton.ett_AccessRight);
		componentData.Scale = 1f;
		entityManager.SetComponentData(singleton.ett_AccessRight, componentData);
		LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(singleton.ett_WallRight);
		componentData2.Scale = 0f;
		componentData2.Position += new float3(0f, 1000f, 0f);
		entityManager.SetComponentData(singleton.ett_WallRight, componentData2);
		using EntityQuery entityQuery2 = entityManager.CreateEntityQuery(typeof(SpecialObj45BloodRoom));
		SpecialObj45BloodRoom singleton2 = entityQuery2.GetSingleton<SpecialObj45BloodRoom>();
		LocalTransform componentData3 = entityManager.GetComponentData<LocalTransform>(singleton2.ett_Wall);
		componentData3.Scale = 0f;
		componentData3.Position += new float3(0f, 1000f, 0f);
		entityManager.SetComponentData(singleton2.ett_Wall, componentData3);
		Object.Destroy(base.gameObject);
	}
}
