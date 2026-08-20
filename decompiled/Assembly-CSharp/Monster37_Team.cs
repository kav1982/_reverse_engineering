using System.Collections.Generic;
using UnityEngine;

public class Monster37_Team : MonoBehaviour
{
	public List<Monster37> allGroup = new List<Monster37>();

	private Dictionary<int, bool> rollUseable = new Dictionary<int, bool>();

	public float teamInterval;

	public float teamIntervalMobile;

	public float launchInterval;

	private float launchTimer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public void launchTeam()
	{
		List<Monster37> list = new List<Monster37>();
		foreach (Monster37 item in allGroup)
		{
			if (!item.moving)
			{
				list.Add(item);
			}
		}
		GeneralTool.RandomizeList(list);
		float num = PlayerMgr.Inst.PlayerPoint.y - roomCenterPoint.y + roomHeight / 2f;
		float num2 = Mathf.Max(0f, (float)(list.Count - 1) * teamInterval) / 2f;
		num += Random.Range(0f - teamInterval, teamInterval);
		if (num < num2 - 1f)
		{
			num = num2 - 1f;
		}
		if (roomHeight - 2f - num < num2)
		{
			num = roomHeight - 2f - num2;
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].verticalOffset = num + (float)((i + 1) / 2) * teamInterval * (float)((i % 2 != 0) ? 1 : (-1));
			list[i].Launch();
		}
	}

	public void Initialize()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		allGroup.Clear();
		launchTimer = launchInterval / 2f;
		if (GameMgr.IsMobile_Static)
		{
			teamInterval = teamIntervalMobile;
			launchInterval *= 1.25f;
		}
	}

	private void Start()
	{
	}

	public void ReportDead(Monster37 dead)
	{
		allGroup.Remove(dead);
	}

	private void Update()
	{
		launchTimer += Time.deltaTime;
		if (launchTimer > launchInterval)
		{
			launchTimer = 0f;
			launchTeam();
		}
	}
}
