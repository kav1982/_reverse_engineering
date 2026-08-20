using System.Collections.Generic;
using UnityEngine;

public class Monster8_3 : UnitBase
{
	private enum UnitState
	{
		BornIdle,
		Idle,
		IdleWalk,
		Attack
	}

	public float shoutInterval;

	public float shoutRadius;

	public int[] shoutID;

	[Header("Idle Walk")]
	public VariableFloat idleTime;

	public VariableFloat idleWalkRadius;

	public VariableFloat idleWalkTime;

	[Range(0f, 1f)]
	[Header("Attack")]
	public float attackChange;

	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public AIPattern pattern;

	private SpellSpawnParams ssp;

	[Header("Pattern2")]
	public float attackBulletCount;

	public VariableInt spellCount;

	public float spellHalfAngle;

	[Header("和谐")]
	public List<Sprite> normalSprites;

	public List<Sprite> harmonySprites;

	public MeshRenderer MR;

	private Sprite nowSprite;

	private UnitState unitState;

	private float idleTimer;

	private float idleWalkTimer;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		base.Anima.SetTrigger("Idle");
		unitState = UnitState.BornIdle;
		idleTimer = 0f;
		idleWalkTimer = 0f;
		if (GameMgr.IsHarmony_Static)
		{
			normalSprites = harmonySprites;
		}
	}

	public override void Update()
	{
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		switch (unitState)
		{
		case UnitState.BornIdle:
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				unitState = UnitState.Idle;
				idleTime.RandomResult();
			}
			break;
		case UnitState.Idle:
			SetMove(Vector3.zero, isFlip: false);
			idleTimer += Time.deltaTime;
			if (idleTimer >= idleTime.result)
			{
				idleTimer = 0f;
				idleWalkTime.RandomResult();
				unitState = UnitState.IdleWalk;
				if (pattern == AIPattern.Pattern1)
				{
					base.Anima.SetTrigger("Walk");
				}
				else
				{
					base.Anima.SetTrigger("Walk1");
				}
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			break;
		case UnitState.IdleWalk:
			idleWalkTimer += Time.deltaTime;
			if (idleWalkTimer >= idleWalkTime.result)
			{
				idleWalkTimer = 0f;
				idleWalkTime.RandomResult();
				unitState = UnitState.Idle;
				base.Anima.SetTrigger("Idle");
			}
			else if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, idleWalkRadius));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			break;
		case UnitState.Attack:
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(unitState);
			break;
		}
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "0":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[0].texture);
			break;
		case "1":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[1].texture);
			break;
		case "2":
			MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[2].texture);
			break;
		case "3":
			if (normalSprites.Count <= 3)
			{
				MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[2].texture);
			}
			else
			{
				MR.material.SetTexture(GameConstManaged.shaderTextureIndex, normalSprites[3].texture);
			}
			break;
		case "Attack":
		{
			if (!base.HaveTarget)
			{
				break;
			}
			Vector3 dir = Tool2D.GetDir();
			SetFlip(ToTargetDir().x);
			if (pattern == AIPattern.Pattern2 || pattern == AIPattern.Pattern4 || pattern == AIPattern.Pattern6)
			{
				for (int i = 0; (float)i < attackBulletCount; i++)
				{
					Vector3 direction = ToTargetDir(0f - spellHalfAngle + (float)i * spellHalfAngle);
					if (targetPpt == PlayerMgr.Inst.PlayerCtrller.myPpt && !PlayerMgr.Inst.PlayerCtrller.IsVisible)
					{
						direction = Tool2D.GetDir(dir, 0f - spellHalfAngle + (float)i * spellHalfAngle);
					}
					UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
					sSPModifier.Direction = direction;
					sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			else if (targetPpt == PlayerMgr.Inst.PlayerCtrller.myPpt && !PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
				sSPModifier2.Direction = dir;
				sSPModifier2.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier2.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			else
			{
				UnitSpellModifier sSPModifier3 = UnitBase.GetSSPModifier(in ssp);
				sSPModifier3.Direction = ToTargetDir();
				sSPModifier3.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier3.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			break;
		}
		case "AttackFinish":
			unitState = UnitState.Idle;
			base.Anima.SetTrigger("Idle");
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (EntityIsValid(info.attackerEntity) && (unitState == UnitState.Idle || unitState == UnitState.IdleWalk))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>(info.attackerEntity);
			if ((componentData.unitCfg.unitType == UnitType.Player || componentData.unitCfg.unitType == UnitType.Teammate || componentData.unitCfg.unitType == UnitType.TeammateNotAttack) && Random.value <= attackChange)
			{
				targetEntity = info.attackerEntity;
				unitState = UnitState.Attack;
				base.Anima.SetTrigger("Attack");
			}
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (pattern >= AIPattern.Pattern2)
		{
			spellCount.RandomResult();
			float num = Random.Range(0f, 360f);
			for (int i = 0; i < spellCount.result; i++)
			{
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.Direction = Tool2D.GetDir(360f / (float)spellCount.result * (float)i + num);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		if (pattern == AIPattern.Pattern3)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFog", base.transform.position, 4f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern4)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFogLarge", base.transform.position, 4f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern5)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFog2", base.transform.position, 6f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
		if (pattern == AIPattern.Pattern6)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster8_HealFogLarge2", base.transform.position, 6f).GetComponent<Monster8_HealFog>().owner = myPpt;
			SEMgr.Inst.monster8Explode.PlaySE();
		}
	}
}
