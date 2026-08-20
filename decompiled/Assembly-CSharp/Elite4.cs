using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using UnityEngine;

public class Elite4 : UnitBase, IDotsCollisionReceiver, IDotsPhysicsReciever
{
	private enum MonsterState
	{
		BornIdle,
		Swim,
		Sprint,
		SprintRecovery
	}

	public int bornSwimTime;

	public float swimForce;

	[Header("SpawnChild")]
	[Range(0f, 1f)]
	public float spawnChance;

	public VariableInt childCount;

	public float childRadius;

	public int childID;

	[Header("Sprint")]
	[Range(0f, 1f)]
	public float sprintChance;

	public float sprintInitialForce;

	public float sprintAcceleration;

	public float sprintBulletInterval;

	public VariableFloat sprintBulletAngleOffset;

	[Range(0f, 1f)]
	[Header("Stage2")]
	public float stage2HPRatio;

	public int stageSpawnChildLimit;

	public float stage2ChildInterval;

	public List<Elite4_Child> allChildren = new List<Elite4_Child>();

	[Header("Spell")]
	public float spellHeight;

	public VariableFloat spellSpeed;

	public VariableFloat spellDuration;

	public VariableInt spellDamage;

	[Header("Audio")]
	public AudioSource as_Spawn;

	private SpellSpawnParams ssp;

	private MonsterState state;

	private int bornSwimTimer;

	private float sprintTimer;

	private bool isStage2;

	private bool isSprintSpawnChild;

	Entity IDotsPhysicsReciever.thisEntity
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
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
		as_Spawn.volume = DataMgr.settingData.GetFinalSound();
	}

	private void Start()
	{
		if (GameMgr.IsMobile_Static)
		{
			spellDuration.value1 *= 0.4f;
			stage2ChildInterval *= 2f;
			sprintBulletInterval *= 1.5f;
			childCount.value2 = 3;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (!isStage2 && state != MonsterState.Sprint && base.CurrentHPRatio <= stage2HPRatio)
		{
			isStage2 = true;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Swim;
				base.Anima.SetTrigger("Swim");
			}
			break;
		case MonsterState.Swim:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Sprint:
		{
			Vector3 force = ((!base.HaveTarget) ? (base.Rigid.linearVelocity.normalized * sprintAcceleration * Time.deltaTime) : (ToTargetDir() * sprintAcceleration * Time.deltaTime));
			base.Rigid.AddForce(force);
			SyncDotsVelocity();
			sprintTimer += Time.deltaTime;
			if (isSprintSpawnChild)
			{
				if (sprintTimer >= stage2ChildInterval)
				{
					sprintTimer = 0f;
					for (int num = allChildren.Count - 1; num >= 0; num--)
					{
						if (allChildren[num].myPpt.AlreadyDead)
						{
							allChildren.RemoveAt(num);
						}
					}
					Elite4_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childID, Tool2D.GetNavMeshPointIngoreZ(base.transform.position)).GetComponent<Elite4_Child>();
					allChildren.Add(component);
					as_Spawn.Play();
				}
			}
			else if (sprintTimer >= sprintBulletInterval)
			{
				sprintTimer -= sprintBulletInterval;
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Damage = spellDamage.RandomResult();
				sSPModifier.Speed = spellSpeed.RandomResult();
				sSPModifier.Duration = spellDuration.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir();
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			SetFlip(force.x);
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case MonsterState.SprintRecovery:
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	protected override void BossDeadStay()
	{
		base.Anima.SetTrigger("Die");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = Vector3.zero;
		SetComponentData(componentData);
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
		componentData2.BossDeadStay();
		SetComponentData(componentData2);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		for (int i = 0; i < allChildren.Count; i++)
		{
			if (allChildren[i] != null && !allChildren[i].myPpt.AlreadyDead)
			{
				allChildren[i].DotsAnnouncedDeath();
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "SwimAddForce":
			SEMgr.Inst.elite4Squish.PlaySE();
			base.CurrentMotion = Tool2D.GetDir() * swimForce;
			break;
		case "SwimFinish":
		{
			bornSwimTimer++;
			if (bornSwimTimer < bornSwimTime)
			{
				base.Anima.SetTrigger("Swim");
				break;
			}
			float value = UnityEngine.Random.value;
			if (value <= spawnChance)
			{
				base.Anima.SetTrigger("Pops");
			}
			else if (value <= spawnChance + sprintChance)
			{
				base.Anima.SetTrigger("Sprint");
			}
			else
			{
				base.Anima.SetTrigger("Swim");
			}
			break;
		}
		case "Pops":
		{
			childCount.RandomResult();
			Vector3[] circleDancePoints = Tool2D.GetCircleDancePoints(base.transform.position, childCount.result, childRadius);
			for (int i = 0; i < circleDancePoints.Length; i++)
			{
				for (int num = allChildren.Count - 1; num >= 0; num--)
				{
					if (allChildren[num].myPpt.AlreadyDead)
					{
						allChildren.RemoveAt(num);
					}
				}
				Elite4_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + childID, Tool2D.GetNavMeshPointIngoreZ(circleDancePoints[i])).GetComponent<Elite4_Child>();
				allChildren.Add(component);
			}
			as_Spawn.Play();
			break;
		}
		case "PopsFinish":
			base.Anima.SetTrigger("Swim");
			break;
		case "Sprint":
		{
			state = MonsterState.Sprint;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = false;
			componentData.ImmuneKnockbackRegister();
			SetComponentData(componentData);
			GetNearestTargetPlayerFirst();
			if (base.HaveTarget)
			{
				base.Rigid.linearVelocity = ToTargetDir() * sprintInitialForce;
			}
			else
			{
				base.Rigid.linearVelocity = Tool2D.GetDir() * sprintInitialForce;
			}
			if (isStage2 && LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count <= stageSpawnChildLimit + 1)
			{
				isSprintSpawnChild = true;
			}
			else
			{
				isSprintSpawnChild = false;
			}
			break;
		}
		case "SprintRecovery":
		{
			state = MonsterState.SprintRecovery;
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = true;
			componentData.ImmuneKnockbackUnregister();
			SetComponentData(componentData);
			break;
		}
		case "SprintFinish":
			state = MonsterState.Swim;
			base.Anima.SetTrigger("Swim");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	void IDotsCollisionReceiver.OnCollisionEnter_Dots(StatefulCollisionEvent collision)
	{
		base.Rigid.linearVelocity = GetComponentData<PhysicsVelocity>().Linear;
	}

	void IDotsCollisionReceiver.OnCollisionStay_Dots(StatefulCollisionEvent collision)
	{
	}

	void IDotsCollisionReceiver.OnCollisionExit_Dots(StatefulCollisionEvent collision)
	{
	}
}
