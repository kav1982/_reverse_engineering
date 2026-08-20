using Unity.Mathematics;
using UnityEngine;

public class T8_Ornament : MonoBehaviour, IEightDir
{
	public Transform tsf_Layer;

	public Transform tsf_Flip;

	public GameObject[] randomKeepGOs;

	public bool isT30;

	public GameObject T30_Up;

	public GameObject T30_Down;

	public GameObject T30_Side;

	private EightDir eightDir;

	private RoomController roomCtrller;

	private void Start()
	{
		if (isT30)
		{
			switch (this.eightDir)
			{
			case EightDir.Left:
			case EightDir.Right:
			{
				Transform obj2 = tsf_Flip;
				EightDir eightDir = this.eightDir;
				obj2.localScale = new Vector3((eightDir != EightDir.Left && eightDir != EightDir.LeftUp && eightDir != EightDir.DownLeft) ? 1 : (-1), 1f, 1f);
				Object.Destroy(T30_Up);
				break;
			}
			case EightDir.LeftUp:
			case EightDir.DownLeft:
			case EightDir.UpRight:
			case EightDir.RightDown:
				if (Mathf.Abs(base.transform.position.x) - (float)(roomCtrller.roomCfg.width / 2) > Mathf.Abs(base.transform.position.y) - (float)(roomCtrller.roomCfg.height / 2))
				{
					Transform obj = tsf_Flip;
					EightDir eightDir = this.eightDir;
					obj.localScale = new Vector3((eightDir != EightDir.Left && eightDir != EightDir.LeftUp && eightDir != EightDir.DownLeft) ? 1 : (-1), 1f, 1f);
					Object.Destroy(T30_Up);
				}
				else
				{
					Object.Destroy(T30_Side);
				}
				break;
			case EightDir.Down:
			case EightDir.Up:
				Object.Destroy(T30_Side);
				break;
			}
		}
		else if (randomKeepGOs.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, randomKeepGOs.Length);
			for (int num2 = randomKeepGOs.Length - 1; num2 >= 0; num2--)
			{
				if (num2 != num)
				{
					Object.Destroy(randomKeepGOs[num2]);
				}
			}
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				tsf_Flip.localScale = new Vector3(-1f, 1f, 1f);
			}
		}
		EightDir eightDir2 = this.eightDir;
		if (eightDir2 == EightDir.LeftUp || (uint)(eightDir2 - 1) <= 1u)
		{
			Vector3 position = base.transform.position;
			position.y = base.transform.position.y - roomCtrller.transform.position.y;
			tsf_Layer.localPosition = new Vector3(0f, 0f, 1.26f + position.y * 0.001f);
		}
		else
		{
			Transform obj3 = tsf_Layer;
			float3 rootPosition = base.transform.position;
			obj3.localPosition = DTool.GetLayerPosition(in rootPosition, LayerCorrectType.Coordinate);
		}
	}

	public void SetDirAndRoomCtrller(EightDir dir, RoomController roomCtrller)
	{
		eightDir = dir;
		this.roomCtrller = roomCtrller;
	}
}
