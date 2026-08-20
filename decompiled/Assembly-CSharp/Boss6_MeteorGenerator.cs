using UnityEngine;

public class Boss6_MeteorGenerator : MonoBehaviour
{
	public float meteorInterval;

	private float meteorTimer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private bool casting;

	public Boss6_Stage2 master;

	public void StartCast()
	{
		casting = true;
	}

	public void StopCast()
	{
		casting = false;
	}

	private void Start()
	{
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y;
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
	}

	private void Update()
	{
		if (!casting)
		{
			return;
		}
		meteorTimer += Time.deltaTime;
		if (meteorTimer > meteorInterval)
		{
			meteorTimer -= meteorInterval;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_BigMeteor", base.transform.position).GetComponent<Boss6_SingleQuake>();
			_ = roomCenterPoint + new Vector3((Random.value - 0.5f) * roomWidth, (Random.value - 0.5f) * roomHeight, 0f);
			if (master.SharedTargetPos() != Vector3.zero)
			{
				master.SharedTargetPos();
			}
		}
	}
}
