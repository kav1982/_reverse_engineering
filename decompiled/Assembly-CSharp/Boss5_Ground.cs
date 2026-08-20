using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss5_Ground : MonoBehaviour
{
	public enum GroundState
	{
		Rest,
		WarningStraight,
		WarningDiagonal,
		Straight,
		Diagonal
	}

	private int MaxRow;

	public int straightRow;

	public int diagonalRow;

	public int secondStageStraightRow;

	public int secondStageDiagonalRow;

	public Boss5 master;

	private int nowRow;

	private int towardsRight;

	private bool DiagonalReversed;

	private bool soundPlayed;

	[Header("预警")]
	public int warningTime;

	private int warningCount;

	public float warningInterval;

	public float warningDelay;

	[Header("召唤物额外伤害")]
	public float teammmateDamageExtraFix;

	[Header("伤害")]
	public float damageInterval;

	private float damageTimer;

	public float damageDistance;

	public int damage;

	public float damageDuration;

	public float attackInterval;

	public float attackTime;

	private float attackCount;

	public float attackDelay;

	private float attackDelayCount;

	[Header("状态机")]
	public GroundState _state;

	private bool stateQuit;

	private bool changedState;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	private float stateExistTime;

	private List<Vector3> attackPoints = new List<Vector3>();

	private List<Vector3> warningPoints = new List<Vector3>();

	private List<Entity> attackedEntities = new List<Entity>();

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private List<UnitDotsSyncSystem.DistanceHitResult> entityInRange = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public GroundState state
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

	private int WithinRow(int origin, int add, int row)
	{
		int num = origin + add;
		if (num >= row)
		{
			return num - row;
		}
		if (num < 0)
		{
			return num + row;
		}
		return num;
	}

	private void GetPoints(bool isStraight)
	{
		Vector3 vector = roomCenterPoint + new Vector3((0f - roomWidth) / 2f + 0.5f, (0f - roomHeight) / 2f + 0.5f, 0f);
		attackPoints.Clear();
		warningPoints.Clear();
		if (isStraight)
		{
			for (int i = 0; (float)i < roomWidth; i++)
			{
				if (i % MaxRow == nowRow)
				{
					for (int j = 0; (float)j < roomHeight; j++)
					{
						attackPoints.Add(vector + new Vector3(i, j, 0f));
					}
				}
				if (i % MaxRow == nowRow)
				{
					for (int k = 0; (float)k < roomHeight; k++)
					{
						warningPoints.Add(vector + new Vector3(i, k, 0f));
					}
				}
			}
			return;
		}
		for (int l = 0; (float)l < roomWidth; l++)
		{
			for (int m = 0; (float)m < roomHeight; m++)
			{
				if (!DiagonalReversed)
				{
					float num;
					for (num = l - m; num - (float)MaxRow < 0f; num += (float)MaxRow)
					{
					}
					if (num % (float)MaxRow == (float)nowRow)
					{
						attackPoints.Add(vector + new Vector3(l, m, 0f));
					}
				}
				else if ((l + m) % MaxRow == nowRow)
				{
					attackPoints.Add(vector + new Vector3(l, m, 0f));
				}
			}
			for (int n = 0; (float)n < roomHeight; n++)
			{
				if (!DiagonalReversed)
				{
					float num2;
					for (num2 = l - n; num2 - (float)MaxRow < 0f; num2 += (float)MaxRow)
					{
					}
					if (num2 % (float)MaxRow == (float)nowRow)
					{
						warningPoints.Add(vector + new Vector3(l, n, 0f));
					}
				}
				else if ((l + n) % MaxRow == nowRow)
				{
					warningPoints.Add(vector + new Vector3(l, n, 0f));
				}
			}
		}
	}

	private void Start()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		if (GameMgr.IsMobile_Static)
		{
			straightRow++;
			diagonalRow++;
			secondStageDiagonalRow++;
			secondStageStraightRow++;
		}
	}

	public void Attack()
	{
		if (Random.Range(0f, 1f) > 0.5f)
		{
			state = GroundState.WarningDiagonal;
		}
		else
		{
			state = GroundState.WarningStraight;
		}
	}

	private void Update()
	{
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
		case GroundState.Rest:
			if (master.isSecondStage)
			{
				diagonalRow = secondStageDiagonalRow;
				straightRow = secondStageStraightRow;
			}
			break;
		case GroundState.WarningStraight:
			if (changedState)
			{
				MaxRow = straightRow;
				warningCount = 0;
				towardsRight = ((!(Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
			}
			if ((float)warningCount * warningInterval < stateExistTime)
			{
				if (stateExistTime >= (float)warningTime * warningInterval)
				{
					state = GroundState.Straight;
					break;
				}
				GetPoints(isStraight: true);
				Debug.Log(nowRow);
				CreateWarning();
				warningCount++;
			}
			break;
		case GroundState.Straight:
			if (changedState)
			{
				attackedEntities.Clear();
				attackCount = 0f;
				attackDelayCount = 0f;
				soundPlayed = false;
			}
			if (attackCount * attackInterval < stateExistTime)
			{
				if (stateExistTime > attackTime * attackInterval)
				{
					state = GroundState.Rest;
					break;
				}
				GetPoints(isStraight: true);
				CreateAttack();
				attackCount += 1f;
			}
			if (attackDelayCount * attackInterval + attackDelay < stateExistTime)
			{
				damageTimer += Time.deltaTime;
				if (damageTimer > damageInterval)
				{
					damageTimer = 0f;
					DealDamage();
				}
				if (attackDelayCount * attackInterval + attackDelay + damageDuration < stateExistTime)
				{
					soundPlayed = false;
					attackDelayCount += 1f;
					attackedEntities.Clear();
					nowRow = WithinRow(nowRow, towardsRight, MaxRow);
				}
			}
			break;
		case GroundState.WarningDiagonal:
			if (changedState)
			{
				MaxRow = diagonalRow;
				warningCount = 0;
				towardsRight = ((!(Random.Range(0f, 1f) > 0.5f)) ? 1 : (-1));
				DiagonalReversed = Random.Range(0f, 1f) > 0.5f;
			}
			if ((float)warningCount * warningInterval < stateExistTime)
			{
				if (stateExistTime > (float)warningTime * warningInterval)
				{
					state = GroundState.Diagonal;
					break;
				}
				GetPoints(isStraight: false);
				CreateWarning();
				warningCount++;
			}
			break;
		case GroundState.Diagonal:
			if (changedState)
			{
				attackedEntities.Clear();
				attackCount = 0f;
				attackDelayCount = 0f;
				soundPlayed = false;
			}
			if (attackCount * attackInterval < stateExistTime)
			{
				if (stateExistTime > attackTime * attackInterval)
				{
					state = GroundState.Rest;
					break;
				}
				GetPoints(isStraight: false);
				CreateAttack();
				attackCount += 1f;
			}
			if (attackDelayCount * attackInterval + attackDelay < stateExistTime)
			{
				damageTimer += Time.deltaTime;
				if (damageTimer > damageInterval)
				{
					damageTimer = 0f;
					DealDamage();
				}
				if (attackDelayCount * attackInterval + attackDelay + damageDuration < stateExistTime)
				{
					soundPlayed = false;
					attackDelayCount += 1f;
					attackedEntities.Clear();
					nowRow = WithinRow(nowRow, towardsRight, MaxRow);
				}
			}
			break;
		}
	}

	public void CreateWarning()
	{
		foreach (Vector3 warningPoint in warningPoints)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_GroundWarning", warningPoint, 3f);
		}
	}

	public void CreateAttack()
	{
		foreach (Vector3 attackPoint in attackPoints)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_GroundAttack", attackPoint, 2f);
		}
	}

	public void DealDamage()
	{
		if (!soundPlayed)
		{
			soundPlayed = true;
			SEMgr.Inst.boss5_GroundTentacle.PlaySE();
		}
		entityInRange.Clear();
		foreach (Vector3 attackPoint in attackPoints)
		{
			UnitDotsSyncSystem.GetCollidersInRange(attackPoint, damageDistance, GameConst.Filter_Friendly, results);
			for (int i = 0; i < results.Count; i++)
			{
				if (!entityInRange.Contains(results[i]))
				{
					entityInRange.Add(results[i]);
				}
			}
		}
		TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss5.Inst.myPpt.myEntity);
		info.damage = damage;
		info.teammateTakeDamageRatio = 3f;
		for (int j = 0; j < entityInRange.Count; j++)
		{
			if (!attackedEntities.Contains(entityInRange[j].entity))
			{
				attackedEntities.Add(entityInRange[j].entity);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch_Large", entityInRange[j].point + Tool2D.GetDir() * Random.Range(0f, 0.2f) + new Vector3(0f, -1f, -0.005f), 1f);
				UnitDotsSyncSystem.AddTakeDamageRequest(entityInRange[j].entity, info);
			}
		}
	}
}
