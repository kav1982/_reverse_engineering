using UnityEngine;

public class Boss5_Hand : UnitBase
{
	public enum HandState
	{
		Invisible,
		Show,
		Finding,
		Knock,
		Lift,
		Floating,
		Rest,
		Hide
	}

	public bool freeAct;

	public bool waitForOther = true;

	public Boss5_Hand otherHand;

	public float moveTime;

	public float roomCenterOffset;

	public VariableFloat findingTime;

	public float doubleFindingTime;

	public float doubleChance;

	private static bool isDouble;

	private bool doubleFollow;

	public VariableFloat doubleFollowHorizontalOffset;

	private float doubleFollowOffsetSign;

	public int beforeFinishDoubleCount;

	private Vector3 moveSpeedLerp;

	private Vector3 moveTargetPoint;

	public Vector3 originPosition;

	public float smallSkillTime;

	public bool isRight;

	public ShockParam knockShake;

	public SpriteRenderer portalRenderer;

	public float portalSwitchInterval;

	private float portalSwitchTimer;

	public Sprite portalSprite1;

	public Sprite portalSprite2;

	private int portalSpriteIndex;

	public static float knockTimes;

	public static float knockCount;

	private bool showDone;

	private bool showAudioPlayed;

	[Header("状态机")]
	public HandState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public GameObject spineRoot;

	private bool masterHandStartHide;

	private bool masterHandShowDone;

