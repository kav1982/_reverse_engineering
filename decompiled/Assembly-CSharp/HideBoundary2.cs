using UnityEngine;

public class HideBoundary2 : HideBoundaryBase
{
	public Transform tsf_Boundary;

	public Transform tsf_Shadow;

	public GameObject pfb_Ash;

	public float ashOffsetWhenLeft;

	public override void Initialize(RoomController roomCtrller, FourDir dir)
	{
		base.Initialize(roomCtrller, dir);
		switch (dir)
		{
		case FourDir.Right:
			tsf_Boundary.localScale = new Vector3(1f, 1f, 1f);
			break;
		case FourDir.Left:
			tsf_Boundary.localScale = new Vector3(-1f, 1f, 1f);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		tsf_Boundary.position = Tool2D.GetLayerPoint(base.transform) + new Vector3(0f, 0f, -3f);
		tsf_Shadow.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
	}

	public override void Disappear()
	{
		switch (dir)
		{
		case FourDir.Right:
			Object.Instantiate(pfb_Ash, base.transform.position, Quaternion.identity, base.transform.parent);
			break;
		case FourDir.Left:
			Object.Instantiate(pfb_Ash, base.transform.position + new Vector3(ashOffsetWhenLeft, 0f, 0f), Quaternion.identity, base.transform.parent);
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		Object.Destroy(base.gameObject);
	}
}
