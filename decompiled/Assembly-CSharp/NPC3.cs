using System.Collections.Generic;
using UnityEngine;

public class NPC3 : NPCBase
{
	protected override NPCPlot ImportantPlot => DataMgr.selectedWorldData.npc3ImportantPlot;

	protected override NPCPlot SchedulePlot => DataMgr.selectedWorldData.npc3SchedulePlot;

	protected override NPCPlot CasualPlot => DataMgr.selectedWorldData.npc3CasualPlot;

	protected override NPCPlot RandomPlot => DataMgr.selectedWorldData.npc3RandomPlotV2;

	public override void UseNPCFunction()
	{
		GameUISingletonMono<UIResearch>.ShowInit();
	}

	protected override void OnDialogFinish(int hdId)
	{
		base.OnDialogFinish(hdId);
		if (ImportantPlot.hdID == 14)
		{
			UseNPCFunction();
		}
	}

	protected override void DialogEvent(string eventStr)
	{
		switch (eventStr)
		{
		case "core1":
		{
			Vector3 position3 = base.transform.position;
			if (base.transform.position.x > PlayerMgr.Inst.PlayerPoint.x)
			{
				position3 += new Vector3(1f, 0f, 0f);
			}
			else
			{
				position3 += new Vector3(-1f, 0f, 0f);
			}
			List<ItemInfo> list3 = new List<ItemInfo>();
			for (int k = 0; k < 1; k++)
			{
				list3.Add(new ItemInfo(ItemType.Resource, 121));
			}
			QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, DTool.ListToBlobArray(list3), position3, 0.2f, 0.5f);
			break;
		}
		case "core2":
		{
			Vector3 position2 = base.transform.position;
			if (base.transform.position.x > PlayerMgr.Inst.PlayerPoint.x)
			{
				position2 += new Vector3(1f, 0f, 0f);
			}
			else
			{
				position2 += new Vector3(-1f, 0f, 0f);
			}
			List<ItemInfo> list2 = new List<ItemInfo>();
			for (int j = 0; j < 2; j++)
			{
				list2.Add(new ItemInfo(ItemType.Resource, 121));
			}
			QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, DTool.ListToBlobArray(list2), position2, 0.2f, 0.5f);
			break;
		}
		case "core3":
		{
			Vector3 position = base.transform.position;
			if (base.transform.position.x > PlayerMgr.Inst.PlayerPoint.x)
			{
				position += new Vector3(1f, 0f, 0f);
			}
			else
			{
				position += new Vector3(-1f, 0f, 0f);
			}
			List<ItemInfo> list = new List<ItemInfo>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(new ItemInfo(ItemType.Resource, 121));
			}
			QuickCreateSystem.Inst.CreateItemDrop(LevelMgr.Inst.CurrentRoomMapPos, DTool.ListToBlobArray(list), position, 0.2f, 0.5f);
			break;
		}
		default:
			Debug.LogError(eventStr);
			break;
		}
	}
}
