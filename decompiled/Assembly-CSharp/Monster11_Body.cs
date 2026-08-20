using Unity.Entities;
using UnityEngine;

public class Monster11_Body : UnitBase
{
	private enum UnitState
	{
		Idle,
		Attack
	}

	[Space(50f)]
	public Transform tsf_Model;

	public MeshRenderer mr1;

	public MeshRenderer mr2;

	public Sprite sprite_Head;

	public Sprite sprite_Body;

	public Sprite sprite_Jaw;

	public float bulletLandRadius;

	[Header("Spell")]
	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	public float attackRange;

	[Header("Pattern2")]
	public AIPattern pattern;

	public VariableFloat boneSpeed;

	public float boneDuration;

	public int boneBounceTime;

	public float boneUpSpeed;

	[Header("和谐模式")]
	public Sprite sprite_Head_Harmony;

	public Sprite sprite_Body_Harmony;

	public Sprite sprite_Jaw_Harmony;

	private UnitState state;

	private Monster11 monster11;

	private bool isHead;

	private SpellSpawnParams ssp;

	public bool IsShow => base.CC_Self.enabled;

	public override void SingleInitialCallback()
	{
		if (pattern == AIPattern.Pattern1)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			sSPModifier.Shooter = myPpt.myEntity;
			sSPModifier.Speed = spellSpeed;
			sSPModifier.Damage = spellDamage;
			sSPModifier.Duration = spellDuration;
			sSPModifier.Gravity = 0f;
			sSPModifier.ApplyToSSP(ref ssp);
		}
		else if (pattern == AIPattern.Pattern2)
		{
			ssp = UnitDotsSyncSystem.GetSpellPrototype(90021);
			UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
			sSPModifier2.Shooter = myPpt.myEntity;
			sSPModifier2.Damage = spellDamage;
			sSPModifier2.Duration = boneDuration;
			sSPModifier2.ReboundCount = 5;
			sSPModifier2.ApplyToSSP(ref ssp);
		}
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.Idle;
		Show();
	}

	public override void Frame1InitialCallback()
	{
		if (isHead)
		{
			myPpt.unitCfg.maxHP *= monster11.headHPRatio;
		}
		else
		{
			myPpt.unitCfg.maxHP = UnitConfig.map[myPpt.unitCfg.id].maxHP;
		}
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP;
	}

	public override void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "ShrinkFlase":
			monster11.SetShrinkFlase();
			break;
		case "Attack":
			if (base.HaveTarget)
			{
				Vector3 vector = base.TargetPoint;
				if (ToPointDistance(base.TargetPoint) > attackRange)
				{
					vector = base.transform.position + ToPointDir(vector) * attackRange;
				}
				Vector3 vector2 = Tool2D.IgnoreZPoint(vector + Tool2D.GetDir() * Random.Range(0f, bulletLandRadius));
				if (pattern == AIPattern.Pattern1)
				{
					UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
					sSPModifier.CurrentFallSpeed = (0f - (base.transform.position.z - vector2.z)) / (Tool2D.IgnoreZDistance(base.transform.position, vector2) / spellSpeed);
					sSPModifier.Direction = ToPointDir(vector2);
					sSPModifier.SpawnPosition = base.transform.position;
					sSPModifier.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
				else if (pattern == AIPattern.Pattern2)
				{
					UnitSpellModifier sSPModifier2 = UnitBase.GetSSPModifier(in ssp);
					sSPModifier2.Speed = boneSpeed.RandomResult();
					sSPModifier2.CurrentFallSpeed = 0f - boneUpSpeed;
					float time = Tool2D.IgnoreZDistance(base.transform.position, vector2) / sSPModifier2.Speed;
					sSPModifier2.Gravity = GeneralTool.CannonAcceleration(0f - base.transform.position.z, 0f - boneUpSpeed, time);
					sSPModifier2.Direction = ToPointDir(vector2);
					sSPModifier2.SpawnPosition = base.transform.position;
					sSPModifier2.ApplyToSSP(ref ssp);
					ShootSpell(ssp);
				}
			}
			break;
		case "AttackFinish":
			state = UnitState.Idle;
			monster11.AttackFinish();
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		monster11.BodyDead(this, ref info);
	}

	public void SetMother(Monster11 monster11, bool isHead)
	{
		this.monster11 = monster11;
		this.isHead = isHead;
		if (isHead)
		{
			tsf_Model.transform.position += new Vector3(0f, 0f, -0.3f) * 0.01f;
			if (GameMgr.IsHarmony_Static)
			{
				mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Head_Harmony.texture);
				mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Jaw_Harmony.texture);
			}
			else
			{
				mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Head.texture);
				mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Jaw.texture);
			}
			mr2.gameObject.SetActive(value: true);
		}
		else
		{
			if (GameMgr.IsHarmony_Static)
			{
				mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Body_Harmony.texture);
				mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Jaw_Harmony.texture);
			}
			else
			{
				mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Body.texture);
				mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_Jaw.texture);
			}
			mr2.gameObject.SetActive(value: false);
		}
	}

	public void Upspring()
	{
		if (state == UnitState.Idle)
		{
			base.Anima.SetTrigger("Upspring");
		}
	}

	public void SetAttack(Entity targetEntity)
	{
		base.targetEntity = targetEntity;
		state = UnitState.Attack;
		base.Anima.SetTrigger("Attack");
	}

	public void Hide()
	{
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = false;
		componentData.showAffect = false;
		SetComponentData(componentData);
		myPpt.tsf_Layer.gameObject.SetActive(value: false);
	}

	public void Show()
	{
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanBeTarget = true;
		componentData.showAffect = true;
		SetComponentData(componentData);
		myPpt.tsf_Layer.gameObject.SetActive(value: true);
	}
}
