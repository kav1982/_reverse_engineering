using Unity.Entities;
using UnityEngine;

public class HideBoundary_Theme3 : HideBoundaryBase
{
	public Vector3 leafOffset;

	public GameObject pfb_Ash;

	public float ashOffsetWhenLeft;

	public Vector3 torchOffset1;

	public Vector3 torchOffset2;

	public override void Initialize(RoomController roomCtrller, FourDir dir)
	{
		base.Initialize(roomCtrller, dir);
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		switch (dir)
		{
		case FourDir.Right:
		{
			base.transform.localScale = Vector3.one;
			Vector3 vector3 = roomCtrller.GetAccessPoint(FourDir.Left) + new Vector3(0f - leafOffset.x - 1f, leafOffset.y, leafOffset.z);
			Entity entity3 = QuickCreateSystem.Inst.CreateSpecialObj(3002, vector3);
			entityManager.SetComponentData(entity3, new ITrap_Dots
			{
				belongRoom = LevelMgr.Inst.CurrentRoomCtrller
			});
			Entity entity4 = QuickCreateSystem.Inst.CreateSpecialObj(3004, base.transform.position + leafOffset);
			SpecialObj30_Dots componentData2 = entityManager.GetComponentData<SpecialObj30_Dots>(entity4);
			componentData2.hasFollowGO = true;
			componentData2.followGO.Value = base.gameObject;
			entityManager.SetComponentData(entity4, componentData2);
			entityManager.SetComponentData(entity4, new ITrap_Dots
			{
				belongRoom = LevelMgr.Inst.CurrentRoomCtrller
			});
			break;
		}
		case FourDir.Left:
		{
			torchOffset1.x = 0f - torchOffset1.x;
			torchOffset2.x = 0f - torchOffset2.x;
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 vector = base.transform.position + new Vector3(0f - leafOffset.x, leafOffset.y, leafOffset.z);
			Entity entity = QuickCreateSystem.Inst.CreateSpecialObj(3002, vector);
			SpecialObj30_Dots componentData = entityManager.GetComponentData<SpecialObj30_Dots>(entity);
			componentData.hasFollowGO = true;
			componentData.followGO.Value = base.gameObject;
			entityManager.SetComponentData(entity, componentData);
			entityManager.SetComponentData(entity, new ITrap_Dots
			{
				belongRoom = LevelMgr.Inst.CurrentRoomCtrller
			});
			Vector3 vector2 = roomCtrller.GetAccessPoint(FourDir.Right) + new Vector3(leafOffset.x + 1f, leafOffset.y, leafOffset.z);
			Entity entity2 = QuickCreateSystem.Inst.CreateSpecialObj(3004, vector2);
			entityManager.SetComponentData(entity2, new ITrap_Dots
			{
				belongRoom = LevelMgr.Inst.CurrentRoomCtrller
			});
			break;
		}
		default:
			Debug.LogError(dir);
			break;
		}
	}

	public override void Disappear()
	{
		switch (dir)
		{
		case FourDir.Right:
			Object.Instantiate(pfb_Ash, base.transform.position, Quaternion.identity, base.transform.parent);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T3Blood", base.transform.position + torchOffset1).transform.localScale = new Vector3(-1f, 1f, 1f);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T3Blood", base.transform.position + torchOffset2).transform.localScale = new Vector3(-1f, 1f, 1f);
			break;
		case FourDir.Left:
			Object.Instantiate(pfb_Ash, base.transform.position + new Vector3(ashOffsetWhenLeft, 0f, 0f), Quaternion.identity, base.transform.parent);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T3Blood", base.transform.position + torchOffset1);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T3Blood", base.transform.position + torchOffset2);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		Object.Destroy(base.gameObject);
	}
}
