using Unity.Physics;
using UnityEngine;

public class Elite5 : UnitBase
{
	private enum MonsterState
	{
		BornIdle,
		JumpBefore,
		Jumping,
		JumpToGround,
		ShootBall,
		Die
	}

	public MeshRenderer mr;

	public Sprite[] sprites;

	public float rotateSpeed;

	public VariableFloat shootBallInterval;

	[Header("Jump")]
	public float jumpGravity;

	public VariableFloat jumpForwardForce;

	public VariableFloat jumpUpForce;

	[Range(0f, 1f)]
	public float highJumpChance;

	public VariableFloat highJumpForwardForce;

	public VariableFloat highJumpUpForce;

	[Header("Attack")]
	public float attackInterval;

	public float spellOffsetForward;

	public float spellOffsetHeight;

	[Header("Spell1")]
	public float spell1Speed;

	public float spell1UpSpeed;

	public float spell1Gravity;

	public float spell1Damage;

	[Header("Spell2")]
	public float rollBallNoWallDistance;

	public float rollBallForwardOffset;

	public float rollBallRecoil;

	public float rollBallSpeed;

	public float rollBallDuration;

	public int rollBallDamage;

	public float rollBallDamageRatio;

	public float rollBallFollowRotateSpeed;

	private MonsterState state;

	private float currentRotateValue;

	private float shootBallIntervalTimer;

	private float attackIntervalTimer;

	private bool isJumpHigher;

	private bool rotateDir;

	private SpellSpawnParams ssp;

	private SpellSpawnParams sspBall;

	private void Start()
	{
		shootBallInterval.RandomResult();
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spell1Speed;
		sSPModifier.Gravity = 0f - spell1Gravity;
		sSPModifier.CurrentFallSpeed = 0f - spell1UpSpeed;
		sSPModifier.Damage = spell1Damage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		sspBall = UnitDotsSyncSystem.GetSpellPrototype(10021);
		sSPModifier = UnitBase.GetSSPModifier(in sspBall);
		sSPModifier.Speed = rollBallSpeed;
		sSPModifier.Damage = rollBallDamage;
		sSPModifier.Duration = rollBallDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.CriticalChance = -999999f;
		sSPModifier.ApplyToSSP(ref sspBall);
		sspBall.SpellExtraSizeRatio = 0.2f;
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		shootBallIntervalTimer += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornIdle:
			SetMove(Vector3.zero);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				state = MonsterState.JumpBefore;
				base.Anima.SetTrigger("Jump");
			}
			break;
		case MonsterState.JumpBefore:
		case MonsterState.JumpToGround:
		case MonsterState.ShootBall:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Jumping:
		{
			if (rotateDir)
			{
				currentRotateValue += rotateSpeed * Time.deltaTime;
			}
			else
			{
				currentRotateValue -= rotateSpeed * Time.deltaTime;
			}
			if (currentRotateValue < 0f)
			{
				currentRotateValue += 360f;
			}
			else if (currentRotateValue >= 360f)
			{
				currentRotateValue -= 360f;
			}
			int num = Mathf.FloorToInt((float)sprites.Length * (currentRotateValue / 360f));
			if (num == sprites.Length)
			{
				num--;
			}
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[num].texture);
			if (isJumpHigher)
			{
				attackIntervalTimer += Time.deltaTime;
				if (attackIntervalTimer >= attackInterval)
				{
					attackIntervalTimer = 0f;
					Vector3 spawnPosition = base.transform.position + Tool2D.GetDir(currentRotateValue) * spellOffsetForward + new Vector3(0f, 0f, 0f - spellOffsetHeight);
					UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
					sSPModifier.SpawnPosition = spawnPosition;
					sSPModifier.Direction = Tool2D.GetDir(currentRotateValue);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			if (base.transform.position.z >= 0f && base.isFalling)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				JumpStop_Dots();
				state = MonsterState.JumpToGround;
				base.Anima.SetTrigger("JumpToGround");
				base.Anima.SetTrigger("HoleIdle");
			}
			break;
		}
		case MonsterState.Die:
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
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
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "JumpAddForce":
		{
			state = MonsterState.Jumping;
			rotateDir = ((Random.Range(0, 2) == 0) ? true : false);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.IsVelocityDeclice = false;
			SetComponentData(componentData);
			if (isJumpHigher)
			{
				JumpStart_Dots(highJumpUpForce.RandomResult(), jumpGravity);
				PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
				componentData2.Linear = Tool2D.GetDir(currentRotateValue) * jumpForwardForce.RandomResult();
				SetComponentData(componentData2);
			}
			else
			{
				JumpStart_Dots(jumpUpForce.RandomResult(), jumpGravity);
				PhysicsVelocity componentData3 = GetComponentData<PhysicsVelocity>();
				componentData3.Linear = Tool2D.GetDir(currentRotateValue) * jumpForwardForce.RandomResult();
				SetComponentData(componentData3);
			}
			break;
		}
		case "Land":
			SEMgr.Inst.monster15Land.PlaySE();
			break;
		case "JumpOnGround":
			if (shootBallIntervalTimer >= shootBallInterval.result && !UnitDotsSyncSystem.SphereCast(new UnityEngine.Ray(base.transform.position, Tool2D.GetDir(currentRotateValue)), 0.5f, rollBallNoWallDistance, GameConst.Filter_Wall))
			{
				shootBallIntervalTimer = 0f;
				shootBallInterval.RandomResult();
				state = MonsterState.ShootBall;
				base.Anima.SetTrigger("ShootBall");
				break;
			}
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
			break;
		case "ShootBall":
		{
			Vector3 spawnPosition = base.transform.position + Tool2D.GetDir(currentRotateValue) * rollBallForwardOffset;
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in sspBall);
			sSPModifier.SpawnPosition = spawnPosition;
			sSPModifier.Direction = Tool2D.GetDir(currentRotateValue);
			sSPModifier.MovementType = SpellSpecialMovementType.ChaseEnemy;
			sSPModifier.ChaseRotateSpeed = rollBallFollowRotateSpeed;
			sSPModifier.Damage = Mathf.CeilToInt((float)rollBallDamage * rollBallDamageRatio);
			sSPModifier.ApplyToSSP(ref sspBall);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.TakeKnockback(-Tool2D.GetDir(currentRotateValue) * rollBallRecoil);
			SetComponentData(componentData);
			ShootSpell(sspBall);
			break;
		}
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
