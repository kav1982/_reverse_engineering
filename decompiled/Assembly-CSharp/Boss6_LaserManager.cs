using System.Collections.Generic;
using UnityEngine;

public class Boss6_LaserManager : MonoBehaviour
{
	public List<Boss6_Laser> leftLasers = new List<Boss6_Laser>();

	public List<Boss6_Laser> upLasers = new List<Boss6_Laser>();

	public List<Boss6_Laser> leftUpLasers = new List<Boss6_Laser>();

	public List<Boss6_Laser> leftDownLasers = new List<Boss6_Laser>();

	public float straightLaserInterval;

	public float diagonalLaserInterval;

	private float roomWidth;

	private float roomHeight;

	private Vector3 roomCenter;

	private void Start()
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		for (int i = 0; i < Mathf.FloorToInt(roomWidth / straightLaserInterval) + 1; i++)
		{
			leftLasers.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/SE_Boss6_Laser", base.transform.position).GetComponent<Boss6_Laser>());
		}
		for (int j = 0; j < Mathf.FloorToInt(roomHeight / straightLaserInterval) + 1; j++)
		{
			upLasers.Add(ObjPoolMgr.Inst.GetGO("Prefabs/EF/SE_Boss6_Laser", base.transform.position).GetComponent<Boss6_Laser>());
		}
	}

	private void Update()
	{
	}
}
