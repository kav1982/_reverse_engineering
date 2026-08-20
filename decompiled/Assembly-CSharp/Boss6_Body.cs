using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss6_Body : UnitBase
{
	public enum MonsterState
	{
		Idle,
		StaticHide,
		FreeFollow,
		FreeFollowSimpleAttack,
		FreeFollowWaveAttack,
		FreeFollowBlockAttack,
		FreeFollowAimAttack,
		FreeFollowAttackAfter,
		FreeHide
	}

	[Header("状态")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	private MonsterState attackState;

	private float afterAttackTime;

	private Vector3 leftDir;

	private Vector3 rightDir;

	[Header("本体相关")]
	public float damageReduceRatio;

	public List<Entity> hitList;

	public Boss6_Stage2 master;

	public Transform tsf_Body;

	public Transform tsf_Cover;

	public Transform tsf_BodyShadow;

	public float bodyHeight;

	public float basicAngle;

	public float basicAngle1;

	public Transform tsf_LeftLeg;

	public Transform tsf_RightLeg;

	public Transform tsf_LeftLeg1;

	public Transform tsf_RightLeg1;

	public Transform tsf_LeftLegShadow;

	public Transform tsf_RightLegShadow;

	public Transform tsf_LeftLegShadow1;

	public Transform tsf_RightLegShadow1;

	public List<SpriteRenderer> SRs = new List<SpriteRenderer>();

	public List<SpriteRenderer> SRs_Shadow = new List<SpriteRenderer>();

	public Color shadowColor;

	public float changeSpriteChance;

	public Sprite bodySprite;

	public Sprite bodySprite1;

	public Sprite bodySprite2;

	public Sprite tailSprite;

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
		for (int i = 0; i < SRs.Count; i++)
		{
			myPpt.RemoveSRFromArray(SRs[i]);
		}
		for (int j = 0; j < SRs_Shadow.Count; j++)
		{
			myPpt.RemoveSRFromArray(SRs_Shadow[j]);
			SRs_Shadow[j].material.color = shadowColor;
		}
	}

	public void SetAttackDir(Vector3 leftDir, Vector3 rightDir)
	{
		this.leftDir = leftDir;
		this.rightDir = rightDir;
	}

	public void SetTail(bool isTail)
	{
		if (isTail)
		{
			tsf_LeftLeg.gameObject.SetActive(value: false);
			tsf_RightLeg.gameObject.SetActive(value: false);
			tsf_LeftLegShadow.gameObject.SetActive(value: false);
			tsf_RightLegShadow.gameObject.SetActive(value: false);
			SRs[0].sprite = tailSprite;
			SRs_Shadow[0].sprite = tailSprite;
			return;
		}
		tsf_LeftLegShadow.gameObject.SetActive(value: true);
		tsf_RightLegShadow.gameObject.SetActive(value: true);
		tsf_LeftLeg.gameObject.SetActive(value: true);
		tsf_RightLeg.gameObject.SetActive(value: true);
		if (GeneralTool.ChanceResult(changeSpriteChance))
		{
			SRs[0].sprite = bodySprite1;
			SRs_Shadow[0].sprite = bodySprite1;
		}
		else if (GeneralTool.ChanceResult(changeSpriteChance))
		{
			SRs[0].sprite = bodySprite2;
			SRs_Shadow[0].sprite = bodySprite2;
		}
		else
		{
			SRs[0].sprite = bodySprite;
			SRs_Shadow[0].sprite = bodySprite;
		}
	}

	public void SetColor(Color color)
	{
		if (SRs[0].material.color != color)
		{
			for (int i = 0; i < SRs.Count; i++)
			{
				SRs[i].material.color = color;
			}
		}
	}

	public override void EveryInitialCallback()
	{
		tsf_Body.transform.localPosition = new Vector3(0f, bodyHeight, 0f - bodyHeight);
		state = MonsterState.Idle;
	}

	public void SetDir(Vector3 dir)
	{
		tsf_Body.localEulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, dir));
	}

	public void SetCoverDir(Vector3 dir)
	{
		tsf_Cover.eulerAngles = new Vector3(0f, 0f, Tool2D.IgnoreZAngleWithSign(Vector3.up, dir));
	}

	public void SetHandDir(float rotateAngle, float rotateAngle1)
	{
		tsf_BodyShadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		tsf_LeftLeg1.localEulerAngles = new Vector3(0f, 0f, basicAngle1 + rotateAngle1);
		tsf_RightLeg1.localEulerAngles = new Vector3(0f, 0f, 0f - basicAngle1 - rotateAngle1);
		tsf_LeftLegShadow1.localEulerAngles = new Vector3(0f, 0f, basicAngle1 + rotateAngle1);
		tsf_RightLegShadow1.localEulerAngles = new Vector3(0f, 0f, 0f - basicAngle1 - rotateAngle1);
		tsf_LeftLeg.localEulerAngles = new Vector3(0f, 0f, basicAngle + rotateAngle);
		tsf_RightLeg.localEulerAngles = new Vector3(0f, 0f, 0f - basicAngle - rotateAngle);
		tsf_LeftLegShadow.localEulerAngles = new Vector3(0f, 0f, basicAngle + rotateAngle);
		tsf_RightLegShadow.localEulerAngles = new Vector3(0f, 0f, 0f - basicAngle - rotateAngle);
	}

	private void SetAfterAttack(float afterTime)
	{
		afterAttackTime = afterTime;
		state = MonsterState.FreeFollowAttackAfter;
	}

	public override void Update()
	{
		if (master.state != Boss6_Stage2.MonsterState.Dead)
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(master.myPpt.myEntity, out var result))
			{
				if (componentData.affect_VenomCurrentStack > 0f)
				{
					result.SetVenom(componentData.affect_VenomDurationTimer, componentData.affect_VenomCurrentStack);
				}
				if (componentData.affect_burnDurationTimer > 0f)
				{
					result.SetBurn(componentData.affect_burnDurationTimer, componentData.affect_burnHPRatioPerHit);
				}
				if (componentData.voidEffectTimer > 0f)
				{
					result.SetVoid(componentData.voidExplosionData);
				}
				SetComponentData(result, master.myPpt.myEntity);
			}
			componentData.ClearVenomState();
			componentData.ClearBurnState();
			componentData.ClearVoidState();
			SetComponentData(componentData);
		}
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
		case MonsterState.Idle:
			if (changedState)
			{
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
				componentData3.CanBeTarget = true;
				componentData3.CanTouch = true;
				SetComponentData(componentData3);
				tsf_Body.gameObject.SetActive(value: true);
				tsf_BodyShadow.gameObject.SetActive(value: true);
			}
			break;
		case MonsterState.FreeFollow:
			if (changedState)
			{
				myPpt.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData4 = GetComponentData<UnitProperty_Dots>();
				componentData4.CanBeTarget = true;
				componentData4.CanTouch = true;
				SetComponentData(componentData4);
				tsf_Body.gameObject.SetActive(value: true);
				tsf_BodyShadow.gameObject.SetActive(value: true);
			}
			break;
		case MonsterState.FreeHide:
			if (changedState)
			{
				myPpt.CC_Self.enabled = false;
				SetDotsCCEnable(isOpen: false);
				UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
				componentData2.CanBeTarget = false;
				componentData2.CanTouch = false;
				SetComponentData(componentData2);
				tsf_Body.gameObject.SetActive(value: false);
				tsf_BodyShadow.gameObject.SetActive(value: false);
				StopAllCoroutines();
			}
			break;
		}
	}

	public void SetDead()
	{
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		componentData.CanBeTarget = false;
		componentData.InvincibleRegister();
		componentData.ChangeColor(myPpt.Color_NormalBody);
		SetComponentData(componentData);
	}

	public override void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		base.BeforeAnnouncedDeath_Dots(ref info);
		if (!master.myPpt.AlreadyDead)
		{
			info.stopAnnouncedDeath = true;
			myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.attackerType == AttackerType.Venom || info.attackerType == AttackerType.Burn)
		{
			info.immuneDamage = true;
			return;
		}
		Boss6_Stage2.Inst.takeDamageInfoBuffer.Add(info);
		info.immuneDamage = true;
	}
}
