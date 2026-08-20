using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class HideBoundary_Theme28 : HideBoundaryBase
{
	public string lightPath;

	public float3 lightOffsetLeft;

	public float3 lightOffsetRight;

	public override void Disappear()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpecialObj44));
		SpecialObj44 singleton = entityQuery.GetSingleton<SpecialObj44>();
		if (dir == FourDir.Left)
		{
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(singleton.ett_LeftAccess);
			componentData.Scale = 1f;
			entityManager.SetComponentData(singleton.ett_LeftAccess, componentData);
			LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(singleton.ett_LeftWall);
			componentData2.Scale = 0f;
			entityManager.SetComponentData(singleton.ett_LeftWall, componentData2);
			entityManager.DestroyEntity(singleton.ett_LeftColliderMiddle);
		}
		else
		{
			LocalTransform componentData3 = entityManager.GetComponentData<LocalTransform>(singleton.ett_RightAccess);
			componentData3.Scale = 1f;
			entityManager.SetComponentData(singleton.ett_RightAccess, componentData3);
			LocalTransform componentData4 = entityManager.GetComponentData<LocalTransform>(singleton.ett_RightWall);
			componentData4.Scale = 0f;
			entityManager.SetComponentData(singleton.ett_RightWall, componentData4);
			entityManager.DestroyEntity(singleton.ett_RightColliderMiddle);
		}
		using EntityQuery entityQuery2 = entityManager.CreateEntityQuery(typeof(Access_T28));
		NativeArray<Entity> nativeArray = entityQuery2.ToEntityArray(Allocator.TempJob);
		foreach (Entity item in nativeArray)
		{
			AccessBase_Dots componentData5 = entityManager.GetComponentData<AccessBase_Dots>(item);
			if (componentData5.roomType == RoomType.Boss && (componentData5.Dir == FourDir.Left || componentData5.Dir == FourDir.Right))
			{
				float3 position = entityManager.GetComponentData<LocalTransform>(item).Position;
				Access_T28 componentData6 = entityManager.GetComponentData<Access_T28>(item);
				GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T28", position + componentData6.torch1Offset);
				GameObject gO2 = ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T28", position + componentData6.torch2Offset);
				if (componentData5.Dir == FourDir.Right)
				{
					gO.transform.localScale = new Vector3(-1f, 1f, 1f);
					gO2.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				if (componentData5.Dir == FourDir.Left)
				{
					ObjPoolMgr.Inst.GetGO(lightPath, position + lightOffsetLeft);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO(lightPath, position + lightOffsetRight);
				}
			}
		}
		nativeArray.Dispose();
		Object.Destroy(base.gameObject);
	}
}
