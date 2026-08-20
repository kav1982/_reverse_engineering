using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Physics;
using UnityEngine;

public class Elite55 : UnitBase
{
	private enum Elite55Skill
	{
		HexAttack,
		CatchTarget
	}

	private enum Elite55State
	{
		BornIdle,
		Move,
		Teleport,
		TeleportEndCastSkill
	}

	private static readonly int MixPercent = Shader.PropertyToID("_MixPercent");

	private UIEndlessEliteHpBar hpBar;

	public Transform WarningObjTransform;

	public SpriteRenderer WarningObjSprite;

	public SpriteRenderer BodySprite;

	private Elite55State state;

	private bool isFaceRight = true;

	public Transform ModelTransform;

	public Transform TeleportEffectTransform;

	private float modelScaleX = 1f;

	public float FaceDirectionChangeDuration;

	public ParticleSystem ChargeParticles;

	[Header("传送相关参数")]
	public float TeleportInterval;

	private float teleportTimer;

	public Vector2 TeleportRange;

	public float ToPlayerMotionDistance;

	private Vector3 teleportPosition;

	public float TeleportWaitTime;

	[Header("正六边攻击")]
	public float HexAttackSkillDuration;

	public float HexAttackStartShootAt;

	public int HexAttackCount;

	public float HexShootInterval;

	public int HexOneSideBulletCount;

	public float HexBulletSpeed;

	public float HexBulletDuration;

	public float HexBulletDamage;

	private float hexInitialAngle;

	public float HexAngleMovePerShoot;

	public float HexSpeedRatioPower;

	public float HexSpeedUpPerShoot;

	public int HexShootWave;

	public float FinishHexBonusWaitTime;

	public float HexWarningLaserWidth;

	[Header("凹型捕获")]
	public float CatchAttackSkillDuration;

	public float CatchAttackStartShootAt;

	public float CatchBaseRange;

	public float CatchRangeUpPerShoot;

	public int CatchShootCount;

	public int CatchOneSideBulletCount;

	public float CatchBulletSpeed;

	public float CatchBulletDuration;

	public float CatchBulletDamage;

	private List<Boss52HorizontalDrone> CatchWarningLaserList = new List<Boss52HorizontalDrone>();

	public float CatchWarningLaserSpawnAt;

	public int CatchShootWave;

	public float FinishCatchBonusWaitTime;

	public float CatchPreStopRotateTime;

	private List<Vector3> CatchBaseDirList = new List<Vector3>();

	private List<Elite55ForceParticle> CatchForceParticleList = new List<Elite55ForceParticle>();

	private int skillCastCounter;

	private float skillTimer;

	private int shootCounter;

	private float shootTimer;

	private bool isSkillInitialize;

	private SpellSpawnParams ssp;

	private UnitSpellModifier usm;

	private Elite55Skill currentSkill;

	private void OnEnable()
	{
		WarningObjTransform.gameObject.SetActive(value: false);
	}

	public override void SingleInitialCallback()
	{
		hpBar = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/UI/UIEndlessEliteHpBar"), myPpt.tsf_Layer.position + new Vector3(0f, myPpt.unitCfg.relicShowHPUIHight - 0.2f, 0f) * myPpt.tsf_Layer.lossyScale.y, Quaternion.identity, myPpt.tsf_Layer).GetComponent<UIEndlessEliteHpBar>();
		hpBar.Initialize(this);
	}

