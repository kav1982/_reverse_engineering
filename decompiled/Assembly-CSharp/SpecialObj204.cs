using System.Collections.Generic;
using PlayerLogger;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class SpecialObj204 : LayerCorrect, IRoomCtrller, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	private enum SpecialObj106State
	{
		Idle,
		Showing,
		Begin,
		Finish,
		Restart
	}

	[Space(50f)]
	public GameObject pfb_Button;

	public Animator anima;

	public float space;

	public float showTime;

	public float restartDelay;

	public UnityEngine.BoxCollider thisCollider;

	private float restartTimer;

	private SpecialObj106State state;

	private RoomController belongCtrller;

	private List<SpecialObj204Button> buttons = new List<SpecialObj204Button>();

	private List<int> buttonTypes = new List<int>();

	private Dictionary<int, int> buttonTypeCounts = new Dictionary<int, int>();

	private bool isEntered;

	private int showType = -1;

	private float showTimer;

	private int lastDownButtonType = -1;

	private int downButtonTimer;

	private int rewardLevel;

	Entity IDotsPhysicsReciever.thisEntity { get; set; }

	private void Start()
	{
		buttonTypes = new List<int> { 0, 0, 1, 1, 2, 2, 3, 3 };
		buttonTypes.Add(Random.Range(0, 4));
		buttonTypes.Upset();
		for (int i = 0; i < buttonTypes.Count; i++)
		{
			if (buttonTypeCounts.ContainsKey(buttonTypes[i]))
			{
				buttonTypeCounts[buttonTypes[i]]++;
			}
			else
			{
				buttonTypeCounts.Add(buttonTypes[i], 1);
			}
		}
		int num = 0;
		for (int j = -1; j <= 1; j++)
		{
			for (int k = -1; k <= 1; k++)
			{
				SpecialObj204Button component = Object.Instantiate(pfb_Button, base.transform.position + new Vector3(j, k, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj204Button>();
				component.Initialize(this, buttonTypes[num]);
				buttons.Add(component);
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
		CollisionFilter collisionFilter = default(CollisionFilter);
		collisionFilter.BelongsTo = 67108864u;
		collisionFilter.CollidesWith = 512u;
		collisionFilter.GroupIndex = 0;
		CollisionFilter filter = collisionFilter;
		UnitPhysicsSyncSystem.RegisterReciever(this, filter, thisCollider);
	}

	private void OnDestroy()
	{
		UnitPhysicsSyncSystem.UnregisterReciever(this);
	}

	private void Update()
	{
		switch (state)
		{
		case SpecialObj106State.Showing:
		{
			showTimer += Time.deltaTime;
			if (!(showTimer >= showTime))
			{
				break;
			}
			showTimer = 0f;
			for (int j = 0; j < buttonTypes.Count; j++)
			{
				if (buttonTypes[j] == showType)
				{
					buttons[j].ShowOver();
				}
				if (buttonTypes[j] == showType + 1)
				{
					buttons[j].Show();
				}
			}
			showType++;
			if (showType >= 4)
			{
				state = SpecialObj106State.Begin;
				anima.SetTrigger("Begin");
				for (int k = 0; k < buttons.Count; k++)
				{
					buttons[k].Ready();
				}
			}
			break;
		}
		case SpecialObj106State.Restart:
			restartTimer += Time.deltaTime;
			if (restartTimer >= restartDelay)
			{
				restartTimer = 0f;
				isEntered = false;
				state = SpecialObj106State.Idle;
				anima.SetTrigger("Idle");
				showType = 0;
				downButtonTimer = 1;
				rewardLevel = 0;
				lastDownButtonType = -1;
				for (int i = 0; i < buttons.Count; i++)
				{
					buttons[i].Reset();
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case SpecialObj106State.Idle:
		case SpecialObj106State.Begin:
		case SpecialObj106State.Finish:
			break;
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		if (isEntered || !(other == PlayerMgr.Inst.PlayerEtt))
		{
			return;
		}
		buttonTypes.Upset();
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].Initialize(this, buttonTypes[i]);
		}
		isEntered = true;
		state = SpecialObj106State.Showing;
		anima.SetTrigger("Down");
		showType = 0;
		for (int j = 0; j < buttonTypes.Count; j++)
		{
			if (buttonTypes[j] == showType)
			{
				buttons[j].Show();
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}

	public void ButtonOn(SpecialObj204Button button)
	{
		if (lastDownButtonType == -1)
		{
			lastDownButtonType = button.Type;
			button.DownRight();
			downButtonTimer = 1;
			SEMgr.Inst.puzzleClick.PlaySE();
		}
		else if (lastDownButtonType == button.Type)
		{
			button.DownRight();
			downButtonTimer++;
			if (downButtonTimer == buttonTypeCounts[button.Type])
			{
				lastDownButtonType = -1;
				rewardLevel++;
				if (rewardLevel == 4)
				{
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
			else
			{
				SEMgr.Inst.puzzleClick.PlaySE();
			}
		}
		else
		{
			button.DownWrong();
			SEMgr.Inst.puzzleFail.PlaySE();
			for (int i = 0; i < buttons.Count; i++)
			{
				buttons[i].DisableInteractive();
			}
			state = SpecialObj106State.Restart;
		}
	}
}
