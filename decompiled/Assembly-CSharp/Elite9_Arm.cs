using System;
using UnityEngine;

public class Elite9_Arm : MonoBehaviour
{
	public enum LegState
	{
		Idle,
		Move,
		Lift,
		Drop,
		Prepare,
		AfterPrepare,
		BeforeAttack,
		Attack,
		AfterAttack,
		AttackBack,
		DoubleAfterAttack,
		LockHeight,
		BeforeKnock,
		Knock
	}

	[Header("手臂部件长度")]
	public float armLength1;

	public float armLength2;

	public float armLength3;

	public float shoulderLength;

	public float shouderStartOffset;

	public float shoulderOffset;

	public float shoulderMaxAngle;

	private float shoulderAngleFix;

	public float shoulderExtraLength;

	public float arm1ExtraLength;

	public float arm2ExtraLength;

	public float arm3ExtraLength;

	[Header("手臂角度ik计算")]
	public float allowedOffset;

	private float resultAngle;

	private float maxAngle;

	private float minAngle;

	private float lengthResult1;

	private float lengthResult2;

	private float lengthResultTotal;

	public float maxDivideTime;

	private float nowDivideTime;

	private float nowDistance;

	private float timeRatio;

	private AnimationCurve movingHeightCurve;

	private AnimationCurve movingAngleCurve;

	private float movingTime;

	private float movingAngle;

	private float movingDistance;

	private float movingHeight;

	private float movingTimer;

	private float beforeMoveHeight;

	private float beforeMoveAngle;

	private float beforeMoveDistance;

	private float beforeMoveRotation;

	private float targetMoveAngle;

	private float targetMoveDistance;

	private float targetMoveRotation;

	private float beforeShoulderAngleFix;

	private float beforeShoulderOffsetFix;

	private float lockedDeltaHeight;

	private float lockHeightOffsetAngle;

	[Header("移动状态")]
	public AnimationCurve liftHeightCurve;

	public AnimationCurve dropHeightCurve;

	public float liftingDistanceFix;

	public float liftMaxHeight;

	public float liftingTime;

	public float droppingTime;

	private float shoulderOffsetFix;

	public Elite9_Arm otherArm;

	public bool isRight;

	[Header("准备姿态")]
	public float prepareAngle;

	public float prepareRotation;

	public float prepareDistanceFix;

	public float afterPrepareDistanceFix;

	public AnimationCurve prepareHeightCurve;

	public AnimationCurve afterPrepareHeightCurve;

	public float prepareTime;

	public float afterPrepareTime;

	public bool prepareDone;

	private bool lockHeightLerp;

	[Header("攻击姿态")]
	public float attackDistanceFix;

	public float beforeAttackDistanceFix;

	public float afterAttackDistanceFix;

	public AnimationCurve beforeAttackHeightCurve;

	public AnimationCurve attackHeightCurve;

	public AnimationCurve afterAttackHeightCurve;

	public AnimationCurve beforeAttackAngleCurve;

	public AnimationCurve attackAngleCurve;

	public AnimationCurve afterAttackAngleCurve;

	public float beforeAttackTime;

	public float attackTime;

	public float afterAttackTime;

	public float doubleAfterAttackTime;

	public float beforeAttackAngle;

	public float attackAngle;

	public float afterAttackAngle;

	private float attackBackShoulderAngle;

	public float attackBackDistanceFix;

	public AnimationCurve attackBackHeightCurve;

	public AnimationCurve attackBackAngleCurve;

	public float attackBackTime;

	public GameObject particleRoot;

	public ParticleSystem bladeParticle;

	[Header("下砸")]
	public AnimationCurve beforeKnockHeightCurve;

	public float beforeKnockTime;

	public AnimationCurve knockHeightCurve;

	public float knockTime;

	public float knockAngle;

	[Header("线渲染器")]
	public LineRenderer lr_Shoulder;

	public LineRenderer lr_Arm1;

	public LineRenderer lr_Arm2;

	public LineRenderer lr_Arm3;

	public LineRenderer lr_ShadowArm1;

	public LineRenderer lr_ShadowArm2;

