using PlayerLogger;
using UnityEngine;

public class SpecialObj211 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public SpecialObj211Rock pfb_Rock;

	public SpecialObj211Button pfb_Button;

	public Vector3[] rockOffsets;

	public Vector3 btnCenterOffset;

	public Color color1;

	public Color color2;

	public Color color3;

	public int beltWidth;

	public int beltHeight;

	public float beltSpeed;

	private SpecialObj211Rock[] rocks;

	private SpecialObj211Button[] btns;

	private RoomController belongCtrller;

	private void Start()
	{
		if (belongCtrller.roomCfg.isFlipped)
		{
			btnCenterOffset.x = 0f - btnCenterOffset.x;
		}
		rocks = new SpecialObj211Rock[rockOffsets.Length];
		btns = new SpecialObj211Button[rockOffsets.Length];
		for (int i = 0; i < rocks.Length; i++)
		{
			rocks[i] = Object.Instantiate(pfb_Rock, base.transform.position + rockOffsets[i], Quaternion.identity, base.transform.parent).GetComponent<SpecialObj211Rock>();
			rocks[i].Initialize(this);
		}
		for (int j = 0; j < btns.Length; j++)
		{
			btns[j] = Object.Instantiate(pfb_Button, base.transform.position + rockOffsets[j] + btnCenterOffset, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj211Button>();
			btns[j].Initialize(this, rocks[j].ColorType);
		}
		int num = -(beltWidth - 1) / 2;
		int num2 = (beltWidth - 1) / 2;
		int num3 = -(beltHeight - 1) / 2;
		int num4 = (beltHeight - 1) / 2;
		for (int k = num; k <= num2; k++)
		{
			for (int l = num3; l <= num4; l++)
			{
				bool flag = true;
				for (int m = 0; m < rockOffsets.Length; m++)
				{
					if ((float)k == rockOffsets[m].x && (float)l == rockOffsets[m].y)
					{
						flag = false;
						break;
					}
				}
				if (k == 0 && l == 0)
				{
					flag = false;
				}
				if (flag)
				{
					int num5 = Random.Range(0, 4);
					Vector3 point = base.transform.position + new Vector3(k, l, 0f) + btnCenterOffset;
					switch (num5)
					{
					case 0:
						CreateBelt(1301, point);
						break;
					case 1:
						CreateBelt(1302, point);
						break;
					case 2:
						CreateBelt(1303, point);
						break;
					case 3:
						CreateBelt(1304, point);
						break;
					default:
						Debug.LogError(num5);
						break;
					}
				}
			}
		}
	}

	private void CreateBelt(int id, Vector3 point)
	{
		SpecialObj13 component = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/SpecialObjs/" + id), point, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj13>();
		component.speed = beltSpeed;
		belongCtrller.TrapRegister(component);
	}

	public void CheckAnswer()
	{
		for (int i = 0; i < rocks.Length; i++)
		{
			if (rocks[i].ColorType != btns[i].CurrentColorType)
			{
				return;
			}
		}
		for (int j = 0; j < btns.Length; j++)
		{
			btns[j].SetInvalid();
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
		belongCtrller.SetAllTrapInvalid();
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
