using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class HideBoundary_Theme6 : HideBoundaryBase
{
	[Space(50f)]
	public GameObject pfb_Ash;

	private EntityManager ettMgr;

	private Entity accessEtt;

	private float3 accessOriginalPosition;

	public override void Initialize(RoomController roomCtrller, FourDir dir)
	{
		base.Initialize(roomCtrller, dir);
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (roomCtrller.accessEttList.Count == 1)
		{
			accessEtt = roomCtrller.accessEttList[0];
			LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(accessEtt);
			accessOriginalPosition = componentData.Position;
			componentData.Position = new float3(500f, 0f, 0f);
			ettMgr.SetComponentData(accessEtt, componentData);
		}
		else
		{
			Debug.LogError("不应该有0个，或多个access:" + roomCtrller.accessEttList.Count);
		}
	}

	public override void Disappear()
	{
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(accessEtt);
		componentData.Position = accessOriginalPosition;
		ettMgr.SetComponentData(accessEtt, componentData);
		ettMgr.GetComponentData<Access_T6_Dots>(accessEtt).accessT6Mono.Value.transform.position = accessOriginalPosition;
		Object.Instantiate(pfb_Ash, base.transform.position, quaternion.identity, base.transform.parent);
	}
}
