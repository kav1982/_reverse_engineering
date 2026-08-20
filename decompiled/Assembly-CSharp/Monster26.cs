using System;
using Unity.Physics;
using UnityEngine;

public class Monster26 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		JumpBefore,
		Jump,
		BigJump,
		JumpToGround,
		Idle,
		AttackBefore,
		Attacking,
		AttackingBones,
		AttackAfter
	}

	[Space(50f)]
	public VariableFloat idleTime;

	[Header("Jump")]
	public float jumpUpSpeed;

	public float jumpForwardSpeed;

	public float jumpGravity;

	public float bigJumpSpeed;

	public float bigJumpForwardSpeed;

	public float bigJumpChance;

	public int bigJumpBullets;

	public float bigJumpGravity;

	public VariableFloat bigJumpSpellSpeed;

	public VariableFloat bigJumpSpellUpSpeed;

	public ParticleSystem JumpParticle;

	[Range(0f, 1f)]
	[Header("Attack")]
	public float attackChance;

	public float attackTime;

	public float attackSpellInterval;

	public float attackAngle;

	public AIPattern pattern;

	[Header("Eye")]
	public Transform tsf_EyeRedRoot;

	public float eyeRedOffset;

	public float eyeRedOffsetLerp;

	[Header("Spell")]
	public float spellHeight;

	public Vector2 spellOffset;

	public int spellDamage;

	public VariableFloat spellSpeed;

	public float spellUpSpeed;

	public float spellGravity;

	[Header("Large Bone")]
	public GameObject largeBonePrefab;

	public Monster26_LargeBone largeBone;

	public float largeBoneUpSpeed;

	public float largeBoneSpeed;

	public float largeBoneGravity;

	public bool randomSmallBone;

	public float smallBoneAmount;

	public float smallBoneUpSpeed;

	public VariableFloat smallBoneSpeed;

	[Header("Audio")]
	public AudioSource as_Born;

	public AudioSource as_Land;

	public AudioSource as_Attack;

	[Header("和谐版")]
	public MeshRenderer mr_Head;

	public MeshRenderer mr_Mouth;

	public MeshRenderer mr_Eye;

	public MeshRenderer mr_EyeBlack;

	public Sprite sprite_Head_H;

	public Sprite sprite_Mouth_H;

	public Sprite sprite_Eye_H;

	public Sprite sprite_EyeBlack_H;

	public ParticleSystem jumpParticles_H;

	public float eyeRedOffsetH;

	private MonsterState state;

	private float idleTimer;

	private float attackTimer;

	private float attackSpellIntervalTimer;

	private MonsterState preState;

	private MonsterState tempState;

	private bool changedState;

	private SpellSpawnParams ssp;

	private SpellSpawnParams ssp1;

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Born.volume = DataMgr.settingData.GetFinalSound();
		as_Land.volume = DataMgr.settingData.GetFinalSound();
		as_Attack.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90021);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ReboundCount = 100;
		sSPModifier.ApplyToSSP(ref ssp);
		if (GameMgr.IsMobile_Static)
		{
			smallBoneAmount -= 2f;
			bigJumpBullets = Mathf.CeilToInt((float)bigJumpBullets * 0.5f);
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		idleTimer = 0f;
		attackTimer = 0f;
		attackSpellIntervalTimer = 0f;
		idleTime.RandomResult();
		base.Anima.SetTrigger("Idle");
		if (GameMgr.IsHarmony_Static)
		{
			mr_Head.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Head_H.texture);
			mr_Mouth.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Mouth_H.texture);
			mr_Eye.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Eye_H.texture);
			mr_EyeBlack.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_EyeBlack_H.texture);
			eyeRedOffset = eyeRedOffsetH;
			JumpParticle = jumpParticles_H;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		changedState = false;
		preState = tempState;
		tempState = state;
		if (preState != state)
		{
			changedState = true;
		}
		if (base.HaveTarget)
		{
			tsf_EyeRedRoot.localPosition = Vector3.Lerp(tsf_EyeRedRoot.localPosition, ToTargetDir() * eyeRedOffset, eyeRedOffsetLerp * Time.deltaTime);
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			idleTimer += Time.deltaTime;
			if (!(idleTimer >= idleTime.result))
			{
				break;
			}
			idleTimer = 0f;
			idleTime.RandomResult();
			if (UnityEngine.Random.value <= attackChance)
			{
				if (pattern == AIPattern.Pattern2)
				{
					state = MonsterState.AttackBefore;
					base.Anima.SetTrigger("AttackBone");
				}
				else
				{
					state = MonsterState.AttackBefore;
					base.Anima.SetTrigger("AttackBefore");
				}
			}
			else if (pattern == AIPattern.Pattern2 && UnityEngine.Random.Range(0f, 1f) < bigJumpChance)
			{
				state = MonsterState.BigJump;
				base.Anima.Play("Monster26_BigJump");
			}
			else
			{
				state = MonsterState.Jump;
				base.Anima.SetTrigger("Jump");
			}
			break;
		case MonsterState.Jump:
			if (base.transform.position.z > 0f && base.isFalling)
			{
				JumpStop_Dots();
				state = MonsterState.JumpToGround;
				base.Anima.SetTrigger("JumpToGround");
				as_Land.Play();
			}
			break;
		case MonsterState.BigJump:
			if (base.transform.position.z > 0f && base.isFalling)
			{
				JumpStop_Dots();
				state = MonsterState.JumpToGround;
				base.Anima.Play("Monster26_BigJumpToGround");
				SEMgr.Inst.monster26BigLand.PlaySE();
				as_Land.Play();
			}
			break;
		case MonsterState.AttackingBones:
			if (changedState)
			{
				_ = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				Vector3 diration = ((!base.HaveTarget) ? Tool2D.GetDir() : ToTargetDir());
				largeBone = UnityEngine.Object.Instantiate(largeBonePrefab, base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT).GetComponent<Monster26_LargeBone>();
				largeBone.diration = diration;
				largeBone.master = this;
				largeBone.masterEntity = myPpt.myEntity;
				largeBone.speed = largeBoneSpeed;
				largeBone.gravity = largeBoneGravity;
				largeBone.CurrentUpSpeed = largeBoneUpSpeed;
			}
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				state = MonsterState.AttackAfter;
				base.Anima.SetTrigger("AttackAfter");
			}
			break;
		case MonsterState.Attacking:
			attackSpellIntervalTimer += Time.deltaTime;
			if (attackSpellIntervalTimer >= attackSpellInterval)
			{
				attackSpellIntervalTimer -= attackSpellInterval;
				Vector3 vector = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				vector.x += UnityEngine.Random.Range(0f - spellOffset.x, spellOffset.x);
				vector.z += UnityEngine.Random.Range(0f - spellOffset.y, spellOffset.y);
				if (!base.HaveTarget)
				{
					GetNearestTarget();
				}
				Vector3 vector2 = ((!base.HaveTarget) ? Tool2D.GetDir() : ToTargetDir(UnityEngine.Random.Range((0f - attackAngle) / 2f, attackAngle / 2f)));
				ssp.SpawnPosition = vector;
				ssp.MovementComponentData.Direction = vector2;
				ssp.MovementComponentData.Speed = spellSpeed.RandomResult();
				ssp.MovementComponentData.CurrentFallSpeed = -3f;
				ShootSpell(ssp);
			}
			attackTimer += Time.deltaTime;
			if (attackTimer >= attackTime)
			{
				attackTimer = 0f;
				state = MonsterState.AttackAfter;
				base.Anima.SetTrigger("AttackAfter");
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case MonsterState.JumpToGround:
		case MonsterState.AttackBefore:
		case MonsterState.AttackAfter:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "BigJumpSound":
			SEMgr.Inst.monster26BigJump.PlaySE();
			break;
		case "Jump":
		{
			GetNearestTarget();
			JumpStart_Dots(jumpUpSpeed, jumpGravity);
			if (base.HaveTarget)
			{
				base.Rigid.linearVelocity = ToTargetDir() * jumpForwardSpeed;
			}
			else
			{
				base.Rigid.linearVelocity = Tool2D.GetDir() * jumpForwardSpeed;
			}
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
			break;
		}
		case "BigJump":
		{
			JumpParticle.Play();
			GetNearestTarget();
			JumpStart_Dots(bigJumpSpeed, bigJumpGravity);
			if (base.HaveTarget)
			{
				base.Rigid.linearVelocity = ToTargetDir() * bigJumpForwardSpeed;
			}
			else
			{
				base.Rigid.linearVelocity = Tool2D.GetDir() * bigJumpForwardSpeed;
			}
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData);
			break;
		}
		case "JumpToGroundShoot":
		{
			spellSpeed.RandomResult();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform.position, -0.1f);
			sSPModifier.Speed = spellSpeed.result;
			sSPModifier.CurrentFallSpeed = -3f;
			for (int j = 0; j < 4; j++)
			{
				sSPModifier.Direction = Tool2D.GetDir(90f * (float)j);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case "BigJumpToGroundShoot":
		{
			JumpParticle.Play();
			CamController.Inst.SetShock(0.2f, 10f, 0.4f);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform.position, -0.1f);
			for (int i = 0; i < bigJumpBullets; i++)
			{
				sSPModifier.Speed = bigJumpSpellSpeed.RandomResult();
				sSPModifier.CurrentFallSpeed = 0f - bigJumpSpellUpSpeed.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir(360f / (float)bigJumpBullets * (float)i);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case "JumpToGroundFinish":
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			break;
		case "BigJumpToGroundFinish":
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			break;
		case "AttackBone":
			state = MonsterState.AttackingBones;
			SEMgr.Inst.monster26AttackBone.PlaySE();
			break;
		case "AttackBoneFinish":
			state = MonsterState.AttackAfter;
			base.Anima.SetTrigger("AttackAfter");
			break;
		case "AttackBeforeFinish":
			state = MonsterState.Attacking;
			base.Anima.SetTrigger("Attacking");
			as_Attack.Play();
			break;
		case "AttackAfterFinish":
			state = MonsterState.Idle;
			base.Anima.SetTrigger("Idle");
			break;
		default:
			Debug.LogError(animaName);
			break;
		case "JumpSound":
			break;
		}
	}

	public void BoneBlast(Vector3 point)
	{
		float num = UnityEngine.Random.Range(0f, 180f);
		smallBoneSpeed.RandomResult();
		for (int i = 0; (float)i < smallBoneAmount; i++)
		{
			ssp1 = ssp;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp1);
			sSPModifier.ReboundCount = 4;
			sSPModifier.Speed = smallBoneSpeed.result;
			sSPModifier.CurrentFallSpeed = 0f - smallBoneUpSpeed;
			sSPModifier.Direction = Tool2D.GetDir(360f / smallBoneAmount * (float)i + num);
			sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(point, -0.1f);
			sSPModifier.ApplyToSSP(ref ssp1);
			ShootSpell(ssp1);
		}
	}
}
