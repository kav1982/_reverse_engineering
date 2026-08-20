using UnityEngine;

public class HideBoundary_Theme0 : HideBoundaryBase
{
	public GameObject pfb_Ash;

	public float ashOffsetWhenLeft;

	public Vector3 torchOffset1;

	public Vector3 torchOffset2;

	public override void Initialize(RoomController roomCtrller, FourDir dir)
	{
		base.Initialize(roomCtrller, dir);
		switch (dir)
		{
		case FourDir.Right:
			base.transform.localScale = Vector3.one;
			break;
		case FourDir.Left:
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
			break;
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
			torchOffset1.x = 0f - torchOffset1.x;
			torchOffset2.x = 0f - torchOffset2.x;
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T0Blood", base.transform.position + torchOffset1).transform.localScale = new Vector3(-1f, 1f, 1f);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T0Blood", base.transform.position + torchOffset2).transform.localScale = new Vector3(-1f, 1f, 1f);
			break;
		case FourDir.Left:
			Object.Instantiate(pfb_Ash, base.transform.position + new Vector3(ashOffsetWhenLeft, 0f, 0f), Quaternion.identity, base.transform.parent);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T0Blood", base.transform.position + torchOffset1);
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/Access_Torch_T0Blood", base.transform.position + torchOffset2);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		Object.Destroy(base.gameObject);
	}
}
