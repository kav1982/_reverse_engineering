using System.Collections.Generic;
using UnityEngine;

public class Monster24 : UnitBase
{
	public enum UnitState
	{
		Reset,
		AppearWait,
		FirstAppear,
		AppearIdle,
		Disappear,
		NormalAppear,
		Appear2Idle,
		AttackIdle,
		Attack
	}

	[Space(50f)]
	public float appearDistace;

	public float appearIdleTime;

	public float afterAttackWaitTime;

	public VariableFloat randomWalkDistance;

	public VariableFloat randomWalkTime;

	public VariableFloat blinkInterval;

	[Range(0f, 1f)]
	public float attackChance;

	[Header("Leg")]
	public GameObject pfb_Leg;

	public Transform tsf_Motion;

	[Header("Spell")]
	public float[] spellHeights;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("Pattern")]
	public AIPattern pattern;

	public int splitUnitID;

	public int splitCount;

	public float splitRadius;

	public int spellCount2;

	public int spellCount3;

	public float spellCenterHeight;

	public VariableFloat spellSpeed2;

	public float spellOffset;

	public float spellAngle;

	public float spellRange;

	public bool repositioned;

	public float repositionLength;

	public static Monster24_List mateList;

	public GameObject mateListPrefab;

	public List<Monster24> sideList;

	public float repositionDelta;

	private float repositionDistance;

	public UnitState state = UnitState.AppearWait;

	private Monster24_Leg leg1;

	private Monster24_Leg leg2;

	private FourDir standDir;

	private float idleTimer;

	private float blinkIntervalTimer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private UnitState preState;

	private UnitState tempState;

	private bool changedState;

	private SpellSpawnParams ssp;

	[Header("和谐模式")]
	public List<AnimationClip> harmonyAnimations = new List<AnimationClip>();

	public void GetRepositionDelta()
	{
		if (mateList == null)
		{
			mateList = Object.Instantiate(mateListPrefab, base.transform.position, base.transform.rotation, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster24_List>();
		}
		repositionDistance = mateList.repositionDistance;
		if (GameMgr.IsMobile_Static)
		{
			repositionDistance = mateList.repositionDistanceMobile;
		}
		Vector3 targetPoint = roomCenterPoint;
		if (base.HaveTarget)
		{
			targetPoint = base.TargetPoint;
		}
		if (standDir == FourDir.Left)
		{
			sideList = mateList.leftList;
		}
		else if (standDir == FourDir.Right)
		{
			sideList = mateList.rightList;
		}
		else if (standDir == FourDir.Up)
		{
			sideList = mateList.topList;
		}
		else
		{
			sideList = mateList.bottomList;
		}
		float num = (float)(sideList.Count - 1) / 2f;
		if (standDir == FourDir.Up || standDir == FourDir.Down)
		{
			if (num * repositionDistance + targetPoint.x > roomWidth / 2f + roomCenterPoint.x)
			{
				targetPoint.x = roomWidth / 2f + roomCenterPoint.x - num * repositionDistance;
			}
			if ((0f - num) * repositionDistance + targetPoint.x < (0f - roomWidth) / 2f + roomCenterPoint.x)
			{
				targetPoint.x = (0f - roomWidth) / 2f + roomCenterPoint.x + num * repositionDistance;
			}
			targetPoint += new Vector3(mateList.randomDelta * repositionDistance, mateList.randomDelta * repositionDistance, 0f);
			repositionDelta = targetPoint.x - num * repositionDistance + repositionDistance * (float)sideList.IndexOf(this) - base.transform.position.x;
		}
		if (standDir == FourDir.Left || standDir == FourDir.Right)
		{
			if (num * repositionDistance + targetPoint.y > roomHeight / 2f + roomCenterPoint.y)
			{
				targetPoint.y = roomHeight / 2f + roomCenterPoint.y - num * repositionDistance - 1f;
			}
			if ((0f - num) * repositionDistance + targetPoint.y < (0f - roomHeight) / 2f + roomCenterPoint.y)
			{
				targetPoint.y = (0f - roomHeight) / 2f + roomCenterPoint.y + num * repositionDistance;
			}
			repositionDelta = (targetPoint + new Vector3(mateList.randomDelta * repositionDistance, mateList.randomDelta * repositionDistance, 0f)).y - num * repositionDistance + repositionDistance * (float)sideList.IndexOf(this) - base.transform.position.y;
		}
	}

	public override void SingleInitialCallback()
	{
		leg1 = Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster24_Leg>();
		leg2 = Object.Instantiate(pfb_Leg, base.transform).GetComponent<Monster24_Leg>();
		leg1.SingleInitial(this, leg2, isLeftLeg: true);
		leg2.SingleInitial(this, leg1, isLeftLeg: false);
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90171);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsMobile_Static)
		{
			spellCount2 = 2;
			spellCount3 = 3;
			spellAngle *= 0.5f;
		}
	}

	public override void EveryInitialCallback()
	{
		leg1.EveryInitial();
		leg2.EveryInitial();
		if (mateList == null)
		{
			mateList = Object.Instantiate(mateListPrefab, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster24_List>();
		}
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		if (base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - (float)LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2f)
		{
			standDir = FourDir.Left;
		}
		else if (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + (float)LevelMgr.Inst.CurrentRoomCfg.theme6Width / 2f)
		{
			standDir = FourDir.Right;
		}
		else if (base.transform.position.y < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - (float)LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2f)
		{
			standDir = FourDir.Down;
		}
		else if (base.transform.position.y > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + (float)LevelMgr.Inst.CurrentRoomCfg.theme6Height / 2f)
		{
			standDir = FourDir.Up;
		}
		else
		{
			Debug.LogWarning("此怪需要放在房间可视范围以外，专门设计如此");
		}
		if (standDir == FourDir.Left)
		{
			mateList.AskToInsert(mateList.leftList, this);
		}
		if (standDir == FourDir.Right)
		{
			mateList.AskToInsert(mateList.rightList, this);
		}
		if (standDir == FourDir.Up)
		{
			mateList.AskToInsert(mateList.topList, this);
		}
		if (standDir == FourDir.Down)
		{
			mateList.AskToInsert(mateList.bottomList, this);
		}
		blinkInterval.RandomResult();
		randomWalkTime.RandomResult();
		repositioned = false;
		state = UnitState.Reset;
		if (!GameMgr.IsHarmony_Static)
		{
			return;
		}
		AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(base.Anima.runtimeAnimatorController);
		base.Anima.runtimeAnimatorController = animatorOverrideController;
		for (int i = 0; i < harmonyAnimations.Count; i++)
		{
			string text = harmonyAnimations[i].name.Substring(0, harmonyAnimations[i].name.Length - 2);
			if (animatorOverrideController[text] != null)
			{
				animatorOverrideController[text] = harmonyAnimations[i];
			}
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		if (state != UnitState.Attack && state != UnitState.AttackIdle)
		{
			blinkIntervalTimer += Time.deltaTime;
			if (blinkIntervalTimer > blinkInterval.result)
			{
				blinkIntervalTimer = 0f;
				blinkInterval.RandomResult();
				base.Anima.SetTrigger("Blink");
			}
		}
		switch (state)
		{
		case UnitState.Reset:
			if (changedState)
			{
				base.Anima.Play("Monster24_Idle");
			}
			state = UnitState.AppearWait;
			break;
		case UnitState.AppearWait:
			if (changedState)
			{
				idleTimer = 0f;
			}
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= appearIdleTime)
			{
				state = UnitState.FirstAppear;
			}
			break;
		case UnitState.FirstAppear:
		{
			if (changedState)
			{
				GetNearestTargetPlayerFirst();
				GetRepositionDelta();
				if (standDir == FourDir.Left || standDir == FourDir.Right)
				{
					base.transform.position += new Vector3(0f, repositionDelta, 0f);
				}
				else
				{
					base.transform.position += new Vector3(repositionDelta, 0f, 0f);
				}
				SyncDotsPosition();
			}
			SetMove(Tool2D.GetDirByFourDirInverted(standDir) * base.MoveSpeed);
			bool flag2 = false;
			switch (standDir)
			{
			case FourDir.Up:
				if (base.transform.position.y < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f - appearDistace)
				{
					flag2 = true;
				}
				break;
			case FourDir.Right:
				if (base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f - appearDistace)
				{
					flag2 = true;
				}
				break;
			case FourDir.Down:
				if (base.transform.position.y > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f + appearDistace)
				{
					flag2 = true;
				}
				break;
			case FourDir.Left:
				if (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f + appearDistace)
				{
					flag2 = true;
				}
				break;
			default:
				Debug.LogError(standDir);
				break;
			}
			if (flag2)
			{
				state = UnitState.AppearIdle;
			}
			break;
		}
		case UnitState.AppearIdle:
			if (changedState)
			{
				base.Anima.SetTrigger("Idle");
				idleTimer = 0f;
			}
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= appearIdleTime)
			{
				state = UnitState.Attack;
			}
			break;
		case UnitState.Attack:
			if (changedState)
			{
				base.Anima.SetTrigger("Attack");
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case UnitState.AttackIdle:
			if (changedState)
			{
				idleTimer = 0f;
				base.Anima.SetTrigger("Idle");
			}
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= afterAttackWaitTime)
			{
				state = UnitState.Disappear;
			}
			break;
		case UnitState.Disappear:
		{
			if (!repositioned)
			{
				SetMove(Tool2D.GetDirByFourDir(standDir) * base.MoveSpeed);
			}
			else
			{
				SetMove(Tool2D.GetDirByFourDir(standDir) * (0f - base.MoveSpeed));
			}
			if (!(Mathf.Abs(base.transform.position.x - roomCenterPoint.x) > roomWidth / 2f + 1f) && !(Mathf.Abs(base.transform.position.y - roomCenterPoint.y) > roomHeight / 2f + 1f))
			{
				break;
			}
			Vector3 vector = Vector3.zero;
			if (standDir == FourDir.Up || standDir == FourDir.Down)
			{
				if (base.transform.position.y - roomCenterPoint.y > roomHeight / 2f)
				{
					vector = -new Vector3(0f, roomHeight + 2f, 0f);
				}
				else if (base.transform.position.y - roomCenterPoint.y < (0f - roomHeight) / 2f)
				{
					vector = new Vector3(0f, roomHeight + 2f, 0f);
				}
			}
			if (standDir == FourDir.Left || standDir == FourDir.Right)
			{
				if (base.transform.position.x - roomCenterPoint.x < (0f - roomWidth) / 2f)
				{
					vector = new Vector3(roomWidth + 2f, 0f, 0f);
				}
				else if (base.transform.position.x - roomCenterPoint.x > roomWidth / 2f)
				{
					vector = -new Vector3(roomWidth + 2f, 0f, 0f);
				}
			}
			if (vector != Vector3.zero)
			{
				Theme6Reposition(vector);
				state = UnitState.NormalAppear;
			}
			break;
		}
		case UnitState.NormalAppear:
		{
			if (repositioned)
			{
				SetMove(Tool2D.GetDirByFourDir(standDir) * base.MoveSpeed);
			}
			else
			{
				SetMove(Tool2D.GetDirByFourDir(standDir) * (0f - base.MoveSpeed));
			}
			bool flag = false;
			if (repositioned)
			{
				switch (standDir)
				{
				case FourDir.Up:
					if (base.transform.position.y > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f + appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Right:
					if (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f + appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Down:
					if (base.transform.position.y < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f - appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Left:
					if (base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f - appearDistace)
					{
						flag = true;
					}
					break;
				default:
					Debug.LogError(standDir);
					break;
				}
			}
			else
			{
				switch (standDir)
				{
				case FourDir.Down:
					if (base.transform.position.y > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f + appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Left:
					if (base.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x - LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f + appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Up:
					if (base.transform.position.y < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.y + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.y / 2f - appearDistace)
					{
						flag = true;
					}
					break;
				case FourDir.Right:
					if (base.transform.position.x < LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x + LevelMgr.Inst.CurrentRoomCtrller.RoomScale.x / 2f - appearDistace)
					{
						flag = true;
					}
					break;
				default:
					Debug.LogError(standDir);
					break;
				}
			}
			if (flag)
			{
				state = UnitState.AppearIdle;
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Shoot":
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			switch (pattern)
			{
			case AIPattern.Pattern1:
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellCenterHeight);
				sSPModifier.Direction = ((!repositioned) ? 1 : (-1)) * Tool2D.GetDirByFourDir((FourDir)(0 - standDir));
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				break;
			case AIPattern.Pattern2:
			{
				for (int j = 0; j < spellCount2; j++)
				{
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellCenterHeight);
					sSPModifier.Direction = ((!repositioned) ? 1 : (-1)) * Tool2D.GetDir(Tool2D.GetDirByFourDir((FourDir)(0 - standDir)), (0f - spellRange) / 2f + spellRange / (float)(spellCount2 - 1) * (float)j);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				break;
			}
			case AIPattern.Pattern3:
			{
				for (int i = 0; i < spellCount3; i++)
				{
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellCenterHeight);
					sSPModifier.Direction = ((!repositioned) ? 1 : (-1)) * Tool2D.GetDir(Tool2D.GetDirByFourDir((FourDir)(0 - standDir)), (0f - spellRange) / 2f + spellRange / (float)(spellCount3 - 1) * (float)i);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				break;
			}
			default:
				Debug.LogError(pattern);
				break;
			}
			break;
		}
		case "AttackFinish":
			state = UnitState.AttackIdle;
			break;
		default:
			Debug.LogError(animaName);
			break;
		case "Blinked":
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (standDir == FourDir.Left)
		{
			mateList.leftList.Remove(this);
		}
		if (standDir == FourDir.Right)
		{
			mateList.rightList.Remove(this);
		}
		if (standDir == FourDir.Up)
		{
			mateList.topList.Remove(this);
		}
		if (standDir == FourDir.Down)
		{
			mateList.bottomList.Remove(this);
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		Vector3 zero = Vector3.zero;
		GetNearestTargetPlayerFirst();
		GetRepositionDelta();
		zero = ((standDir != FourDir.Left && standDir != FourDir.Right) ? new Vector3(repositionDelta, 0f, 0f) : new Vector3(0f, repositionDelta, 0f));
		repositioned = !repositioned;
		changeValue += zero;
		base.Theme6Reposition(changeValue);
		leg1.ChangePointImmediate(changeValue);
		leg2.ChangePointImmediate(changeValue);
		SyncDotsPosition();
	}
}
