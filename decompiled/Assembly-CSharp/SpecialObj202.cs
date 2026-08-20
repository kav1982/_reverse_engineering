using System.Collections.Generic;
using PlayerLogger;
using UnityEngine;

public class SpecialObj202 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public GameObject pfb_Button;

	public float space;

	private Dictionary<int, SpecialObj202Button> buttons = new Dictionary<int, SpecialObj202Button>();

	private RoomController belongCtrller;

	private void Start()
	{
		new List<int> { 1, 2, 3, 4 }.Upset();
		int num = 1;
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				SpecialObj202Button component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3(i, j, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj202Button>();
				component.Initialize(this, num);
				buttons.Add(num, component);
				num++;
			}
		}
		if (belongCtrller.roomCfg.isFlipped)
		{
			base.transform.position += new Vector3(space * 2f, 0f, 0f);
		}
		else
		{
			base.transform.position += new Vector3((0f - space) * 2f, 0f, 0f);
		}
	}

	public void ButtonEntry(SpecialObj202Button button)
	{
		button.Change();
		switch (button.Index)
		{
		case 1:
			buttons[2].Change();
			buttons[4].Change();
			break;
		case 2:
			buttons[1].Change();
			buttons[3].Change();
			buttons[5].Change();
			break;
		case 3:
			buttons[2].Change();
			buttons[6].Change();
			break;
		case 4:
			buttons[1].Change();
			buttons[5].Change();
			buttons[7].Change();
			break;
		case 5:
			buttons[2].Change();
			buttons[4].Change();
			buttons[6].Change();
			buttons[8].Change();
			break;
		case 6:
			buttons[3].Change();
			buttons[5].Change();
			buttons[9].Change();
			break;
		case 7:
			buttons[4].Change();
			buttons[8].Change();
			break;
		case 8:
			buttons[5].Change();
			buttons[7].Change();
			buttons[9].Change();
			break;
		case 9:
			buttons[6].Change();
			buttons[8].Change();
			break;
		default:
			Debug.LogError(button.Index);
			break;
		}
		bool flag = true;
		foreach (KeyValuePair<int, SpecialObj202Button> button2 in buttons)
		{
			if (!button2.Value.IsOn)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			foreach (KeyValuePair<int, SpecialObj202Button> button3 in buttons)
			{
				button3.Value.Correct();
			}
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
			int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
			ItemInfo itemInfo = default(ItemInfo);
			itemInfo.type = ItemType.Spell;
			itemInfo.id = specialRoomSpell;
			ItemInfo info = itemInfo;
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, info, base.transform.position);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
			SEMgr.Inst.puzzleSucceed.PlaySE();
		}
		else
		{
			SEMgr.Inst.puzzleClick.PlaySE();
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
