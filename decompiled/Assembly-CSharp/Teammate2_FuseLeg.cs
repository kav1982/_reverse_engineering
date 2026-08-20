using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SpriteEffectSystem;
using Unity.Mathematics;
using UnityEngine;

public class Teammate2_FuseLeg : MonoBehaviour
{
	private enum LegState
	{
		Idle,
		Move,
		Attack,
		EssenceFindTarget,
		EssenceAttack
	}

	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int nodeCount;

	public float moveSpeedRadio;

	public VariableFloat legRadiusRatio;

	public float correctExtraDistanceRatio;

	public float middleHeight;

	[Header("Color")]
	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECFire;

	public Material mat_ECVoid;

	[Header("EssenceLegColor")]
	public Material mat_EsECFrozen;

	public Material mat_EsECMucus;

	public Material mat_EsECPlayer;

	public Material mat_EsECVenom;

	public Material mat_EsECFire;

	public Material mat_EsECVoid;

	[Header("Safe Mode")]
	public Material mat_ECFrozenSafe;

	public Material mat_ECMucusSafe;

	public Material mat_ECPlayerSafe;

	public Material mat_ECVenomSafe;

	public Material mat_ECFireSafe;

	public Material mat_ECVoidSafe;

	public float legBaseWidth;

	public SpriteEffectAnima[] dropBloodSpriteAnimas = Array.Empty<SpriteEffectAnima>();

	private LegState state;

	private Teammate2FuseController teammate2;

	private UnitProperty ownerPpt;

	private Vector3 dir;

	private Vector3 currentEndPoint;

	private Vector3 moveToEndPoint;

	private Vector3 moveBeforeEndPoint;

	private UnitProperty targetPpt;

	private float attackIntervalTimer;

	private float legsfloatanlge;

	private float legsFloatTimer;

	private Vector3 legInitialDir = Vector3.zero;

	private Vector3 currentFloatDir = Vector3.zero;

	private float legsFloatingSpeed = 2f;

	private float legsFloatLengthRatio;

	private float legSinTimer;

	private float instanceLerpRatio = 1f;

	private float randomEssenceLegValue;

	private float essenceLegLerpvalue;

	private bool essenceAttacking;

	private List<Collider> attackedCollider = new List<Collider>();

	public Transform essenceLegBladeParentTransform;

	public Transform essenceLegColorfulBladeTransform;

	public Transform essenceLegShadowBladeTransform;

	public GameObject essenceLegBladePlayer;

	public GameObject essenceLegBladeFrozen;

	public GameObject essenceLegBladeFire;

	public GameObject essenceLegBladeMucus;

	public GameObject essenceLegBladeVenom;

	public GameObject essenceLegBladeVoid;

	public GameObject essenceLegSafeBladePlayer;

	public GameObject essenceLegSafeBladeFrozen;

	public GameObject essenceLegSafeBladeFire;

	public GameObject essenceLegSafeBladeMucus;

	public GameObject essenceLegSafeBladeVenom;

	public GameObject essenceLegSafeBladeVoid;

	private bool stabBack;

	private int stabMiddlePointAngle;

	private float stabMiddlePointDistance;

	private float stabEndPointDistance;

	private float overStabDistance;

	private Vector3 idleLookPoint = Vector3.zero;

	private Vector3 essenceFirstPoint = Vector3.zero;

	private GameObject essenceLegBlade;

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private float LegRadius => ownerPpt.UnitBas.SummonerSpellBase.spellCfg.radius;

	private Vector3 NormalPoint => ownerPpt.transform.position + dir * LegRadius;

	private Vector3 OwnerPoint => ownerPpt.transform.position;

	public int legIndex { get; set; }

	public int legTotalNum { get; set; }

	public bool isEssenceLeg { get; set; }

	public AnimationCurve essenceLegAttackLerpCurve { get; set; }

	public float essenceAttackDuration { get; set; }

	public float essenceAttackTimer { get; set; }

