using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite14_DelaySlash : MonoBehaviour
{
	public enum SlashState
	{
		Warning,
		Slash
	}

	[Header("数值")]
	public float knockback;

	public int damage;

	public float damageWidth;

	public float dashDamageLength;

	public float sideDamageLength;

	public LayerMask attackLayers;

	private float damageLength;

	private List<Entity> attackedEnitities = new List<Entity>();

	[Header("表现")]
	public VariableFloat warningSpeed;

	public VariableFloat moveSpeed;

	private bool isSideSlash;

	public VariableFloat sideWarningSpeed;

	public VariableFloat sideMoveSpeed;

	public ParticleSystem attackParticle;

	public AnimationCurve attackWidthCurve;

	public AnimationCurve attackLengthCurve;

	public ShockParam camreraShock;

	public Animator Anima;

	public AnimaEvent animaEvent;

	public LineRenderer lr_Warning;

	public LineRenderer lr_Attack;

	public List<Material> Mt_Warning;

	public List<Material> Mt_Attack;

	private Vector3 direction;

	private Vector3 startPoint;

	private Vector3 endPoint;

	public Color damageColor;

	public float damageTransparency;

	public float warningTransparency;

	private int TransparencyId;

	private int ColorId;

	[Header("状态")]
	public SlashState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	private bool firstInitialize;

	private UnitDotsSyncSystem.RayCastHitResult[] hits;

	public SlashState state
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
		}
	}

	public void Initialize(Vector3 direction, bool isSideSlash = false, int mat = 0)
	{
		this.isSideSlash = isSideSlash;
		this.direction = direction.normalized;
		if (isSideSlash)
		{
			damageLength = sideDamageLength;
		}
		else
		{
			damageLength = dashDamageLength;
		}
		startPoint = base.transform.position - direction * damageLength / 2f;
		endPoint = base.transform.position + direction * damageLength / 2f;
		if (!firstInitialize)
		{
			Object.Destroy(lr_Warning.material);
			lr_Warning.material = Mt_Warning[mat];
			Object.Destroy(lr_Attack.material);
			lr_Attack.material = Mt_Attack[mat];
			animaEvent.DoAction = AnimaAction;
			TransparencyId = Shader.PropertyToID("_Transparency");
			ColorId = Shader.PropertyToID("_GlowColor");
			firstInitialize = true;
		}
		Anima.Rebind();
		Anima.enabled = true;
		lr_Warning.enabled = false;
		lr_Attack.enabled = false;
		lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(startPoint, LayerCorrectType.GroundEffectLow));
		lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(endPoint, LayerCorrectType.GroundEffectLow));
		lr_Attack.SetPosition(0, Tool2D.GetLayerPoint(startPoint) + new Vector3(0f, 0f, -0.1f));
		lr_Attack.SetPosition(1, Tool2D.GetLayerPoint(endPoint) + new Vector3(0f, 0f, -0.1f));
		state = SlashState.Warning;
		attackedEnitities.Clear();
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
		case SlashState.Warning:
			if (changedState)
			{
				Anima.speed = warningSpeed.RandomResult();
				if (isSideSlash)
				{
					Anima.speed = sideWarningSpeed.RandomResult();
				}
				Anima.Play("Warning", 0, 0f);
				lr_Warning.enabled = true;
				lr_Attack.enabled = false;
				moveSpeed.RandomResult();
				sideMoveSpeed.RandomResult();
			}
			base.transform.position += Time.deltaTime * direction * (isSideSlash ? sideMoveSpeed.result : moveSpeed.result);
			startPoint = base.transform.position - direction * damageLength / 2f;
			endPoint = base.transform.position + direction * damageLength / 2f;
			lr_Warning.SetPosition(0, Tool2D.GetLayerPoint(startPoint, LayerCorrectType.GroundEffectLow));
			lr_Warning.SetPosition(1, Tool2D.GetLayerPoint(endPoint, LayerCorrectType.GroundEffectLow));
			break;
		case SlashState.Slash:
			if (changedState)
			{
				SEMgr.Inst.monster39Hit.PlaySE().pitch = Random.Range(0.9f, 1.1f);
				Anima.speed = 1f;
				lr_Warning.enabled = false;
				lr_Attack.enabled = true;
				CamController.Inst.SetShock(camreraShock);
				Anima.Play("Slash", 0, 0f);
				attackParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, -3f, -3f));
				attackParticle.transform.right = direction;
				attackParticle.Play();
			}
			lr_Attack.widthMultiplier = attackWidthCurve.Evaluate(stateExistTime);
			startPoint = base.transform.position - direction * damageLength / 2f * attackLengthCurve.Evaluate(stateExistTime);
			endPoint = base.transform.position + direction * damageLength / 2f * attackLengthCurve.Evaluate(stateExistTime);
			lr_Attack.SetPosition(0, Tool2D.GetLayerPoint(startPoint) + new Vector3(0f, 0f, -0.1f));
			lr_Attack.SetPosition(1, Tool2D.GetLayerPoint(endPoint) + new Vector3(0f, 0f, -0.1f));
			break;
		}
		lr_Warning.material.SetFloat(TransparencyId, warningTransparency);
		lr_Attack.material.SetFloat(TransparencyId, damageTransparency);
		lr_Attack.material.SetColor(ColorId, damageColor);
	}

	private void DealDamage()
	{
		Debug.DrawLine(base.transform.position - damageLength * direction * 0.5f, base.transform.position + damageLength * direction * 0.5f, Color.white, 0.5f);
		hits = UnitDotsSyncSystem.SphereCastAll(base.transform.position - (damageLength - 1f) * direction * 0.5f, Tool2D.IgnoreZPoint(direction), damageWidth / 2f, damageLength - 1f, GameConst.Filter_MonsterAoe);
		for (int i = 0; i < hits.Length; i++)
		{
			if (attackedEnitities.Contains(hits[i].entity))
			{
				continue;
			}
			attackedEnitities.Add(hits[i].entity);
			Entity entity = hits[i].entity;
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
		case "WarningFinish":
			state = SlashState.Slash;
			break;
		case "Slash":
			DealDamage();
			break;
		case "SlashFinish":
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			break;
		}
	}
}
