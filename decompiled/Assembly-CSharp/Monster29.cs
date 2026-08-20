using UnityEngine;

public class Monster29 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		BornFromMonster28,
		Fly
	}

	[Space(50f)]
	public SpriteRenderer sr;

	public float rotateSpeed;

	public float bornFromMonster28Time;

	[Header("Tantacle")]
	public LineRenderer lr_Tantacle;

	public LineRenderer lr_Shadow;

	public int tantacleNodeCount;

	public int tantacleNotLerpCount;

	public float tantacleSegmentLength;

	public float tantacleLerp;

	public float tantacleOffset;

	private bool isChangeToT6Correction;

	private float checkInRoomTimer;

	private MonsterState state;

	private Vector3 currentDir;

	private float bornFromMonster28Timer;

	private Vector3[] tantaclePoints;

	private Vector3 noTargetPoint;

	private Vector3 roomScale;

	private Vector3 roomCenter;

	public override void SingleInitialCallback()
	{
		tantaclePoints = new Vector3[tantacleNodeCount];
		lr_Tantacle.positionCount = tantacleNodeCount;
		lr_Shadow.positionCount = tantacleNodeCount;
		sr.sprite = null;
	}

	public override void EveryInitialCallback()
	{
		for (int i = 0; i < tantacleNodeCount; i++)
		{
			tantaclePoints[i] = base.transform.position;
			lr_Tantacle.SetPosition(i, Tool2D.GetLayerPoint(tantaclePoints[i]));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(tantaclePoints[i], 1.05f));
			lr_Tantacle.transform.ChangeAllLayer("Default");
			lr_Shadow.transform.ChangeAllLayer("Default");
		}
		isChangeToT6Correction = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.unitCfg.theme6Reposition = false;
		SetComponentData(componentData);
		roomScale = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
		roomCenter = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomScale.x -= 2f;
		roomScale.y -= 2f;
		roomScale.x *= 0.5f;
		roomScale.y *= 0.5f;
		noTargetPoint = roomCenter + new Vector3(roomScale.x * Random.Range(-1f, 1f), roomScale.y * Random.Range(-1f, 1f), 0f);
	}

	public override void Update()
	{
		if (lr_Tantacle.startColor != sr.color)
		{
			lr_Tantacle.startColor = sr.color;
			lr_Tantacle.endColor = sr.color;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (!isChangeToT6Correction)
		{
			checkInRoomTimer += Time.deltaTime;
			if (checkInRoomTimer > 0.1f)
			{
				checkInRoomTimer = 0f;
				if (base.transform.position.x == Mathf.Clamp(base.transform.position.x, 0f - roomScale.x + roomCenter.x, roomScale.x + roomCenter.x) && base.transform.position.y == Mathf.Clamp(base.transform.position.y, 0f - roomScale.y + roomCenter.y, roomScale.y + roomCenter.y))
				{
					isChangeToT6Correction = true;
					UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
					componentData.unitCfg.theme6Reposition = true;
					SetComponentData(componentData);
					lr_Tantacle.transform.ChangeAllLayer("Model");
					lr_Shadow.transform.ChangeAllLayer("Model");
				}
			}
		}
		for (int i = 0; i < tantacleNodeCount; i++)
		{
			if (i == 0)
			{
				tantaclePoints[i] = base.transform.position + currentDir * tantacleOffset + new Vector3(0f, 0f, 0f - sr.transform.localPosition.y);
			}
			else if (i < tantacleNotLerpCount)
			{
				tantaclePoints[i] = tantaclePoints[i - 1] - currentDir * tantacleSegmentLength;
			}
			else
			{
				tantaclePoints[i] = Vector3.Lerp(tantaclePoints[i], tantaclePoints[i - 1] - currentDir * tantacleSegmentLength, tantacleLerp * Time.deltaTime);
			}
			lr_Tantacle.SetPosition(i, Tool2D.GetLayerPoint(tantaclePoints[i]));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(tantaclePoints[i], 1.05f));
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTarget();
				state = MonsterState.Fly;
			}
			break;
		case MonsterState.Fly:
			if (base.HaveTarget)
			{
				currentDir = Tool2D.DirMoveTowards(currentDir, ToTargetDir(), rotateSpeed * Time.deltaTime);
			}
			else
			{
				checkTargetIntervalTimer += Time.deltaTime;
				if (checkTargetIntervalTimer >= 3f)
				{
					checkTargetIntervalTimer = 0f;
					GetNearestTarget();
					if (!base.HaveTarget)
					{
						noTargetPoint = roomCenter + new Vector3(roomScale.x * Random.Range(-1f, 1f), roomScale.y * Random.Range(-1f, 1f), 0f);
					}
				}
				currentDir = Tool2D.DirMoveTowards(currentDir, ToPointDir(noTargetPoint), rotateSpeed * Time.deltaTime);
			}
			SetMove(currentDir * base.MoveSpeed);
			break;
		case MonsterState.BornFromMonster28:
			bornFromMonster28Timer += Time.deltaTime;
			if (bornFromMonster28Timer >= bornFromMonster28Time)
			{
				bornFromMonster28Timer = 0f;
				GetNearestTarget();
				state = MonsterState.Fly;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void Theme6Reposition(Vector3 changeValue)
	{
		base.Theme6Reposition(changeValue);
		for (int i = 0; i < tantacleNodeCount; i++)
		{
			tantaclePoints[i] += changeValue;
		}
	}

	public void BornFromMonster28(Vector3 bornForce)
	{
		currentDir = bornForce.normalized;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.TakeKnockback(bornForce);
		SetComponentData(componentData);
	}
}
