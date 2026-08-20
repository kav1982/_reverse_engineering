using System;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Monster6 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		Crawl,
		JumpBefore,
		Jumping,
		JumpToGround,
		Sucking
	}

	public VariableFloat crawlRadius;

	[Range(0f, 1f)]
	public float jumpChance = 0.2f;

	public VariableFloat jumpForwardForce;

	public VariableFloat jumpUpForce;

	public float jumpGravity;

	public float checkBloodDistance;

	public float suckBloodSpeed;

	public GameObject pfb_Monster6Corpse;

	public float suckRange;

	public MeshRenderer mr;

	public SpriteRenderer sr;

	private Sprite nowSprite;

	[Header("Spell")]
	public VariableInt deadBulletCount;

	public VariableFloat deadBulletForwardGrow;

	public int deadBulletCountGrow;

	public VariableFloat bulletForwardSpeed;

	public VariableFloat bulletUpSpeed;

	public float bulletGravity;

	public int spellDamage;

	private SpellSpawnParams ssp;

	[Header("sound")]
	public AudioSource as_Suck;

	public float originalMaxHealth;

	public MonsterState state;

	private GameObject bloodGO;

	private float totalSuckSizeAmount;

	private float suckBloodAmount;

	public override void SingleInitialCallback()
	{
		originalMaxHealth = myPpt.unitCfg.maxHP;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Damage = spellDamage;
		sSPModifier.Gravity = 0f - bulletGravity;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		base.Anima.Play("Monster6_Idle");
		state = MonsterState.BornIdle;
		bloodGO = null;
		totalSuckSizeAmount = 0f;
		suckBloodAmount = 0f;
		nowSprite = sr.sprite;
	}

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
		as_Suck.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void Update()
	{
		if (nowSprite != sr.sprite)
		{
			nowSprite = sr.sprite;
			mr.material.SetTexture(GameConstManaged.shaderTextureIndex, nowSprite.texture);
		}
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
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, crawlRadius));
			}
			break;
		case MonsterState.Crawl:
		{
			if (bloodGO == null)
			{
				if (navInfo.allCornerArrived)
				{
					GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, crawlRadius));
					break;
				}
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed * base.transform.localScale.x);
				CheckNavInfo();
				break;
			}
			SetMove(ToPointDir(bloodGO.transform.position) * base.MoveSpeed * base.transform.localScale.x);
			float num4 = suckRange + base.CC_Self.radius * base.transform.localScale.x;
			if ((base.transform.position - bloodGO.transform.position).sqrMagnitude < num4 * num4)
			{
				state = MonsterState.Sucking;
				as_Suck.Play();
				base.Anima.SetTrigger("Sucking");
			}
			break;
		}
		case MonsterState.JumpBefore:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Jumping:
			SetMove(Vector3.zero);
			if (base.transform.position.z > 0f)
			{
				SEMgr.Inst.monster6Land.PlaySE();
				base.transform.position = Tool2D.IgnoreZPoint(base.transform);
				LocalTransform componentData2 = GetComponentData<LocalTransform>();
				componentData2.Position = base.transform.position;
				SetComponentData(componentData2);
				JumpStop_Dots();
				state = MonsterState.JumpToGround;
				base.Anima.SetTrigger("JumpToGround");
			}
			break;
		case MonsterState.JumpToGround:
			SetMove(Vector3.zero);
			break;
		case MonsterState.Sucking:
		{
			SetMove(Vector3.zero);
			if (bloodGO == null)
			{
				as_Suck.Stop();
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
				break;
			}
			float num = suckRange + base.CC_Self.radius * base.transform.localScale.x;
			if ((base.transform.position - bloodGO.transform.position).sqrMagnitude > num * num)
			{
				as_Suck.Stop();
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
				break;
			}
			float num2 = Time.deltaTime * suckBloodSpeed;
			totalSuckSizeAmount += num2;
			base.transform.localScale = Vector3.one * Mathf.Sqrt(1f + totalSuckSizeAmount);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Scale = base.transform.localScale.x;
			SetComponentData(componentData);
			suckBloodAmount += num2 * originalMaxHealth;
			if (suckBloodAmount >= 1f)
			{
				suckBloodAmount -= 1f;
				myPpt.unitCfg.currentHP += 1f;
				myPpt.unitCfg.maxHP += 1f;
			}
			if (bloodGO.transform.localScale.x > 1f)
			{
				float num3 = Mathf.Pow(bloodGO.transform.localScale.x, 2f) - num2;
				if (num3 > 1f)
				{
					bloodGO.transform.localScale = Vector3.one * Mathf.Sqrt(num3);
				}
				else
				{
					bloodGO.transform.localScale = Vector3.one * num3;
				}
			}
			else if (bloodGO.transform.localScale.x > 0f)
			{
				bloodGO.transform.localScale = Vector3.one * (bloodGO.transform.localScale.x - num2);
			}
			else
			{
				UnityEngine.Object.Destroy(bloodGO);
				as_Suck.Stop();
				state = MonsterState.Crawl;
				base.Anima.SetTrigger("Crawl");
				CheckBlood();
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		}
	}

	private void CheckBlood()
	{
		UnityEngine.Collider nearestColliderByTag = GeneralTool.GetNearestColliderByTag(base.transform.position, checkBloodDistance, "Monster6Corpse");
		if (nearestColliderByTag == null)
		{
			bloodGO = null;
		}
		else
		{
			bloodGO = nearestColliderByTag.gameObject;
		}
	}

	public void ForceJump(Vector3 jumpDir)
	{
		state = MonsterState.Jumping;
		base.Anima.Play("Monster6_Jumping");
		base.Rigid.linearVelocity = jumpDir * jumpForwardForce.RandomResult() * base.transform.localScale.x;
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = base.Rigid.linearVelocity;
		SetComponentData(componentData);
		JumpStart_Dots(jumpUpForce.RandomResult(), jumpGravity);
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "CrawlAddForce":
			if (bloodGO != null)
			{
				SetMove(ToPointDir(bloodGO.transform.position) * base.MoveSpeed * base.transform.localScale.x);
			}
			else
			{
				SetMove(Tool2D.GetDir() * base.MoveSpeed * base.transform.localScale.x);
			}
			break;
		case "CrawlFinish":
			CheckBlood();
			if (bloodGO == null && UnityEngine.Random.value < jumpChance)
			{
				state = MonsterState.JumpBefore;
				base.Anima.SetTrigger("Jump");
			}
			break;
		case "JumpAddForce":
		{
			state = MonsterState.Jumping;
			base.Anima.SetTrigger("Jumping");
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
			base.Rigid.linearVelocity = Tool2D.GetDir() * jumpForwardForce.RandomResult() * base.transform.localScale.x;
			PhysicsVelocity componentData2 = GetComponentData<PhysicsVelocity>();
			componentData2.Linear = base.Rigid.linearVelocity;
			SetComponentData(componentData2);
			JumpStart_Dots(jumpUpForce.RandomResult(), jumpGravity);
			break;
		}
		case "JumpToGroundFinish":
			state = MonsterState.Crawl;
			base.Anima.SetTrigger("Crawl");
			GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, crawlRadius));
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		int num = deadBulletCount.RandomResult() + (int)((base.transform.localScale.x - 1f) * 10f * (float)deadBulletCountGrow);
		float minInclusive = (base.transform.localScale.x - 1f) * 10f * deadBulletForwardGrow.value1;
		float maxInclusive = (base.transform.localScale.x - 1f) * 10f * deadBulletForwardGrow.value2;
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; i < num; i++)
		{
			sSPModifier.Speed = bulletForwardSpeed.RandomResult() + UnityEngine.Random.Range(minInclusive, maxInclusive);
			sSPModifier.CurrentFallSpeed = 0f - bulletUpSpeed.RandomResult();
			sSPModifier.Direction = Tool2D.GetDir();
			sSPModifier.SpawnPosition = base.transform.position;
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
		UnityEngine.Object.Instantiate(pfb_Monster6Corpse, Tool2D.IgnoreZPoint(base.transform), Tool2D.GetRotation(), LevelMgr.Inst.CurrentRoomT).GetComponent<Monster6_Corpse>().Initialize(base.transform.localScale.x);
	}
}
