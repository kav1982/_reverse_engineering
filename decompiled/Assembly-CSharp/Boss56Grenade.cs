using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Entities;
using UnityEngine;

public class Boss56Grenade : MonoBehaviour
{
	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private static readonly int ProgressLower = Shader.PropertyToID("progress");

	private static readonly int ProgressLowerWithUnderline = Shader.PropertyToID("_progress");

	[Header("Visual")]
	[SerializeField]
	private List<Renderer> crossLineRenderers = new List<Renderer>();

	[SerializeField]
	private float crossLineChargeStartProgress = 0.65f;

	[SerializeField]
	private bool createWarningCircleOnInitialize = true;

	[SerializeField]
	private float launchHeightZ = -0.5f;

	[SerializeField]
	private float arcExtraHeightZ = -0.25f;

	[SerializeField]
	private float beforeExplosionHeightZ = -0.3f;

	[SerializeField]
	private float beforeExplosionHeightStartProgress = 0.75f;

	public Transform CenterTransform;

	private readonly List<MaterialPropertyBlock> crossLineBlocks = new List<MaterialPropertyBlock>();

	private Vector3 startPoint;

	private Vector3 startGroundPoint;

	private float moveTimer;

	private float lifeTimer;

	private bool hasArrived;

	public float FuseTime { get; private set; }

	public float ExplosionRadius { get; private set; }

	public float DamageRadius { get; private set; }

	public float Damage { get; private set; }

	public Vector3 TargetPoint { get; private set; }

	public float MoveDuration { get; private set; }

	public bool IsInitialized { get; private set; }

	private void Awake()
	{
		CollectCrossLineRenderers();
		EnsurePropertyBlocks();
	}

	private void OnEnable()
	{
		ResetRuntimeState();
		SetCrossLineProgress(0f);
		CenterTransform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 360));
	}

	private void OnDisable()
	{
		ResetRuntimeState();
		SetCrossLineProgress(0f);
	}

	private void Update()
	{
		if (!IsInitialized)
		{
			return;
		}
		lifeTimer += Time.deltaTime;
		UpdateMove();
		if (!(lifeTimer >= Mathf.Max(FuseTime, MoveDuration)))
		{
			return;
		}
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite57_VMissileExplosionShort", base.transform.position, Quaternion.identity, Vector3.one * ExplosionRadius / 2f, 5f);
		SEMgr.Inst.elite57VMissileExplosion.PlaySE(SEPlayMode.Replay, 3, 0.16f);
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(TargetPoint, DamageRadius * 0.85f, GameConst.Filter_MonsterAoe, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, Damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
				if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(distanceHitResult.entity))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = Damage;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
		}
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public void Initialize(float fuseTime, float explosionRadius, float damage, Vector3 targetPoint, float moveDuration, float damageRadius = -1f)
	{
		FuseTime = Mathf.Max(0f, fuseTime);
		ExplosionRadius = Mathf.Max(0f, explosionRadius);
		DamageRadius = ((damageRadius >= 0f) ? Mathf.Max(0f, damageRadius) : ExplosionRadius);
		Damage = damage;
		TargetPoint = Tool2D.IgnoreZPoint(targetPoint);
		MoveDuration = Mathf.Max(0.01f, moveDuration);
		startGroundPoint = Tool2D.IgnoreZPoint(base.transform.position);
		startPoint = startGroundPoint + new Vector3(0f, 0f, launchHeightZ);
		base.transform.position = startPoint;
		moveTimer = 0f;
		lifeTimer = 0f;
		hasArrived = false;
		IsInitialized = true;
		CenterTransform.DOLocalRotate(new Vector3(0f, 0f, UnityEngine.Random.Range(-720, 720)), fuseTime);
		SetCrossLineProgress(0f);
		CreateWarningCircle();
	}

	public void InitialData(float fuseTime, float explosionRadius, float damage, Vector3 targetPoint, float moveDuration, float damageRadius = -1f)
	{
		Initialize(fuseTime, explosionRadius, damage, targetPoint, moveDuration, damageRadius);
	}

	private void UpdateMove()
	{
		if (hasArrived)
		{
			base.transform.position = TargetPoint + new Vector3(0f, 0f, beforeExplosionHeightZ);
			SetCrossLineProgress(1f);
			return;
		}
		moveTimer += Time.deltaTime;
		float num = Mathf.Clamp01(moveTimer / MoveDuration);
		float t = EaseOutCubic(num);
		Vector3 position = Vector3.LerpUnclamped(startGroundPoint, TargetPoint, t);
		position.z = GetHeightZ(num);
		base.transform.position = position;
		float crossLineProgress = Mathf.InverseLerp(crossLineChargeStartProgress, 1f, num);
		SetCrossLineProgress(crossLineProgress);
		if (num >= 1f)
		{
			hasArrived = true;
			base.transform.position = TargetPoint + new Vector3(0f, 0f, beforeExplosionHeightZ);
			SetCrossLineProgress(1f);
		}
	}

	private float GetHeightZ(float progress)
	{
		progress = Mathf.Clamp01(progress);
		float a = launchHeightZ + Mathf.Sin(progress * MathF.PI) * arcExtraHeightZ;
		float progress2 = Mathf.InverseLerp(beforeExplosionHeightStartProgress, 1f, progress);
		return Mathf.Lerp(t: EaseOutCubic(progress2), a: a, b: beforeExplosionHeightZ);
	}

	private void CreateWarningCircle()
	{
		if (createWarningCircleOnInitialize && !(ExplosionRadius <= 0f))
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle" + (GameMgr.IsHarmony_Static ? " Purple" : ""), TargetPoint).GetComponent<WarningArea>().Initialize(ExplosionRadius, FuseTime);
		}
	}

	private void CollectCrossLineRenderers()
	{
		if (crossLineRenderers.Count > 0)
		{
			return;
		}
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].name.StartsWith("CrossLine"))
			{
				crossLineRenderers.Add(componentsInChildren[i]);
			}
		}
	}

	private void EnsurePropertyBlocks()
	{
		while (crossLineBlocks.Count < crossLineRenderers.Count)
		{
			crossLineBlocks.Add(new MaterialPropertyBlock());
		}
	}

	private void SetCrossLineProgress(float value)
	{
		EnsurePropertyBlocks();
		value = Mathf.Clamp01(value);
		for (int i = 0; i < crossLineRenderers.Count; i++)
		{
			Renderer renderer = crossLineRenderers[i];
			if (!(renderer == null))
			{
				MaterialPropertyBlock materialPropertyBlock = crossLineBlocks[i];
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetFloat(Progress, value);
				materialPropertyBlock.SetFloat(ProgressLower, value);
				materialPropertyBlock.SetFloat(ProgressLowerWithUnderline, value);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}
		}
	}

	private void ResetRuntimeState()
	{
		IsInitialized = false;
		DamageRadius = 0f;
		moveTimer = 0f;
		lifeTimer = 0f;
		hasArrived = false;
	}

	private static float EaseOutCubic(float progress)
	{
		progress = Mathf.Clamp01(progress);
		float num = 1f - progress;
		return 1f - num * num * num;
	}
}