	public LineRenderer lr_ShadowArm3;

	public LineRenderer lr_ShadowShoulder;

	[Header("行走约束参数")]
	public float moveSpeed;

	public float normalDistance;

	public float moveMinRatio;

	public float armRepositionAngle;

	public float armMaxAngle;

	public float armMinAngle;

	[Header("手臂旋转")]
	public float armMaxRotation;

	public float armRotationFix;

	[Header("引用组件")]
	public Elite9 master;

	public Elite9_BodyLerp armRoot;

	[Header("和谐模式")]
	public Material mt_Shoulder_H;

	public Material mt_Arm1_H;

	public Material mt_Arm2_H;

	public Material mt_Arm3_H;

	public Material mt_Shoulder_H_S;

	public Material mt_Arm1_H_S;

	public Material mt_Arm2_H_S;

	public Material mt_Arm3_H_S;

	public ParticleSystem bladeParticle_H;

	public GameObject particleRoot_H;

	private Vector3 masterLastMotion;

	public bool canMove;

	public LegState _state;

	private bool stateQuit;

	private bool changedState;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private bool frame1Initiated;

	private Vector3 startPoint => armRoot.truePosition + Tool2D.GetDir(masterLastMotion, isRight ? (-90f + shoulderMaxAngle * shoulderAngleFix) : (90f - shoulderMaxAngle * shoulderAngleFix)).normalized * shoulderLength + new Vector3(0f, 0f, shoulderOffset * shoulderOffsetFix);

	private Vector3 groundStartPoint => Tool2D.IgnoreZPoint(startPoint);

	private float offsetAngle => Tool2D.IgnoreZAngleWithSign(masterLastMotion, Tool2D.IgnoreZPoint(currentEndPoint - groundStartPoint)) + master.moveFixAngle;

