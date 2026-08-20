using Unity.Mathematics;
using UnityEngine;

public class Tile_T15_Rail : MonoBehaviour, IEightDir
{
	public Transform tsf_Layer;

	public GameObject go_D;

	public GameObject go_R;

	public GameObject go_RD;

	public GameObject go_U;

	public GameObject go_UR;

	private float delayTimer;

	public void SetDirAndRoomCtrller(EightDir dir, RoomController roomCtrller)
	{
		Transform obj = tsf_Layer;
		float3 rootPosition = base.transform.position;
		obj.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		switch (dir)
		{
		case EightDir.Up:
			flag4 = true;
			flag6 = UnityEngine.Random.Range(0, 2) == 0;
			break;
		case EightDir.UpRight:
			flag5 = true;
			break;
		case EightDir.Right:
			flag2 = true;
			break;
		case EightDir.RightDown:
			flag3 = true;
			break;
		case EightDir.Down:
			flag = true;
			flag6 = UnityEngine.Random.Range(0, 2) == 0;
			break;
		case EightDir.DownLeft:
			flag3 = true;
			flag6 = true;
			break;
		case EightDir.Left:
			flag2 = true;
			flag6 = true;
			break;
		case EightDir.LeftUp:
			flag5 = true;
			flag6 = true;
			break;
		default:
			Debug.LogError(dir);
			break;
		}
		if (!flag)
		{
			Object.Destroy(go_D);
		}
		if (!flag2)
		{
			Object.Destroy(go_R);
		}
		if (!flag3)
		{
			Object.Destroy(go_RD);
		}
		if (!flag4)
		{
			Object.Destroy(go_U);
		}
		if (!flag5)
		{
			Object.Destroy(go_UR);
		}
		if (flag6)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	private void Update()
	{
		delayTimer += Time.deltaTime;
		if (delayTimer >= 0.1f)
		{
			base.transform.GetComponentInChildren<MeshRenderer>().material.SetFloat("_TimeOffset", UnityEngine.Random.Range(0f, 1000f));
			Object.Destroy(this);
		}
	}
}
