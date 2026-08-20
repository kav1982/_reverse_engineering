using System;
using System.Collections.Generic;
using PlayerLogger;
using UnityEngine;

public class SpecialObj207 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public GameObject pfb_Button;

	public SpecialObj207Hit pfb_hit;

	public float space;

	private Dictionary<int, SpecialObj207Button> buttons = new Dictionary<int, SpecialObj207Button>();

	private RoomController belongCtrller;

	private System.Random ran = new System.Random();

	private int orderIndex;

	private List<int> _orders;

	private int roomCase;

	private SpecialObj207Hit _hit1;

	private SpecialObj207Hit _hit2;

	private SpecialObj207Hit _hit3;

	private SpecialObj207Hit _hit4;

	public int GetClass(float x, float y)
	{
		int[,] array = new int[4, 6];
		switch (roomCase)
		{
		case 1:
			array = new int[4, 6]
			{
				{ 1, 2, 3, 4, 1, 2 },
				{ 4, 3, 2, 1, 4, 3 },
				{ 1, 2, 3, 4, 1, 2 },
				{ 4, 3, 2, 1, 4, 3 }
			};
			break;
		case 2:
			array = new int[4, 6]
			{
				{ 4, 3, 2, 1, 4, 3 },
				{ 1, 2, 1, 2, 1, 2 },
				{ 4, 3, 4, 3, 4, 3 },
				{ 1, 2, 3, 4, 1, 2 }
			};
			break;
		case 3:
			array = new int[4, 6]
			{
				{ 1, 4, 3, 2, 1, 4 },
				{ 2, 1, 3, 3, 4, 3 },
				{ 3, 4, 3, 2, 1, 2 },
				{ 4, 1, 2, 3, 4, 1 }
			};
			break;
		case 4:
			array = new int[4, 6]
			{
				{ 3, 2, 1, 4, 3, 2 },
				{ 4, 1, 2, 3, 4, 1 },
				{ 1, 4, 1, 4, 3, 2 },
				{ 2, 3, 2, 3, 4, 1 }
			};
			break;
		case 5:
			array = new int[4, 6]
			{
				{ 1, 2, 3, 4, 1, 2 },
				{ 2, 1, 4, 3, 4, 3 },
				{ 3, 2, 3, 2, 1, 2 },
				{ 4, 1, 4, 1, 4, 3 }
			};
			break;
		case 6:
			array = new int[4, 6]
			{
				{ 4, 3, 2, 1, 4, 3 },
				{ 1, 2, 3, 4, 1, 2 },
				{ 4, 3, 2, 1, 4, 3 },
				{ 1, 2, 3, 4, 1, 2 }
			};
			break;
		case 7:
			array = new int[4, 6]
			{
				{ 1, 4, 1, 4, 1, 2 },
				{ 2, 3, 2, 3, 4, 3 },
				{ 3, 2, 3, 2, 1, 2 },
				{ 4, 1, 4, 1, 4, 3 }
			};
			break;
		case 8:
			array = new int[4, 6]
			{
				{ 1, 2, 3, 4, 1, 2 },
				{ 4, 3, 2, 1, 4, 3 },
				{ 3, 4, 1, 2, 3, 4 },
				{ 2, 1, 4, 3, 2, 1 }
			};
			break;
		case 9:
			array = new int[4, 6]
			{
				{ 2, 1, 4, 4, 1, 2 },
				{ 1, 2, 3, 3, 4, 3 },
				{ 3, 4, 1, 2, 1, 2 },
				{ 4, 3, 2, 1, 4, 3 }
			};
			break;
		case 10:
			array = new int[4, 6]
			{
				{ 1, 2, 3, 4, 1, 2 },
				{ 4, 1, 2, 3, 4, 3 },
				{ 3, 4, 3, 2, 1, 4 },
				{ 2, 1, 4, 3, 2, 1 }
			};
			break;
		}
		int num = (int)((double)x + 2.5);
		int num2 = (int)(0.0 - ((double)y - 1.5));
		return array[num2, num];
	}

	private void Start()
	{
		roomCase = ran.Next(1, 11);
		_orders = new List<int> { 1, 2, 3, 4 };
		_orders.Upset();
		int num = 1;
		for (float num2 = -2.5f; (double)num2 <= 2.5; num2 += 1f)
		{
			for (float num3 = -1.5f; (double)num3 <= 1.5; num3 += 1f)
			{
				SpecialObj207Button component = UnityEngine.Object.Instantiate(pfb_Button, base.transform.position + new Vector3(num2, num3, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj207Button>();
				component.Initialize(this, _orders[GetClass(num2, num3) - 1]);
				buttons.Add(num, component);
				num++;
			}
		}
		_hit1 = UnityEngine.Object.Instantiate(pfb_hit, base.transform.position + new Vector3(-1.5f, 6f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj207Hit>();
		_hit1.Initialize(_orders[0]);
		_hit2 = UnityEngine.Object.Instantiate(pfb_hit, base.transform.position + new Vector3(-0.5f, 6f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj207Hit>();
		_hit2.Initialize(_orders[1]);
		_hit3 = UnityEngine.Object.Instantiate(pfb_hit, base.transform.position + new Vector3(0.5f, 6f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj207Hit>();
		_hit3.Initialize(_orders[2]);
		_hit4 = UnityEngine.Object.Instantiate(pfb_hit, base.transform.position + new Vector3(1.5f, 6f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj207Hit>();
		_hit4.Initialize(_orders[3]);
		foreach (KeyValuePair<int, SpecialObj207Button> button in buttons)
		{
			button.Value.LongBlink();
		}
		if (belongCtrller.roomCfg.isFlipped)
		{
			base.transform.position += new Vector3(space * -5.5f, 0f, 0f);
		}
		else
		{
			base.transform.position += new Vector3((0f - space) * -5.5f, 0f, 0f);
		}
	}

	public void ButtonEntry(SpecialObj207Button button)
	{
		button.Change();
		bool flag = true;
		foreach (KeyValuePair<int, SpecialObj207Button> button2 in buttons)
		{
			if (!button2.Value.IsOn)
			{
				flag = false;
				break;
			}
		}
		if (!button.IsOn)
		{
			flag = false;
			orderIndex = 0;
			foreach (KeyValuePair<int, SpecialObj207Button> button3 in buttons)
			{
				button3.Value.Failed();
			}
			SEMgr.Inst.puzzleFail.PlaySE();
			_hit1.Failed();
			_hit2.Failed();
			_hit3.Failed();
			_hit4.Failed();
			return;
		}
		if (button.GetCls() != _orders[orderIndex])
		{
			flag = false;
			orderIndex = 0;
			foreach (KeyValuePair<int, SpecialObj207Button> button4 in buttons)
			{
				button4.Value.Failed();
			}
			SEMgr.Inst.puzzleFail.PlaySE();
			_hit1.Failed();
			_hit2.Failed();
			_hit3.Failed();
			_hit4.Failed();
			return;
		}
		if (flag)
		{
			foreach (KeyValuePair<int, SpecialObj207Button> button5 in buttons)
			{
				button5.Value.Correct();
			}
			_hit1.Correct();
			_hit2.Correct();
			_hit3.Correct();
			_hit4.Correct();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
			int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
			QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, specialRoomSpell), base.transform.position);
			LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
			belongCtrller.SetAllTrapInvalid();
			SEMgr.Inst.puzzleSucceed.PlaySE();
			return;
		}
		switch (orderIndex)
		{
		case 0:
			_hit1.Correct();
			break;
		case 1:
			_hit2.Correct();
			break;
		case 2:
			_hit3.Correct();
			break;
		case 3:
			_hit4.Correct();
			break;
		}
		orderIndex++;
		if (orderIndex == 4)
		{
			orderIndex = 0;
			_hit1.Blink();
			_hit2.Blink();
			_hit3.Blink();
			_hit4.Blink();
		}
		SEMgr.Inst.puzzleClick.PlaySE();
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