	public HandState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			findingTime.value1 *= 1.2f;
			findingTime.value2 *= 1.2f;
			doubleFindingTime *= 1.2f;
		}
	}

	public override void EveryInitialCallback()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		state = HandState.Invisible;
	}

	private Vector3 GetRandomKnockPoint()
	{
		Vector3 result = default(Vector3);
		if (isRight)
		{
			result.x = Random.Range(roomCenterPoint.x, roomCenterPoint.x + roomWidth / 2f);
		}
		else
		{
			result.x = Random.Range(roomCenterPoint.x, roomCenterPoint.x - roomWidth / 2f);
		}
		result.y = Random.Range(roomCenterPoint.y - roomHeight / 2f, roomCenterPoint.y + roomHeight / 2f);
		return result;
	}

	public override void Frame1InitialCallback()
	{
		base.Frame1InitialCallback();
		if (isRight)
		{
			SetFlip(1f);
			return;
		}
		SetFlip(-1f);
		Vector3 localPosition = base.SAnima.gameObject.transform.localPosition;
		localPosition.x = 0f - localPosition.x;
		base.SAnima.gameObject.transform.localPosition = localPosition;
	}

	public override void Update()
	{
		portalSwitchTimer += Time.deltaTime;
		if (portalSwitchTimer > portalSwitchInterval)
		{
			portalSwitchTimer = 0f;
			if (portalSpriteIndex == 0)
			{
				portalSpriteIndex = 1;
				portalRenderer.sprite = portalSprite1;
			}
			else
			{
				portalSpriteIndex = 0;
				portalRenderer.sprite = portalSprite2;
			}
		}
		base.Update();
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case HandState.Invisible:
			if (changedState)
			{
				base.Anima.Play("Boss5_Hand_Invisible");
				spineRoot.SetActive(value: false);
			}
			if (freeAct)
			{
				if (isRight)
				{
					waitForOther = false;
				}
				state = HandState.Show;
			}
			break;
		case HandState.Show:
			if (changedState)
			{
				base.transform.position = originPosition;
				SyncDotsPosition();
				if (isRight)
				{
					Boss5.Inst.rightHandAnima.AnimationState.SetAnimation(0, "afterAttack", loop: false);
				}
				else
				{
					Boss5.Inst.leftHandAnima.AnimationState.SetAnimation(0, "afterAttack", loop: false);
				}
				base.SAnima.timeScale = 1f;
				showDone = false;
				base.Anima.Play("Boss5_Hand_Show");
				masterHandShowDone = false;
				showAudioPlayed = false;
			}
			if (stateExistTime > 0.5f && !showAudioPlayed)
			{
				showAudioPlayed = true;
				SEMgr.Inst.boss5_Portal.PlaySE();
			}
			if (stateExistTime > 1f && !masterHandShowDone)
			{
				masterHandShowDone = true;
				if (isRight)
				{
					Boss5.Inst.rightHandAnima.AnimationState.SetAnimation(0, "attack3", loop: true);
				}
				else
				{
					Boss5.Inst.leftHandAnima.AnimationState.SetAnimation(0, "attack3", loop: true);
				}
			}
			if (stateExistTime > 0.6f && !spineRoot.activeSelf)
			{
				spineRoot.SetActive(value: true);
				base.SAnima.enabled = true;
				base.SAnima.AnimationState.SetAnimation(0, "show", loop: false);
			}
			if (!waitForOther && showDone)
			{
				state = HandState.Finding;
			}
			break;
		case HandState.Finding:
		{
			if (changedState)
			{
				base.SAnima.timeScale = 1f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				findingTime.RandomResult();
				base.Anima.Play("Boss5_Hand_Float");
				GetNearestTargetPlayerFirst();
				moveTargetPoint = GetRandomKnockPoint();
				if (!doubleFollow)
				{
					isDouble = Random.Range(0f, 1f) < doubleChance;
					otherHand.doubleFollow = true;
					doubleFollow = false;
					if (knockCount > knockTimes - 1f - (float)beforeFinishDoubleCount)
					{
						isDouble = true;
					}
					if (isDouble && !doubleFollow)
					{
						knockCount += 1f;
						otherHand.waitForOther = false;
					}
					else
					{
						knockCount += 1f;
					}
				}
				doubleFollowHorizontalOffset.RandomResult();
				doubleFollowOffsetSign = ((!(Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
			}
			if (base.HaveTarget)
			{
				if ((isRight && base.TargetPointIgnoreZ.x - roomCenterPoint.x > 0f - roomCenterOffset) || (!isRight && base.TargetPointIgnoreZ.x - roomCenterPoint.x < roomCenterOffset))
				{
					moveTargetPoint = base.TargetPointIgnoreZ;
				}
				else
				{
					moveTargetPoint.y = base.TargetPointIgnoreZ.y;
				}
			}
			Vector3 vector = ((!doubleFollow) ? Vector3.zero : new Vector3(0f, doubleFollowHorizontalOffset.result * doubleFollowOffsetSign, 0f));
			Vector3 target = moveTargetPoint + vector;
			target.y = Mathf.Clamp(target.y, roomCenterPoint.y - roomHeight / 2f, roomCenterPoint.y + roomHeight / 2f);
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref moveSpeedLerp, moveTime);
			SyncDotsPosition();
			SetMove(Vector3.zero);
			if (isDouble && !doubleFollow)
			{
				if (stateExistTime > doubleFindingTime)
				{
					state = HandState.Knock;
					otherHand.state = HandState.Knock;
				}
			}
			else if (stateExistTime > findingTime.result)
			{
				state = HandState.Knock;
			}
			break;
		}
		case HandState.Knock:
			if (changedState)
			{
				base.SAnima.timeScale = 1f;
				base.SAnima.AnimationState.SetAnimation(0, "attack", loop: false);
				doubleFollow = false;
				isDouble = false;
				base.Anima.Play("Boss5_Hand_Knock");
			}
			SetMove(Vector3.zero);
			break;
		case HandState.Lift:
			if (changedState)
			{
				base.Anima.Play("Boss5_Hand_Lift");
			}
			SetMove(Vector3.zero);
			break;
		case HandState.Floating:
			if (changedState)
			{
				base.SAnima.timeScale = 1f;
				base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
				base.Anima.Play("Boss5_Hand_Float");
			}
			if (knockCount >= knockTimes)
			{
				freeAct = false;
			}
			if (!freeAct && !doubleFollow)
			{
				if (!otherHand.freeAct)
				{
					state = HandState.Hide;
				}
				break;
			}
			if (!waitForOther)
			{
				state = HandState.Finding;
			}
			SetMove(Vector3.zero);
			break;
		case HandState.Hide:
			if (changedState)
			{
				base.SAnima.timeScale = 1f;
				base.SAnima.AnimationState.SetAnimation(0, "hide", loop: false);
				base.Anima.Play("Boss5_Hand_Hide");
				masterHandStartHide = false;
				showAudioPlayed = false;
			}
			if (stateExistTime > 0.5f && !showAudioPlayed)
			{
				showAudioPlayed = true;
				SEMgr.Inst.boss5_Portal.PlaySE();
			}
			if (stateExistTime > 0.6f && !masterHandStartHide)
			{
				masterHandStartHide = true;
				if (isRight)
				{
					Boss5.Inst.rightHandAnima.AnimationState.SetAnimation(0, "beforeAttack", loop: false);
				}
				else
				{
					Boss5.Inst.leftHandAnima.AnimationState.SetAnimation(0, "beforeAttack", loop: false);
				}
			}
			break;
		case HandState.Rest:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		base.AnimaAction(animaName);
		switch (animaName)
		{
		case "ShowDone":
			base.Anima.Play("Boss5_Hand_Float");
			base.SAnima.timeScale = 1f;
			base.SAnima.AnimationState.SetAnimation(0, "idle", loop: true);
			if (!waitForOther)
			{
				state = HandState.Finding;
			}
			showDone = true;
			break;
		case "KnockDown":
			KnockGround();
			state = HandState.Lift;
			otherHand.waitForOther = false;
			waitForOther = true;
			break;
		case "LiftDone":
			state = HandState.Floating;
			break;
		case "HideDone":
			state = HandState.Invisible;
			if (isRight)
			{
				Boss5.Inst.rightHandAnima.AnimationState.SetAnimation(0, "idle2", loop: true);
			}
			else
			{
				Boss5.Inst.leftHandAnima.AnimationState.SetAnimation(0, "idle2", loop: true);
			}
			spineRoot.SetActive(value: false);
			break;
		}
	}

	private void KnockGround()
	{
		Invoke("DelayShock", 0.1f);
		for (int i = 0; i < 4; i++)
		{
			Boss5_Wave component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_LargeWave", base.transform.position, 6f).GetComponent<Boss5_Wave>();
			if (i == 0)
			{
				component.Initialize(FourDir.Left);
			}
			if (i == 1)
			{
				component.Initialize(FourDir.Right);
			}
			if (i == 2)
			{
				component.Initialize(FourDir.Up);
			}
			if (i == 3)
			{
				component.Initialize(FourDir.Down);
			}
		}
	}

	private void DelayShock()
	{
		CamController.Inst.SetShock(knockShake);
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		info.immuneDamage = true;
		base.BeforeTakeDamage(info);
	}
}
