using System;
using System.Collections;
using UnityEngine;

public class Elite11_Child : UnitBase, IComparable
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

	public VariableFloat farDistance;

	public float closeChance;

	public VariableFloat teleportCD;

	private float teleportCDTimer;

	public float hideTime;

	public Transform tsf_Motion;

	public Shadow thisShadow;

	[Header("表现")]
	public Elite11_ChildTentacle hand1;

	public Elite11_ChildTentacle hand2;

	public SpriteRenderer sr_DirtFore;

	public SpriteRenderer sr_DirtBack;

	public VariableFloat asPitch;

	[Header("攻击")]
	public VariableFloat AttackCD;

	private float attackCDTimer;

	[Header("子弹模式")]
	public float spellSpeed;

	public float spellHeight;

	public int spellDamage;

	public int spellCount;

	public float spellLifeTime;

	public float bulletKnockBack;

	private SpellSpawnParams ssp1;

	[Header("封锁子弹模式")]
	public VariableFloat blockSpellDistance;

	public VariableFloat blockSpellDuration;

	public VariableFloat blockSpellSlowDownTime;

	public VariableInt blockSpellCount;

	private SpellSpawnParams ssp2;

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

	public int CompareTo(object obj)
	{
		Vector3 position = Elite11.Inst.transform.position;
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, base.transform.position - position);
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, (obj as Elite11_Child).transform.position - position);
		if (num2 < 0f)
		{
			num2 += 360f;
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	public override void SingleInitialCallback()
	{
		ssp1 = UnitDotsSyncSystem.GetSpellPrototype(90221);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp1);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellLifeTime;
		sSPModifier.ApplyToSSP(ref ssp1);
		ssp2 = UnitDotsSyncSystem.GetSpellPrototype(90441);
		sSPModifier = UnitBase.GetSSPModifier(in ssp2);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.ApplyToSSP(ref ssp2);
		myPpt.RemoveSRFromArray(sr_DirtFore);
		myPpt.RemoveSRFromArray(sr_DirtBack);
	}

	public override void EveryInitialCallback()
	{
		state = MonsterState.BornIdle;
		base.Anima.Play("BornIdle");
		attackCDTimer = UnityEngine.Random.value * AttackCD.value2;
		teleportCDTimer = UnityEngine.Random.value * teleportCD.value2;
		SetCantAttack();
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
		switch (state)
		{
		case MonsterState.BornIdle:
			if (changedState)
			{
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanBeTarget = false;
				componentData.showAffect = false;
				componentData.CanTouch = false;
				SetComponentData(componentData);
				base.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				thisShadow.Hide();
				base.Anima.Play("BornIdle");
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
			}
			else if (attackCDTimer > AttackCD.result)
			{
				attackCDTimer = 0f;
				state = MonsterState.Attack;
			}
			else
			{
				attackCDTimer += Time.deltaTime / (float)Elite11.Inst.spawner.children.Count;
				teleportCDTimer += Time.deltaTime / (float)Elite11.Inst.spawner.children.Count;
				SetMove(Vector3.zero);
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
					GetNearestTarget();
				}
			}
			SetMove(Vector3.zero);
			attackCDTimer += Time.deltaTime / (float)Elite11.Inst.spawner.children.Count;
			teleportCDTimer += Time.deltaTime / (float)Elite11.Inst.spawner.children.Count;
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		}
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		if (!base.CC_Self.enabled)
		{
			info.immuneDamage = true;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		SEMgr.Inst.elite11ChildAttack.PlaySE().pitch = asPitch.RandomResult();
		if (Elite11.Inst != null)
		{
			Elite11.Inst.spawner.ReportDead(this);
		}
		base.AfterDead(ref info);
	}

	private void ShootBullet()
	{
		blockSpellCount.RandomResult();
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp2);
		sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
		for (int i = 0; i < blockSpellCount.result; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir();
			sSPModifier.Duration = blockSpellDuration.RandomResult();
			sSPModifier.Float1 = blockSpellSlowDownTime.RandomResult();
			sSPModifier.Speed = blockSpellDistance.RandomResult() / sSPModifier.Float1 * 2f;
			sSPModifier.Float2 = sSPModifier.Speed;
			sSPModifier.Knockback = bulletKnockBack;
			sSPModifier.ApplyToSSP(ref ssp2);
			ShootSpell(ssp2);
		}
	}

	private IEnumerator ShootStraightBullet(Vector3 dir)
	{
		SEMgr.Inst.elite11ChildAttack1.PlaySE().pitch = asPitch.RandomResult();
		UnitSpellModifier usm = UnitBase.GetSSPModifier(in ssp1);
		usm.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
		for (int i = 0; i < 3; i++)
		{
			usm.Direction = dir;
			usm.Knockback = bulletKnockBack;
			usm.ApplyToSSP(ref ssp1);
			ShootSpell(ssp1);
			yield return new WaitForSeconds(0.13f);
		}
		yield return null;
	}

	public void SetCanAttack()
	{
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.showAffect = true;
		componentData.CanBeTarget = true;
		componentData.CanTouch = true;
		SetComponentData(componentData);
		hand1.UnlockMotion();
		hand2.UnlockMotion();
	}

	public void SetCantAttack()
	{
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.showAffect = false;
		componentData.CanBeTarget = false;
		componentData.CanTouch = false;
		SetComponentData(componentData);
		hand1.LockMotion();
		hand2.LockMotion();
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "HideFinish":
			GetNearestTarget();
			if (base.HaveTarget)
			{
				if (UnityEngine.Random.value < closeChance)
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
				base.transform.position = Tool2D.GetNavMeshPointIngoreZ(Elite11.elite11Position, farDistance);
			}
			SyncDotsPosition();
			state = MonsterState.Show;
			break;
		case "CanAttack":
			SetCanAttack();
			break;
		case "CantAttack":
			SetCantAttack();
			break;
		case "ShowFinish":
			state = MonsterState.Idle;
			break;
		case "Born":
			SetCanAttack();
			break;
		case "BornFinish":
			state = MonsterState.Idle;
			break;
		case "AttackFinish":
			state = MonsterState.Idle;
			break;
		case "Attack":
		{
			Vector3 oldDir = Tool2D.GetDir();
			GetNearestTarget();
			if (base.HaveTarget)
			{
				oldDir = ToTargetDir();
			}
			oldDir = Tool2D.GetDir(oldDir, UnityEngine.Random.Range(-10, 10));
			SetFlip(oldDir.x);
			StartCoroutine(ShootStraightBullet(oldDir));
			ShootBullet();
			break;
		}
		case "BornSE":
			SEMgr.Inst.elite11ChildBorn.PlaySE();
			break;
		case "HideSE":
			SEMgr.Inst.elite11ChildHide.PlaySE();
			break;
		}
	}
}
