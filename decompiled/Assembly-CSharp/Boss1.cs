using System;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

[RequireComponent(typeof(UnitProperty))]
public class Boss1 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	public enum MonsterState
	{
		BornIdle,
		NoTargetMove,
		MoveToTarget,
		SprintBefore,
		SprintJumping,
		Sprinting,
		Flying,
		FlyLand,
		MadnessBefore,
		MadnessJumping,
		MadnessJumpAgain,
		Die
	}

	public Transform tsf_Rotate;

	public float gravity;

	public float noTargetSpeedLimit;

	[Header("Sprint")]
	public VariableFloat sprintInterval;

	public float sprintJumpUpForce;

	public float sprintForwardForce;

	public float sprintMaxDuration;

	[Header("Fly")]
	public GameObject pfb_LandEffect;

	public float flyUpForce;

	public int landingBulletCount;

	public ShockParam landShock;

	[Header("Ram")]
	public GameObject pfb_CollisionEffect;

	public GameObject pfb_CollisionEffect_H;

	public float minRamMagnitude;

	public float perMagnitudeBulletCount;

	public float collisionBulletAngleForNormal;

	[Header("Madness")]
	public VariableFloat madnessInterval;

	public int madnessJumpTime;

	public float madnessExtraGravity;

	public float madnessJumpUpForce;

	public float madnessJumpForwardForce;

	[Header("Spell")]
	public float collideSpellOffset;

	public float collideSpellHeightRandom;

	public float landSpellOffset;

	public VariableFloat spellCollideForwardSpeed;

	public VariableFloat spellCollideUpSpeed;

	public float spellCollideGravity;

	public float spellLandForwardSpeed;

	public float spellLandUpSpeed;

	public float spellLandGravity;

	[Header("和谐模式")]
	public MeshRenderer MR;

	public UnityEngine.Material mt_H;

	[Header("状态机")]
	public MonsterState _state;

	private float stateExistTime;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private Vector3 sprintDir;

	private Vector3 lastVelocity;

	private float sprintDurationTimer;

	private float perimeter;

	private float jumpIntervalTimer;

	private float madnessIntervalTimer;

	private int madnessJumpTimer;

	private bool deadRotate;

	private SpellSpawnParams ssp;

	private bool thisFrameKnockedWall;

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

	public Entity thisEntity { get; set; }

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			sprintForwardForce *= 0.8f;
			spellLandForwardSpeed *= 0.8f;
		}
	}

	public override void EveryInitialCallback()
	{
		perimeter = base.CC_Self.radius * MathF.PI * 2f;
		sprintInterval.RandomResult();
		madnessInterval.RandomResult();
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.IsVelocityDeclice = false;
		SetComponentData(componentData);
		if (GameMgr.IsMobile_Static)
		{
			sprintForwardForce *= 0.8f;
			spellLandForwardSpeed *= 0.8f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Gravity = 0f - spellLandGravity;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Destroy(MR.material);
			MR.material = mt_H;
			pfb_CollisionEffect = pfb_CollisionEffect_H;
		}
	}

	public override void Update()
	{
		thisFrameKnockedWall = false;
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		lastVelocity = base.Rigid.linearVelocity;
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
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
				}
				else
				{
					state = MonsterState.NoTargetMove;
				}
			}
			break;
		case MonsterState.NoTargetMove:
			if (changedState)
			{
				base.Rigid.linearVelocity += Tool2D.GetDir() * base.MoveSpeed * Time.deltaTime;
				SyncDotsVelocity();
			}
			if (noTargetSpeedLimit * noTargetSpeedLimit > base.Rigid.linearVelocity.sqrMagnitude)
			{
				base.Rigid.linearVelocity += base.Rigid.linearVelocity.normalized * base.MoveSpeed * Time.deltaTime;
				SyncDotsVelocity();
			}
			RotateForMove(base.Rigid.linearVelocity);
			jumpIntervalTimer += Time.deltaTime;
			if (jumpIntervalTimer >= sprintInterval.result)
			{
				jumpIntervalTimer = 0f;
				sprintInterval.RandomResult();
				state = MonsterState.SprintBefore;
			}
			madnessIntervalTimer += Time.deltaTime;
			if (madnessIntervalTimer >= madnessInterval.result)
			{
				madnessIntervalTimer = 0f;
				madnessInterval.RandomResult();
				state = MonsterState.MadnessBefore;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
				}
			}
			break;
		case MonsterState.MoveToTarget:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.NoTargetMove;
				break;
			}
			base.Rigid.linearVelocity += ToTargetDir() * base.MoveSpeed * Time.deltaTime;
			SyncDotsVelocity();
			jumpIntervalTimer += Time.deltaTime;
			if (jumpIntervalTimer >= sprintInterval.result)
			{
				jumpIntervalTimer = 0f;
				sprintInterval.RandomResult();
				state = MonsterState.SprintBefore;
			}
			RotateForMove(base.Rigid.linearVelocity);
			madnessIntervalTimer += Time.deltaTime;
			if (madnessIntervalTimer >= madnessInterval.result)
			{
				madnessIntervalTimer = 0f;
				madnessInterval.RandomResult();
				state = MonsterState.MadnessBefore;
			}
			break;
		case MonsterState.SprintBefore:
			if (changedState)
			{
				base.Anima.SetTrigger("SprintBefore");
			}
			base.Rigid.linearVelocity = Vector3.Lerp(base.Rigid.linearVelocity, Vector3.zero, moveLerp * Time.deltaTime);
			SyncDotsVelocity();
			break;
		case MonsterState.SprintJumping:
		{
			if (changedState)
			{
				JumpStart_Dots(sprintJumpUpForce, gravity);
				if (base.HaveTarget)
				{
					sprintDir = ToTargetDir();
				}
				else
				{
					sprintDir = Tool2D.GetDir();
				}
			}
			Vector3 velocity2 = sprintDir * sprintForwardForce;
			RotateForMove(velocity2);
			if (base.transform.position.z >= 0f && base.isFalling)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.IsVelocityDeclice = false;
				SetComponentData(componentData3);
				state = MonsterState.Sprinting;
				base.Rigid.linearVelocity = sprintDir * sprintForwardForce;
				SyncDotsVelocity();
			}
			break;
		}
		case MonsterState.Sprinting:
			if (base.Rigid.linearVelocity != sprintDir * sprintForwardForce)
			{
				base.Rigid.linearVelocity = sprintDir * sprintForwardForce;
				SyncDotsVelocity();
			}
			RotateForMove(base.Rigid.linearVelocity);
			sprintDurationTimer += Time.deltaTime;
			if (sprintDurationTimer >= sprintMaxDuration)
			{
				sprintDurationTimer = 0f;
				if (base.HaveTarget)
				{
					state = MonsterState.MoveToTarget;
				}
				else
				{
					state = MonsterState.NoTargetMove;
				}
			}
			break;
		case MonsterState.Flying:
			if (changedState)
			{
				JumpStart_Dots(flyUpForce, gravity);
				CamController.Inst.SetShock(landShock);
			}
			RotateForMove(base.Rigid.linearVelocity);
			if (base.transform.position.z >= 0f && base.isFalling)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.IsVelocityDeclice = false;
				SetComponentData(componentData2);
				state = MonsterState.FlyLand;
				LandCreateSpell();
				CamController.Inst.SetShock(landShock);
				sprintDurationTimer = 0f;
			}
			break;
		case MonsterState.FlyLand:
			if (changedState)
			{
				base.Anima.SetTrigger("FlyLand");
			}
			base.Rigid.linearVelocity = Vector3.Lerp(base.Rigid.linearVelocity, Vector3.zero, moveLerp * Time.deltaTime);
			SyncDotsVelocity();
			break;
		case MonsterState.MadnessBefore:
			if (changedState)
			{
				base.Anima.SetTrigger("MadnessBefore");
			}
			base.Rigid.linearVelocity = Vector3.Lerp(base.Rigid.linearVelocity, Vector3.zero, moveLerp * Time.deltaTime);
			SyncDotsVelocity();
			break;
		case MonsterState.MadnessJumping:
			if (changedState)
			{
				JumpStart_Dots(madnessJumpUpForce, madnessExtraGravity);
				madnessJumpTimer++;
				if (base.HaveTarget)
				{
					base.Rigid.linearVelocity = ToTargetDir() * madnessJumpForwardForce;
				}
				else
				{
					base.Rigid.linearVelocity = base.Rigid.linearVelocity.normalized * madnessJumpForwardForce;
				}
				SyncDotsVelocity();
			}
			RotateForMove(base.Rigid.linearVelocity);
			if (base.transform.position.z >= 0f && base.isFalling)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.IsVelocityDeclice = false;
				SetComponentData(componentData4);
				if (madnessJumpTimer == madnessJumpTime)
				{
					madnessJumpTimer = 0;
					state = MonsterState.FlyLand;
				}
				else
				{
					state = MonsterState.MadnessJumpAgain;
				}
				LandCreateSpell();
				CamController.Inst.SetShock(landShock);
			}
			break;
		case MonsterState.MadnessJumpAgain:
			if (changedState)
			{
				base.Anima.SetTrigger("MadnessLandAndJump");
			}
			base.Rigid.linearVelocity = Vector3.Lerp(base.Rigid.linearVelocity, Vector3.zero, moveLerp * Time.deltaTime);
			SyncDotsVelocity();
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.Die:
			if (changedState)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
				JumpStop_Dots();
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.IsVelocityDeclice = false;
				SetComponentData(componentData);
			}
			SyncDotsVelocity();
			if (deadRotate)
			{
				Vector3 velocity = Vector3.right * sprintForwardForce;
				RotateForMove(velocity);
			}
			break;
		}
	}

	public void LateUpdate()
	{
		for (int i = 0; i < myPpt.MR_Models.Length; i++)
		{
			myPpt.MR_Models[i].material.SetColor(GameConstManaged.shaderColorIndex, myPpt.BaseColor);
		}
	}

	private void RotateForMove(Vector3 velocity)
	{
		Vector3 vector = new Vector3(velocity.y / perimeter * 360f, 0f, (0f - velocity.x) / perimeter * 360f);
		tsf_Rotate.Rotate(vector * Time.deltaTime, Space.World);
	}

	private void LandCreateSpell()
	{
		SEMgr.Inst.boss1Land.PlaySE();
		if (!GameMgr.IsHarmony_Static)
		{
			UnityEngine.Object.Instantiate(pfb_CollisionEffect, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT);
			UnityEngine.Object.Instantiate(pfb_LandEffect, Tool2D.IgnoreZPoint(base.transform.position, 1.05f), Quaternion.identity, LevelMgr.Inst.CurrentRoomT);
		}
		float num = 360f / (float)landingBulletCount;
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellLandForwardSpeed;
		sSPModifier.CurrentFallSpeed = 0f - spellLandUpSpeed;
		for (int i = 0; i < landingBulletCount; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir(num * (float)i);
			sSPModifier.SpawnPosition = base.transform.position + Tool2D.GetDir(num * (float)i) * landSpellOffset;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	public unsafe void OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		Entity otherEntity = collision.GetOtherEntity(myPpt.myEntity);
		PhysicsCollider componentData = GetComponentData<PhysicsCollider>(otherEntity);
		bool flag = false;
		flag = componentData.ColliderPtr->GetCollisionFilter().BelongsTo == 256;
		if (thisFrameKnockedWall)
		{
			return;
		}
		if (flag)
		{
			thisFrameKnockedWall = true;
			Vector3 vector = collision.CollisionDetails.FirstContactPosition;
			vector.z = Mathf.Min(0f, vector.z);
			float magnitude = Tool2D.IgnoreZPoint(lastVelocity).magnitude;
			if (magnitude < minRamMagnitude)
			{
				return;
			}
			SEMgr.Inst.boss1Land.PlaySE();
			int num = (int)(magnitude * perMagnitudeBulletCount);
			UnityEngine.Object.Instantiate(pfb_CollisionEffect, vector, Quaternion.identity);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < num; i++)
			{
				float degree = UnityEngine.Random.Range((0f - collisionBulletAngleForNormal) / 2f, collisionBulletAngleForNormal / 2f);
				Vector3 vector2 = Tool2D.IgnoreZPoint(-collision.GetNormalFrom(myPpt.myEntity));
				sSPModifier.Speed = spellCollideForwardSpeed.RandomResult();
				sSPModifier.CurrentFallSpeed = 0f - spellCollideUpSpeed.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir(vector2, degree);
				sSPModifier.SpawnPosition = vector + vector2 * collideSpellOffset;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			if (state == MonsterState.Sprinting)
			{
				state = MonsterState.Flying;
			}
		}
		base.Rigid.linearVelocity = Vector3.zero;
	}

	public void OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	public void OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}

	protected override void BossDeadStay()
	{
		state = MonsterState.Die;
		base.Anima.SetTrigger("Die");
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "Jump":
			state = MonsterState.SprintJumping;
			break;
		case "FlyLandFinish":
			if (base.HaveTarget)
			{
				state = MonsterState.MoveToTarget;
			}
			else
			{
				state = MonsterState.NoTargetMove;
			}
			break;
		case "MadnessJump":
			state = MonsterState.MadnessJumping;
			break;
		case "DeadRotateStart":
			deadRotate = true;
			break;
		case "DeadRotateFinish":
			deadRotate = false;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