	public LegState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateQuit = true;
			_state = value;
		}
	}

	private void Update()
	{
		if (!frame1Initiated)
		{
			frame1Initiated = true;
			Frame1Initial();
		}
		Debug.DrawRay(master.transform.position, master.moveDiration * 2f, Color.green);
		Debug.DrawRay(master.transform.position, Tool2D.GetDir(master.moveDiration, master.moveFixAngle) * 2f);
		Debug.DrawRay(master.transform.position, Tool2D.GetDir(masterLastMotion, isRight ? (0f - armRepositionAngle + master.moveFixAngle) : (armRepositionAngle + master.moveFixAngle)).normalized * 2f, isRight ? Color.blue : Color.red);
		masterLastMotion = master.moveDiration.normalized;
		if (!master.enabled)
		{
			if (lr_Arm1.startColor != master.myPpt.BaseColor)
			{
				lr_Arm1.startColor = master.myPpt.BaseColor;
				lr_Arm1.endColor = master.myPpt.BaseColor;
				lr_Arm2.startColor = master.myPpt.BaseColor;
				lr_Arm2.endColor = master.myPpt.BaseColor;
				lr_Arm3.startColor = master.myPpt.BaseColor;
				lr_Arm3.endColor = master.myPpt.BaseColor;
				lr_Shoulder.startColor = master.myPpt.BaseColor;
				lr_Shoulder.endColor = master.myPpt.BaseColor;
			}
			return;
		}
		if (Input.GetKeyDown(KeyCode.Q) && state != LegState.Prepare)
		{
			state = LegState.Prepare;
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
		switch (state)
		{
		case LegState.Idle:
			currentEndPoint.z = 0f;
			break;
		case LegState.Lift:
			if (changedState)
			{
				SetArmMoveStart(liftingTime, normalDistance * liftingDistanceFix, armRepositionAngle, liftHeightCurve);
			}
			movingTimer += Time.deltaTime;
			SetArmMove();
			if (movingTimer > movingTime)
			{
				state = LegState.Drop;
			}
			else
			{
				shoulderOffsetFix = Mathf.Lerp(1f, 0f, liftHeightCurve.Evaluate(timeRatio));
			}
			break;
		case LegState.Drop:
			if (changedState)
			{
				SetArmMoveStart(droppingTime, normalDistance, armRepositionAngle, dropHeightCurve);
			}
			movingTimer += Time.deltaTime;
			SetArmMove();
			if (movingTimer > movingTime)
			{
				SEMgr.Inst.elite9Step.PlaySE();
				currentEndPoint.z = 0f;
				state = LegState.Idle;
			}
			else
			{
				shoulderOffsetFix = Mathf.Lerp(1f, 0f, dropHeightCurve.Evaluate(timeRatio));
			}
			break;
		case LegState.Prepare:
			if (changedState)
			{
				SetArmMoveStart(prepareTime, normalDistance * prepareDistanceFix, prepareAngle, prepareHeightCurve, null, prepareRotation);
				prepareDone = false;
			}
			if (movingTimer < movingTime)
			{
				movingTimer += Time.deltaTime;
			}
			SetArmMove(useMoveFix: false);
			if (movingTimer >= movingTime)
			{
				state = LegState.LockHeight;
			}
			break;
		case LegState.AfterPrepare:
			if (changedState)
			{
				SetArmMoveStart(afterPrepareTime, normalDistance * afterPrepareDistanceFix, armRepositionAngle, afterPrepareHeightCurve, null, 0f, lockHeightLerp: true);
			}
			movingTimer += Time.deltaTime;
			SetArmMove();
			if (movingTimer > movingTime)
			{
				SEMgr.Inst.elite9Step.PlaySE();
				if (otherArm.isRight)
				{
					otherArm.canMove = true;
				}
				state = LegState.Idle;
			}
			break;
		case LegState.BeforeAttack:
			if (changedState)
			{
				beforeShoulderAngleFix = shoulderAngleFix;
				beforeShoulderOffsetFix = shoulderOffsetFix;
				SetArmMoveStart(beforeAttackTime, normalDistance * beforeAttackDistanceFix, beforeAttackAngle, beforeAttackHeightCurve, beforeAttackAngleCurve, 0.45f);
			}
			shoulderAngleFix = Mathf.Lerp(beforeShoulderAngleFix, 0f, liftHeightCurve.Evaluate(timeRatio));
			shoulderOffsetFix = Mathf.Lerp(beforeShoulderOffsetFix, 0f, liftHeightCurve.Evaluate(timeRatio));
			SetArmMove(useMoveFix: false);
			movingTimer += Time.deltaTime;
			if (movingTimer > movingTime)
			{
				state = LegState.LockHeight;
			}
			break;
		case LegState.Attack:
			if (changedState)
			{
				SetArmMoveStart(attackTime, normalDistance * attackDistanceFix, attackAngle, attackHeightCurve, attackAngleCurve, 0.75f);
				bladeParticle.Play();
			}
			movingTimer += Time.deltaTime;
			shoulderAngleFix = Mathf.Lerp(0f, 1f, liftHeightCurve.Evaluate(timeRatio));
			shoulderOffsetFix = Mathf.Lerp(1f, 0f, dropHeightCurve.Evaluate(timeRatio));
			SetArmMove(useMoveFix: false);
			if (movingTimer > movingTime)
			{
				if (master.state == Elite9.MonsterState.DoubleSlash)
				{
					state = LegState.DoubleAfterAttack;
				}
				else
				{
					state = LegState.AfterAttack;
				}
			}
			break;
		case LegState.AfterAttack:
			if (changedState)
			{
				bladeParticle.Stop();
				SetArmMoveStart(afterAttackTime, normalDistance * afterAttackDistanceFix, afterAttackAngle, afterAttackHeightCurve, afterAttackAngleCurve, 0.45f);
			}
			movingTimer += Time.deltaTime;
			shoulderAngleFix = Mathf.Lerp(0f, 1f, dropHeightCurve.Evaluate(timeRatio));
			shoulderOffsetFix = Mathf.Lerp(1f, 0f, liftHeightCurve.Evaluate(timeRatio));
			SetArmMove(useMoveFix: false);
			if (movingTimer > movingTime)
			{
				shoulderAngleFix = 0f;
				shoulderAngleFix = 0f;
				state = LegState.LockHeight;
			}
			break;
		case LegState.DoubleAfterAttack:
			if (changedState)
			{
				bladeParticle.Stop();
				SetArmMoveStart(doubleAfterAttackTime, normalDistance * afterAttackDistanceFix, afterAttackAngle, afterAttackHeightCurve, afterAttackAngleCurve, 0.45f);
			}
			movingTimer += Time.deltaTime;
			shoulderAngleFix = Mathf.Lerp(0f, 1f, dropHeightCurve.Evaluate(timeRatio));
			shoulderOffsetFix = Mathf.Lerp(1f, 0f, liftHeightCurve.Evaluate(timeRatio));
			SetArmMove(useMoveFix: false);
			if (movingTimer > movingTime)
			{
				shoulderAngleFix = 0f;
				shoulderAngleFix = 0f;
				state = LegState.LockHeight;
			}
			break;
		case LegState.AttackBack:
			if (changedState)
			{
				attackBackShoulderAngle = shoulderAngleFix;
				shoulderAngleFix = 0f;
				bladeParticle.Stop();
				SetArmMoveStart(attackBackTime, normalDistance * attackBackDistanceFix, armRepositionAngle, attackBackHeightCurve);
			}
			shoulderAngleFix = Mathf.Lerp(attackBackShoulderAngle, 0f, movingTimer / attackBackTime);
			movingTimer += Time.deltaTime;
			SetArmMove();
			if (movingTimer > movingTime)
			{
				SEMgr.Inst.elite9Step.PlaySE();
				state = LegState.Idle;
			}
			break;
		case LegState.BeforeKnock:
			if (changedState)
			{
				beforeShoulderOffsetFix = shoulderOffsetFix;
				SetArmMoveStart(beforeKnockTime, normalDistance * 0.8f, knockAngle, beforeKnockHeightCurve);
			}
			shoulderOffsetFix = Mathf.Lerp(beforeShoulderOffsetFix, 0f, liftHeightCurve.Evaluate(timeRatio));
			movingTimer += Time.deltaTime;
			if (movingTimer > movingTime)
			{
				state = LegState.LockHeight;
			}
			else
			{
				SetArmMove(useMoveFix: false);
			}
			break;
		case LegState.Knock:
			if (changedState)
			{
				bladeParticle.Play();
				SetArmMoveStart(knockTime, normalDistance * 0.8f, knockAngle, knockHeightCurve);
			}
			movingTimer += Time.deltaTime;
			shoulderOffsetFix = Mathf.Lerp(0f, 1f, liftHeightCurve.Evaluate(timeRatio));
			if (movingTimer > movingTime)
			{
				bladeParticle.Stop();
				master.GroundKnock(Tool2D.IgnoreZPoint(currentEndPoint));
				state = LegState.Idle;
			}
			else
			{
				SetArmMove(useMoveFix: false);
			}
			break;
		case LegState.LockHeight:
			if (changedState)
			{
				lockedDeltaHeight = startPoint.z - currentEndPoint.z;
			}
			currentEndPoint = startPoint + Tool2D.GetDir(masterLastMotion, movingAngle).normalized * movingDistance + new Vector3(0f, 0f, 0f - lockedDeltaHeight);
			break;
		default:
			Debug.LogError(state);
			break;
		}
		Divide();
		Vector3 normalized = (currentEndPoint - startPoint).normalized;
		Vector3 dir = Tool2D.GetDir(Tool2D.IgnoreZPoint(normalized), 90f);
		Vector3 vector = startPoint + normalized * lengthResult1 + Quaternion.AngleAxis((isRight ? (0f - armRotationFix) : armRotationFix) * armMaxRotation, normalized) * (Quaternion.AngleAxis(0f - resultAngle, dir) * normalized) * armLength2 / 2f;
		Vector3 vector2 = startPoint + normalized * lengthResult1 + Quaternion.AngleAxis((isRight ? (0f - armRotationFix) : armRotationFix) * armMaxRotation, normalized) * (Quaternion.AngleAxis(0f - (resultAngle + 180f), dir) * normalized) * armLength2 / 2f;
		Vector3 v = groundStartPoint;
		lr_ShadowArm1.SetPosition(0, Tool2D.IgnoreZPoint(v, 1.05f));
		lr_ShadowArm1.SetPosition(1, Tool2D.IgnoreZPoint(vector, 1.05f));
		lr_ShadowArm2.SetPosition(0, Tool2D.IgnoreZPoint(vector, 1.05f));
		lr_ShadowArm2.SetPosition(1, Tool2D.IgnoreZPoint(vector2, 1.05f));
		lr_ShadowArm3.SetPosition(0, Tool2D.IgnoreZPoint(vector2 - (currentEndPoint - vector2), 1.05f));
		lr_ShadowArm3.SetPosition(1, Tool2D.IgnoreZPoint(vector2, 1.05f));
		lr_ShadowArm3.SetPosition(2, Tool2D.IgnoreZPoint(currentEndPoint, 1.05f));
		lr_Shoulder.SetPosition(0, Tool2D.IgnoreZPoint(startPoint, 1.05f));
		lr_Shoulder.SetPosition(1, Tool2D.IgnoreZPoint(startPoint + (armRoot.truePosition - startPoint).normalized * (shoulderLength - shouderStartOffset), 1.05f));
		lr_Arm1.SetPosition(0, Tool2D.GetLayerPoint(startPoint));
		lr_Arm1.SetPosition(1, Tool2D.GetLayerPoint(vector + (vector - startPoint) * arm1ExtraLength));
		lr_Arm2.SetPosition(0, Tool2D.GetLayerPoint(vector));
		lr_Arm2.SetPosition(1, Tool2D.GetLayerPoint(vector2 + (vector2 - vector) * arm2ExtraLength));
		lr_Arm3.SetPosition(0, Tool2D.GetLayerPoint(vector2 - (currentEndPoint - vector2) * arm3ExtraLength));
		lr_Arm3.SetPosition(1, Tool2D.GetLayerPoint(vector2));
		lr_Arm3.SetPosition(2, Tool2D.GetLayerPoint(currentEndPoint));
		lr_Shoulder.SetPosition(0, Tool2D.GetLayerPoint(startPoint) + (startPoint - armRoot.truePosition).normalized * shoulderExtraLength);
		lr_Shoulder.SetPosition(1, Tool2D.GetLayerPoint(startPoint + (armRoot.truePosition - startPoint).normalized * (shoulderLength - shouderStartOffset)));
		if (Tool2D.IgnoreZAngleWithSign(lr_Arm1.GetPosition(0) - lr_Arm1.GetPosition(1), lr_Arm2.GetPosition(1) - lr_Arm1.GetPosition(1)) < 0f)
		{
			lr_Arm3.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Arm3.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
		}
		else
		{
			lr_Arm3.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Arm3.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
		}
		particleRoot.transform.position = lr_Arm3.GetPosition(0);
		particleRoot.transform.localScale = Vector3.one * Vector3.Distance(lr_Arm3.GetPosition(0), lr_Arm3.GetPosition(2)) / 2f;
		particleRoot.transform.right = lr_Arm3.GetPosition(2) - lr_Arm3.GetPosition(0);
		if (lr_Arm1.startColor != master.myPpt.BaseColor)
		{
			lr_Arm1.startColor = master.myPpt.BaseColor;
			lr_Arm1.endColor = master.myPpt.BaseColor;
			lr_Arm2.startColor = master.myPpt.BaseColor;
			lr_Arm2.endColor = master.myPpt.BaseColor;
			lr_Arm3.startColor = master.myPpt.BaseColor;
			lr_Arm3.endColor = master.myPpt.BaseColor;
			lr_Shoulder.startColor = master.myPpt.BaseColor;
			lr_Shoulder.endColor = master.myPpt.BaseColor;
		}
	}

	private void SetArmMoveStart(float movingTime, float targetMoveDistance, float targetMoveAngle, AnimationCurve movingHeightCurve = null, AnimationCurve movingAngleCurve = null, float armRotationFix = 0f, bool lockHeightLerp = false)
	{
		this.targetMoveAngle = targetMoveAngle;
		this.targetMoveDistance = targetMoveDistance;
		targetMoveRotation = armRotationFix;
		this.movingAngleCurve = movingAngleCurve;
		beforeMoveRotation = this.armRotationFix;
		beforeMoveAngle = offsetAngle;
		movingTimer = 0f;
		beforeMoveDistance = Tool2D.IgnoreZPoint(currentEndPoint - groundStartPoint).magnitude;
		this.movingTime = movingTime;
		this.movingHeightCurve = movingHeightCurve;
		this.lockHeightLerp = lockHeightLerp;
		beforeMoveHeight = 0f - currentEndPoint.z;
	}

	private void SetArmMove(bool useMoveFix = true)
	{
		float num = (useMoveFix ? 1 : 0);
		timeRatio = movingTimer / movingTime;
		if (movingAngleCurve == null)
		{
			movingAngle = Mathf.Lerp(beforeMoveAngle, isRight ? (0f - targetMoveAngle + master.moveFixAngle * num) : (targetMoveAngle + master.moveFixAngle * num), timeRatio);
		}
		else
		{
			movingAngle = FakeLerp(beforeMoveAngle, isRight ? (0f - targetMoveAngle + master.moveFixAngle * num) : (targetMoveAngle + master.moveFixAngle * num), movingAngleCurve.Evaluate(timeRatio));
		}
		movingDistance = Mathf.Lerp(beforeMoveDistance, targetMoveDistance, timeRatio);
		armRotationFix = Mathf.Lerp(beforeMoveRotation, targetMoveRotation, timeRatio);
		if (movingHeightCurve != null)
		{
			if (lockHeightLerp)
			{
				movingHeight = beforeMoveHeight * movingHeightCurve.Evaluate(timeRatio);
			}
			else
			{
				movingHeight = liftMaxHeight * movingHeightCurve.Evaluate(timeRatio);
			}
		}
		currentEndPoint = groundStartPoint + Tool2D.GetDir(masterLastMotion, movingAngle).normalized * movingDistance + new Vector3(0f, 0f, 0f - movingHeight);
	}

	private float FakeLerp(float x, float y, float t)
	{
		return y * t + x * (1f - t);
	}

	public void SingleInitial(Elite9 master, bool rightLeg)
	{
		lr_Arm1.positionCount = 2;
		lr_Arm2.positionCount = 2;
		lr_Arm3.positionCount = 3;
		lr_Shoulder.positionCount = 2;
		this.master = master;
		isRight = rightLeg;
		if (GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Destroy(lr_Arm1.material);
			UnityEngine.Object.Destroy(lr_Arm2.material);
			UnityEngine.Object.Destroy(lr_Arm3.material);
			UnityEngine.Object.Destroy(lr_Shoulder.material);
			UnityEngine.Object.Destroy(lr_ShadowArm1.material);
			UnityEngine.Object.Destroy(lr_ShadowArm2.material);
			UnityEngine.Object.Destroy(lr_ShadowArm3.material);
			UnityEngine.Object.Destroy(lr_ShadowShoulder.material);
			lr_Arm1.material = mt_Arm1_H;
			lr_Arm2.material = mt_Arm2_H;
			lr_Arm3.material = mt_Arm3_H;
			lr_Shoulder.material = mt_Shoulder_H;
			lr_ShadowArm1.material = mt_Arm1_H_S;
			lr_ShadowArm2.material = mt_Arm2_H_S;
			lr_ShadowArm3.material = mt_Arm3_H_S;
			lr_ShadowShoulder.material = mt_Shoulder_H_S;
			particleRoot = particleRoot_H;
			bladeParticle = bladeParticle_H;
		}
	}

	public void EveryInitial()
	{
		lr_Arm1.SetPosition(0, Vector3.zero);
		lr_Arm1.SetPosition(1, Vector3.zero);
		lr_Arm2.SetPosition(0, Vector3.zero);
		lr_Arm2.SetPosition(1, Vector3.zero);
		lr_Arm3.SetPosition(0, Vector3.zero);
		lr_Arm3.SetPosition(1, Vector3.zero);
		lr_Arm3.SetPosition(2, Vector3.zero);
		lr_Shoulder.SetPosition(0, Vector3.zero);
		lr_Shoulder.SetPosition(1, Vector3.zero);
		lr_ShadowArm1.SetPosition(0, Vector3.zero);
		lr_ShadowArm1.SetPosition(1, Vector3.zero);
		lr_ShadowArm2.SetPosition(0, Vector3.zero);
		lr_ShadowArm2.SetPosition(1, Vector3.zero);
		lr_ShadowArm3.SetPosition(0, Vector3.zero);
		lr_ShadowArm3.SetPosition(1, Vector3.zero);
		lr_ShadowArm3.SetPosition(2, Vector3.zero);
		lr_ShadowShoulder.SetPosition(0, Vector3.zero);
		lr_ShadowShoulder.SetPosition(1, Vector3.zero);
	}

	public void Frame1Initial()
	{
		masterLastMotion = Vector3.right;
		moveToEndPoint = groundStartPoint + Tool2D.GetDir(Vector3.right, isRight ? (0f - armRepositionAngle) : armRepositionAngle).normalized * normalDistance;
		currentEndPoint = moveToEndPoint;
		state = LegState.Idle;
		if (isRight)
		{
			canMove = true;
		}
		if (!isRight)
		{
			lr_Arm1.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Arm1.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_Arm2.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Arm2.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_Shoulder.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_Shoulder.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_ShadowArm1.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_ShadowArm1.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_ShadowArm2.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_ShadowArm2.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
			lr_ShadowShoulder.material.SetVector("_Tile", new Vector4(1f, -1f, 0f, 0f));
			lr_ShadowShoulder.material.SetVector("_Offset", new Vector4(0f, 1f, 0f, 0f));
		}
		else
		{
			lr_Arm1.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Arm1.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_Arm2.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Arm2.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_Shoulder.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_Shoulder.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_ShadowArm1.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_ShadowArm1.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_ShadowArm2.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_ShadowArm2.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
			lr_ShadowShoulder.material.SetVector("_Tile", new Vector4(1f, 1f, 0f, 0f));
			lr_ShadowShoulder.material.SetVector("_Offset", new Vector4(0f, 0f, 0f, 0f));
		}
	}

	private void SingleDivide()
	{
		float num = Mathf.Cos(resultAngle * (MathF.PI / 180f)) * 2f * armLength2 / 2f;
		float num2 = armLength2 * armLength2 / 4f - armLength1 * armLength1;
		float num3 = num * num - 4f * num2;
		if (num3 < 0f)
		{
			Debug.LogWarning("肢体必须大于中间肢体的一半，否则会没有解");
			return;
		}
		lengthResult1 = (0f - num + Mathf.Pow(num3, 0.5f)) / 2f;
		float num4 = Mathf.Cos(resultAngle * (MathF.PI / 180f)) * 2f * armLength2 / 2f;
		float num5 = armLength2 * armLength2 / 4f - armLength3 * armLength3;
		float f = num4 * num4 - 4f * num5;
		lengthResult2 = (0f - num4 + Mathf.Pow(f, 0.5f)) / 2f;
		lengthResultTotal = lengthResult1 + lengthResult2;
	}

	private void Divide()
	{
		maxAngle = 179.5f;
		minAngle = 0.5f;
		nowDivideTime = 0f;
		resultAngle = (maxAngle + minAngle) / 2f;
		SingleDivide();
		nowDistance = Vector3.Distance(startPoint, currentEndPoint);
		if (nowDistance > armLength1 + armLength2 + armLength3)
		{
		}
		while (Mathf.Abs(lengthResultTotal - nowDistance) > allowedOffset && nowDivideTime < maxDivideTime)
		{
			if (lengthResultTotal > nowDistance)
			{
				maxAngle = resultAngle;
				resultAngle = (maxAngle + minAngle) / 2f;
			}
			else
			{
				minAngle = resultAngle;
				resultAngle = (maxAngle + minAngle) / 2f;
			}
			SingleDivide();
			nowDivideTime += 1f;
		}
	}
}
