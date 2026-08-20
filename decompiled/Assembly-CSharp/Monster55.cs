using Unity.Transforms;
using UnityEngine;

public class Monster55 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Idle,
		Hide,
		Show,
		Attack
	}

	public StateVariableMgr varMgr = new StateVariableMgr();

	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("钻地")]
	public VariableFloat closeDistance;

	public float sight;

	public VariableFloat farDistance;

	public float closeChance;

	public VariableFloat teleportCD;

	private float teleportCDTimer;

	public float hideTime;

	public Transform tsf_Motion;

	[Header("攻击")]
	public float attackDistance;

	public int bulletCount;

	public VariableFloat AttackCD;

	public float attackAngle;

	public MeshRenderer mr;

	public Sprite sprite_Normal;

	public Sprite sprite_Attack;

	public Sprite sprite_Charge;

	private float attackCDTimer;

	[Header("子弹模式")]
	public float spellSpeed;

	public float spellHeight;

	public int spellDamage;

	public int spellCount;

	public float spellLifeTime;

	private SpellSpawnParams ssp;

	[Header("表现")]
	public SpriteRenderer sr_DirtFore;

	public SpriteRenderer sr_DirtBack;

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
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellLifeTime;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		myPpt.RemoveSRFromArray(sr_DirtFore);
		myPpt.RemoveSRFromArray(sr_DirtBack);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		base.Anima.Play("Idle");
		attackCDTimer = Random.value * AttackCD.value2;
		teleportCDTimer = Random.value * teleportCD.value2;
		SetIsCanBeAttack(canBeAttack: true);
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
	}

	public override void Update()
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
		SetMove(Vector3.zero);
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				base.Anima.Play("Idle");
			}
			if (stateExistTime > 0.5f)
			{
				state = MonsterState.Idle;
			}
			break;
		case MonsterState.Idle:
			if (changedState)
			{
				base.Anima.Play("Idle");
				AttackCD.RandomResult();
				teleportCD.RandomResult();
			}
			if (teleportCDTimer > teleportCD.result)
			{
				teleportCDTimer = 0f;
				state = MonsterState.Hide;
				break;
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer > 1f)
			{
				GetNearestTarget(checkWall: true);
			}
			if (base.HaveTarget && ToTargetDistanceSqr() < attackDistance * attackDistance && attackCDTimer > AttackCD.result)
			{
				attackCDTimer = 0f;
				state = MonsterState.Attack;
			}
			else
			{
				attackCDTimer += Time.deltaTime;
				teleportCDTimer += Time.deltaTime;
			}
			break;
		case MonsterState.Hide:
			if (changedState)
			{
				base.Anima.Play("Hide");
			}
			break;
		case MonsterState.Show:
			if (changedState)
			{
				base.Anima.Play("Show");
			}
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.Anima.Play("Attack", 0, 0f);
				AttackCD.RandomResult();
				if (!base.HaveTarget)
				{
					GetNearestTarget(checkWall: true);
				}
			}
			attackCDTimer += Time.deltaTime;
			teleportCDTimer += Time.deltaTime;
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (!base.CC_Self.enabled)
		{
			info.immuneDamage = true;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "HideFinish":
		{
			GetNearestTarget();
			if (base.HaveTarget)
			{
				if (Random.value < closeChance)
				{
					base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, closeDistance);
				}
				else
				{
					base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint, farDistance);
				}
			}
			else
			{
				base.transform.position = Tool2D.GetNavMeshPointIngoreZ(base.transform.position, closeDistance);
			}
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			state = MonsterState.Show;
			break;
		}
		case "CanAttack":
			SetIsCanBeAttack(canBeAttack: true);
			break;
		case "CantAttack":
			SetIsCanBeAttack(canBeAttack: false);
			break;
		case "ShowFinish":
			state = MonsterState.Idle;
			break;
		case "Born":
			SetIsCanBeAttack(canBeAttack: true);
			break;
		case "BornFinish":
			state = MonsterState.Idle;
			break;
		case "AttackCharge":
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Charge.texture);
			break;
		case "Attack":
		{
			GetNearestTarget(checkWall: true);
			SEMgr.Inst.monster55_Attack.PlaySE();
			Vector3 oldDir = (base.HaveTarget ? ToTargetDir() : Tool2D.GetDir());
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < bulletCount; i++)
			{
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Direction = Tool2D.GetDir(oldDir, (0f - attackAngle) / 2f + attackAngle / (float)bulletCount * (float)i);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Attack.texture);
			break;
		}
		case "AttackFinish":
			state = MonsterState.Idle;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Normal.texture);
			break;
		case "BornSE":
			SEMgr.Inst.elite11ChildBorn.PlaySE();
			break;
		case "HideSE":
			SEMgr.Inst.elite11ChildHide.PlaySE();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	private void SetIsCanBeAttack(bool canBeAttack)
	{
		base.CC_Self.enabled = canBeAttack;
		SetDotsCCEnable(canBeAttack);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = canBeAttack;
		componentData.CanTouch = canBeAttack;
		componentData.showAffect = canBeAttack;
		SetComponentData(componentData);
	}
}
