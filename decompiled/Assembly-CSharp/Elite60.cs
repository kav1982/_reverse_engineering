using DG.Tweening;
using UnityEngine;

public class Elite60 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Move,
		PreAttack,
		Aim,
		Attack,
		AfterAttack
	}

	private enum Elite60Skill
	{
		RotatingLaser,
		ContinuousLaser
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private Elite60Skill currentSkill;

	private UIEndlessEliteHpBar hpBar;

	public Transform rotateObject;

	[Header("移动相关")]
	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public VariableFloat randomMoveAngle;

	public VariableFloat randomDistance;

	public int moveCount = 3;

	public float BornIdleTime = 1f;

	public float idleTime = 1f;

	private Vector3 playerPosOffset;

	[Header("技能1相关参数")]
	public float time1 = 5f;

	public float ChargeTime = 0.5f;

	public float CreateBulletTime = 1f;

	public float rotateTime = 1f;

	public float bulletSpeed = 10f;

	public float bulletRotateSpeed = 180f;

	private bool isRotate;

	private bool isCreateBullet;

	private bool isShoot;

	private Elite60Bullet bulletBuffer1;

	private Elite60Bullet bulletBuffer2;

	[Header("技能2相关参数")]
	public float time2 = 5f;

	public float warningTime2 = 1f;

	public float shootTime2 = 0.5f;

	public float shootInterval2 = 0.5f;

	public float trackingSpeed = 1.5f;

	public bool useSkill2 = true;

	private float nextShootTime;

	private Vector3 moveTargetPoint;

	private Vector3 playerPoint;

	private Vector3 targetDir;

	private int currentMoveCount;

	private bool isPlayBackAnim;

	private Vector3 DirBuffer;

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

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		hpBar = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
		currentMoveCount = moveCount;
		currentSkill = Elite60Skill.ContinuousLaser;
		isPlayBackAnim = true;
		isCreateBullet = false;
		isShoot = false;
	}

	public override void EveryInitialCallback()
	{
		base.EveryInitialCallback();
		hpBar.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
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
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f + BornIdleTime)
			{
				state = MonsterState.Move;
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (stateExistTime > idleTime)
			{
				state = MonsterState.Move;
			}
			break;
		case MonsterState.Move:
			if (changedState)
			{
				if (currentMoveCount == 0)
				{
					currentMoveCount = moveCount;
					state = MonsterState.PreAttack;
					break;
				}
				currentMoveCount--;
				base.Anima.Play("Move");
				randomDistance.RandomResult();
				randomMoveTime.RandomResult();
				playerPoint = PlayerMgr.Inst.PlayerPoint;
				targetDir = ToPointDir(playerPoint);
				moveTargetPoint = playerPoint + targetDir * randomDistance.result;
			}
			if (Tool2D.IgnoreZPoint(base.transform.position - moveTargetPoint).sqrMagnitude > moveThreshold * moveThreshold)
			{
				SetMove(ToPointDir(moveTargetPoint) * base.MoveSpeed, isFlip: false);
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
				state = MonsterState.Idle;
			}
			if (stateExistTime > randomMoveTime.result)
			{
				SetMove(Vector3.zero, isFlip: false);
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.PreAttack:
			if (changedState)
			{
				if (currentSkill == Elite60Skill.RotatingLaser)
				{
					base.Anima.Play("PreAttack1");
				}
				else
				{
					if (!useSkill2)
					{
						currentSkill = Elite60Skill.RotatingLaser;
						state = MonsterState.PreAttack;
						break;
					}
					base.Anima.Play("PreAttack2");
				}
			}
			SetMove(Vector3.zero, isFlip: false);
			if (currentSkill != 0)
			{
				rotateObject.right = ToPointDir(PlayerMgr.Inst.PlayerPoint);
			}
			break;
		case MonsterState.Aim:
			if (changedState)
			{
				playerPosOffset = base.transform.position - PlayerMgr.Inst.PlayerPoint;
				base.CurrentMotion = Vector3.zero;
				DirBuffer = ToPointDir(PlayerMgr.Inst.PlayerPoint);
				rotateObject.right = DirBuffer;
				GameObject obj = ShootLaser(DirBuffer, warningTime2, shootTime2);
				LineRenderer laserLineRenderer = obj.GetComponent<Elite60Laser>().LaserLineRenderer;
				laserLineRenderer.useWorldSpace = true;
				obj.transform.SetParent(rotateObject);
				laserLineRenderer.useWorldSpace = false;
			}
			if (stateExistTime > warningTime2)
			{
				state = MonsterState.Attack;
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				if (currentSkill == Elite60Skill.RotatingLaser)
				{
					isRotate = true;
					isCreateBullet = true;
					isShoot = true;
				}
				else
				{
					nextShootTime = shootInterval2;
				}
			}
			if (currentSkill == Elite60Skill.RotatingLaser)
			{
				if (stateExistTime > ChargeTime && isCreateBullet)
				{
					GetTwoRandomLines(out var line1A, out var line1B, out var line2A, out var line2B);
					Vector3 dir = ToPointDir(PlayerMgr.Inst.PlayerPoint);
					bulletBuffer1 = CreateBullet(base.transform.position + line1A, base.transform.position + line1B, dir);
					bulletBuffer2 = CreateBullet(base.transform.position + line2A, base.transform.position + line2B, dir);
					isCreateBullet = false;
				}
				if (stateExistTime > ChargeTime + CreateBulletTime && isRotate)
				{
					rotateObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
					rotateObject.transform.DOLocalRotate(new Vector3(0f, 0f, 180f), rotateTime);
					bulletBuffer1.Rotate(new Vector3(0f, 0f, 180f), rotateTime);
					bulletBuffer2.Rotate(new Vector3(0f, 0f, 180f), rotateTime);
					isRotate = false;
				}
				if (stateExistTime > ChargeTime + CreateBulletTime + rotateTime && isShoot)
				{
					bulletBuffer1.Shoot(bulletSpeed, bulletRotateSpeed, ToPointDir(PlayerMgr.Inst.PlayerPoint));
					bulletBuffer2.Shoot(bulletSpeed, bulletRotateSpeed, ToPointDir(PlayerMgr.Inst.PlayerPoint));
					bulletBuffer1 = null;
					bulletBuffer2 = null;
					isShoot = false;
				}
				if (stateExistTime > time1)
				{
					state = MonsterState.AfterAttack;
				}
			}
			else
			{
				if (stateExistTime > nextShootTime && stateExistTime < time2)
				{
					ShootLaser(DirBuffer, -1f, shootTime2).transform.SetParent(rotateObject);
					nextShootTime += shootInterval2;
				}
				if (stateExistTime >= time2)
				{
					state = MonsterState.AfterAttack;
				}
			}
			break;
		case MonsterState.AfterAttack:
			if (changedState)
			{
				if (currentSkill == Elite60Skill.RotatingLaser)
				{
					base.Anima.Play("AfterAttack1");
					currentSkill = Elite60Skill.ContinuousLaser;
				}
				else
				{
					base.Anima.Play("AfterAttack2");
					currentSkill = Elite60Skill.RotatingLaser;
				}
				rotateObject.right = Vector3.right;
			}
			break;
		}
	}

	private void LateUpdate()
	{
		if ((state == MonsterState.Attack || state == MonsterState.Aim) && currentSkill == Elite60Skill.ContinuousLaser)
		{
			base.CurrentMotion = Vector3.zero;
			Vector3 b = PlayerMgr.Inst.PlayerPoint + playerPosOffset;
			b.z = base.transform.position.z;
			base.transform.position = Vector3.Lerp(base.transform.position, b, trackingSpeed * Time.deltaTime);
			SyncDotsPositionSafe();
			rotateObject.right = DirBuffer;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		default:
			_ = animaName == "Shoot";
			break;
		case "AttackFinish":
			state = MonsterState.AfterAttack;
			break;
		case "Aim":
			state = MonsterState.Aim;
			break;
		case "Attack":
			state = MonsterState.Attack;
			break;
		case "Idle":
			state = MonsterState.Idle;
			break;
		}
	}

	private GameObject ShootLaser(Vector3 shootDir, float warningTimer, float shootTime)
	{
		Vector3 position = base.transform.position;
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite60Laser", position);
		Elite60Laser component = gO.GetComponent<Elite60Laser>();
		component.InitDroneData(0.1f, 40f, 1.2f, 10f, shootDir, 0.1f, laserDamageIsDps: false, warningTimer, shootTime, shootTime, 0f, 0f, null, default(Vector3), 0f, 0f, 0f, default(Vector3), 0.1f, 0.05f, disableChargeSE: false, disableShootSE: false, isShadow: true);
		component.ShootByOtherSource(myPpt.myEntity);
		return gO;
	}

	private Elite60Bullet CreateBullet(Vector3 pointA, Vector3 pointB, Vector3 dir)
	{
		Vector3 position = base.transform.position;
		Elite60Bullet component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite60Bullet", position).GetComponent<Elite60Bullet>();
		component.Initialize(pointA, pointB, dir, myPpt.myEntity, 0.3f);
		component.speed = 0f;
		component.rotateSpeed = 0f;
		return component;
	}

	private void GetTwoRandomLines(out Vector3 line1A, out Vector3 line1B, out Vector3 line2A, out Vector3 line2B)
	{
		Vector3 vector = new Vector3(-0.8f, 0.8f, 0f);
		Vector3 vector2 = new Vector3(0.8f, 0.8f, 0f);
		Vector3 vector3 = new Vector3(0.8f, -0.8f, 0f);
		Vector3 vector4 = new Vector3(-0.8f, -0.8f, 0f);
		Vector3[] array = new Vector3[4] { vector, vector2, vector3, vector4 };
		(int, int)[][] obj = new(int, int)[4][]
		{
			new(int, int)[8]
			{
				(0, 1),
				(1, 2),
				(1, 2),
				(2, 3),
				(2, 3),
				(3, 0),
				(3, 0),
				(0, 1)
			},
			new(int, int)[16]
			{
				(0, 1),
				(0, 2),
				(0, 1),
				(1, 3),
				(1, 2),
				(1, 3),
				(1, 2),
				(0, 2),
				(2, 3),
				(0, 2),
				(2, 3),
				(1, 3),
				(3, 0),
				(1, 3),
				(3, 0),
				(0, 2)
			},
			new(int, int)[2]
			{
				(0, 2),
				(1, 3)
			},
			new(int, int)[4]
			{
				(0, 1),
				(2, 3),
				(1, 2),
				(3, 0)
			}
		};
		int num = Random.Range(0, 4);
		(int, int)[] obj2 = obj[num];
		int maxExclusive = obj2.Length / 2;
		int num2 = Random.Range(0, maxExclusive) * 2;
		(int, int) tuple = obj2[num2];
		(int, int) tuple2 = obj2[num2 + 1];
		line1A = array[tuple.Item1];
		line1B = array[tuple.Item2];
		line2A = array[tuple2.Item1];
		line2B = array[tuple2.Item2];
	}
}
