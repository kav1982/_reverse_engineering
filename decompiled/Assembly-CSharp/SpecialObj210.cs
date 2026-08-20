using PlayerLogger;
using UnityEngine;

public class SpecialObj210 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public SpecialObj210Button pfb_button;

	public float space;

	public int length;

	public float ratetime;

	private RoomController belongCtrller;

	private bool iscal;

	private int frame;

	private int[] ranarray;

	private int anseweindex;

	private float dtime;

	private SpecialObj210Button _button;

	private SpecialObj210Button _button1;

	private SpecialObj210Button _button2;

	private SpecialObj210Button _button3;

	private SpecialObj210Button _button4;

	public bool IsRight { get; private set; }

	public bool IsPlaying { get; private set; }

	private void Start()
	{
		ranarray = new int[length];
		for (int i = 0; i < length; i++)
		{
			ranarray[i] = Random.Range(1, 5);
		}
		_button = Object.Instantiate(pfb_button, base.transform.position + new Vector3(-0f, 0f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj210Button>();
		_button.Initialize(this, 0);
		_button1 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(-2.5f, 2.5f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj210Button>();
		_button1.Initialize(this, 1);
		_button2 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(2.5f, 2.5f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj210Button>();
		_button2.Initialize(this, 2);
		_button3 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(-2.5f, -2.5f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj210Button>();
		_button3.Initialize(this, 3);
		_button4 = Object.Instantiate(pfb_button, base.transform.position + new Vector3(2.5f, -2.5f, 0f) * space, Quaternion.identity, base.transform.parent).GetComponent<SpecialObj210Button>();
		_button4.Initialize(this, 4);
	}

	private void Update()
	{
		if (!iscal)
		{
			return;
		}
		dtime += Time.deltaTime;
		if (frame < length)
		{
			if (dtime >= ratetime)
			{
				dtime = 0f;
				switch (ranarray[frame])
				{
				case 1:
					_button1.Blink();
					break;
				case 2:
					_button2.Blink();
					break;
				case 3:
					_button3.Blink();
					break;
				case 4:
					_button4.Blink();
					break;
				}
				frame++;
			}
		}
		else
		{
			_button.Blink();
			iscal = false;
			IsPlaying = false;
		}
	}

	public void ButtonEntry(SpecialObj210Button button)
	{
		SEMgr.Inst.puzzleClick.PlaySE();
		switch (button.Getcla())
		{
		case 0:
			SetRan();
			break;
		case 1:
			PutAnswer(1);
			break;
		case 2:
			PutAnswer(2);
			break;
		case 3:
			PutAnswer(3);
			break;
		case 4:
			PutAnswer(4);
			break;
		}
	}

	public void SetRan()
	{
		dtime = 0f;
		frame = 0;
		iscal = false;
		IsPlaying = false;
		anseweindex = 0;
		IsRight = false;
		ranarray = new int[length];
		for (int i = 0; i < length; i++)
		{
			ranarray[i] = Random.Range(1, 5);
		}
		iscal = true;
		IsPlaying = true;
	}

	public void PutAnswer(int x)
	{
		if (ranarray != null)
		{
			if (anseweindex < 7 && x == ranarray[anseweindex])
			{
				anseweindex++;
			}
			else if (anseweindex == 7 && x == ranarray[anseweindex])
			{
				IsRight = true;
				_button.Correct();
				_button1.Correct();
				_button2.Correct();
				_button3.Correct();
				_button4.Correct();
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Puzzle_Correct", base.transform.position, 2f);
				int specialRoomSpell = OutputMgr.GetSpecialRoomSpell();
				QuickCreateSystem.Inst.CreateItem(LevelMgr.Inst.CurrentRoomMapPos, new ItemInfo(ItemType.Spell, specialRoomSpell), base.transform.position);
				LevelMgr.Inst.RoomFinishLogger?.AddCurrentSideRoomReward(PlayerLogger.Item.CreateSpell(specialRoomSpell));
				SEMgr.Inst.puzzleSucceed.PlaySE();
			}
			else
			{
				SEMgr.Inst.puzzleFail.PlaySE();
				_button1.Wrong();
				_button2.Wrong();
				_button3.Wrong();
				_button4.Wrong();
				IsPlaying = false;
				anseweindex = 0;
			}
		}
	}

	public void SetRoomCtrlller(RoomController levelCtrller)
	{
		belongCtrller = levelCtrller;
	}
}