	public float essenceLegDamageRatio { get; set; }

	public bool haveTarget => targetPpt != null;

	public int headIndex { get; set; }

	private void OnEnable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Combine(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	private void OnDisable()
	{
		EventMgr.SafeModeStateChange = (Action)Delegate.Remove(EventMgr.SafeModeStateChange, new Action(SetSafeMode));
	}

	public void SetSafeMode()
	{
		if (DataMgr.settingData.SafeMode)
		{
			lr_Leg.enabled = false;
			lr_Shadow.enabled = false;
			switch (teammate2.SummonerSpellBase.ColorType)
			{
			case SpellColorType.Frozen:
				lr_Leg.material = mat_ECFrozenSafe;
				break;
			case SpellColorType.Mucus:
				lr_Leg.material = mat_ECMucusSafe;
				break;
			case SpellColorType.Fire:
				lr_Leg.material = mat_ECFireSafe;
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				lr_Leg.material = mat_ECPlayerSafe;
				break;
			case SpellColorType.Venom:
				lr_Leg.material = mat_ECVenomSafe;
				break;
			case SpellColorType.Void:
				lr_Leg.material = mat_ECVoidSafe;
				break;
			default:
				Debug.LogError(teammate2.SummonerSpellBase.ColorType);
				if (lr_Leg.material != mat_ECPlayer)
				{
					lr_Leg.material = mat_ECPlayer;
				}
				break;
			}
		}
		else
		{
			lr_Leg.enabled = true;
			lr_Shadow.enabled = true;
			switch (teammate2.SummonerSpellBase.ColorType)
			{
			case SpellColorType.Frozen:
				lr_Leg.material = (isEssenceLeg ? mat_EsECFrozen : mat_ECFrozen);
				break;
			case SpellColorType.Mucus:
				lr_Leg.material = (isEssenceLeg ? mat_EsECMucus : mat_ECMucus);
				break;
			case SpellColorType.Fire:
				lr_Leg.material = (isEssenceLeg ? mat_EsECFire : mat_ECFire);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				lr_Leg.material = (isEssenceLeg ? mat_EsECPlayer : mat_ECPlayer);
				break;
			case SpellColorType.Venom:
				lr_Leg.material = (isEssenceLeg ? mat_EsECVenom : mat_ECVenom);
				break;
			case SpellColorType.Void:
				lr_Leg.material = (isEssenceLeg ? mat_EsECVoid : mat_ECVoid);
				break;
			default:
				Debug.LogError(teammate2.SummonerSpellBase.ColorType);
				if (lr_Leg.material != mat_ECPlayer)
				{
					lr_Leg.material = mat_ECPlayer;
				}
				break;
			}
		}
		SpawnEssenceLeg();
	}

	public void HideLegs()
	{
		lr_Leg.enabled = false;
		lr_Shadow.enabled = false;
		essenceLegBladeParentTransform.gameObject.SetActive(value: false);
	}

	public void ShowLegs()
	{
		lr_Leg.enabled = true;
		lr_Shadow.enabled = true;
		essenceLegBladeParentTransform.gameObject.SetActive(isEssenceLeg);
	}

	public void EssencelegSetFuseState()
	{
		if (!essenceLegBlade)
		{
			return;
		}
		Material material = essenceLegBlade.transform.Find("Blade").GetComponent<SpriteRenderer>().material;
		material.SetInt(UseFuseShineEffect, 1);
		material.DOFloat(1f, FuseShineProcess, 1.3f);
		foreach (Transform item in essenceLegBlade.transform)
		{
			ParticleSystem component = item.GetComponent<ParticleSystem>();
			if ((bool)component)
			{
				component.Stop();
			}
		}
	}

	private void Update()
	{
		attackIntervalTimer += Time.deltaTime * teammate2.SummonerSpellBase.GetSummonValueRatio().attackSpeedRatio;
		switch (state)
		{
		case LegState.Idle:
			if ((NormalPoint - currentEndPoint).sqrMagnitude > LegRadius * (legRadiusRatio.result + correctExtraDistanceRatio) * LegRadius * (legRadiusRatio.result + correctExtraDistanceRatio))
			{
				state = LegState.Move;
				if (teammate2.CurrentMotion.sqrMagnitude > teammate2.myPpt.Rigid.linearVelocity.sqrMagnitude)
				{
					moveToEndPoint = NormalPoint + teammate2.CurrentMotion.normalized * LegRadius * legRadiusRatio.RandomResult();
				}
				else
				{
					moveToEndPoint = NormalPoint + teammate2.myPpt.Rigid.linearVelocity.normalized * LegRadius * legRadiusRatio.RandomResult();
				}
				if (Physics.Raycast(OwnerPoint, moveToEndPoint - OwnerPoint, out var hitInfo, 100f, LayerMask.GetMask("Wall", "Abyss", "Cliff")) && (hitInfo.point - OwnerPoint).sqrMagnitude < (moveToEndPoint - OwnerPoint).sqrMagnitude)
				{
					moveToEndPoint = Tool2D.IgnoreZPoint(hitInfo.point);
				}
				moveBeforeEndPoint = currentEndPoint;
			}
			break;
		case LegState.Move:
		{
			float num2 = MathF.Abs(teammate2.myPpt.MoveSpeed);
			if (teammate2.myPpt.Rigid.linearVelocity.sqrMagnitude > num2 * num2)
			{
				num2 = teammate2.myPpt.Rigid.linearVelocity.magnitude;
			}
			currentEndPoint = Vector3.MoveTowards(currentEndPoint, moveToEndPoint, num2 * moveSpeedRadio * Time.deltaTime);
			if (currentEndPoint == moveToEndPoint)
			{
				state = LegState.Idle;
			}
			break;
		}
		case LegState.Attack:
		{
			if (targetPpt == null || teammate2.beingControlledByTeammate6)
			{
				state = LegState.Idle;
				break;
			}
			float num = MathF.Abs(teammate2.myPpt.MoveSpeed);
			currentEndPoint = Vector3.MoveTowards(currentEndPoint, targetPpt.transform.position, num * moveSpeedRadio * Time.deltaTime);
			if (currentEndPoint == targetPpt.transform.position)
			{
				lr_Leg.material.SetInt("_IsSuck", 1);
				if (attackIntervalTimer >= teammate2.attackInterval)
				{
					attackIntervalTimer = 0f;
					DealDamageToTarget(isEssenceAttack: false);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Teammate2SuckBlood").GetComponent<Teammate2SuckBlood>().Initialize(lr_Leg);
					if (!GameMgr.IsHarmony_Static)
					{
						SpellSpriteEffectController.Inst.PlayEffectIgnoreSpellBase(dropBloodSpriteAnimas[UnityEngine.Random.Range(0, dropBloodSpriteAnimas.Length)], new EffectPlayParam
						{
							Position = Tool2D.GetLayerPoint(targetPpt.transform.position)
						});
					}
					teammate2.RecoveryOnce();
				}
			}
			else
			{
				lr_Leg.material.SetInt("_IsSuck", 0);
			}
			break;
		}
		case LegState.EssenceAttack:
			if (targetPpt == null)
			{
				state = LegState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case LegState.EssenceFindTarget:
			break;
		}
		if (isEssenceLeg)
		{
			LegEssenceLockingTarget();
		}
		else
		{
			LegNormalMovement();
		}
		UpdateEssenceAttackState();
	}

	private void DealDamageToTarget(bool isEssenceAttack, Collider targetCollider = null, int realDamage = 0)
	{
		UnitProperty unitProperty = (targetCollider ? targetCollider.GetComponent<UnitProperty>() : null);
		UnitProperty unitProperty2 = ((unitProperty == null) ? targetPpt : unitProperty);
		TakeDamageInfo info = new TakeDamageInfo
		{
			criticalChance = ownerPpt.UnitBas.SummonerSpellBase.GetCriticalChance(),
			wandChargeData = ownerPpt.UnitBas.SummonerSpellBase.wandChargeData
		};
		float damage = ((realDamage > 0) ? ((float)realDamage) : GetAttackDamage());
		teammate2.SummonerSpellBase.ApplyVoidEffect(targetPpt);
		unitProperty2.TakeDamage(damage, ownerPpt, info);
		teammate2.SummonerSpellBase.ApplyElementEffect(unitProperty2);
		teammate2.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(unitProperty2.transform.position);
		teammate2.SummonerSpellBase.CheckIfPullCrystalIsValidToAttack(info, unitProperty2);
	}

	private float GetAttackDamage()
	{
		return teammate2.SummonerSpellBase.spellCfg.damage;
	}

	private void LegNormalMovement()
	{
		Vector3 vector = currentEndPoint;
		if (state == LegState.Move)
		{
			vector = GeneralTool.QuadraticBezierCurve(moveBeforeEndPoint, (moveBeforeEndPoint + moveToEndPoint) / 2f + new Vector3(0f, 0f, -1f), moveToEndPoint, Vector3.Distance(moveBeforeEndPoint, currentEndPoint) / Vector3.Distance(moveBeforeEndPoint, moveToEndPoint));
		}
		Vector3 v = vector + new Vector3(0f, 0f, 0f - middleHeight);
		float z = (0f - teammate2.tsf_Motion.localPosition.y) * teammate2.myPpt.tsf_Layer.localScale.x - (float)headIndex * teammate2.bodyDistance * teammate2.myPpt.tsf_Layer.localScale.x * teammate2.bodyTransform.localScale.x;
		Vector3 vector2 = OwnerPoint + new Vector3(0f, 0f, z);
		for (int i = 0; i < nodeCount; i++)
		{
			lr_Leg.SetPosition(i, Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(vector2, v, vector, (float)i / ((float)nodeCount - 1f))));
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(vector2, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(vector, 1.05f));
	}

	public void LegEssenceLockingTarget(bool isInstanceMove = false)
	{
		int num = ((legIndex % 2 == 0) ? 1 : (-1));
		Vector3 vector = ((targetPpt == null) ? (teammate2.transform.position + legInitialDir * stabMiddlePointDistance) : currentEndPoint);
		float t = essenceLegLerpvalue;
		float t2 = (isInstanceMove ? 1f : 0.2f);
		idleLookPoint = Vector3.Lerp(idleLookPoint, currentEndPoint + teammate2.SummonerSpellBase.Direction * 5f, t2);
		vector = idleLookPoint;
		Vector3 vector2 = (targetPpt ? targetPpt.transform.position : vector);
		Vector3 vector3 = Tool2D.IgnoreZPoint(teammate2.headCenterTransform.position);
		float y = (float)headIndex * teammate2.bodyDistance * teammate2.myPpt.tsf_Layer.localScale.x * teammate2.bodyTransform.localScale.x;
		vector3 += new Vector3(0f, y, 0f);
		float num2 = Mathf.Sin(legsFloatTimer * 240f * (MathF.PI / 180f)) * 4f;
		if (num2 <= 0f)
		{
			num2 *= 0.5f;
		}
		Vector3 vector4 = vector3 + Tool2D.GetDir((vector2 - vector3).normalized, ((float)stabMiddlePointAngle + num2) * (float)num) * stabMiddlePointDistance;
		Vector3 vector5 = vector4 + Tool2D.GetDir((vector2 - vector4).normalized, 0f - num2) * stabEndPointDistance;
		Vector3 b = vector4 + (vector5 - vector4).normalized * (Vector3.Distance(vector2, vector4) + overStabDistance - stabEndPointDistance);
		Vector3 b2 = vector2 + (vector2 - vector4).normalized * overStabDistance;
		Vector3 v = Vector3.Lerp(vector4, b, t);
		Vector3 v2 = Vector3.Lerp(vector5, b2, t);
		legsFloatTimer += Time.deltaTime;
		legSinTimer += Time.deltaTime;
		for (int i = 0; i < nodeCount; i++)
		{
			float num3 = (isInstanceMove ? 1f : (15f * Time.deltaTime * (1f - (float)(i / nodeCount) / 2f)));
			Vector3 position = Vector3.Lerp(lr_Leg.GetPosition(i), Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(vector3, v, v2, (float)i / ((float)nodeCount - 1f))), num3 * instanceLerpRatio);
			lr_Leg.SetPosition(i, position);
			essenceFirstPoint = position;
		}
		essenceLegBladeParentTransform.transform.position = Tool2D.IgnoreZPoint(lr_Leg.GetPosition(lr_Leg.positionCount - 1));
		essenceLegBladeParentTransform.transform.right = Tool2D.IgnoreZPoint(lr_Leg.GetPosition(lr_Leg.positionCount - 1) - lr_Leg.GetPosition(lr_Leg.positionCount - 2));
		float y2 = (teammate2.headCenterTransform.position - teammate2.transform.position).y;
		for (int j = 0; j < nodeCount; j++)
		{
			lr_Shadow.SetPosition(j, Tool2D.IgnoreZPoint(GeneralTool.QuadraticBezierCurve(vector3, v, v2, (float)j / ((float)nodeCount - 1f)), 1.05f) + new Vector3(0f, 0f - y2, 0.3f));
		}
		essenceLegShadowBladeTransform.position = Tool2D.IgnoreZPoint(essenceLegBladeParentTransform.position + new Vector3(0f, 0f - y2, 0f), 1.05f);
	}

	public void LegEssenceAttackStart(UnitProperty targetPpt)
	{
		if (!essenceAttacking && !(targetPpt == null))
		{
			SEMgr.Inst.teammate2Stab.PlaySE();
			essenceAttacking = true;
			state = LegState.EssenceFindTarget;
			essenceAttackTimer = 0f;
			stabBack = false;
			if (essenceAttackDuration < 0.25f)
			{
				essenceAttackDuration = 0.25f;
			}
		}
	}

	public void ResetEssenceLegAttackData()
	{
		stabBack = true;
		SetRandomLegvalue();
		float maxInclusive = Mathf.Sqrt(teammate2.SummonerSpellBase.radiusRatio * teammate2.SummonerSpellBase.finalRadiusRatio);
		stabMiddlePointAngle = UnityEngine.Random.Range(105, 180);
		stabMiddlePointDistance = UnityEngine.Random.Range(4.5f, 3.5f) * UnityEngine.Random.Range(1f, maxInclusive);
		stabEndPointDistance = UnityEngine.Random.Range(3f, 2f) * UnityEngine.Random.Range(1f, maxInclusive);
		overStabDistance = UnityEngine.Random.Range(3f, 4.4f) * UnityEngine.Random.Range(1f, maxInclusive);
	}

	private void UpdateEssenceAttackState()
	{
		if (essenceAttacking)
		{
			EssenceAttackDealDamageNearBy();
			essenceAttackTimer += Time.deltaTime;
			float num = essenceAttackTimer / 0.25f;
			essenceLegLerpvalue = essenceLegAttackLerpCurve.Evaluate(Mathf.Clamp(num, 0f, 1f));
			if (num >= 0.5f && !stabBack)
			{
				ResetEssenceLegAttackData();
			}
			if (essenceAttackTimer / essenceAttackDuration >= 1f)
			{
				essenceAttacking = false;
				essenceAttackTimer -= essenceAttackDuration;
				attackedCollider.Clear();
			}
		}
	}

	private void EssenceAttackDealDamageNearBy()
	{
		if (teammate2.beingControlledByTeammate6)
		{
			return;
		}
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(Tool2D.IgnoreZPoint(lr_Leg.GetPosition(nodeCount - 1)), 0.5f, "Monster", "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness");
		int num = Mathf.CeilToInt((SpellConfig.dic[teammate2.SummonerSpellBase.spellCfg.id].damage + teammate2.GetDamageUpEffectValue()) * teammate2.SummonerSpellBase.damageRatio * teammate2.SummonerSpellBase.finalDamageRatio * essenceLegDamageRatio + teammate2.SummonerSpellBase.SIP.finalDamageExtra);
		foreach (Collider item in collidersByTag.Where((Collider e) => e.gameObject.activeInHierarchy))
		{
			if (item.gameObject.CompareAnyTag("Spell", "RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1002RollBall spell1002RollBall))
				{
					if (componentInParent is Spell1003Butterfly spell1003Butterfly)
					{
						spell1003Butterfly.HitEFAndRecycle();
					}
				}
				else
				{
					spell1002RollBall.TakeDamage(num);
				}
			}
			else if (!attackedCollider.Contains(item) && item.gameObject.CompareAnyTag("Monster"))
			{
				DealDamageToTarget(isEssenceAttack: true, item, num);
				attackedCollider.Add(item);
				if (DataMgr.settingData.SafeMode)
				{
					SEMgr.Inst.teammate2SafeStabHit.PlaySE();
				}
				else
				{
					SEMgr.Inst.teammate2StabHit.PlaySE();
				}
				teammate2.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(item.transform.position);
				SpawnEssenceHitEffect(item.transform.position - Tool2D.IgnoreZPoint(lr_Leg.GetPosition(nodeCount - 1)));
			}
			else if (!attackedCollider.Contains(item))
			{
				DealDamageToTarget(isEssenceAttack: true, item, num);
			}
		}
	}

	private void SpawnEssenceHitEffect(Vector3 direction)
	{
		string text = "Hit_";
		if (DataMgr.settingData.SafeMode)
		{
			text += "Safe";
		}
		teammate2.SummonerSpellBase.GetEffect(text + teammate2.SummonerSpellBase.ColorType, Tool2D.IgnoreZPoint(targetPpt.transform) + new Vector3(0f, 0.2f, 0f), 0.8f).transform.right = direction;
	}

	private void SetRandomLegvalue()
	{
		randomEssenceLegValue = UnityEngine.Random.Range(1f, 2f);
	}

	public void Initialize(Teammate2FuseController teammate2, Vector3 originalDir, bool isEssenceLeg = false)
	{
		CancelTarget();
		this.teammate2 = teammate2;
		dir = originalDir;
		ownerPpt = teammate2.GetComponent<UnitProperty>();
		Reposition();
		this.isEssenceLeg = isEssenceLeg;
		ResetEssenceLegAttackData();
		essenceLegLerpvalue = 0f;
		essenceAttackTimer = 0f;
		attackedCollider.Clear();
		essenceLegBladeParentTransform.gameObject.SetActive(isEssenceLeg);
		stabBack = false;
		idleLookPoint = currentEndPoint + teammate2.SummonerSpellBase.Direction * 5f;
		essenceLegBlade = null;
		essenceAttacking = false;
		essenceAttackTimer = 0f;
		essenceLegLerpvalue = 0f;
		ResetEssenceLegAttackData();
		attackedCollider.Clear();
		lr_Leg.positionCount = nodeCount;
		lr_Shadow.positionCount = (isEssenceLeg ? nodeCount : 2);
		lr_Shadow.gameObject.SetActive(value: true);
		float num = Mathf.Min(ownerPpt.UnitBas.SummonerSpellBase.transform.localScale.x, 3f);
		if (isEssenceLeg)
		{
			num *= 1.5f;
		}
		lr_Leg.widthMultiplier = legBaseWidth;
		lr_Shadow.widthMultiplier = legBaseWidth;
		lr_Leg.widthMultiplier *= num;
		lr_Shadow.widthMultiplier *= num;
		essenceLegBladeParentTransform.localScale = Vector3.one * num * 0.9f;
		legsfloatanlge = UnityEngine.Random.Range(20, 30) * 2;
		legsFloatTimer = UnityEngine.Random.Range(0, 10);
		legInitialDir = dir;
		currentFloatDir = dir;
		legsFloatingSpeed = UnityEngine.Random.Range(1.5f, 3f);
		legsFloatLengthRatio = UnityEngine.Random.Range(1f, 1.5f);
		SpawnEssenceLeg();
		switch (teammate2.SummonerSpellBase.ColorType)
		{
		case SpellColorType.Frozen:
			if (lr_Leg.material != mat_ECFrozen)
			{
				lr_Leg.material = mat_ECFrozen;
			}
			break;
		case SpellColorType.Mucus:
			if (lr_Leg.material != mat_ECMucus)
			{
				lr_Leg.material = mat_ECMucus;
			}
			break;
		case SpellColorType.Fire:
			if (lr_Leg.material != mat_ECFire)
			{
				lr_Leg.material = mat_ECFire;
			}
			break;
		case SpellColorType.Player:
		case SpellColorType.Thunder:
			if (lr_Leg.material != mat_ECPlayer)
			{
				lr_Leg.material = mat_ECPlayer;
			}
			break;
		case SpellColorType.Venom:
			if (lr_Leg.material != mat_ECVenom)
			{
				lr_Leg.material = mat_ECVenom;
			}
			break;
		case SpellColorType.Void:
			if (lr_Leg.material != mat_ECVoid)
			{
				lr_Leg.material = mat_ECVoid;
			}
			break;
		default:
			Debug.LogError(teammate2.SummonerSpellBase.ColorType);
			if (lr_Leg.material != mat_ECPlayer)
			{
				lr_Leg.material = mat_ECPlayer;
			}
			break;
		}
		instanceLerpRatio = 100f;
		if (this.teammate2.floatingBationMode && isEssenceLeg)
		{
			LegEssenceLockingTarget();
		}
		instanceLerpRatio = 1f;
		SetSafeMode();
	}

	private void SpawnEssenceLeg()
	{
		essenceLegColorfulBladeTransform.DestroyAllChild();
		if (isEssenceLeg)
		{
			GameObject original = null;
			switch (teammate2.SummonerSpellBase.ColorType)
			{
			case SpellColorType.Frozen:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladeFrozen : essenceLegBladeFrozen);
				break;
			case SpellColorType.Mucus:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladeMucus : essenceLegBladeMucus);
				break;
			case SpellColorType.Player:
			case SpellColorType.Thunder:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladePlayer : essenceLegBladePlayer);
				break;
			case SpellColorType.Venom:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladeVenom : essenceLegBladeVenom);
				break;
			case SpellColorType.Void:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladeVoid : essenceLegBladeVoid);
				break;
			case SpellColorType.Fire:
				original = (DataMgr.settingData.SafeMode ? essenceLegSafeBladeFire : essenceLegBladeFire);
				break;
			}
			essenceLegBlade = UnityEngine.Object.Instantiate(original, Vector3.zero, quaternion.identity, essenceLegColorfulBladeTransform);
			essenceLegBlade.transform.localRotation = quaternion.identity;
			essenceLegBlade.transform.localPosition = Vector3.zero;
		}
	}

	public void SetTarget(UnitProperty targetPpt)
	{
		state = LegState.Attack;
		this.targetPpt = targetPpt;
	}

	public void SetEssenceTarget(UnitProperty targetPpt)
	{
		state = LegState.EssenceFindTarget;
		this.targetPpt = targetPpt;
	}

	public void CancelTarget()
	{
		state = LegState.Idle;
		lr_Leg.material.SetInt("_IsSuck", 0);
		targetPpt = null;
	}

	public void Reposition()
	{
		moveToEndPoint = OwnerPoint + dir + Tool2D.GetDir() * LegRadius * legRadiusRatio.RandomResult();
		currentEndPoint = moveToEndPoint;
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
		moveToEndPoint += changeValue;
		currentEndPoint += changeValue;
	}
}
