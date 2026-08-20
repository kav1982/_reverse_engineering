using System;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;

public class Teammate6Sync : MonoBehaviour
{
	public class HoldingTeammateData
	{
		public float SoulBombDamage;

		public float SoulBombRange;

		public Entity BombTeammateEntity = Entity.Null;

		public Entity BombOwnerEntity = Entity.Null;

		public Entity chasingTarget = Entity.Null;

		public float BombScale = 1f;

		public float BombShadowScale = 1f;

		public float3 BombTargetEndPosition = float3.zero;

		public Teammate6BombState state;

		public float3 direction = float3.zero;

		public Spell2006CannonBarrelController targetCannonScript;

		public float duration = 3f;

		public float currentRotationAngle;

		public float3 BombPosition = float3.zero;

		public float BombSpeed = 28f;

		public TeammateType BombType;

		public SpellMovementComponentData Movement;

		public float RotateAngle = UnityEngine.Random.Range(0f, 360f);

		public Spell2006RopeController LinkedHook;

		public GameObject BombTrailGameObject;

		public GameObject BombOutlookObject;
	}

	public SkeletonAnimation SAnima;

	public SkeletonAnimation SAnimaHand;

	public Animator Anima;

	public Material bodyBaseMaterial;

	private Material bodyMaterial;

	public Transform BarrelTransform;

	public Transform ModelTransform;

	public Transform ProgressTransform;

	public Transform HookTransform;

	[FormerlySerializedAs("bombShootPosition")]
	[HideInInspector]
	public Vector3 CannonLookingTargetPosition;

	[HideInInspector]
	public float TeammateSpeedRatio = 1f;

	[HideInInspector]
	public List<Spell2006CannonBarrelController> CannonControllerList = new List<Spell2006CannonBarrelController>();

	[HideInInspector]
	public List<(Spell2006RopeController Hook, Entity TargetEntity)> HookTeammateDataList = new List<(Spell2006RopeController, Entity)>();

	private static readonly int UseGhostEffect = Shader.PropertyToID("_UseGhostEffect");

	private static readonly int UseFuseShineEffect = Shader.PropertyToID("_UseFuseShineEffect");

	private static readonly int FuseShineProcess = Shader.PropertyToID("_FuseShineProcess");

	private bool isSingleInstanceFinish;

	[HideInInspector]
	public List<HoldingTeammateData> teammateBombList = new List<HoldingTeammateData>();

	[HideInInspector]
	public bool barrelLockingTarget;

	public Transform BarrelInnerTransform;

	private int cannonCount;

	public void DataInitialize(TeammateData teammateData, RefRW<SpellConfigComponentData> config)
	{
		if (!isSingleInstanceFinish)
		{
			isSingleInstanceFinish = true;
			bodyMaterial = UnityEngine.Object.Instantiate(bodyBaseMaterial);
			SAnima.CustomMaterialOverride.Add(bodyBaseMaterial, bodyMaterial);
			SAnimaHand.CustomMaterialOverride.Add(bodyBaseMaterial, bodyMaterial);
		}
		SAnima.AnimationState.SetAnimation(0, "Idle", loop: true);
		SAnimaHand.AnimationState.SetAnimation(0, "Idle", loop: true);
		HideAllCannon();
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(UseGhostEffect, 0f);
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetInt(UseFuseShineEffect, 0);
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(FuseShineProcess, 0f);
		Anima.speed = teammateData.TeammateSpeedRatio;
		SAnima.timeScale = Anima.speed;
		SAnimaHand.timeScale = Anima.speed;
		InitialBodySkin(teammateData, config);
		Anima.SetTrigger("Idle");
		ChangeSpineAnimationState(SAnima, "Idle", isLoop: true);
		ChangeSpineAnimationState(SAnimaHand, "Idle", isLoop: true);
		Anima.Play("Idle");
		CannonDataInitialize(teammateData, config);
		CannonLookingTargetPosition = base.transform.position;
		cannonCount = teammateData.TeammateCurrentFuseLevel + 1;
		HookTeammateDataList.Clear();
		teammateBombList.Clear();
	}

	public void Update()
	{
		UpdateCannonDirection();
		UpdateCannonTattooState();
	}

