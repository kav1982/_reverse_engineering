using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster15 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		Idle,
		JumpBefore,
		Jumping,
		JumpToGround
	}

	public AIPattern pattern;

	public float jumpGravity;

	[Header("普通跳")]
	public VariableFloat jumpInterval;

	public float jumpForwardForce;

	public float jumpUpForce;

	[Range(0f, 1f)]
	[Header("高跳")]
	public float highJumpChance;

	public float highJumpForwardForce;

	public float highJumpUpForce;

	[Header("Pattern2")]
	public int deadBoyID;

	public VariableInt deadBoyCount;

	public float deadBoyXYOffset;

	public float deadBoyHeight;

	[Header("Pattern3")]
	public bool rollBallIsPlayerDerate;

	public float rollballSpeed;

	public float rollballDuration;

	public int rollballDamage;

	public float rollBallDamageRatio;

	[Header("Spell")]
	public float spellOffset;

	public float spellHeight;

	public float spellForwardSpeed;

	public float spellUpSpeed;

	public float spellGravity;

	[Header("困难变异")]
	public bool isHardVariant;

	public ShockParam shockParam;

	private SpellSpawnParams ssp;

	private SpellSpawnParams sspRollBall;

	private MonsterState state;

	private float jumpIntervalTimer;

	private bool isJumpHigher;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellForwardSpeed;
		if (GameMgr.IsMobile_Static)
		{
			sSPModifier.Speed *= 0.8f;
		}
		sSPModifier.CurrentFallSpeed = 0f - spellUpSpeed;
		sSPModifier.Gravity = 0f - spellGravity;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		if (pattern == AIPattern.Pattern3)
		{
			sSPModifier.ColorType = SpellColorType.Mucus;
			sSPModifier.ApplyToSSP(ref ssp);
			ssp.ElementComponentData.MucusSpellSpeedRatio = 0.7f;
			ssp.ElementComponentData.MucusMoveSpeedRatio = 0.6f;
			ssp.ElementComponentData.MucusDuration = 3f;
			sspRollBall = UnitDotsSyncSystem.GetSpellPrototype(10021);
			sSPModifier = UnitBase.GetSSPModifier(in sspRollBall);
			sSPModifier.Speed = rollballSpeed;
			sSPModifier.Damage = rollballDamage;
			sSPModifier.Duration = rollballDuration;
			sSPModifier.CriticalChance = -99999f;
			sSPModifier.ColorType = SpellColorType.Mucus;
			sSPModifier.ApplyToSSP(ref sspRollBall);
			sspRollBall.ElementComponentData.MucusSpellSpeedRatio = 0.7f;
			sspRollBall.ElementComponentData.MucusMoveSpeedRatio = 0.6f;
			sspRollBall.ElementComponentData.MucusDuration = 3f;
		}
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		jumpIntervalTimer = 0f;
		isJumpHigher = false;
		jumpInterval.RandomResult();
		base.Anima.SetTrigger("Idle");
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (state)
		{
		case MonsterState.BornIdle:
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			jumpIntervalTimer += Time.deltaTime;
			if (jumpIntervalTimer >= jumpInterval.result)
			{
				jumpIntervalTimer = 0f;
				jumpInterval.RandomResult();
				state = MonsterState.JumpBefore;
				if (Random.value <= highJumpChance)
				{
					isJumpHigher = true;
					base.Anima.SetTrigger("JumpHigher");
				}
				else
				{
					isJumpHigher = false;
					base.Anima.SetTrigger("Jump");
				}
			}
			break;
		case MonsterState.Jumping:
		{
			if (!(base.transform.position.z > 0f))
			{
				break;
			}
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			JumpStop_Dots();
			if (isJumpHigher)
			{
				base.Anima.SetTrigger("JumpHigherToGround");
			}
			else
			{
				base.Anima.SetTrigger("JumpToGround");
			}
			state = MonsterState.JumpToGround;
			if (isJumpHigher)
			{
				if (isHardVariant)
				{
					CamController.Inst.SetShock(shockParam);
					for (int i = 0; i < 8; i++)
					{
						Vector3 dir = Tool2D.GetDir(i * 45);
						ssp.MovementComponentData.Direction = dir;
						Vector3 vector = base.transform.position + dir * spellOffset;
						vector.z = 0f - spellHeight;
						ssp.SpawnPosition = vector;
						ShootSpell(ssp);
					}
				}
				else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy)
				{
					for (int j = 0; j < 2; j++)
					{
						Vector3 dir2 = Tool2D.GetDir(j * 180 + 90);
						ssp.MovementComponentData.Direction = dir2;
						Vector3 vector2 = base.transform.position + dir2 * spellOffset;
						vector2.z = 0f - spellHeight;
						ssp.SpawnPosition = vector2;
						ShootSpell(ssp);
					}
				}
				else
				{
					for (int k = 0; k < 4; k++)
					{
						Vector3 dir3 = Tool2D.GetDir(k * 90);
						ssp.MovementComponentData.Direction = dir3;
						Vector3 vector3 = base.transform.position + dir3 * spellOffset;
						vector3.z = 0f - spellHeight;
						ssp.SpawnPosition = vector3;
						ShootSpell(ssp);
					}
				}
				if (pattern == AIPattern.Pattern3)
				{
					MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius);
				}
			}
			if (isJumpHigher && isHardVariant)
			{
				SEMgr.Inst.monster15LandHeavy.PlaySE();
			}
			else
			{
				SEMgr.Inst.monster15Land.PlaySE();
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case MonsterState.JumpBefore:
		case MonsterState.JumpToGround:
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "JumpAddForce"))
		{
			if (animaName == "JumpOnGroundFinish")
			{
				state = MonsterState.Idle;
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		state = MonsterState.Jumping;
		Vector3 linearVelocity;
		float upForce;
		if (isJumpHigher)
		{
			GetNearestTarget();
			linearVelocity = ((!base.HaveTarget) ? (Tool2D.GetDir() * highJumpForwardForce) : (ToTargetDir() * highJumpForwardForce));
			upForce = highJumpUpForce;
		}
		else
		{
			linearVelocity = Tool2D.GetDir() * jumpForwardForce;
			upForce = jumpUpForce;
		}
		base.Rigid.linearVelocity = linearVelocity;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(upForce, jumpGravity);
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		switch (pattern)
		{
		case AIPattern.Pattern2:
		{
			deadBoyCount.RandomResult();
			float num = 360f / (float)deadBoyCount.result;
			for (int l = 0; l < deadBoyCount.result; l++)
			{
				Vector3 dir = Tool2D.GetDir(num * (float)l);
				Vector3 point = base.transform.position + dir * deadBoyXYOffset + new Vector3(0f, 0f, 0f - deadBoyHeight);
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + deadBoyID, point).GetComponent<Monster6>().ForceJump(dir);
			}
			break;
		}
		case AIPattern.Pattern3:
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in sspRollBall);
			if (isHardVariant)
			{
				for (int i = 0; i < 8; i++)
				{
					sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform);
					sSPModifier.Direction = Tool2D.GetDir(i * 45);
					sSPModifier.ApplyToSSP(ref sspRollBall);
					ShootSpell(sspRollBall);
				}
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Easy)
			{
				for (int j = 0; j < 2; j++)
				{
					sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform);
					sSPModifier.Direction = Tool2D.GetDir(j * 180 + 90);
					sSPModifier.ApplyToSSP(ref sspRollBall);
					ShootSpell(sspRollBall);
				}
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform);
					sSPModifier.Direction = Tool2D.GetDir(k * 90);
					sSPModifier.ApplyToSSP(ref sspRollBall);
					ShootSpell(sspRollBall);
				}
			}
			MucusSystem.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.CC_Self.radius * 2f);
			break;
		}
		default:
			Debug.LogError(pattern);
			break;
		case AIPattern.Pattern1:
			break;
		}
	}
}
