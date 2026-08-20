using System;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class Elite2 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		RunToTarget,
		RunRandom,
		SprintCharge,
		Sprinting,
		Attack
	}

	[Range(0f, 1f)]
	public float sprintChance;

	[Range(0f, 1f)]
	public float sprintChanceAttenuation;

	public float sprintChargeTime;

	public float sprintForce;

	public float sprintingTime;

	public float sprintDrag;

	[Range(0f, 1f)]
	public float runRandomChange;

	public float minDistanceToTarget;

	public VariableFloat runTime;

	public VariableFloat runRandomDistance;

	public VariableFloat attackInterval;

	public float speedMultiplier = 1f;

	[Header("Spell")]
	public int spellCount;

	public float spellLandRadius;

	public float spellHeight;

	public VariableFloat spellUpSpeed;

	public float spellGravity;

	public int spellDamage;

	public GameObject imageRoot;

	[Header("Audio")]
	public AudioSource as_Vomit;

	public AudioSource as_Gulu;

	[Header("Particle")]
	public ParticleSystem dust;

	[Header("WarningLine")]
	public LineRenderer warningLine;

	public LineRenderer warningLine_H;

	public float warningLineDistance;

	[Header("和谐")]
	public SpriteRenderer sr_Body;

	public List<Sprite> sprites = new List<Sprite>();

	public List<Sprite> sprites_H = new List<Sprite>();

	private SpellSpawnParams ssp;

	private Vector3 aimedDir;

	private MonsterState preState;

	private MonsterState tempState;

	private MonsterState state;

	private float runTimer;

	private float sprintChargeTimer;

	private float sprintingTimer;

	private int continuousSprintCount;

	private float attackIntervalTimer;

	private void ImageFlip(float x)
	{
		if (x < 0f)
		{
			imageRoot.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			imageRoot.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundChange));
		SoundChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundChange));
	}

	private void SoundChange()
	{
		as_Vomit.volume = DataMgr.settingData.GetFinalSound();
		as_Gulu.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsHarmony_Static)
		{
			warningLine.enabled = false;
			warningLine = warningLine_H;
			sprites = sprites_H;
			sr_Body.sprite = sprites[0];
		}
		else
		{
			warningLine.enabled = false;
		}
		if (GameMgr.IsChAge14_Static)
		{
			warningLine = warningLine_H;
		}
	}

	public override void EveryInitialCallback()
	{
		runTime.RandomResult();
		attackInterval.RandomResult();
		warningLine.positionCount = 10;
		warningLine.enabled = false;
		aimedDir = Tool2D.GetDir();
	}

	public override void Update()
	{
		preState = tempState;
		tempState = state;
		_ = preState;
		_ = state;
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		attackIntervalTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (!(bornIdleTimer >= 0.5f))
			{
				break;
			}
			base.Anima.SetTrigger("Run");
			if (UnityEngine.Random.value <= runRandomChange)
			{
				state = MonsterState.RunRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
				break;
			}
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				state = MonsterState.RunToTarget;
				break;
			}
			state = MonsterState.RunRandom;
			GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
			break;
		case MonsterState.RunToTarget:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.RunRandom;
				break;
			}
			GetNavInfo(base.TargetPoint);
			if (ToTargetDistanceSqr() > 0.040000003f)
			{
				base.CurrentMotion = ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * speedMultiplier;
			}
			else
			{
				base.CurrentMotion = Vector3.zero;
			}
			ImageFlip(ToPointDir(navInfo.ToGoPoint).x);
			if (ToTargetDistanceSqr() < minDistanceToTarget * minDistanceToTarget)
			{
				runTimer = runTime.result;
			}
			CheckRunTime();
			break;
		case MonsterState.RunRandom:
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
			}
			else
			{
				base.CurrentMotion = ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * speedMultiplier;
				ImageFlip(ToPointDir(navInfo.ToGoPoint).x);
				CheckNavInfo();
			}
			CheckRunTime();
			break;
		case MonsterState.SprintCharge:
		{
			if (base.HaveTarget)
			{
				ImageFlip(ToTargetDir().x);
			}
			if (base.HaveTarget)
			{
				aimedDir = ToTargetDir();
			}
			else
			{
				GetNearestTargetPlayerFirst();
			}
			if (!warningLine.enabled && base.HaveTarget)
			{
				warningLine.enabled = true;
			}
			Vector3 position = base.transform.position;
			Vector3 b = position + aimedDir * warningLineDistance;
			for (int i = 0; i < warningLine.positionCount; i++)
			{
				Vector3 rootPoint = Vector3.Lerp(position, b, (float)i / (float)(warningLine.positionCount - 1));
				warningLine.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffect));
			}
			sprintChargeTimer += Time.deltaTime;
			if (sprintChargeTimer >= sprintChargeTime)
			{
				sprintChargeTimer = 0f;
				dust.Play();
				state = MonsterState.Sprinting;
				base.Anima.SetTrigger("Sprinting");
				SEMgr.Inst.elite2Spring.PlaySE();
				PhysicsVelocity componentData4 = GetComponentData<PhysicsVelocity>();
				componentData4.Linear = aimedDir * sprintForce;
				SetComponentData(componentData4);
				PhysicsDamping componentData5 = GetComponentData<PhysicsDamping>();
				componentData5.Linear = sprintDrag;
				SetComponentData(componentData5);
				warningLine.enabled = false;
				aimedDir = Tool2D.GetDir();
			}
			break;
		}
		case MonsterState.Sprinting:
		{
			sprintingTimer += Time.deltaTime;
			if ((double)sprintingTimer >= (double)sprintingTime * 0.7)
			{
				dust.Stop();
			}
			if (!(sprintingTimer >= sprintingTime))
			{
				break;
			}
			sprintingTimer = 0f;
			continuousSprintCount++;
			if (UnityEngine.Random.value <= sprintChance * Mathf.Pow(sprintChanceAttenuation, continuousSprintCount))
			{
				SEMgr.Inst.elite2SpringUp.PlaySE();
				state = MonsterState.SprintCharge;
				base.Anima.SetTrigger("Charge");
				break;
			}
			continuousSprintCount = 0;
			base.Anima.SetTrigger("Run");
			base.Rigid.linearDamping = 0f;
			PhysicsDamping componentData = GetComponentData<PhysicsDamping>();
			componentData.Linear = 0f;
			SetComponentData(componentData);
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = Vector3.zero;
			SetComponentData(componentData2);
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.IsVelocityDeclice = true;
			componentData3.ImmuneKnockbackUnregister();
			SetComponentData(componentData3);
			if (UnityEngine.Random.value <= runRandomChange)
			{
				state = MonsterState.RunRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
				break;
			}
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				state = MonsterState.RunToTarget;
			}
			else
			{
				state = MonsterState.RunRandom;
			}
			break;
		}
		case MonsterState.Attack:
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckRunTime()
	{
		runTimer += Time.deltaTime;
		if (!(runTimer >= runTime.result))
		{
			return;
		}
		runTimer = 0f;
		runTime.RandomResult();
		if (attackIntervalTimer >= attackInterval.result)
		{
			myPpt.ImmuneKnockbackRegister_Dots();
			attackIntervalTimer = 0f;
			attackInterval.RandomResult();
			state = MonsterState.Attack;
			base.Anima.SetTrigger("Attack");
		}
		else if (UnityEngine.Random.value <= sprintChance)
		{
			SEMgr.Inst.elite2SpringUp.PlaySE();
			state = MonsterState.SprintCharge;
			base.Anima.SetTrigger("Charge");
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = false;
			componentData.ImmuneKnockbackRegister();
			SetComponentData(componentData);
			base.CurrentMotion = Vector3.zero;
			base.Rigid.linearVelocity = Vector3.zero;
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData2);
		}
		else if (UnityEngine.Random.value <= runRandomChange)
		{
			state = MonsterState.RunRandom;
			GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
		}
		else
		{
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				state = MonsterState.RunToTarget;
			}
			else
			{
				state = MonsterState.RunRandom;
			}
		}
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Die");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = Vector3.zero;
		SetComponentData(componentData);
		PhysicsMass componentData2 = GetComponentData<PhysicsMass>();
		componentData2.InverseMass = 0f;
		SetComponentData(componentData2);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
		componentData3.BossDeadStay();
		SetComponentData(componentData3);
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
		case "0":
			sr_Body.sprite = sprites[0];
			break;
		case "1":
			sr_Body.sprite = sprites[1];
			break;
		case "2":
			sr_Body.sprite = sprites[2];
			break;
		case "MoveFast":
			SEMgr.Inst.elite2Bounce.PlaySE();
			speedMultiplier = 1.5f;
			dust.Play();
			break;
		case "DustStop":
			dust.Stop();
			break;
		case "MoveSlow":
			speedMultiplier = 0.5f;
			break;
		case "SE_Gulu":
			as_Gulu.Play();
			break;
		case "SE_Vomit":
			as_Vomit.Play();
			as_Gulu.Stop();
			break;
		case "Aimed":
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				aimedDir = ToTargetDir();
			}
			break;
		case "Attack":
		{
			GetNearestTargetPlayerFirst();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			if (base.HaveTarget)
			{
				for (int i = 0; i < spellCount; i++)
				{
					Vector3 point = Tool2D.IgnoreZPoint(base.TargetPoint + Tool2D.GetDir() * UnityEngine.Random.Range(0f, spellLandRadius));
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.Direction = ToPointDir(point);
					float horizontalDistance = ToPointDistance(point);
					sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed.RandomResult();
					sSPModifier.Speed = GeneralTool.CannonSpeed(0f - sSPModifier.CurrentFallSpeed, spellHeight, spellGravity, horizontalDistance);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				break;
			}
			Vector3 vector = PlayerMgr.Inst.PlayerPoint + Tool2D.GetDir() * UnityEngine.Random.Range(spellLandRadius, 2f * spellLandRadius);
			for (int j = 0; j < spellCount; j++)
			{
				Vector3 point2 = Tool2D.IgnoreZPoint(vector + Tool2D.GetDir() * UnityEngine.Random.Range(0f, spellLandRadius));
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = ToPointDir(point2);
				float horizontalDistance2 = ToPointDistance(point2);
				sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed.RandomResult();
				sSPModifier.Speed = GeneralTool.CannonSpeed(0f - sSPModifier.CurrentFallSpeed, spellHeight, spellGravity, horizontalDistance2);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case "AttackFinish":
			base.Anima.SetTrigger("Run");
			myPpt.ImmuneKnockbackUnregister_Dots();
			if (UnityEngine.Random.value <= runRandomChange)
			{
				state = MonsterState.RunRandom;
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, runRandomDistance));
				break;
			}
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				state = MonsterState.RunToTarget;
			}
			else
			{
				state = MonsterState.RunRandom;
			}
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
