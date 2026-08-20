using UnityEngine;

public class Monster28 : UnitBase
{
	private enum MonsterState
	{
		RandomMove,
		Idle
	}

	[Space(50f)]
	public SpriteRenderer sr;

	public float maxRotateSpeed;

	public float timeToChangeT6Correction;

	public float moveTime;

	public VariableFloat idleTime;

	[Header("Tantacle")]
	public Transform tsf_Motion;

	public Transform tsf_TantacleParent;

	public Monster28_Tentacle pfb_Tantacle;

	public Monster28_Tentacle pfb_Tentacle1;

	public Monster28_Tentacle pfb_TenTacle2;

	public int tantacleCount;

	public float tantacleOffset;

	[Header("Child")]
	public int childID;

	[Range(0f, 1f)]
	public float childBornRotateSpeedRatio;

	public Vector3 childBornOffset;

	public float childBornInterval;

	public float childBornKnockback;

	public int maxChildCount;

	[Header("Dead")]
	public int deadCreateChildCount;

	public VariableFloat deadKnockback;

	[Header("困难变异")]
	public float strongChildChance;

	public int strongChildID;

	public AIPattern pattern;

	private Monster28_Tentacle[] tentacles;

	private MonsterState state;

	private Vector3 moveDir = Vector3.zero;

	private float timeToChangeT6CorrectionTimer;

	private bool isChangeToT6Correction;

	private float moveTimer;

	private float idleTimer;

	private float bornChildIntervalTimer;

	private int childCounter;

	public override void SingleInitialCallback()
	{
		tentacles = new Monster28_Tentacle[tantacleCount];
		Vector3 dir = Tool2D.GetDir();
		for (int i = 0; i < tantacleCount; i++)
		{
			if (pattern == AIPattern.Pattern2)
			{
				if (i < 5)
				{
					tentacles[i] = Object.Instantiate(pfb_Tentacle1, tsf_TantacleParent);
					dir = Tool2D.GetDir(dir, 72f);
					tentacles[i].transform.localPosition = dir * tantacleOffset;
					tentacles[i].transform.up = dir;
					tentacles[i].SingleInitial(this);
				}
				else if (i < 10)
				{
					dir = Tool2D.GetDir();
					tentacles[i] = Object.Instantiate(pfb_TenTacle2, tsf_TantacleParent);
					tentacles[i].transform.localPosition = dir * tantacleOffset;
					tentacles[i].transform.up = dir;
					tentacles[i].SingleInitial(this);
				}
				else
				{
					dir = Tool2D.GetDir();
					tentacles[i] = Object.Instantiate(pfb_Tantacle, tsf_TantacleParent);
					tentacles[i].transform.localPosition = dir * tantacleOffset;
					tentacles[i].transform.up = dir;
					tentacles[i].SingleInitial(this);
				}
			}
			else
			{
				dir = Tool2D.GetDir();
				tentacles[i] = Object.Instantiate(pfb_Tantacle, tsf_TantacleParent);
				tentacles[i].transform.localPosition = dir * tantacleOffset;
				tentacles[i].transform.up = dir;
				tentacles[i].SingleInitial(this);
			}
		}
		sr.sprite = null;
		if (GameMgr.IsMobile_Static)
		{
			maxChildCount = Mathf.CeilToInt((float)maxChildCount * 0.5f);
			deadCreateChildCount = Mathf.CeilToInt((float)deadCreateChildCount * 0.5f);
			childBornInterval *= 2f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.RandomMove;
		timeToChangeT6CorrectionTimer = 0f;
		isChangeToT6Correction = false;
		moveTimer = 0f;
		idleTimer = 0f;
		bornChildIntervalTimer = 0f;
		childCounter = 0;
		moveDir = ToPointDir(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.theme6Reposition = false;
		SetComponentData(componentData);
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].EveryInitial();
			tentacles[i].transform.ChangeAllLayer("Default");
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		float num = Mathf.Lerp(0f, maxRotateSpeed, base.CurrentMotion.sqrMagnitude / (myPpt.unitCfg.moveSpeed * myPpt.unitCfg.moveSpeed));
		tsf_TantacleParent.Rotate(0f, 0f, num * Time.deltaTime);
		if (num >= maxRotateSpeed * childBornRotateSpeedRatio)
		{
			bornChildIntervalTimer += Time.deltaTime;
			if (bornChildIntervalTimer >= childBornInterval)
			{
				bornChildIntervalTimer = 0f;
				if (pattern == AIPattern.Pattern2)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + (GeneralTool.ChanceResult(strongChildChance) ? strongChildID : childID), base.transform.position + childBornOffset).GetComponent<Monster29>().BornFromMonster28(Tool2D.GetDir() * deadKnockback.RandomResult());
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childID, base.transform.position + childBornOffset).GetComponent<Monster29>().BornFromMonster28(Tool2D.GetDir() * deadKnockback.RandomResult());
				}
				childCounter++;
				if (childCounter >= maxChildCount)
				{
					DotsAnnouncedDeath();
				}
			}
		}
		if (!isChangeToT6Correction)
		{
			timeToChangeT6CorrectionTimer += Time.deltaTime;
			if (timeToChangeT6CorrectionTimer >= timeToChangeT6Correction)
			{
				isChangeToT6Correction = true;
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.unitCfg.theme6Reposition = true;
				SetComponentData(componentData);
				for (int i = 0; i < tentacles.Length; i++)
				{
					tentacles[i].transform.ChangeAllLayer("Model");
				}
			}
		}
		switch (state)
		{
		case MonsterState.RandomMove:
			SetMove(moveDir * base.MoveSpeed);
			moveTimer += Time.deltaTime;
			if (moveTimer >= moveTime)
			{
				moveTimer = 0f;
				state = MonsterState.Idle;
				idleTime.RandomResult();
			}
			break;
		case MonsterState.Idle:
			SetMove(Vector3.zero);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				moveDir = Tool2D.GetDir();
				state = MonsterState.RandomMove;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < deadCreateChildCount; i++)
		{
			if (pattern == AIPattern.Pattern2)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + (GeneralTool.ChanceResult(strongChildChance) ? strongChildID : childID), base.transform.position + childBornOffset).GetComponent<Monster29>().BornFromMonster28(Tool2D.GetDir() * deadKnockback.RandomResult());
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childID, base.transform.position + childBornOffset).GetComponent<Monster29>().BornFromMonster28(Tool2D.GetDir() * deadKnockback.RandomResult());
			}
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		Debug.Log(changeValue);
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i].Theme6Reposition(changeValue);
		}
	}
}
