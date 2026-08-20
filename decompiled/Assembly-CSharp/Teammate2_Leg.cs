using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Teammate2_Leg : MonoBehaviour
{
	public LineRenderer lr_Leg;

	public LineRenderer lr_Shadow;

	public int nodeCount;

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

	private LegsData legsData;

	private EssenceLegsData essenceLegsData;

	private const float FuseHeadLegRootZ = 1.05f;

	public Teammate2Show Owner;

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

	private GameObject essenceLegBlade;

	private GameObject sparkEffect;

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public int legIndex { get; set; }

	public bool isEssenceLeg { get; set; }

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
			switch (Owner.colorType)
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
				Debug.LogError(Owner.colorType);
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
			switch (Owner.colorType)
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
				Debug.LogError(Owner.colorType);
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
		essenceLegBladeParentTransform.gameObject.SetActive(value: false);
		Material material = essenceLegBlade.transform.Find("Blade").GetComponent<SpriteRenderer>().material;
		material.SetInt(UseFuseShineEffect, 1);
		if (material.HasFloat(FuseShineProcess))
		{
			material.DOFloat(1f, FuseShineProcess, 1.3f);
		}
		foreach (Transform item in essenceLegBlade.transform)
		{
			ParticleSystem component = item.GetComponent<ParticleSystem>();
			if ((bool)component)
			{
				component.Stop();
			}
		}
	}

	public void HideOrShow(bool isShow)
	{
		if (!essenceLegBlade)
		{
			return;
		}
		essenceLegBladeParentTransform.gameObject.SetActive(isShow);
		essenceLegBlade.transform.Find("Blade").GetComponent<SpriteRenderer>().material.SetInt(Transparency, isShow ? 1 : 0);
		foreach (Transform item in essenceLegBlade.transform)
		{
			ParticleSystem component = item.GetComponent<ParticleSystem>();
			if ((bool)component)
			{
				if (isShow)
				{
					component.Play();
				}
				else
				{
					component.Stop();
				}
			}
		}
	}

	public void Update()
	{
		if (isEssenceLeg)
		{
			LegEssenceLockingTarget();
		}
		else
		{
			LegNormalMovement();
		}
		lr_Leg.material.SetInt("_IsSuck", (legsData.LegState == LegState.Attack) ? 1 : 0);
	}

	private void LegNormalMovement()
	{
		float3 @float = legsData.MoveToEndPoint;
		if (legsData.LegState == LegState.Move)
		{
			float num = math.distance(legsData.MoveBeforeEndPoint, legsData.MoveToEndPoint);
			@float = GeneralTool.QuadraticBezierCurve(legsData.MoveBeforeEndPoint, (legsData.MoveBeforeEndPoint + legsData.MoveToEndPoint) / 2f + new float3(0f, 0f, -1f), legsData.MoveToEndPoint, (num == 0f) ? 1f : (math.distance(legsData.MoveBeforeEndPoint, legsData.CurrentEndPoint) / num));
		}
		float3 float2 = @float + new float3(0f, 0f, 0f - middleHeight);
		Vector3 v = ((!legsData.IsFuseLeg) ? Owner.mainHeadRootPos : (Owner.mainHeadRootPos + Vector3.back * ((float)(legsData.FuseHeadIndex + 1) * 1.05f * Owner.rootScale * Owner.spellScale)));
		for (int i = 0; i < nodeCount; i++)
		{
			Vector3 layerPoint = Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(v, float2, @float, (float)i / ((float)nodeCount - 1f)));
			lr_Leg.SetPosition(i, layerPoint);
		}
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(Owner.transform.position, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(@float, 1.05f));
	}

	public void LegEssenceLockingTarget()
	{
		for (int i = 0; i < nodeCount; i++)
		{
			Vector3 layerPoint = Tool2D.GetLayerPoint(GeneralTool.QuadraticBezierCurve(essenceLegsData.HeadPos, essenceLegsData.MiddlePoint, essenceLegsData.EndPoint + new float3(0f, 0.5f, 0f), (float)i / ((float)nodeCount - 1f)));
			lr_Leg.SetPosition(i, layerPoint);
		}
		essenceLegBladeParentTransform.transform.position = Tool2D.IgnoreZPoint(lr_Leg.GetPosition(lr_Leg.positionCount - 1));
		essenceLegBladeParentTransform.transform.right = Tool2D.IgnoreZPoint(lr_Leg.GetPosition(lr_Leg.positionCount - 1) - lr_Leg.GetPosition(lr_Leg.positionCount - 2));
		float num = 0.68f;
		for (int j = 0; j < nodeCount; j++)
		{
			lr_Shadow.SetPosition(j, Tool2D.IgnoreZPoint(GeneralTool.QuadraticBezierCurve(essenceLegsData.HeadPos, essenceLegsData.MiddlePoint, essenceLegsData.EndPoint + new float3(0f, 0.5f, 0f), (float)j / ((float)nodeCount - 1f)), 1.05f) + new Vector3(0f, 0f - num, 0.3f));
		}
		essenceLegShadowBladeTransform.position = Tool2D.IgnoreZPoint(essenceLegBladeParentTransform.position + new Vector3(0f, 0f - num, 0f), 1.05f);
	}

	public void Initialize(Teammate2Show owner, bool isEssenceLeg = false)
	{
		Owner = owner;
		this.isEssenceLeg = isEssenceLeg;
		essenceLegBladeParentTransform.gameObject.SetActive(isEssenceLeg);
		essenceLegBlade = null;
		lr_Leg.positionCount = nodeCount;
		lr_Shadow.positionCount = (isEssenceLeg ? nodeCount : 2);
		lr_Shadow.gameObject.SetActive(value: true);
		float num = Mathf.Min(Owner.transform.localScale.x, 3f);
		if (isEssenceLeg)
		{
			num *= 1.5f;
		}
		lr_Leg.widthMultiplier *= num;
		lr_Shadow.widthMultiplier *= num;
		essenceLegBladeParentTransform.localScale = Vector3.one * Mathf.Max(num * 0.25f, 1.5f);
		SpawnEssenceLeg();
		switch (Owner.colorType)
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
			Debug.LogError(Owner.colorType);
			if (lr_Leg.material != mat_ECPlayer)
			{
				lr_Leg.material = mat_ECPlayer;
			}
			break;
		}
		if (isEssenceLeg)
		{
			LegEssenceLockingTarget();
		}
		SetSafeMode();
	}

	private void SpawnEssenceLeg()
	{
		essenceLegColorfulBladeTransform.DestroyAllChild();
		if (isEssenceLeg)
		{
			GameObject original = null;
			switch (Owner.colorType)
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
			sparkEffect = essenceLegBlade.transform.Find("Sparks").gameObject;
			essenceLegBlade.transform.localRotation = quaternion.identity;
			essenceLegBlade.transform.localPosition = Vector3.back * 0.1f;
		}
	}

	public void ClearParticle()
	{
		sparkEffect.SetActive(value: false);
		StartCoroutine(DelayActiveParticle());
	}

	private IEnumerator DelayActiveParticle()
	{
		yield return null;
		yield return null;
		sparkEffect.SetActive(value: true);
	}

	public void SetTarget(UnitProperty targetPpt)
	{
	}

	public void SetEssenceTarget(UnitProperty targetPpt)
	{
	}

	public void CancelTarget()
	{
	}

	public void Reposition()
	{
	}

	public void Theme6Reposition(Vector3 changeValue)
	{
	}

	public void SyncDotsData(LegsData legsData)
	{
		this.legsData = legsData;
	}

	public void SyncEssenceDotsData(EssenceLegsData legsData)
	{
		essenceLegsData = legsData;
	}

	public void SuckOnce()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Teammate2SuckBlood").GetComponent<Teammate2SuckBlood>().Initialize(lr_Leg);
	}
}
