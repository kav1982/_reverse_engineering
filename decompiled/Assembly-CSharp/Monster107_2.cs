using System.Collections.Generic;
using UnityEngine;

public class Monster107_2 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		RandomMove,
		Follow,
		PrepareAttack,
		MoveAttack,
		IdleAttack,
		Dead
	}

	[Header("待机和随机移动")]
	public VariableFloat idleTime;

	public VariableFloat randomMoveTime;

	public VariableFloat randomMoveRadius;

	public float checkInterval;

	public float checkIntervalTimer;

	public float maxFollowDistance;

	[Header("攻击")]
	public bool isAttacking;

	private SpellInitialParameter chainBulletInit = new SpellInitialParameter();

	private SpellInitialParameter hammerBulletInit = new SpellInitialParameter();

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public VariableFloat attackCD;

	public float attackCDTimer;

	public List<SpellBase> chainBullets = new List<SpellBase>();

	public List<float> bulletTimers = new List<float>();

	public List<bool> spawnFlags = new List<bool>();

	public SpellBase hammerBullet;

	public float spacing;

	public Transform handPivot;

	[Header("状态机")]
	public MonsterState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public float currentAngle;

	public float rotationSpeed;

	public int rotateCircle;

	public bool isAlone;

	public float expandCounter;

	public float expandSpeed;

	public float reSpawnSpeed;

	public float rotateOffset;

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
		chainBulletInit.spelldataConfig = SpellConfig.GetConfigCopy(90361);
		chainBulletInit.spelldataConfig.speed = spellSpeed;
		chainBulletInit.spelldataConfig.duration = spellDuration;
		chainBulletInit.spelldataConfig.damage = spellDamage;
		chainBulletInit.ownerPpt = myPpt;
		hammerBulletInit.spelldataConfig = SpellConfig.GetConfigCopy(90371);
		hammerBulletInit.spelldataConfig.speed = spellSpeed;
		hammerBulletInit.spelldataConfig.duration = spellDuration;
		hammerBulletInit.spelldataConfig.damage = spellDamage;
		hammerBulletInit.ownerPpt = myPpt;
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		isAttacking = false;
		attackCD.RandomResult();
		base.CC_Self.enabled = true;
		myPpt.CanTouch = true;
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
			SetMove(Vector3.zero);
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Follow;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				idleTime.RandomResult();
				base.Anima.Play("Eye", 1);
			}
			SetMove(Vector3.zero);
			if (stateExistTime > idleTime.result)
			{
				state = MonsterState.RandomMove;
			}
			break;
		case MonsterState.RandomMove:
		{
			ref Vector3 reference = ref varMgr.RegV3(0);
			if (changedState)
			{
				base.Anima.Play("Idle");
				randomMoveTime.RandomResult();
				randomMoveRadius.RandomResult();
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
			}
			if (stateExistTime > randomMoveTime.result)
			{
				state = MonsterState.Idle;
				break;
			}
			checkIntervalTimer += Time.deltaTime;
			if (checkIntervalTimer >= checkInterval)
			{
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.Follow;
				}
			}
			CheckNavInfo();
			if (navInfo.allCornerArrived)
			{
				reference = base.transform.position + Tool2D.GetDir() * randomMoveRadius.result;
				GetNavInfo(reference);
				break;
			}
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			break;
		}
		case MonsterState.Follow:
		{
			if (changedState)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.Idle;
				break;
			}
			float num = 360f - ToTargetDegree();
			if (Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > maxFollowDistance * maxFollowDistance)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				if (num > 0f && num < 45f)
				{
					base.Anima.Play("MoveBack");
				}
				else if (num > 45f && num < 135f)
				{
					base.Anima.Play("MoveLeft&Right");
				}
				else if (num > 135f && num < 225f)
				{
					base.Anima.Play("MoveFront");
				}
				else if (num > 225f && num < 315f)
				{
					base.Anima.Play("MoveLeft&Right");
				}
				else
				{
					base.Anima.Play("MoveBack");
				}
			}
			else
			{
				base.Anima.Play("Idle");
				SetMove(Vector3.zero);
			}
			CheckAttack();
			if (base.CurrentMotion.x < 0f)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			break;
		}
		case MonsterState.PrepareAttack:
			if (changedState)
			{
				base.Anima.Play("TakeOutChain");
				chainBullets.Clear();
				bulletTimers.Clear();
				spawnFlags.Clear();
				currentAngle = 0f;
				rotateCircle = 0;
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.MoveAttack:
			if (changedState)
			{
				base.Anima.Play("MoveAttack");
			}
			if (isAttacking)
			{
				RotateChain();
			}
			GetNavInfo(base.TargetPoint);
			SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			if (!base.HaveTarget || Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) < maxFollowDistance * maxFollowDistance)
			{
				state = MonsterState.IdleAttack;
			}
			break;
		case MonsterState.IdleAttack:
			if (changedState)
			{
				base.Anima.Play("IdleAttack");
			}
			if (isAttacking)
			{
				RotateChain();
			}
			SetMove(Vector3.zero);
			if (base.HaveTarget && Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > maxFollowDistance * maxFollowDistance)
			{
				state = MonsterState.MoveAttack;
			}
			break;
		case MonsterState.Dead:
			if (changedState)
			{
				base.Anima.Play("Dead");
			}
			SetMove(Vector3.zero);
			break;
		}
	}

	private void LateUpdate()
	{
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		myPpt.MR_Models[0].GetPropertyBlock(materialPropertyBlock);
		if (materialPropertyBlock.GetColor("_Color") != myPpt.BaseColor)
		{
			materialPropertyBlock.SetColor("_Color", myPpt.BaseColor);
			for (int i = 0; i < myPpt.MR_Models.Length; i++)
			{
				myPpt.MR_Models[i].SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	public void RotateChain()
	{
		currentAngle += (0f - rotationSpeed) * Time.deltaTime * base.transform.localScale.x;
		if (currentAngle >= 360f)
		{
			currentAngle -= 360f;
			rotateCircle++;
		}
		else if (currentAngle <= -360f)
		{
			currentAngle += 360f;
			rotateCircle++;
		}
		Vector3 dir = Tool2D.GetDir(Vector3.up, currentAngle);
		for (int i = 0; i < chainBullets.Count; i++)
		{
			chainBullets[i].transform.position = handPivot.position - new Vector3(0f, 0f, spellHeight) + dir * i * spacing * expandCounter + dir * rotateOffset;
			bulletTimers[i] += reSpawnSpeed * Time.deltaTime;
			if (!chainBullets[i].gameObject.activeSelf && !spawnFlags[i])
			{
				bulletTimers[i] = -1f;
				spawnFlags[i] = true;
			}
			if (spawnFlags[i] && bulletTimers[i] > 0f)
			{
				if (i == chainBullets.Count - 1)
				{
					SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + hammerBulletInit.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - spellHeight) + handPivot.position).GetComponent<SpellBase>();
					hammerBulletInit.spelldataConfig.speed = 0f;
					component.Initialize(hammerBulletInit);
					component.isThroughWall = true;
					chainBullets[i] = component;
					spawnFlags[i] = false;
					hammerBullet = component;
				}
				else
				{
					SpellBase component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + chainBulletInit.spelldataConfig.prefab, new Vector3(0f, 0f, 0f - spellHeight) + handPivot.position).GetComponent<SpellBase>();
					chainBulletInit.spelldataConfig.speed = 0f;
					component2.Initialize(chainBulletInit);
					component2.isThroughWall = true;
					chainBullets[i] = component2;
					spawnFlags[i] = false;
				}
			}
		}
		if (rotateCircle >= 3)
		{
			if (expandCounter > 0f && !isAlone)
			{
				expandCounter -= expandSpeed * Time.deltaTime;
			}
			else if (expandCounter > 0f && isAlone && rotateCircle >= 4)
			{
				expandCounter -= 1.5f * expandSpeed * Time.deltaTime;
			}
			else if (expandCounter < 0.1f)
			{
				foreach (SpellBase chainBullet in chainBullets)
				{
					chainBullet.spellCfg.duration = 0f;
				}
				chainBullets.Clear();
				state = MonsterState.Follow;
				isAttacking = false;
				attackCDTimer = 0f;
				attackCD.RandomResult();
			}
			if (base.HaveTarget && isAlone && IsDirectionSimilar(dir, Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, hammerBullet.transform.position - new Vector3(0f, 0.2f, 0f))))
			{
				GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + hammerBulletInit.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - spellHeight) + hammerBullet.transform.position);
				hammerBulletInit.shootDirection = Tool2D.IgnoreZV2ToV1Normal(base.TargetPoint, hammerBullet.transform.position - new Vector3(0f, 0.2f, 0f));
				hammerBulletInit.spelldataConfig.speed = 12f;
				gO.GetComponent<SpellBase>().Initialize(hammerBulletInit);
				int num = 3;
				for (int j = 0; j < num; j++)
				{
					chainBullets[chainBullets.Count - 1].spellCfg.duration = 0f;
					chainBullets.RemoveAt(chainBullets.Count - 1);
				}
				isAlone = false;
			}
		}
		else if (expandCounter < 1f)
		{
			expandCounter += 2f * expandSpeed * Time.deltaTime;
		}
	}

	private bool IsDirectionSimilar(Vector3 dir1, Vector3 dir2, float threshold = 15f)
	{
		return Vector3.Angle(dir1, dir2) <= threshold;
	}

	public void CheckAttack()
	{
		attackCDTimer += Time.deltaTime;
		if (attackCDTimer > attackCD.result && !isAttacking)
		{
			state = MonsterState.PrepareAttack;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SEMgr.Inst.monster12Land.PlaySE();
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Attack"))
		{
			if (animaName == "Dead")
			{
				myPpt.AnnouncedDeath();
			}
			return;
		}
		for (int i = 0; i < 8; i++)
		{
			SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + chainBulletInit.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - spellHeight) + handPivot.position).GetComponent<SpellBase>();
			chainBulletInit.spelldataConfig.speed = 0f;
			component.Initialize(chainBulletInit);
			component.isThroughWall = true;
			chainBullets.Add(component);
			spawnFlags.Add(item: false);
			bulletTimers.Add(0f);
		}
		hammerBullet = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + hammerBulletInit.spelldataConfig.prefab, new Vector3(0f, -0.2f, 0f - spellHeight) + handPivot.position).GetComponent<SpellBase>();
		hammerBulletInit.spelldataConfig.speed = 0f;
		hammerBullet.Initialize(hammerBulletInit);
		hammerBullet.isThroughWall = true;
		chainBullets.Add(hammerBullet);
		spawnFlags.Add(item: false);
		bulletTimers.Add(0f);
		isAttacking = true;
		if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count == 1)
		{
			isAlone = true;
			base.Anima.Play("EyeRed", 1);
		}
		else
		{
			isAlone = false;
		}
		if (base.HaveTarget && Tool2D.IgnoreZDistanceSqr(base.transform.position, base.TargetPoint) > maxFollowDistance * maxFollowDistance)
		{
			state = MonsterState.MoveAttack;
		}
		else
		{
			state = MonsterState.IdleAttack;
		}
	}
}
