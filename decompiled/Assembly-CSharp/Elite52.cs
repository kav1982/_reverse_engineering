using Unity.Physics;
using UnityEngine;

public class Elite52 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Move,
		JumpPrepare,
		Jump,
		JumpAfter
	}

	private StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("Jump")]
	public VariableFloat jumpOffsetRange;

	public float maxJumpDistance;

	public float gravity;

	public float upSpeed;

	public float attackCD;

	public float playerMotionPredictRatio;

	public ShockParam shock;

	[Header("Warning")]
	public LineRenderer warningLine1;

	public LineRenderer warningLine2;

	public float warningLineLength;

	private Vector3 lastAimPoint;

	private bool useCrossDrop = true;

	private bool jumping;

	private UIEndlessEliteHpBar hpBar;

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
		hpBar = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		stateExistTime = 0f;
		bornIdleTimer = 0f;
		jumping = false;
		useCrossDrop = true;
		hpBar.gameObject.SetActive(value: true);
		HideDropWarning();
		SetCanTouch(canTouch: true);
		ZeroPhysicsVelocity();
		GetNearestTargetPlayerFirst();
	}

	public unsafe override void Update()
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
			SetMove(Vector3.zero, isFlip: false);
			if (bornIdleTimer > 0.5f)
			{
				state = MonsterState.Move;
				stateExistTime = 2f;
			}
			break;
		case MonsterState.Move:
			if (changedState)
			{
				base.Anima.Play("Move");
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
				if (stateExistTime > attackCD && ToTargetDistanceSqr() < maxJumpDistance * maxJumpDistance)
				{
					lastAimPoint = base.TargetPoint;
					state = MonsterState.JumpPrepare;
				}
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			break;
		case MonsterState.JumpPrepare:
			if (changedState)
			{
				base.Anima.Play("JumpBefore");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				lastAimPoint = base.TargetPoint;
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Jump:
			if (changedState)
			{
				base.Anima.Play("Jump");
				SEMgr.Inst.monster310_Jump.PlaySE();
				StartJump();
				SetCanTouch(canTouch: false);
				base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
				PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
				componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
				SetComponentData(componentData);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (jumping && base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				SetCanTouch(canTouch: true);
				HideDropWarning();
				base.gameObject.layer = LayerMask.NameToLayer("Monster");
				PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
				componentData2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.Collide);
				SetComponentData(componentData2);
				state = MonsterState.JumpAfter;
			}
			break;
		case MonsterState.JumpAfter:
			if (changedState)
			{
				base.Anima.Play("JumpAfter");
				jumping = false;
				CreateDropPattern();
			}
			SetMove(Vector3.zero, isFlip: false);
			ZeroPhysicsVelocity();
			break;
		}
	}

	private void StartJump()
	{
		if (base.HaveTarget)
		{
			lastAimPoint = base.TargetPoint;
			if (targetEntity == PlayerMgr.Inst.PlayerEtt)
			{
				lastAimPoint += PlayerMgr.Inst.PlayerCtrller.CurrentMotion * playerMotionPredictRatio;
			}
		}
		Vector3 vector = Tool2D.GetDir() * jumpOffsetRange.RandomResult();
		Vector3 vector2 = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPointIngoreZ(lastAimPoint + vector) - base.transform.position);
		float num = Mathf.Min(maxJumpDistance, vector2.magnitude);
		if (num <= 0.01f)
		{
			vector2 = Tool2D.IgnoreZPoint(Tool2D.GetNavMeshPointIngoreZ(base.transform.position + Tool2D.GetDir() * Mathf.Min(maxJumpDistance, 1f)) - base.transform.position);
			num = vector2.magnitude;
		}
		Vector3 normalized = vector2.normalized;
		float num2 = GeneralTool.CannonSpeed(upSpeed, 0f, gravity, num);
		Vector3 center = base.transform.position + normalized * num;
		base.Rigid.linearVelocity = normalized * num2;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(upSpeed, gravity);
		SetCanTouch(canTouch: false);
		jumping = true;
		SetFlip(normalized.x);
		ShowDropWarning(center);
	}

	private void CreateDropPattern()
	{
		CamController.Inst.SetShock(shock);
		SEMgr.Inst.boss51LineAttack.PlaySE().pitch = 1.2f;
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite52_Drop", Tool2D.IgnoreZPoint(base.transform.position)).GetComponent<Elite52_Drop>().Initialize(myPpt.myEntity, useCrossDrop);
		useCrossDrop = !useCrossDrop;
	}

	private void ShowDropWarning(Vector3 center)
	{
		Vector3 dir = (useCrossDrop ? Vector3.right : Tool2D.GetDir(45f));
		Vector3 dir2 = (useCrossDrop ? Vector3.up : Tool2D.GetDir(135f));
		SetWarningLine(warningLine1, center, dir);
		SetWarningLine(warningLine2, center, dir2);
	}

	private void SetWarningLine(LineRenderer line, Vector3 center, Vector3 dir)
	{
		if (!(line == null))
		{
			line.positionCount = 10;
			line.enabled = true;
			Vector3 a = center - dir * warningLineLength * 0.5f;
			Vector3 b = center + dir * warningLineLength * 0.5f;
			for (int i = 0; i < line.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(a, b, (float)i / (float)(line.positionCount - 1));
				line.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
		}
	}

	private void HideDropWarning()
	{
		if (warningLine1 != null)
		{
			warningLine1.enabled = false;
		}
		if (warningLine2 != null)
		{
			warningLine2.enabled = false;
		}
	}

	protected override void BossDeadStay()
	{
		HideDropWarning();
		base.BossDeadStay();
	}

	private void SetCanTouch(bool canTouch)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = canTouch;
		SetComponentData(componentData);
	}

	private void ZeroPhysicsVelocity()
	{
		base.Rigid.linearVelocity = Vector3.zero;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "JumpPrepareFinish"))
		{
			if (animaName == "JumpAfterFinish")
			{
				state = MonsterState.Move;
			}
		}
		else
		{
			state = MonsterState.Jump;
		}
	}
}
