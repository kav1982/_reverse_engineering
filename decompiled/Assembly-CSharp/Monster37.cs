using System.Collections.Generic;
using UnityEngine;

public class Monster37 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Charge,
		Waiting
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("运动")]
	public Vector3 moveDiration;

	public float currentSpeed;

	public float accleration;

	public float deaccleration;

	[Header("队列")]
	public GameObject teamPrefab;

	public float borderOffset;

	private bool moveRight;

	public static Monster37_Team team;

	public bool moving;

	public float verticalOffset;

	[Header("预警")]
	public float warningTime;

	private Vector3 warningPoint;

	public float waitTime;

	public ParticleSystem warningParticle;

	[Header("粒子")]
	public ParticleSystem StunParticle;

	[Header("尾巴")]
	public List<Monster37_Tail> tails;

	[Header("伤害控制")]
	public Monster37_AttackZone AttackZone;

	[Header("二模式")]
	public AIPattern pattern;

	public float rotateSpeed;

	public Vector3 moveDirationFixed;

	public float maxRotateRange;

	public SpriteRenderer eyeRenderer;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public MonsterState state
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
			varMgr.Clear();
		}
	}

	public bool tailFrozen => myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening;

	public override void SingleInitialCallback()
	{
		if (pattern == AIPattern.Pattern2)
		{
			myPpt.RemoveSRFromArray(eyeRenderer);
		}
		if (GameMgr.IsHarmony_Static)
		{
			if (pattern == AIPattern.Pattern1)
			{
				base.SAnima.initialSkinName = "Monster37_1_HX";
			}
			else
			{
				base.SAnima.initialSkinName = "Monster37_2_HX";
			}
		}
	}

	public override void EveryInitialCallback()
	{
		if (LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme6_Chapter3 || LevelMgr.Inst.CurrentRoomCtrller.roomCfg.themeType == RoomThemeType.Theme22_Chapter3_Shortcut1)
		{
			roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
			roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Width;
			roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme6Height;
		}
		if (team == null)
		{
			team = Object.Instantiate(teamPrefab, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster37_Team>();
			team.Initialize();
		}
		bornIdleTimer = 0f;
		state = MonsterState.Waiting;
		moveRight = true;
		team.allGroup.Add(this);
		if (base.transform.position.x < roomCenterPoint.x)
		{
			moveRight = true;
		}
		else
		{
			moveRight = false;
		}
		base.SAnima.AnimationState.SetAnimation(0, "Run", loop: true);
		base.SAnima.Update(Random.Range(0f, 0.433f));
		for (int i = 0; i < tails.Count; i++)
		{
			tails[i].Initialize();
		}
		if (base.transform.position.x < roomCenterPoint.x)
		{
			Vector3 position = base.transform.position;
			position.x = roomCenterPoint.x - roomWidth / 2f - 3f;
			base.transform.position = position;
			SyncDotsPosition();
		}
		else
		{
			Vector3 position2 = base.transform.position;
			position2.x = roomCenterPoint.x + roomWidth / 2f + 3f;
			base.transform.position = position2;
			SyncDotsPosition();
		}
	}

	public void Launch()
	{
		if (!moving)
		{
			state = MonsterState.Charge;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			currentSpeed = 0f;
			return;
		}
		warningParticle.transform.position = Tool2D.GetLayerPoint(warningPoint);
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
		case MonsterState.Charge:
			if (changedState)
			{
				currentSpeed = 0f;
				moving = true;
				base.Anima.Play("Move");
				GetNearestTarget();
				if (moveRight)
				{
					moveDiration = Vector3.right;
					Vector3 position3 = base.transform.position;
					position3.y = roomCenterPoint.y - roomHeight / 2f + verticalOffset;
					position3.x = roomCenterPoint.x - (roomWidth / 2f + borderOffset);
					base.transform.position = position3;
				}
				else
				{
					moveDiration = Vector3.left;
					Vector3 position4 = base.transform.position;
					position4.y = roomCenterPoint.y - roomHeight / 2f + verticalOffset;
					position4.x = roomCenterPoint.x + (roomWidth / 2f + borderOffset);
					base.transform.position = position4;
				}
				SyncDotsPosition();
				warningParticle.Play();
				warningPoint = new Vector3((float)((!moveRight) ? 1 : (-1)) * roomWidth / 2f + roomCenterPoint.x, base.transform.position.y, 0f);
				warningParticle.transform.position = Tool2D.GetLayerPoint(warningPoint);
				warningParticle.transform.localScale = new Vector3(moveRight ? 1 : (-1), 1f, 1f);
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanBeTarget = true;
				SetComponentData(componentData2);
				moveDirationFixed = moveDiration;
				SyncDotsPosition();
			}
			if (stateExistTime > warningTime)
			{
				warningParticle.Stop();
			}
			if (stateExistTime < waitTime)
			{
				SetMove(Vector3.zero);
				break;
			}
			currentSpeed += accleration * Time.deltaTime;
			if (currentSpeed > myPpt.unitCfg.moveSpeed)
			{
				currentSpeed = myPpt.unitCfg.moveSpeed;
			}
			if (pattern == AIPattern.Pattern2)
			{
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				if (base.HaveTarget && ((moveRight && base.TargetPoint.x - base.transform.position.x > 0f) || (!moveRight && base.TargetPoint.x - base.transform.position.x < 0f)))
				{
					moveDirationFixed = Tool2D.RotateTowardsAroundZAxis(moveDirationFixed, ToTargetDir(), rotateSpeed * Time.deltaTime);
					float num = Tool2D.IgnoreZAngleWithSign(moveDiration, moveDirationFixed);
					if (num < 0f - maxRotateRange)
					{
						moveDirationFixed = Tool2D.GetDir(moveDiration, 0f - maxRotateRange);
					}
					else if (num > maxRotateRange)
					{
						moveDirationFixed = Tool2D.GetDir(moveDiration, maxRotateRange);
					}
				}
				SetMove(moveDirationFixed.normalized * currentSpeed * base.MoveSpeed / myPpt.unitCfg.moveSpeed);
			}
			else
			{
				SetMove(moveDiration.normalized * currentSpeed * base.MoveSpeed / myPpt.unitCfg.moveSpeed);
			}
			if ((moveRight && base.transform.position.x - roomCenterPoint.x > roomWidth / 2f + borderOffset) || (!moveRight && base.transform.position.x - roomCenterPoint.x < (0f - roomWidth) / 2f - borderOffset))
			{
				moveRight = !moveRight;
				state = MonsterState.Waiting;
			}
			break;
		case MonsterState.Waiting:
			if (changedState)
			{
				moving = false;
				base.Anima.Play("Idle");
				if (moveRight)
				{
					moveDiration = Vector3.right;
					Vector3 position = base.transform.position;
					position.y = roomCenterPoint.y - roomHeight / 2f + verticalOffset;
					position.x = roomCenterPoint.x - (roomWidth / 2f + borderOffset);
					base.transform.position = position;
				}
				else
				{
					moveDiration = Vector3.left;
					Vector3 position2 = base.transform.position;
					position2.y = roomCenterPoint.y - roomHeight / 2f + verticalOffset;
					position2.x = roomCenterPoint.x + (roomWidth / 2f + borderOffset);
					base.transform.position = position2;
				}
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = false;
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		team.ReportDead(this);
		for (int i = 0; i < tails.Count; i++)
		{
			tails[i].Hide();
		}
		base.AfterDead(ref info);
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		AttackZone.BeforeReposotion();
		base.Theme6Reposition(changeValue);
		AttackZone.AfterReposotion();
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "FootStep"))
		{
			if (!(animaName == "Drop"))
			{
				_ = animaName == "ShootBullet";
			}
			else
			{
				SEMgr.Inst.monster37_Step.PlaySE();
			}
		}
	}
}