	public override void EveryInitialCallback()
	{
		GetNearestTargetPlayerFirst();
		state = Elite55State.BornIdle;
		currentSkill = Elite55Skill.HexAttack;
		teleportTimer = 0f;
		shootTimer = 0f;
		skillCastCounter = 0;
		isSkillInitialize = false;
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90471);
		usm = UnitBase.GetSSPModifier(in ssp);
		usm.Duration = HexBulletDuration;
		usm.Speed = HexBulletSpeed;
		usm.Damage = HexBulletDamage * GameConstManaged.endlessMonsterDamageRatio;
		usm.Shooter = myPpt.myEntity;
		usm.ApplyToSSP(ref ssp);
		ssp.DisableResize = true;
		hpBar.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		UpdateState();
	}

	private void UpdateState()
	{
		switch (state)
		{
		case Elite55State.BornIdle:
			EnterState(Elite55State.Move);
			break;
		case Elite55State.Move:
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (base.HaveTarget)
			{
				GetNavInfo(base.TargetPoint);
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
				teleportTimer += Time.deltaTime;
			}
			else
			{
				SetMove(Vector3.zero, isFlip: false);
			}
			FaceToPlayer();
			UpdateFaceDirection();
			if (base.HaveTarget && teleportTimer >= TeleportInterval)
			{
				EnterState(Elite55State.Teleport);
			}
			break;
		case Elite55State.Teleport:
			teleportTimer += Time.deltaTime;
			isFaceRight = teleportPosition.x >= base.transform.position.x;
			UpdateFaceDirection();
			if (teleportTimer >= TeleportWaitTime)
			{
				EnterState(Elite55State.TeleportEndCastSkill);
			}
			SetMove(Vector3.zero, isFlip: false);
			break;
		case Elite55State.TeleportEndCastSkill:
			SetMove(Vector3.zero, isFlip: false);
			if (skillTimer >= 0.3f)
			{
				FaceToPlayer();
				UpdateFaceDirection();
			}
			skillTimer += Time.deltaTime;
			switch (currentSkill)
			{
			case Elite55Skill.HexAttack:
			{
				if (!isSkillInitialize)
				{
					isSkillInitialize = true;
					for (int num7 = 0; num7 < 6; num7++)
					{
						Vector3 dir = Tool2D.GetDir(hexInitialAngle + (float)(60 * num7));
						Boss52HorizontalDrone component3 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_HorizontalLaserDrone", base.transform.position).GetComponent<Boss52HorizontalDrone>();
						float hexWarningLaserWidth = HexWarningLaserWidth;
						float delayLaserTimer = HexAttackStartShootAt - 0.1f;
						Vector3 initialMoveDirection = dir;
						component3.InitDroneData(0.1f, 25f, hexWarningLaserWidth, 10f, dir, delayLaserTimer, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 6f, 0f, 0.4f, initialMoveDirection);
						component3.ShootByOtherSource(myPpt.myEntity);
					}
				}
				if (skillTimer >= HexAttackSkillDuration)
				{
					skillCastCounter++;
					EnterState(Elite55State.Move);
				}
				if (skillTimer < HexAttackStartShootAt || shootCounter >= HexAttackCount)
				{
					break;
				}
				shootTimer += Time.deltaTime;
				if (shootTimer < HexShootInterval)
				{
					break;
				}
				shootTimer -= HexShootInterval;
				if (shootCounter == 0)
				{
					ChargeParticles.Stop();
					SEMgr.Inst.elite55HexAttack.PlaySE();
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite55_HexBurst", base.transform.position, 5f);
				}
				List<Vector3> list = new List<Vector3>();
				for (int num8 = 0; num8 < 6; num8++)
				{
					list.Add(Tool2D.GetDir(hexInitialAngle + (float)(60 * num8)));
				}
				for (int num9 = 0; num9 < 6; num9++)
				{
					Vector3 vector7 = list[num9];
					Vector3 vector8 = ((num9 == 5) ? list[0] : list[num9 + 1]);
					Vector3 vector9 = vector8 - vector7;
					float num10 = Tool2D.IgnoreZDistance(vector7, vector8) / (float)(HexOneSideBulletCount - 1);
					for (int num11 = 0; num11 < HexOneSideBulletCount - 1; num11++)
					{
						Vector3 v2 = vector7 + vector9 * num10 * num11;
						float num12 = Mathf.Pow(Tool2D.IgnoreZDistance(Vector3.zero, v2), HexSpeedRatioPower);
						usm = UnitBase.GetSSPModifier(in ssp);
						v2 = v2.normalized;
						usm.SpawnPosition = base.transform.position + v2 * num12 * 0.5f + new Vector3(0f, 0f, -0.5f);
						usm.Direction = v2;
						usm.Speed = (HexBulletSpeed + HexSpeedUpPerShoot * (float)shootCounter) * num12;
						usm.ApplyToSSP(ref ssp);
						UnitDotsSyncSystem.ShootSpell(ssp);
					}
				}
				hexInitialAngle += HexAngleMovePerShoot;
				shootCounter++;
				break;
			}
			case Elite55Skill.CatchTarget:
			{
				if (skillTimer >= CatchAttackSkillDuration)
				{
					skillCastCounter++;
					EnterState(Elite55State.Move);
				}
				if (skillTimer < CatchAttackStartShootAt - CatchPreStopRotateTime)
				{
					Vector3 oldDir = Tool2D.IgnoreZV2ToV1Normal(PlayerMgr.Inst.PlayerPoint, base.transform.position);
					CatchBaseDirList.Clear();
					for (int i = 0; i < 6; i++)
					{
						CatchBaseDirList.Add(Tool2D.GetDir(oldDir, 60 * i));
					}
				}
				if (CatchWarningLaserList.Count == 0 && skillTimer >= CatchWarningLaserSpawnAt)
				{
					for (int j = 0; j < 3; j++)
					{
						float num = CatchBaseRange + (float)(CatchShootCount - 1) / 2f * CatchRangeUpPerShoot;
						Vector3 vector = base.transform.position + CatchBaseDirList[3 + j] * num;
						Vector3 normalized = (((j == 2) ? CatchBaseDirList[0] : CatchBaseDirList[4 + j]) - vector).normalized;
						Boss52HorizontalDrone component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_HorizontalLaserDrone", vector).GetComponent<Boss52HorizontalDrone>();
						component.InitDroneData(0.1f, (j == 2) ? 0f : num, (float)CatchShootCount * CatchRangeUpPerShoot * 0.8f, 10f, normalized, CatchAttackStartShootAt - 0.2f - CatchWarningLaserSpawnAt, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0f, default(Vector3), 0.1f, 0.05f, disableChargeSE: true, disableShootSE: true);
						component.ShootByOtherSource(myPpt.myEntity);
						CatchWarningLaserList.Insert(0, component);
						vector = base.transform.position + CatchBaseDirList[2 - j] * num;
						normalized = ((j == 2) ? CatchBaseDirList[5] : (CatchBaseDirList[1 - j] - vector)).normalized;
						component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss52_HorizontalLaserDrone", vector).GetComponent<Boss52HorizontalDrone>();
						component.InitDroneData(0.1f, (j == 2) ? 0f : num, (float)CatchShootCount * CatchRangeUpPerShoot * 0.8f, 10f, normalized, CatchAttackStartShootAt - 0.2f - CatchWarningLaserSpawnAt, 0.1f, 0.5f, 0f, 0f, null, default(Vector3), 0f, 0f, 0f, default(Vector3), 0.1f, 0.05f, disableChargeSE: true, disableShootSE: true);
						component.ShootByOtherSource(myPpt.myEntity);
						CatchWarningLaserList.Add(component);
					}
					for (int k = 0; k < 5; k++)
					{
						Elite55ForceParticle component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite55_CatchChargeParticle", base.transform.position, CatchAttackStartShootAt - CatchWarningLaserSpawnAt + 0.5f).GetComponent<Elite55ForceParticle>();
						component2.Initialize();
						CatchForceParticleList.Add(component2);
					}
					ChargeParticles.Play();
					SEMgr.Inst.elite55Charge.PlaySE();
				}
				else
				{
					for (int l = 0; l < CatchWarningLaserList.Count / 2; l++)
					{
						Vector3 vector2 = CatchBaseDirList[3 + l];
						Vector3 normalized2 = (((l == 2) ? CatchBaseDirList[0] : CatchBaseDirList[4 + l]) - vector2).normalized;
						Boss52HorizontalDrone boss52HorizontalDrone = CatchWarningLaserList[3 + l];
						boss52HorizontalDrone.transform.position = base.transform.position + vector2 * (CatchBaseRange + (float)(CatchShootCount - 1) / 2f * CatchRangeUpPerShoot);
						boss52HorizontalDrone.ForceUpdateCurrentDirection(normalized2);
						vector2 = CatchBaseDirList[3 - l];
						normalized2 = (CatchBaseDirList[2 - l] - vector2).normalized;
						Boss52HorizontalDrone boss52HorizontalDrone2 = CatchWarningLaserList[2 - l];
						boss52HorizontalDrone2.transform.position = base.transform.position + vector2 * (CatchBaseRange + (float)(CatchShootCount - 1) / 2f * CatchRangeUpPerShoot);
						boss52HorizontalDrone2.ForceUpdateCurrentDirection(normalized2);
					}
					for (int m = 0; m < CatchForceParticleList.Count; m++)
					{
						Elite55ForceParticle elite55ForceParticle = CatchForceParticleList[m];
						Vector3 vector3 = CatchBaseDirList[m + 1];
						elite55ForceParticle.UpdateFuseParticleEffect(targetPos: base.transform.position + vector3 * (CatchBaseRange + (float)(CatchShootCount - 1) / 2f * CatchRangeUpPerShoot), startPos: base.transform.position + vector3 * 0.5f);
					}
				}
				if (skillTimer > CatchAttackStartShootAt - 0.3f && ChargeParticles.isPlaying)
				{
					ChargeParticles.Stop();
				}
				if (skillTimer < CatchAttackStartShootAt || shootCounter >= CatchShootCount)
				{
					break;
				}
				SEMgr.Inst.elite55CatchAttack.PlaySE();
				for (int n = 1; n < 6; n++)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite55_CatchBurst", base.transform.position + CatchBaseDirList[n] * (CatchBaseRange + (float)(CatchShootCount - 1) / 2f * CatchRangeUpPerShoot), 1.5f);
				}
				shootCounter = CatchShootCount;
				for (int num2 = 0; num2 < CatchShootCount; num2++)
				{
					for (int num3 = 1; num3 < 5; num3++)
					{
						Vector3 vector4 = CatchBaseDirList[num3];
						Vector3 vector5 = CatchBaseDirList[num3 + 1];
						Vector3 vector6 = vector5 - vector4;
						float num4 = Tool2D.IgnoreZDistance(vector4, vector5) / (float)(CatchOneSideBulletCount - 1);
						for (int num5 = 0; num5 < CatchOneSideBulletCount - 1; num5++)
						{
							Vector3 v = vector4 + vector6 * num4 * num5;
							float num6 = Mathf.Pow(Tool2D.IgnoreZDistance(Vector3.zero, v), HexSpeedRatioPower);
							usm = UnitBase.GetSSPModifier(in ssp);
							v = v.normalized;
							usm.SpawnPosition = base.transform.position + v * num6 * (CatchBaseRange + CatchRangeUpPerShoot * (float)num2) + new Vector3(0f, 0f, -0.5f);
							usm.Direction = (CatchBaseDirList[1] - CatchBaseDirList[2]).normalized;
							usm.Speed = CatchBulletSpeed;
							usm.Damage = CatchBulletDamage;
							usm.Duration = CatchBulletDuration;
							usm.ApplyToSSP(ref ssp);
							UnitDotsSyncSystem.ShootSpell(ssp);
						}
					}
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		}
	}

	private void FaceToPlayer()
	{
		if (base.HaveTarget)
		{
			isFaceRight = base.TargetPoint.x >= base.transform.position.x;
		}
	}

	private void UpdateFaceDirection(bool instantLerp = false)
	{
		float num = (isFaceRight ? Mathf.Abs(modelScaleX) : (0f - Mathf.Abs(modelScaleX)));
		if (instantLerp)
		{
			num = Mathf.Lerp(base.transform.localScale.x, num, 10f * Time.deltaTime);
			ModelTransform.localScale = new Vector3(num, ModelTransform.localScale.y, ModelTransform.localScale.z);
		}
		else
		{
			ModelTransform.DOScaleX(num, FaceDirectionChangeDuration);
		}
		TeleportEffectTransform.localScale = new Vector3(isFaceRight ? Mathf.Abs(ModelTransform.localScale.x) : (0f - Mathf.Abs(ModelTransform.localScale.x)), ModelTransform.localScale.y, ModelTransform.localScale.z);
	}

	private unsafe void EnterState(Elite55State newState)
	{
		state = newState;
		switch (state)
		{
		case Elite55State.Move:
			teleportTimer = 0f;
			switch (currentSkill)
			{
			case Elite55Skill.HexAttack:
				if (skillCastCounter >= HexShootWave)
				{
					currentSkill = Elite55Skill.CatchTarget;
					teleportTimer -= FinishHexBonusWaitTime;
					skillCastCounter = 0;
				}
				break;
			case Elite55Skill.CatchTarget:
				if (skillCastCounter >= CatchShootWave)
				{
					currentSkill = Elite55Skill.HexAttack;
					teleportTimer -= FinishCatchBonusWaitTime;
					skillCastCounter = 0;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			break;
		case Elite55State.Teleport:
		{
			teleportTimer = 0f;
			teleportPosition = Tool2D.GetNavMeshPointIngoreZ(base.TargetPoint + UnityEngine.Random.insideUnitSphere.IgnoreZ().normalized * UnityEngine.Random.Range(TeleportRange.x, TeleportRange.y)) + new Vector3(0f, 1.1f, 0f);
			WarningObjSprite.material.SetFloat(MixPercent, 0f);
			BodySprite.material.SetFloat(MixPercent, 0f);
			WarningObjTransform.gameObject.SetActive(value: true);
			WarningObjTransform.position = teleportPosition + new Vector3(0f, 0.65f, 0f);
			WarningObjSprite.material.DOFloat(1f, MixPercent, TeleportWaitTime - 0.1f);
			BodySprite.material.DOFloat(1f, MixPercent, TeleportWaitTime - 0.1f);
			SEMgr.Inst.monster312_Teleport.PlaySE();
			myPpt.CanTouch = false;
			myPpt.CC_Self.enabled = false;
			SetCanTouch(canTouch: false);
			PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
			componentData2.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.RaiseTriggerEvents);
			SetComponentData(componentData2);
			break;
		}
		case Elite55State.TeleportEndCastSkill:
		{
			shootCounter = 0;
			shootTimer = 0f;
			skillTimer = 0f;
			isSkillInitialize = false;
			CatchWarningLaserList.Clear();
			CatchForceParticleList.Clear();
			myPpt.CanTouch = true;
			myPpt.CC_Self.enabled = true;
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite55_TeleportEnd", base.transform.position, 2f);
			base.transform.position = Tool2D.GetNavMeshPointIngoreZ(teleportPosition);
			SyncDotsPosition();
			WarningObjTransform.gameObject.SetActive(value: false);
			WarningObjSprite.material.DOFloat(0f, MixPercent, 0.1f);
			BodySprite.material.DOFloat(0f, MixPercent, 0.1f);
			SetCanTouch(canTouch: true);
			PhysicsCollider componentData = GetComponentData<PhysicsCollider>();
			componentData.ColliderPtr->SetCollisionResponse(CollisionResponsePolicy.Collide);
			SetComponentData(componentData);
			switch (currentSkill)
			{
			case Elite55Skill.HexAttack:
				ChargeParticles.Play();
				SEMgr.Inst.elite55Charge.PlaySE();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Elite55Skill.CatchTarget:
				break;
			}
			break;
		}
		case Elite55State.BornIdle:
			break;
		}
	}

	private void SetCanTouch(bool canTouch)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = canTouch;
		SetComponentData(componentData);
	}
}
