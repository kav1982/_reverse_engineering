using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster42 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		MoveUp,
		MoveDown,
		LieDown,
		BackToIdle,
		Attack,
		JumpPrepare,
		Jump,
		AfterJump
	}

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("移动贝塞尔曲线")]
	public float lengthScale;

	public LineRenderer thisLineRenderer;

	public LineRenderer shadow;

	public int linePoints;

	public float neckLength;

	public float headLength;

	public float moveMaxMiddleAngle;

	public float middleLength;

	public float moveMaxRange;

	public VariableFloat moveTime;

	private float moveTimer;

	private float moveMiddleAngle;

	private float moveEndAngle;

	private float moveStartAngle;

	private float moveTargetBezierCurve;

	public Vector3 moveDirection;

	private Vector3 staticHeadStartPoint;

	private Vector3 staticHeadMiddlePoint;

	private Vector3 headsMiddlePoint;

	private Vector3 moveHeadMiddlePoint;

	private Vector3 moveHeadEndPoint;

	private Monster42_Head movingHead;

	private Monster42_Head staticHead;

	private bool headSwitched;

	[Header("heads")]
	public List<Transform> headSpriteTransform;

	public List<Animator> headAnima;

	public List<Monster42_Head> Heads;

	public List<SpriteRenderer> headSprites;

	public List<Transform> headShadow;

	public List<Transform> headShadowCenter;

	public List<Transform> headVertical;

	public List<AnimaEvent> headEvent;

	[Header("idle")]
	public AnimationCurve scaleCurve;

	public VariableFloat idleTime;

	public float idleChance;

	public float idleFrequency;

	public float idleLerp;

	private float idleTimer;

	private float idleExistTimer;

	private float idleLengthScale;

	private float idleScale;

	public AnimationCurve idleCurve;

	[Header("二模式")]
	public AIPattern pattern;

	public List<Vector3> railPointsDelta;

	public float prepareLerp;

	public float prepareLength;

	public float jumpLength;

	public float jumpMiddleLength;

	public float jumpMaxMiddleAngle;

	public float jumpMaxRange;

	public float jumpTime;

	public float shockAmplitude;

	public float shockSpeed;

	public float shockTime;

	public float waveAngleInterval;

	public float waveCount;

	[Header("接近重定位")]
	public float closeMaxMiddleAngle;

	private float closeNowMiddleAngle;

	public float closeMaxRange;

	private bool targetClose;

	private bool keepDistance;

	[Header("spells")]
	public int spellDamage;

	public float spellSpeed;

	public float spellDuration;

	public float spellHeight;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	[Header("attack")]
	public float attackRadius;

	public float knockback;

	public int attackDamage;

	[Header("extra")]
	public float velocityLerp;

	public float shadowOffsetY;

	public float neckExtraFix;

	[Header("落点预警")]
	public SpriteRenderer sr_Warning;

	public Sprite spriteWarning_H;

	private Vector3 _destination;

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
		}
	}

	private Vector3 destination
	{
		get
		{
			if (base.HaveTarget)
			{
				return base.TargetPointIgnoreZ;
			}
			return _destination;
		}
	}

	private void GetDestination()
	{
		Vector3 a = LevelMgr.Inst.CurrentRoomCtrller.RoomScale;
		Vector3 vector = (_destination = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPoint(LevelMgr.Inst.CurrentRoomCtrller.CenterPoint + Vector3.Scale(a, new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0f)))));
	}

	public void OnValidate()
	{
		moveMaxRange = Mathf.Sin(moveMaxMiddleAngle * (MathF.PI / 180f) / 2f) * middleLength * 2f;
		jumpMaxRange = Mathf.Sin(jumpMaxMiddleAngle * (MathF.PI / 180f) / 2f) * jumpMiddleLength * 2f;
		closeMaxRange = Mathf.Sin(closeMaxMiddleAngle * (MathF.PI / 180f) / 2f) * middleLength * 2f;
	}

	public void ShareDamage(TakeDamageInfo_Dots info)
	{
		for (int i = 0; i <= 1; i++)
		{
			Heads[i].myPpt.SetBeHitColor();
		}
		UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		thisLineRenderer.positionCount = linePoints;
		if (GameMgr.IsChAge14_Static)
		{
			sr_Warning.sprite = spriteWarning_H;
		}
		myPpt.RemoveSRFromArray(sr_Warning);
		if (GameMgr.IsMobile_Static)
		{
			moveTime.value1 *= 1.25f;
			moveTime.value2 *= 1.25f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		headEvent[0].DoAction = AnimaAction;
		headEvent[1].DoAction = AnimaAction;
		headShadow[0].gameObject.SetActive(value: true);
		headShadow[1].gameObject.SetActive(value: true);
		base.EveryInitialCallback();
		Heads.Clear();
		for (int i = 0; i < 2; i++)
		{
			Heads.Add(ObjPoolMgr.Inst.GetGO("Prefabs/Units/104221", base.transform.position).GetComponent<Monster42_Head>());
			Heads[i].Master = this;
		}
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		shadow.positionCount = 2;
		moveStartAngle = 0f;
		moveMiddleAngle = 180f;
		moveEndAngle = 0f;
		moveDirection = Tool2D.GetDir();
		staticHead = Heads[0];
		movingHead = Heads[1];
		movingHead.SetPosition(base.transform.position - new Vector3(0f, 0f, headLength + 2f * neckLength + 2f * middleLength));
		idleLengthScale = scaleCurve.Evaluate(1f);
		SetBody();
		GetDestination();
	}

	public override void Update()
	{
		base.transform.position = Tool2D.IgnoreZPoint(staticHead.transform.position);
		LocalTransform componentData = GetComponentData<LocalTransform>();
		componentData.Position = base.transform.position;
		SetComponentData(componentData);
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
				headAnima[0].Play("Monster42_ClawIdle", 0, 0f);
				headAnima[1].Play("Monster42_ClawLifted");
				sr_Warning.enabled = false;
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
		{
			if (changedState)
			{
				if (UnityEngine.Random.Range(0f, 1f) < idleChance)
				{
					idleTimer = idleTime.RandomResult();
				}
				GetNearestTarget();
				checkTargetIntervalTimer = 0f;
				idleExistTimer = 0f;
				moveTime.RandomResult();
				sr_Warning.enabled = false;
			}
			idleTimer -= Time.deltaTime;
			idleExistTimer += Time.deltaTime;
			if (idleTimer > 0f)
			{
				idleLengthScale = Mathf.Lerp(idleLengthScale, idleCurve.Evaluate(idleExistTimer * idleFrequency), idleLerp);
				break;
			}
			GetNearestTarget();
			idleLengthScale = Mathf.Lerp(idleLengthScale, scaleCurve.Evaluate(1f), idleLerp);
			if (!((double)Mathf.Abs(idleLengthScale - scaleCurve.Evaluate(1f)) < 0.01))
			{
				break;
			}
			GetNavInfo(destination);
			Vector3 vector = Tool2D.IgnoreZPoint(navInfo.ToGoPoint) - staticHead.transform.position;
			if (vector.sqrMagnitude > moveMaxRange * moveMaxRange)
			{
				keepDistance = false;
				targetClose = false;
				moveDirection = vector.normalized * moveMaxRange;
				state = MonsterState.MoveDown;
				break;
			}
			GetDestination();
			moveDirection = vector.normalized * moveMaxRange;
			targetClose = true;
			keepDistance = false;
			if (targetClose)
			{
				closeNowMiddleAngle = 2f * Mathf.Asin(vector.magnitude * 0.5f / middleLength) * 57.29578f;
				moveDirection = moveDirection.normalized * vector.magnitude;
			}
			if (vector.sqrMagnitude < closeMaxRange * closeMaxRange)
			{
				moveDirection = Tool2D.GetNavMeshPointIngoreZ(staticHead.transform.position, moveMaxRange) - base.transform.position;
				closeNowMiddleAngle = 2f * Mathf.Asin(moveDirection.magnitude * 0.5f / middleLength) * 57.29578f;
				targetClose = false;
				keepDistance = true;
			}
			state = MonsterState.MoveDown;
			break;
		}
		case MonsterState.MoveUp:
			if (changedState)
			{
				GetNearestTarget();
				if (base.HaveTarget)
				{
					SwitchHead();
				}
				else
				{
					SwitchHead();
				}
				moveTimer = 0f;
			}
			moveTimer += Time.deltaTime;
			if (!targetClose)
			{
				moveMiddleAngle = Mathf.Lerp(moveMaxMiddleAngle, 180f, moveTimer / moveTime.result);
				moveEndAngle = Mathf.Lerp(180f, 0f, moveTimer / moveTime.result);
				moveStartAngle = Mathf.Lerp(moveMaxMiddleAngle / 2f, 0f, moveTimer / moveTime.result);
				idleLengthScale = scaleCurve.Evaluate(moveTimer / moveTime.result);
			}
			else
			{
				moveMiddleAngle = Mathf.Lerp(closeNowMiddleAngle, 180f, moveTimer / moveTime.result);
				moveEndAngle = Mathf.Lerp(180f, 0f, moveTimer / moveTime.result);
				moveStartAngle = Mathf.Lerp(closeNowMiddleAngle / 2f, 0f, moveTimer / moveTime.result);
				idleLengthScale = scaleCurve.Evaluate(moveTimer / moveTime.result);
			}
			if (base.HaveTarget && !keepDistance && ToTargetDistanceSqr() > moveMaxRange * moveMaxRange)
			{
				GetNavInfo(destination);
				moveDirection = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDirection, Tool2D.IgnoreZPoint(navInfo.ToGoPoint) - staticHead.transform.position, 0.01f, 0f));
			}
			if (moveTimer > moveTime.result)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.MoveDown:
			if (changedState)
			{
				GetNearestTarget();
				moveTimer = 0f;
				sr_Warning.enabled = true;
				headAnima[1].Play("Monster42_ClawDropping");
			}
			sr_Warning.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, moveTimer / moveTime.result * 2f));
			sr_Warning.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(staticHead.transform.position + moveDirection), LayerCorrectType.GroundEffectLow);
			moveTimer += Time.deltaTime;
			staticHeadStartPoint = staticHead.transform.position + new Vector3(0f, 0f, 0f - headLength);
			staticHeadMiddlePoint = staticHeadStartPoint + new Vector3(0f, 0f, 0f - neckLength);
			if (!targetClose && !keepDistance)
			{
				moveMiddleAngle = Mathf.Lerp(moveMaxMiddleAngle, 180f, 1f - moveTimer / moveTime.result);
				moveEndAngle = Mathf.Lerp(180f, 0f, 1f - moveTimer / moveTime.result);
				moveStartAngle = Mathf.Lerp(moveMaxMiddleAngle / 2f, 0f, 1f - moveTimer / moveTime.result);
				idleLengthScale = scaleCurve.Evaluate(1f - moveTimer / moveTime.result);
			}
			else
			{
				moveMiddleAngle = Mathf.Lerp(closeNowMiddleAngle, 180f, 1f - moveTimer / moveTime.result);
				moveEndAngle = Mathf.Lerp(180f, 0f, 1f - moveTimer / moveTime.result);
				moveStartAngle = Mathf.Lerp(closeNowMiddleAngle / 2f, 0f, 1f - moveTimer / moveTime.result);
				idleLengthScale = scaleCurve.Evaluate(1f - moveTimer / moveTime.result);
			}
			if (base.HaveTarget && !keepDistance && ToTargetDistanceSqr() > moveMaxRange * moveMaxRange)
			{
				GetNavInfo(destination);
				moveDirection = Tool2D.IgnoreZPoint(Vector3.RotateTowards(moveDirection, Tool2D.IgnoreZPoint(navInfo.ToGoPoint) - staticHead.transform.position, 0.01f, 0f));
			}
			if (moveTimer > moveTime.result)
			{
				state = MonsterState.LieDown;
			}
			break;
		case MonsterState.LieDown:
			if (changedState)
			{
				sr_Warning.enabled = false;
				state = MonsterState.MoveUp;
			}
			break;
		}
		SetBody();
	}

	public void Impact()
	{
		SEMgr.Inst.monster42Land.PlaySE();
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(staticHead.transform.position, attackRadius, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, attackDamage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
					info.damage = attackDamage;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, staticHead.transform.position) * knockback;
					info.teammateTakeDamageRatio = 3f;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		if (pattern == AIPattern.Pattern2)
		{
			string text = "EF_Monster42_BladeWaveVertical";
			if (GameMgr.IsChAge14_Static)
			{
				text = "EF_Monster42_BladeWaveVertical_H";
			}
			CamController.Inst.SetShock(shockAmplitude, shockSpeed, shockTime);
			if (GameMgr.IsChAge14_Static)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster42_WaveLarge_H", staticHead.transform.position, 5f);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster42_WaveLarge", staticHead.transform.position, 5f);
			}
			UnityEngine.Random.Range(0f, 360f);
			for (int j = 0; (float)j < waveCount; j++)
			{
				Vector3 dir = Tool2D.GetDir(-moveDirection.normalized, waveAngleInterval * ((float)j - (waveCount - 1f) / 2f));
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, staticHead.transform.position + new Vector3(0f, -0.3f, 0f)).GetComponent<Elite9_BladeWaves>().Initialize(dir, myPpt);
			}
		}
		else if (GameMgr.IsChAge14_Static)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster42_Wave_H", staticHead.transform.position, 5f);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster42_Wave", staticHead.transform.position, 5f);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int i = 0; i < 2; i++)
		{
			Heads[i].ManualDie();
		}
	}

	public void SwitchHead()
	{
		moveDirection *= -1f;
		Monster42_Head monster42_Head = movingHead;
		movingHead = staticHead;
		staticHead = monster42_Head;
		staticHead.SetPosition(Tool2D.IgnoreZPoint(staticHead.transform.position));
		movingHead.SetPosition(Tool2D.IgnoreZPoint(movingHead.transform.position));
		headAnima[0].Play("Monster42_ClawDrop", 0, 0f);
		headAnima[1].Play("Monster42_ClawLift");
		staticHead.SetCanTouch(canTouch: true);
		movingHead.SetCanTouch(canTouch: false);
		headSwitched = true;
	}

	public void SetBody()
	{
		if (thisLineRenderer.startColor != myPpt.BaseColor)
		{
			thisLineRenderer.startColor = myPpt.BaseColor;
			thisLineRenderer.endColor = myPpt.BaseColor;
		}
		staticHeadStartPoint = staticHead.transform.position + new Vector3(0f, 0f, 0f - headLength);
		staticHeadMiddlePoint = staticHeadStartPoint + new Vector3(0f, 0f, 0f - neckLength);
		Vector3 dir = Tool2D.GetDir(moveDirection, -90f);
		if (dir == Vector3.zero)
		{
			dir = Tool2D.GetDir();
		}
		Vector3 vector = Quaternion.AngleAxis(moveStartAngle, dir) * new Vector3(0f, 0f, 0f - middleLength) * idleLengthScale;
		headsMiddlePoint = staticHeadMiddlePoint + vector;
		moveHeadMiddlePoint = headsMiddlePoint + Quaternion.AngleAxis(0f - moveMiddleAngle, dir) * -vector;
		Vector3 vector2 = Quaternion.AngleAxis(moveEndAngle, dir) * new Vector3(0f, 0f, -1f);
		moveHeadEndPoint = moveHeadMiddlePoint + vector2 * neckLength * idleLengthScale;
		movingHead.SetPosition(moveHeadMiddlePoint + vector2 * (neckLength + headLength) * idleLengthScale);
		headSpriteTransform[0].transform.position = Tool2D.GetLayerPoint(staticHeadStartPoint) + myPpt.Tsf_BeHit.localPosition + new Vector3(0f, 0f, headLength) * 0.001f;
		headSpriteTransform[0].transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.GetLayerPoint(staticHeadStartPoint) - Tool2D.GetLayerPoint(staticHead.transform.position)));
		headSpriteTransform[1].transform.position = Tool2D.GetLayerPoint(moveHeadEndPoint) + myPpt.Tsf_BeHit.localPosition + new Vector3(0f, 0f, headLength) * 0.001f;
		headSpriteTransform[1].transform.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, Tool2D.GetLayerPoint(moveHeadEndPoint) - Tool2D.GetLayerPoint(movingHead.transform.position)));
		if (headSwitched)
		{
			SetSingleFlip(headSprites[0], moveDirection.x);
			headSwitched = false;
		}
		if (moveDirection.x > 0f)
		{
			if (headSpriteTransform[1].transform.up == Vector3.down)
			{
				SetSingleFlip(headSprites[1], flipX: true);
			}
			else
			{
				SetSingleFlip(headSprites[1], flipX: false);
			}
		}
		else if (headSpriteTransform[1].transform.up == Vector3.down)
		{
			SetSingleFlip(headSprites[1], flipX: false);
		}
		else
		{
			SetSingleFlip(headSprites[1], flipX: true);
		}
		Vector3 v = staticHeadStartPoint + new Vector3(0f, 0f, 1f) * ((headSprites[0].transform.localPosition + Vector3.Scale(headVertical[0].localPosition, headSprites[0].transform.localScale)).magnitude + neckExtraFix * idleLengthScale);
		Vector3 v2 = moveHeadEndPoint + vector2 * ((headSprites[1].transform.localPosition + Vector3.Scale(headVertical[1].localPosition, headSprites[1].transform.localScale)).magnitude + neckExtraFix * idleLengthScale);
		for (int i = 0; i < linePoints; i++)
		{
			Vector3 rootPoint = FivePointsBezierCurve(v, staticHeadMiddlePoint, headsMiddlePoint, moveHeadMiddlePoint, v2, (float)i / (float)linePoints);
			rootPoint += myPpt.Tsf_BeHit.localPosition;
			thisLineRenderer.SetPosition(i, Tool2D.GetLayerPoint(rootPoint) + new Vector3(0f, 0f, headLength) * 0.001f);
		}
		shadow.SetPosition(0, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Heads[0].transform.position), LayerCorrectType.Shadow) + new Vector3(0f, shadowOffsetY, 0f));
		shadow.SetPosition(1, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Heads[1].transform.position), LayerCorrectType.Shadow) + new Vector3(0f, shadowOffsetY, 0f));
		headShadow[0].position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(new Vector3(headShadowCenter[0].position.x, staticHead.transform.position.y, staticHead.transform.position.z)), LayerCorrectType.Shadow) + new Vector3(0f, shadowOffsetY, 0f);
		headShadow[1].position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(new Vector3(headShadowCenter[1].position.x, movingHead.transform.position.y, movingHead.transform.position.z)), LayerCorrectType.Shadow) + new Vector3(0f, shadowOffsetY, 0f);
	}

	public Vector3 FivePointsBezierCurve(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float t)
	{
		float num = 1f - t;
		return num * num * num * num * v0 + 4f * num * num * num * t * v1 + 6f * num * num * t * t * v2 + 4f * t * t * t * num * v3 + t * t * t * t * v4;
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "Impact")
		{
			Impact();
			staticHead.SetCanTouch(canTouch: true);
		}
	}
}
