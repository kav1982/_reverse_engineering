using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite14_Shadow : MonoBehaviour, IComparable<Elite14_Shadow>
{
	public enum ShadowState
	{
		Show,
		Dash,
		Fade
	}

	[Header("表现")]
	public Transform tsf_Layer;

	public Animator Anima;

	public AnimaEvent animaEvent;

	public SpriteRenderer sr;

	public LineRenderer lr_warning;

	public LineRenderer lr_Damage;

	public float widthFixer;

	public AnimationCurve warningWidthCurve;

	public AnimationCurve attackWidthCurve;

	public AnimationCurve shadowTransparencyCurve;

	public float shadowExistTime;

	public ParticleSystem slashParticle;

	public ParticleSystem trailParticle;

	public ShockParam shockParam;

	public Color damageColor;

	public float damageTransparency;

	public float warningTransparency;

	private int TransparencyId;

	private int ColorId;

	private Vector3 startPoint;

	private Vector3 endPoint;

	private Vector3 centerPoint;

	private float startAngle;

	[Header("数值")]
	public Vector3 direction;

	public float damage;

	public float knockBack;

	public float damageLength;

	[Header("判定")]
	public LayerMask attackLayers;

	public float damageWidth;

	public Collider trigger;

	public List<Entity> dashedEntities = new List<Entity>();

	[Header("状态机")]
	public ShadowState _state;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private bool firstInitialize;

	private UnitDotsSyncSystem.RayCastHitResult[] hits;

	public ShadowState state
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

	public void Initialize(Vector3 direction, Vector3 centerPoint)
	{
		this.direction = direction.normalized;
		startPoint = centerPoint - this.direction * damageLength / 2f;
		endPoint = centerPoint + this.direction * damageLength / 2f;
		this.centerPoint = centerPoint;
		lr_warning.positionCount = 10;
		lr_Damage.positionCount = 10;
		for (int i = 0; i < 10; i++)
		{
			Vector3 rootPoint = Vector3.Lerp(startPoint, endPoint, (float)i / 10f);
			lr_warning.SetPosition(i, Tool2D.GetLayerPoint(rootPoint, LayerCorrectType.GroundEffectLow));
			lr_Damage.SetPosition(i, Tool2D.GetLayerPoint(rootPoint) + new Vector3(0f, 0f, -0.03f));
		}
		lr_warning.enabled = false;
		lr_Damage.enabled = false;
		if (!firstInitialize)
		{
			animaEvent.DoAction = AnimaAction;
			TransparencyId = Shader.PropertyToID("_Transparency");
			ColorId = Shader.PropertyToID("_GlowColor");
			firstInitialize = true;
		}
		animaEvent.DoAction = AnimaAction;
		sr.enabled = false;
		sr.flipX = direction.x < 0f;
		state = ShadowState.Show;
		base.transform.position = startPoint;
		trailParticle.Stop();
		trailParticle.Clear();
		dashedEntities.Clear();
	}

	public int CompareTo(Elite14_Shadow other)
	{
		float num = Tool2D.IgnoreZAngleWithSign(Vector3.up, direction);
		if (num < 0f)
		{
			num += 360f;
		}
		float num2 = Tool2D.IgnoreZAngleWithSign(Vector3.up, other.direction);
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

	private void Update()
	{
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
		case ShadowState.Show:
		{
			ref float reference = ref varMgr.RegFloat(0);
			if (changedState)
			{
				SEMgr.Inst.elite14ShadowAttackShow.PlaySE();
				Anima.Play("Show");
				reference = Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				lr_warning.enabled = true;
				lr_Damage.enabled = false;
			}
			lr_warning.widthMultiplier = warningWidthCurve.Evaluate(stateExistTime / reference) * widthFixer;
			if (stateExistTime > reference)
			{
				state = ShadowState.Dash;
			}
			break;
		}
		case ShadowState.Dash:
		{
			ref float reference2 = ref varMgr.RegFloat(0);
			if (changedState)
			{
				slashParticle.transform.position = Tool2D.GetLayerPoint(centerPoint);
				slashParticle.transform.localEulerAngles = Vector3.forward * (Tool2D.IgnoreZAngleWithSign(Vector3.up, direction) + 90f);
				slashParticle.Play();
				SEMgr.Inst.elite14ShadowAttack.PlaySE().pitch = UnityEngine.Random.Range(0.9f, 1.2f);
				CamController.Inst.SetShock(shockParam);
				Anima.Play("Dash");
				reference2 = Anima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
				lr_warning.enabled = false;
				lr_Damage.enabled = true;
				sr.enabled = true;
				trailParticle.Play();
			}
			tsf_Layer.position = Tool2D.GetLayerPoint(Vector3.Lerp(startPoint, endPoint, Mathf.Lerp(0.1f, 0.9f, stateExistTime / shadowExistTime)));
			Color color = sr.color;
			color.a = shadowTransparencyCurve.Evaluate(stateExistTime / shadowExistTime);
			sr.color = color;
			lr_Damage.widthMultiplier = attackWidthCurve.Evaluate(stateExistTime / reference2) * widthFixer;
			if (stateExistTime > reference2)
			{
				trailParticle.Stop();
				Elite14.MiniPool.RecycleGO(base.gameObject);
			}
			break;
		}
		case ShadowState.Fade:
			if (changedState)
			{
				lr_warning.enabled = false;
				lr_Damage.enabled = false;
				Anima.Play("Hide");
			}
			break;
		}
		lr_warning.material.SetFloat(TransparencyId, warningTransparency);
		lr_Damage.material.SetFloat(TransparencyId, damageTransparency);
		lr_Damage.material.SetColor(ColorId, damageColor);
	}

	private void DealDamage()
	{
		Debug.DrawLine(base.transform.position - damageLength * direction * 0.5f, base.transform.position + damageLength * direction * 0.5f, Color.white, 0.5f);
		hits = UnitDotsSyncSystem.SphereCastAll(base.transform.position - (damageLength - 1f) * direction * 0.5f, Tool2D.IgnoreZPoint(direction), damageWidth / 2f, damageLength - 1f, GameConst.Filter_MonsterAoe);
		for (int i = 0; i < hits.Length; i++)
		{
			Entity entity = hits[i].entity;
			if (dashedEntities.Contains(entity))
			{
				continue;
			}
			dashedEntities.Add(entity);
			_ = ref hits[i];
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite14.Inst.myPpt.myEntity);
				info.damage = damage;
				info.teammateTakeDamageRatio = 4f;
				if (UnitDotsSyncSystem.GetComponentData<UnitProperty_Dots>(entity).unitCfg.unitType != UnitType.Brittleness)
				{
					SEMgr.Inst.elite14BladeHit.PlaySE();
					Elite14.MiniPool.GetGO("Prefabs/EF/EF_Elite14_BladeHit", Tool2D.GetLayerPoint(hits[i].point) + new Vector3(0f, 0f, -0.5f), Quaternion.LookRotation(Tool2D.GetDir(direction, 90f), Vector3.back), 2f);
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
	}

	private void AnimaAction(string action)
	{
		switch (action)
		{
		case "ShowFinish":
			state = ShadowState.Dash;
			break;
		case "Slash":
			DealDamage();
			break;
		case "HideFinish":
			Elite14.MiniPool.RecycleGO(base.gameObject);
			break;
		case "AttackFinish":
			break;
		}
	}
}
