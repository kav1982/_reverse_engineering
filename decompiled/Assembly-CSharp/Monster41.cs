using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class Monster41 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Track,
		Stay,
		Back
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	public float headHeightBase;

	public float headHeightOffset;

	public float headHeightSpeed;

	public VariableFloat headHeightPhase;

	public float headHeight;

	public float range;

	public float showRange;

	public Vector3 headPoint;

	public float headMoveSpeed;

	public LineRenderer lr_Neck;

	public LineRenderer lr_Neck_Shadow;

	public AnimationCurve HeightCurveReverse;

	public AnimationCurve HeightCurve;

	public float CurveLength;

	[Header("停留")]
	public float stayTime;

	[Header("头部")]
	public float biteRange;

	public int biteDamage;

	public Transform tsf_HeadShadow;

	public Transform tsf_Head;

	public Transform tsf_DamagePoint;

	public Animator HeadAnima;

	public Monster41_AnimaEvent animaEventExtra;

	public Monster41_Head headUnit;

	[Header("头部阴影")]
	public MeshRenderer headMR;

	public MeshRenderer headMRShadow;

	public Transform head;

	public Transform headShadow;

	[Header("头部跟踪")]
	public float slowTime;

	private Vector3 headTargetPoint;

	public float headTargetPointOffset;

	private List<RaycastHit> bigWallHits = new List<RaycastHit>();

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

	public Vector3 fromBody => headPoint - base.transform.position;

	public override void SingleInitialCallback()
	{
		lr_Neck.positionCount = 8;
		lr_Neck_Shadow.positionCount = 2;
		animaEventExtra.DoAction = AnimaAction;
		myPpt.RemoveMRFromArray(headMRShadow);
	}

	public override void EveryInitialCallback()
	{
		headUnit = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + (myPpt.unitCfg.id + 20), base.transform.position).GetComponent<Monster41_Head>();
		headUnit.Initialize(this);
		state = MonsterState.BornIdle;
		headPoint = base.transform.position;
		lr_Neck_Shadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_Neck_Shadow.SetPosition(0, Tool2D.GetLayerPoint(headPoint, LayerCorrectType.Shadow));
		for (int i = 0; i < 6; i++)
		{
			lr_Neck.SetPosition(i, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, (0f - headHeight) * HeightCurve.Evaluate((float)i / 5f))));
		}
		lr_Neck.SetPosition(7, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0.2f)));
		lr_Neck.SetPosition(6, lr_Neck.GetPosition(5));
		tsf_Head.transform.position = lr_Neck.GetPosition(6);
		tsf_HeadShadow.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		tsf_Head.transform.up = Vector3.up;
		headHeightPhase.RandomResult();
	}

	public override void Update()
	{
		lr_Neck.material.SetColor("_Color", myPpt.BaseColor);
		headMRShadow.material.SetTexture(GameConstManaged.shaderTextureIndex, headMR.material.GetTexture(GameConstManaged.shaderTextureIndex));
		headShadow.localScale = head.lossyScale;
		HeadAnima.speed = base.Anima.speed;
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
		headHeight = headHeightBase + headHeightOffset * Mathf.Sin(headHeightSpeed * 2f * MathF.PI * Time.time + headHeightPhase.result);
		if (state == MonsterState.BornIdle || state == MonsterState.Idle)
		{
			for (int i = 0; i < 6; i++)
			{
				lr_Neck.SetPosition(6 - i, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, (0f - headHeight) * HeightCurve.Evaluate((float)i / 5f))));
			}
			lr_Neck.SetPosition(7, Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, 0f, 0.2f)));
			lr_Neck.SetPosition(0, lr_Neck.GetPosition(1));
			tsf_Head.transform.position = lr_Neck.GetPosition(0);
			tsf_Head.transform.up = Vector3.up;
		}
		else
		{
			float num = Mathf.Min(fromBody.magnitude, CurveLength);
			for (int j = 0; j < 6; j++)
			{
				lr_Neck.SetPosition(6 - j, Tool2D.GetLayerPoint(base.transform.position + fromBody.normalized * num * j / 5f + new Vector3(0f, 0f, (0f - headHeight) * HeightCurve.Evaluate((float)j / 5f))));
			}
			bool flag = fromBody.magnitude > CurveLength;
			lr_Neck.SetPosition(0, (!flag) ? lr_Neck.GetPosition(1) : Tool2D.GetLayerPoint(base.transform.position + fromBody + new Vector3(0f, 0f, 0f - headHeight)));
			tsf_Head.transform.position = lr_Neck.GetPosition(0);
			tsf_Head.transform.up = Vector3.Lerp(Vector3.up, fromBody, fromBody.magnitude / CurveLength);
		}
		tsf_HeadShadow.position = Tool2D.GetLayerPoint(headPoint, LayerCorrectType.Shadow);
		tsf_HeadShadow.up = tsf_Head.transform.up;
		lr_Neck_Shadow.SetPosition(1, Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow));
		lr_Neck_Shadow.SetPosition(0, Tool2D.GetLayerPoint(headPoint, LayerCorrectType.Shadow));
		headUnit.transform.position = Tool2D.IgnoreZPoint(tsf_DamagePoint.position);
		headUnit.SyncDotsPositionSafe();
		if (state == MonsterState.BornIdle || state == MonsterState.Idle)
		{
			headUnit.closeImmume = true;
		}
		else
		{
			headUnit.closeImmume = false;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				HeadAnima.Play("Idle");
				bornIdleTimer = 0f;
			}
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Idle;
			}
			else
			{
				SetMove(Vector3.zero);
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				HeadAnima.Play("Idle");
				GetNearestTarget();
			}
			if (!base.HaveTarget)
			{
				checkTargetIntervalTimer += Time.deltaTime;
			}
			if (checkTargetIntervalTimer > 0.1f)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < showRange * showRange)
			{
				state = MonsterState.Track;
				SEMgr.Inst.monster41Stretch.PlaySE();
			}
			SetMove(Vector3.zero);
			break;
		case MonsterState.Track:
		{
			if (changedState)
			{
				GetNearestTarget();
				if (!base.HaveTarget)
				{
					state = MonsterState.Back;
				}
			}
			if (!base.HaveTarget)
			{
				GetNearestTarget();
			}
			if (!base.HaveTarget || ToTargetDistanceSqr() > showRange * showRange)
			{
				state = MonsterState.Stay;
				break;
			}
			headTargetPoint = GetHeadPoint();
			Vector3 vector2 = headTargetPoint - headPoint;
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget();
			}
			if ((headPoint - base.TargetPointIgnoreZ).sqrMagnitude < (showRange - range) * (showRange - range))
			{
				HeadAnima.Play("Attack");
			}
			if (vector2.sqrMagnitude < headMoveSpeed * Time.deltaTime * headMoveSpeed * Time.deltaTime)
			{
				headPoint = headTargetPoint;
			}
			else
			{
				headPoint += vector2.normalized * headMoveSpeed * Time.deltaTime * Mathf.Lerp(0f, 1f, stateExistTime / slowTime);
			}
			SetMove(Vector3.zero);
			break;
		}
		case MonsterState.Stay:
			if (changedState)
			{
				HeadAnima.Play("Idle");
			}
			if (!base.HaveTarget || ToTargetDistanceSqr() > showRange * showRange)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < showRange * showRange)
			{
				state = MonsterState.Track;
			}
			else if (stateExistTime > stayTime)
			{
				state = MonsterState.Back;
			}
			else if (base.HaveTarget)
			{
				headTargetPoint = GetHeadPoint();
				headPoint += (headTargetPoint - headPoint).normalized * headMoveSpeed * Time.deltaTime * Mathf.Lerp(1f, 0f, stateExistTime / slowTime);
			}
			break;
		case MonsterState.Back:
		{
			if (changedState)
			{
				HeadAnima.Play("Idle");
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget();
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < showRange * showRange)
			{
				state = MonsterState.Track;
			}
			Vector3 vector = headPoint - base.transform.position;
			headPoint -= vector.normalized * headMoveSpeed * Time.deltaTime * Mathf.Lerp(0f, 1f, stateExistTime / slowTime);
			if ((double)vector.sqrMagnitude < 0.01)
			{
				state = MonsterState.Idle;
			}
			SetMove(Vector3.zero);
			break;
		}
		}
	}

	public Vector3 GetHeadPoint()
	{
		if (!base.HaveTarget)
		{
			return Vector3.zero;
		}
		float num = Mathf.Min(range + headTargetPointOffset, ToTargetDistance());
		RaycastHit[] array = Physics.RaycastAll(base.transform.position, ToTargetDir(), num, LayerMask.GetMask("Wall"));
		bigWallHits.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].collider.gameObject.name == "WallCollider")
			{
				bigWallHits.Add(array[i]);
			}
		}
		float num2 = num;
		for (int j = 0; j < bigWallHits.Count; j++)
		{
			num2 = Mathf.Min(bigWallHits[j].distance, num2);
			Debug.DrawLine(bigWallHits[j].point, base.transform.position);
		}
		return base.transform.position + ToTargetDir() * num2 - ToTargetDir() * headTargetPointOffset;
	}

	private void Damage()
	{
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(Tool2D.IgnoreZPoint(tsf_DamagePoint), biteRange, GameConst.Filter_MonsterAoeNoSpell, list);
		if (list.Count == 0)
		{
			return;
		}
		EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
		for (int i = 0; i < list.Count; i++)
		{
			if (!GetComponentData<UnitProperty_Dots>(list[i].entity).Affect_InAbyss)
			{
				TakeDamageInfo_Dots element = TakeDamageInfo_Dots.NewInfo(myPpt.myEntity);
				element.damage = biteDamage;
				entityCommandBuffer.AppendToBuffer(list[i].entity, element);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MonsterPunch", list[i].point, 2f);
			}
		}
		entityCommandBuffer.Playback(UnitDotsSyncSystem.entityMgr);
		entityCommandBuffer.Dispose();
	}

	public void ShareDamage(TakeDamageInfo_Dots info)
	{
		info.knockbackForce = Vector3.zero;
		UnitDotsSyncSystem.AddTakeDamageRequest(myPpt.myEntity, info);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		headUnit.DotsAnnouncedDeath();
		headUnit.canDie = true;
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "Attack":
			Damage();
			break;
		case "Bite":
			SEMgr.Inst.monster41Bite.PlaySE();
			break;
		case "AttackFinish":
			if (!base.HaveTarget || (headPoint - base.TargetPointIgnoreZ).sqrMagnitude > (showRange - range) * (showRange - range))
			{
				HeadAnima.Play("Idle");
			}
			break;
		}
	}
}