	public void LateUpdate()
	{
		UpdateHoldingTeammateState();
	}

	public (int cannonIndex, int inCannonIndex) GetCannonData(int bombIndex)
	{
		return (bombIndex % cannonCount, Mathf.CeilToInt((float)Mathf.Max(0, bombIndex + 1) / (float)cannonCount) - 1);
	}

	public void UpdateHoldingTeammateState()
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		for (int i = 0; i < teammateBombList.Count; i++)
		{
			HoldingTeammateData holdingTeammateData = teammateBombList[i];
			if (!entityManager.HasComponent<LocalTransform>(holdingTeammateData.BombTeammateEntity) || !entityManager.HasComponent<LocalTransform>(holdingTeammateData.BombOwnerEntity))
			{
				holdingTeammateData.duration = 0f;
				break;
			}
			switch (holdingTeammateData.state)
			{
			case Teammate6BombState.Hook_Backing:
				UpdateHookBackingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Holding_BackUpAmmo:
				UpdateBackUpAmmoHoldingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Holding_Barrel:
				UpdateBarrelHoldingTeammateState(holdingTeammateData);
				break;
			case Teammate6BombState.Shooting:
				holdingTeammateData.duration -= Time.deltaTime;
				UpdateShootingTeammateState(holdingTeammateData);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case Teammate6BombState.QuickReloading:
				break;
			}
		}
	}

	public void ThrowHook(Entity target, float hookProgressDuration)
	{
		Spell2006RopeController component = SpawnHook().GetComponent<Spell2006RopeController>();
		component.InitialHookEffect(HookTransform, target, hookProgressDuration);
		HookTeammateDataList.Add((component, target));
	}

	private GameObject SpawnHook()
	{
		return PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/" + 2006 + "/" + 2006 + "_Hook");
	}

	private void UpdateHookBackingTeammateState(HoldingTeammateData data)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(data.BombTeammateEntity);
		data.BombPosition = data.LinkedHook.HookTransform.position;
		componentData.Position = Tool2D.IgnoreZPoint(data.BombPosition);
		entityManager.SetComponentData(data.BombTeammateEntity, componentData);
		data.BombOutlookObject.transform.position = data.BombPosition;
	}

	private void UpdateBackUpAmmoHoldingTeammateState(HoldingTeammateData data)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(data.BombTeammateEntity);
		data.BombPosition = data.targetCannonScript.BackUpTeammateBombTransform.position;
		componentData.Position = Tool2D.IgnoreZPoint(data.BombPosition);
		entityManager.SetComponentData(data.BombTeammateEntity, componentData);
		if (data.BombOutlookObject != null)
		{
			data.BombOutlookObject.transform.position = data.BombPosition;
			data.BombOutlookObject.transform.right = BarrelInnerTransform.transform.right;
		}
	}

	private void UpdateShootingTeammateState(HoldingTeammateData data)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(data.BombTeammateEntity);
		switch (data.Movement.Type)
		{
		case SpellSpecialMovementType.Normal:
			data.BombPosition += data.direction * data.BombSpeed * Time.deltaTime;
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		case SpellSpecialMovementType.ChaseOwner:
			if (entityManager.HasComponent<LocalTransform>(data.Movement.ChaseTarget))
			{
				LocalTransform componentData2 = entityManager.GetComponentData<LocalTransform>(data.Movement.ChaseTarget);
				ref float3 direction = ref data.direction;
				float3 target = DTool.IgnoreZDir(in componentData2.Position, in data.BombPosition);
				data.direction = DTool.DirMoveTowardsIgnoreZ(in direction, in target, data.BombSpeed * data.Movement.ChaseRotateSpeed * Time.deltaTime);
				data.BombPosition += data.direction * data.BombSpeed * Time.deltaTime;
			}
			break;
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
			data.direction = Vector3.Lerp((Vector3)data.direction, Tool2D.IgnoreZV2ToV1(mousePoint, data.BombPosition).normalized, data.BombSpeed * Time.deltaTime * data.Movement.ChaseMouseLerpSpeed);
			data.BombPosition += data.direction * data.BombSpeed * Time.deltaTime;
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			data.direction = float3.zero;
			float num = 360f / (MathF.PI * 2f * data.Movement.AroundRadius / data.BombSpeed) * Time.deltaTime;
			data.RotateAngle += num;
			float3 @float = Tool2D.GetDir(data.RotateAngle + 90f);
			data.BombPosition = entityManager.GetComponentData<LocalTransform>(data.BombOwnerEntity).Position + @float * data.Movement.AroundRadius;
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		componentData.Position = Tool2D.IgnoreZPoint(data.BombPosition);
		entityManager.SetComponentData(data.BombTeammateEntity, componentData);
		if (data.BombOutlookObject != null)
		{
			data.BombOutlookObject.transform.position = Tool2D.IgnoreZPoint(data.BombPosition);
			data.BombOutlookObject.transform.right = data.direction;
		}
	}

	public bool CheckIfCannonHasValidAmmo()
	{
		foreach (HoldingTeammateData teammateBomb in teammateBombList)
		{
			if (teammateBomb.state == Teammate6BombState.Holding_Barrel)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckBackUpAmmoState()
	{
		foreach (HoldingTeammateData teammateBomb in teammateBombList)
		{
			if (teammateBomb.state == Teammate6BombState.Holding_BackUpAmmo)
			{
				return true;
			}
		}
		return false;
	}

	public void OnEnterDelayDeathEvent()
	{
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetInt(UseGhostEffect, 1);
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.OnEnterDelayDeathEvent();
		}
	}

	public void OnEnterFuseStateEvent()
	{
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetInt(UseFuseShineEffect, 1);
		SAnima.CustomMaterialOverride[bodyBaseMaterial].SetFloat(FuseShineProcess, 0f);
		SAnima.CustomMaterialOverride[bodyBaseMaterial].DOFloat(1f, FuseShineProcess, 1.3f);
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.OnEnterFuseStateEvent();
		}
	}

	private void UpdateBarrelHoldingTeammateState(HoldingTeammateData data)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		if (entityManager.HasComponent<LocalTransform>(data.BombTeammateEntity))
		{
			LocalTransform componentData = entityManager.GetComponentData<LocalTransform>(data.BombTeammateEntity);
			if (data.BombOutlookObject != null)
			{
				data.BombOutlookObject.transform.position = data.targetCannonScript.TeammateBombPosition.position;
				data.BombOutlookObject.transform.right = data.targetCannonScript.transform.right;
				float num = Mathf.Abs(data.BombOutlookObject.transform.localScale.x);
				data.BombOutlookObject.transform.localScale = ((ModelTransform.localScale.x == 1f) ? (Vector3.one * num) : (new Vector3(-1f, 1f, 1f) * num));
			}
			Transform teammateBombPosition = data.targetCannonScript.TeammateBombPosition;
			componentData.Position = Tool2D.IgnoreZPoint(teammateBombPosition.position);
			switch (data.BombType)
			{
			case TeammateType.teammate3:
				componentData.Position = Tool2D.IgnoreZPoint(teammateBombPosition.position + new Vector3(-0.5f * ModelTransform.localScale.x, 0.3f, -0.25f));
				break;
			case TeammateType.teammate4:
				componentData.Position = Tool2D.IgnoreZPoint(teammateBombPosition.position + new Vector3(-0.5f * ModelTransform.localScale.x, 0.3f, -0.25f));
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case TeammateType.teammate1:
			case TeammateType.teammate2:
			case TeammateType.teammate5:
			case TeammateType.teammate6:
			case TeammateType.teammate7:
				break;
			}
			entityManager.SetComponentData(data.BombTeammateEntity, componentData);
		}
	}

	public void UpdateSummonFaceDirection(bool faceRight)
	{
		ModelTransform.localScale = (faceRight ? Vector3.one : new Vector3(-1f, 1f, 1f));
	}

	private void CannonDataInitialize(TeammateData teammateData, RefRW<SpellConfigComponentData> config)
	{
		int num = teammateData.TeammateCurrentFuseLevel + 1 - CannonControllerList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = SpawnCannon();
				CannonControllerList.Add(gameObject.GetComponent<Spell2006CannonBarrelController>());
			}
		}
		for (int j = 0; j <= teammateData.TeammateCurrentFuseLevel; j++)
		{
			CannonControllerList[j].gameObject.SetActive(value: true);
			if (j % 2 == 0)
			{
				CannonControllerList[j].transform.localPosition = new Vector3(0f, 0.3f + 0.6f * (float)j / 2f, 0.03f * (float)j / 2f);
			}
			else
			{
				CannonControllerList[j].transform.localPosition = new Vector3(0.25f, 0.5f + 0.6f * (float)(j - 1) / 2f, 0.05f + 0.03f * (float)(j - 1) / 2f);
			}
			if (j % 2 == 0)
			{
				CannonControllerList[j].TattooToggle(toggle: true);
			}
			if (j <= 3 && j == teammateData.TeammateCurrentFuseLevel)
			{
				CannonControllerList[j].HandToggle(toggle: true);
			}
			CannonControllerList[j].BarrelInitialize(config.ValueRW.ColorType, this);
		}
	}

	private void UpdateCannonTattooState()
	{
		float x = ProgressTransform.localScale.x;
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.UpdateTattooProgress(x);
		}
	}

	private void UpdateCannonDirection()
	{
		if (barrelLockingTarget)
		{
			BarrelInnerTransform.transform.right = Tool2D.DirMoveTowards(BarrelInnerTransform.transform.right, Tool2D.IgnoreZV2ToV1(CannonLookingTargetPosition, base.transform.position).normalized * ModelTransform.localScale.x, 240f * TeammateSpeedRatio * Time.deltaTime);
			return;
		}
		BarrelTransform.localEulerAngles = Vector3.zero;
		BarrelInnerTransform.transform.right = Tool2D.DirMoveTowards(BarrelInnerTransform.right, BarrelTransform.right, 480f * TeammateSpeedRatio * Time.deltaTime);
	}

	private GameObject SpawnCannon()
	{
		return UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/" + 2006 + "/" + 2006 + "_Cannon"), BarrelTransform);
	}

	public GameObject SpawnTeammateBall(LocalTransform transform, TeammateData teammateData, SpellColorType color)
	{
		GameObject gO = PlayerMgr.Inst.MiniPool.GetGO("Prefabs/Spell/" + 2006 + "/" + 2006 + "_TeammateBall");
		gO.GetComponent<Spell2006BombOutlookController>().InitializeOutlookDots(teammateData.TeammateType, color);
		gO.transform.localScale = transform.Scale * Vector3.one;
		gO.transform.position = transform.Position.IgnoreZ();
		return gO;
	}

	private void InitialBodySkin(TeammateData teammateData, RefRW<SpellConfigComponentData> config)
	{
		SAnima.initialSkinName = GetSkinName("Teammate6_0", config.ValueRW.ColorType);
		if (teammateData.AdvanceSkillLevel <= 0)
		{
			SAnimaHand.initialSkinName = GetSkinName("Teammate6_1", config.ValueRW.ColorType);
		}
		else
		{
			SAnimaHand.initialSkinName = GetSkinName("Teammate6_2", config.ValueRW.ColorType);
		}
		SAnima.Initialize(overwrite: true);
		SAnimaHand.Initialize(overwrite: true);
	}

	private string GetSkinName(string baseName, SpellColorType colorType)
	{
		string text = baseName;
		switch (colorType)
		{
		case SpellColorType.Frozen:
			text += "_Frozen";
			break;
		case SpellColorType.Mucus:
			text += "_Mucus";
			break;
		case SpellColorType.Venom:
			text += "_Venom";
			break;
		case SpellColorType.Fire:
			text += "_Fire";
			break;
		case SpellColorType.Thunder:
			text += "_Thunder";
			break;
		case SpellColorType.Void:
			text += "_Void";
			break;
		}
		return text;
	}

	private void HideAllCannon()
	{
		foreach (Spell2006CannonBarrelController cannonController in CannonControllerList)
		{
			cannonController.gameObject.SetActive(value: false);
		}
	}

	public void ChangeSpineAnimationState(SkeletonAnimation targetSanima, string targetAnimationName, bool isLoop)
	{
		targetSanima.AnimationState.SetAnimation(0, targetAnimationName, isLoop);
	}
}
