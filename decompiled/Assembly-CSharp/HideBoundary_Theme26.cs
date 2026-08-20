using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class HideBoundary_Theme26 : HideBoundaryBase
{
	public override void Initialize(RoomController roomCtrller, FourDir dir)
	{
		base.Initialize(roomCtrller, dir);
		StartCoroutine(InitializedIE());
	}

	private IEnumerator InitializedIE()
	{
		yield return null;
		yield return null;
		yield return null;
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpecialObj42));
		SpecialObj42 singleton = entityQuery.GetSingleton<SpecialObj42>();
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(singleton.ett_AccessLeft);
		componentData.Scale = 0f;
		entityManager.SetComponentData(singleton.ett_AccessLeft, componentData);
	}

	public override void Disappear()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		using EntityQuery entityQuery = entityManager.CreateEntityQuery(typeof(SpecialObj42));
		SpecialObj42 singleton = entityQuery.GetSingleton<SpecialObj42>();
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(singleton.ett_AccessLeft);
		componentData.Scale = 1f;
		entityManager.SetComponentData(singleton.ett_AccessLeft, componentData);
		LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(singleton.ett_WallLeft);
		componentData2.Scale = 0f;
		entityManager.SetComponentData(singleton.ett_WallLeft, componentData2);
		entityManager.DestroyEntity(singleton.ett_ColliderLeftMiddle);
		using EntityQuery entityQuery2 = entityManager.CreateEntityQuery(typeof(Access_T26));
		NativeArray<Entity> nativeArray = entityQuery2.ToEntityArray(Allocator.TempJob);
		float3 @float = float3.zero;
		string path = null;
		float3 float2 = float3.zero;
		foreach (Entity item in nativeArray)
		{
			AccessBase_Dots componentData3 = entityManager.GetComponentData<AccessBase_Dots>(item);
			if (componentData3.Dir == FourDir.Left)
			{
				@float = entityManager.GetComponentData<LocalTransform>(item).Position;
				Access_T26 componentData4 = entityManager.GetComponentData<Access_T26>(item);
				ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T26", @float + componentData4.torch1Offset);
				ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T26", @float + componentData4.torch2Offset);
			}
			else if (componentData3.Dir == FourDir.Right)
			{
				GetGO componentData5 = entityManager.GetComponentData<GetGO>(item);
				path = componentData5.path.Value;
				float2 = componentData5.offset;
				float2.x = 0f - float2.x;
			}
		}
		ObjPoolMgr.Inst.GetGO(path, @float + float2);
		nativeArray.Dispose();
		Object.Destroy(base.gameObject);
	}
}
