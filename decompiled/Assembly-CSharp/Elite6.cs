using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class Elite6 : UnitBase
{
	public enum MonsterState
	{
		BornIdle,
		FlyRandom,
		FlyAroundTarget,
		Distort,
		Attack,
		AttackStage2,
		CatchShoot,
		Die
	}

	public Transform tsf_EF;

	public VariableFloat keepDistanceWithPlayer;

	public VariableFloat actionInterval;

	[Header("AirDistort")]
	public GameObject go_AirDistort;

	public GameObject go_AirDistortShoot;

	[Range(0f, 1f)]
	public float airDistortChance;

	public float airDistortRadius;

	public VariableFloat airDistortDuration;

	public float airDistortStopLerp;

	public float airDistortChangeThreshold;

	[Range(0f, 1f)]
	public float airDistortShootChance;

	public float airDistortShootSpeedRatio;

	public float catchSpellDamageRatio;

	[Header("Shoot")]
	public float shootAngle;

	public float shootInterval;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellDuration;

	[Header("ShootStage2")]
	[Range(0f, 1f)]
	public float stage2HPRatio;

	[Range(0f, 1f)]
	public float stage2AttackRatio;

	[Header("Audio")]
	public AudioSource as_AirDistort;

	[Header("和谐模式")]
	public Sprite brain_H;

	public Sprite brain;

	public SpriteRenderer sr_Brain;

	[Header("新spine")]
	public GameObject spineGO;

	public GameObject spriteGO;

	private EntityQuery blackHoleQuery;

	private EntityQuery snakeWalkQuery;

	[Header("状态机")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	public StateVariableMgr varMgr = new StateVariableMgr();

	private float stateExistTime;

	private Vector3 keepOffset;

	private float actionIntervalTimer;

	private float airDistortDurationTimer;

	private bool attackToWide = true;

	private Vector3 shootDir;

	private List<Entity> catchedSpells = new List<Entity>();

	private List<Vector3> catchBulletOffsets = new List<Vector3>();

	private List<Entity> stage2ShootSpells = new List<Entity>();

	private SpellSpawnParams ssp;

	private CollisionFilter catchFilter = new CollisionFilter
	{
		BelongsTo = 2560u,
		CollidesWith = 25165824u,
		GroupIndex = 0
	};

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private List<SpellAbilityType> allowCaptureType = new List<SpellAbilityType>
	{
		SpellAbilityType.Bullet,
		SpellAbilityType.Rollball,
		SpellAbilityType.Butterfly,
		SpellAbilityType.BlackHole,
		SpellAbilityType.PreFirework,
		SpellAbilityType.HoverTorch,
		SpellAbilityType.BackMP,
		SpellAbilityType.SnakeWalk,
		SpellAbilityType.Rainbow,
		SpellAbilityType.ArcaneNova,
		SpellAbilityType.Dash,
		SpellAbilityType.ManaCoin,
		SpellAbilityType.Boomerang,
		SpellAbilityType.ShiningStar,
		SpellAbilityType.MrBingArrow,
		SpellAbilityType.DimensionTraveller,
		SpellAbilityType.BulletParabola,
		SpellAbilityType.ShotGun
	};

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

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
		blackHoleQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(Spell1007BlackHoleData), typeof(LocalTransform));
		snakeWalkQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(Spell1010SnakeData), typeof(LocalTransform));
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		blackHoleQuery.Dispose();
		snakeWalkQuery.Dispose();
	}

	private void SoundVolumeChange()
	{
		as_AirDistort.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			airDistortShootSpeedRatio *= 0.7f;
			airDistortRadius *= 0.9f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(10011, UnitType.Elite);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Speed = spellSpeed;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
	}

	public override void EveryInitialCallback()
	{
		go_AirDistort.SetActive(value: false);
		actionInterval.RandomResult();
		airDistortDuration.RandomResult();
		if (GameMgr.IsHarmony_Static)
		{
			sr_Brain.sprite = brain_H;
			spriteGO.SetActive(value: true);
			spineGO.SetActive(value: false);
		}
		else
		{
			sr_Brain.sprite = brain;
			spriteGO.SetActive(value: false);
			spineGO.SetActive(value: true);
		}
		state = MonsterState.BornIdle;
	}

	public override void Update()
	{
		tsf_EF.position = Tool2D.GetLayerPoint(base.transform);
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
		case MonsterState.BornIdle:
			if (changedState)
			{
				go_AirDistort.SetActive(value: false);
				base.Anima.SetTrigger("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_Idle", loop: true);
			}
			SetMove(Vector3.zero, isFlip: false);
			bornIdleTimer += Time.deltaTime;
			if (bornIdleTimer >= 0.5f)
			{
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.FlyAroundTarget;
				}
				else
				{
					state = MonsterState.FlyRandom;
				}
			}
			break;
		case MonsterState.FlyRandom:
			if (changedState)
			{
				base.Anima.SetTrigger("Idle");
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_Idle", loop: true);
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, keepDistanceWithPlayer));
			}
			if (navInfo.allCornerArrived)
			{
				GetNavInfo(Tool2D.GetNavMeshPoint(base.transform.position, keepDistanceWithPlayer));
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
				CheckNavInfo();
			}
			checkTargetIntervalTimer += Time.deltaTime;
			if (checkTargetIntervalTimer >= 1f)
			{
				checkTargetIntervalTimer = 0f;
				GetNearestTargetPlayerFirst();
				if (base.HaveTarget)
				{
					state = MonsterState.FlyAroundTarget;
				}
			}
			TryAct();
			break;
		case MonsterState.FlyAroundTarget:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_Idle", loop: true);
				base.Anima.SetTrigger("Idle");
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.FlyRandom;
				break;
			}
			GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + keepOffset));
			if (navInfo.allCornerArrived)
			{
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			SetFlip(ToTargetDir().x);
			TryAct();
			break;
		case MonsterState.Distort:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_AirDistort", loop: true);
				go_AirDistort.SetActive(value: true);
				as_AirDistort.Play();
				base.Anima.SetTrigger("AirDistort");
			}
			if (!base.HaveTarget)
			{
				GetNearestTargetPlayerFirst();
			}
			if (!base.HaveTarget)
			{
				state = MonsterState.FlyRandom;
				break;
			}
			GetNavInfo(Tool2D.GetNavMeshPoint(base.TargetPoint + keepOffset));
			if (navInfo.allCornerArrived)
			{
				keepOffset = Tool2D.GetDir() * keepDistanceWithPlayer.RandomResult();
			}
			else
			{
				SetMove(ToPointDir(navInfo.ToGoPoint) * base.MoveSpeed);
			}
			SetFlip(ToTargetDir().x);
			AirDistortCheck();
			break;
		case MonsterState.Attack:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_Attack", loop: false);
				base.Anima.SetTrigger("Attack");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.AttackStage2:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_AttackStage2", loop: false);
				base.Anima.SetTrigger("AttackStage2");
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.CatchShoot:
			if (changedState)
			{
				base.SAnima.AnimationState.SetAnimation(0, "Elite6_CatchShoot", loop: false);
				base.Anima.SetTrigger("CatchShoot");
				go_AirDistortShoot.SetActive(value: true);
			}
			SetMove(Vector3.zero, isFlip: false);
			if (base.HaveTarget)
			{
				SetFlip(ToTargetDir().x);
			}
			break;
		case MonsterState.Die:
			SetMove(Vector3.zero, isFlip: false);
			break;
		default:
			Debug.LogError(state);
			break;
		}
		for (int num = catchedSpells.Count - 1; num >= 0; num--)
		{
			if (!EntityIsValid(catchedSpells[num]))
			{
				catchedSpells.RemoveAt(num);
				catchBulletOffsets.RemoveAt(num);
			}
			else
			{
				LocalTransform componentData = GetComponentData<LocalTransform>(catchedSpells[num]);
				componentData.Position = base.transform.position + catchBulletOffsets[num];
				SetComponentData(componentData, catchedSpells[num]);
				SpellMovementComponentData componentData2 = GetComponentData<SpellMovementComponentData>(catchedSpells[num]);
				componentData2.CurrentFallSpeed = 0f;
				componentData2.Speed = 0f;
				SetComponentData(componentData2, catchedSpells[num]);
			}
		}
	}

	private void TryAct()
	{
		actionIntervalTimer += Time.deltaTime;
		if (actionIntervalTimer >= actionInterval.result)
		{
			actionIntervalTimer = 0f;
			actionInterval.RandomResult();
			if (UnityEngine.Random.value <= airDistortChance)
			{
				state = MonsterState.Distort;
			}
			else if (base.CurrentHPRatio <= stage2HPRatio && UnityEngine.Random.value <= stage2AttackRatio)
			{
				state = MonsterState.AttackStage2;
			}
			else
			{
				state = MonsterState.Attack;
			}
		}
	}

	private void AirDistortCheck()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, airDistortRadius, catchFilter, results);
		NativeArray<Entity> nativeArray = blackHoleQuery.ToEntityArray(Allocator.Temp);
		NativeArray<LocalTransform> nativeArray2 = blackHoleQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
		for (int i = 0; i < nativeArray.Length; i++)
		{
			if (ToPointDistanceSqr(nativeArray2[i].Position) < airDistortRadius * airDistortRadius)
			{
				results.Add(new UnitDotsSyncSystem.DistanceHitResult
				{
					entity = nativeArray[i],
					point = nativeArray2[i].Position
				});
			}
		}
		NativeArray<Entity> nativeArray3 = snakeWalkQuery.ToEntityArray(Allocator.Temp);
		NativeArray<LocalTransform> nativeArray4 = snakeWalkQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
		for (int j = 0; j < nativeArray3.Length; j++)
		{
			if (ToPointDistanceSqr(nativeArray4[j].Position) < airDistortRadius * airDistortRadius)
			{
				results.Add(new UnitDotsSyncSystem.DistanceHitResult
				{
					entity = nativeArray3[j],
					point = nativeArray4[j].Position
				});
			}
		}
		for (int k = 0; k < results.Count; k++)
		{
			Entity entity = results[k].entity;
			Vector3 point = results[k].point;
			if (!UnitDotsSyncSystem.HasComponent<SpellConfigComponentData>(entity))
			{
				continue;
			}
			SpellConfigComponentData componentData = GetComponentData<SpellConfigComponentData>(entity);
			if (!allowCaptureType.Contains(componentData.AbilityType) || componentData.ShooterType == UnitType.Elite)
			{
				continue;
			}
			if (componentData.ColorType == SpellColorType.Monster)
			{
				if (componentData.AbilityType != SpellAbilityType.JudgementBlade && !catchedSpells.Contains(entity))
				{
					catchedSpells.Add(entity);
					catchBulletOffsets.Add(point - base.transform.position);
				}
			}
			else
			{
				if (catchedSpells.Contains(entity))
				{
					continue;
				}
				SpellMovementComponentData componentData2 = GetComponentData<SpellMovementComponentData>(entity);
				float num = Mathf.Lerp(componentData2.Speed, 0f, airDistortStopLerp * Time.deltaTime);
				float num2 = Mathf.Lerp(componentData2.Gravity, 0f, airDistortStopLerp * Time.deltaTime);
				float num3 = (componentData2.IsFallSpell ? componentData2.CurrentFallSpeed : Mathf.Lerp(componentData2.CurrentFallSpeed, 0f, airDistortStopLerp * Time.deltaTime));
				componentData2.Speed = num;
				componentData2.Gravity = num2;
				componentData2.CurrentFallSpeed = num3;
				SetComponentData(componentData2, entity);
				if (num <= airDistortChangeThreshold && num2 <= airDistortChangeThreshold && num3 <= airDistortChangeThreshold && componentData.AbilityType != SpellAbilityType.HighPressureWasher && componentData.AbilityType != SpellAbilityType.DragonBreath && componentData.AbilityType != SpellAbilityType.GiantBubble)
				{
					SpellSpawnParams spellPrototype = UnitDotsSyncSystem.GetSpellPrototype(componentData.Id / 10 * 10 + 1);
					UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in spellPrototype);
					if (componentData.AbilityType != SpellAbilityType.JudgementBlade)
					{
						sSPModifier.Speed = 0f;
					}
					if (componentData.AbilityType == SpellAbilityType.Dash)
					{
						sSPModifier.Penetrate.Base = 99999;
					}
					if (componentData.AbilityType == SpellAbilityType.BlackHole)
					{
						sSPModifier.Knockback = 0f;
					}
					sSPModifier.Direction = Vector3.up;
					sSPModifier.Duration = 100000000f;
					sSPModifier.SpawnPosition = point;
					sSPModifier.Shooter = myPpt.myEntity;
					sSPModifier.Damage = Mathf.CeilToInt(componentData.Damage.Base * catchSpellDamageRatio);
					sSPModifier.ApplyToSSP(ref spellPrototype);
					spellPrototype.DisableShootSound = true;
					UnitDotsSyncSystem.ShootSpell(spellPrototype);
					ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002016.GetText(), UITextFloatType.Normal, point);
					UnitDotsSyncSystem.entityMgr.SetComponentEnabled<SpellDestroyTag>(entity, value: true);
				}
			}
		}
		airDistortDurationTimer += Time.deltaTime;
		if (!(airDistortDurationTimer >= airDistortDuration.result))
		{
			return;
		}
		airDistortDurationTimer = 0f;
		airDistortDuration.RandomResult();
		go_AirDistort.SetActive(value: false);
		as_AirDistort.Pause();
		if (UnityEngine.Random.value <= airDistortShootChance)
		{
			bool flag = false;
			for (int l = 0; l < catchedSpells.Count; l++)
			{
				if (EntityIsValid(catchedSpells[l]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				state = MonsterState.CatchShoot;
				return;
			}
			state = MonsterState.BornIdle;
			catchedSpells.Clear();
			catchBulletOffsets.Clear();
		}
		else
		{
			state = MonsterState.BornIdle;
		}
	}

	private IEnumerator ShootBullet()
	{
		int _shootTime = (int)(180f / shootAngle) + 1;
		UnitSpellModifier usm = UnitBase.GetSSPModifier(in ssp);
		for (int i = 0; i < _shootTime; i++)
		{
			float num = (attackToWide ? (shootAngle * (float)i) : (180f - shootAngle * (float)i));
			if (i == 0 || i == _shootTime - 1)
			{
				usm.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				usm.Direction = Tool2D.GetDir(shootDir, num);
				usm.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			else
			{
				usm.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				usm.Direction = Tool2D.GetDir(shootDir, num);
				usm.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				usm.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				usm.Direction = Tool2D.GetDir(shootDir, 0f - num);
				usm.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
			yield return new WaitForSeconds(shootInterval);
		}
	}

	protected override void BossDeadStay()
	{
		base.SAnima.timeScale = 0f;
		base.Anima.SetTrigger("Die");
		base.enabled = false;
		base.Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
		go_AirDistort.SetActive(value: false);
		go_AirDistortShoot.SetActive(value: false);
		as_AirDistort.Stop();
	}

	public override void AnimaAction(string animaName)
	{
		if (base.deadStayed)
		{
			return;
		}
		switch (animaName)
		{
		case "Shoot":
			attackToWide = ((UnityEngine.Random.Range(0, 2) == 0) ? true : false);
			shootDir = ToPointDir(PlayerMgr.Inst.PlayerPoint);
			StartCoroutine(ShootBullet());
			break;
		case "Stop":
		{
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, 12f, catchFilter, results);
			for (int i = 0; i < results.Count; i++)
			{
				Entity entity = results[i].entity;
				if (UnitDotsSyncSystem.TryGetComponent<SpellConfigComponentData>(entity, out var result) && result.ShooterType == UnitType.Elite && result.AbilityType == SpellAbilityType.Bullet && !catchedSpells.Contains(entity))
				{
					result.Duration.Base = 9999999f;
					SetComponentData(result, entity);
					SpellMovementComponentData componentData6 = GetComponentData<SpellMovementComponentData>(entity);
					componentData6.Speed = 0f;
					SetComponentData(componentData6, entity);
					catchedSpells.Add(entity);
					catchBulletOffsets.Add(results[i].point - base.transform.position);
				}
			}
			stage2ShootSpells.Clear();
			break;
		}
		case "AttackFinish":
			state = MonsterState.FlyAroundTarget;
			break;
		case "CatchShoot":
			foreach (Entity catchedSpell in catchedSpells)
			{
				if (EntityIsValid(catchedSpell))
				{
					SpellConfigComponentData componentData = GetComponentData<SpellConfigComponentData>(catchedSpell);
					SpellMovementComponentData componentData2 = GetComponentData<SpellMovementComponentData>(catchedSpell);
					SpellConfig spellConfig = SpellConfig.dic[componentData.Id];
					componentData.Duration.Base = componentData.DurationTimer + spellConfig.duration / airDistortShootSpeedRatio;
					Vector3 vector = (base.HaveTarget ? ToTargetDir() : Tool2D.GetDir());
					componentData2.Direction = vector;
					if (spellConfig.abilityType == SpellAbilityType.Butterfly)
					{
						Spell1003ButterflyData componentData3 = GetComponentData<Spell1003ButterflyData>(catchedSpell);
						componentData3.StartTraceTargets = false;
						SetComponentData(componentData3, catchedSpell);
					}
					_ = spellConfig.abilityType;
					_ = 1016;
					if (componentData.AbilityType == SpellAbilityType.BulletParabola)
					{
						componentData2.CurrentFallSpeed = -0.5f;
					}
					if (componentData.AbilityType == SpellAbilityType.Boomerang)
					{
						Spell1022BoomerangData componentData4 = GetComponentData<Spell1022BoomerangData>(catchedSpell);
						componentData4.extraLerpSpeed = 0f;
						componentData4.IgnoreRecycleDurationTimer = 0f;
						SetComponentData(componentData4, catchedSpell);
					}
					else if (componentData.AbilityType != SpellAbilityType.JudgementBlade)
					{
						componentData2.Speed = spellConfig.speed * airDistortShootSpeedRatio;
					}
					_ = componentData.AbilityType;
					_ = 1026;
					if (componentData.AbilityType == SpellAbilityType.SnakeWalk)
					{
						Spell1010SnakeData componentData5 = GetComponentData<Spell1010SnakeData>(catchedSpell);
						componentData5.TargetDirection = vector;
						SetComponentData(componentData5, catchedSpell);
					}
					SetComponentData(componentData, catchedSpell);
					SetComponentData(componentData2, catchedSpell);
				}
			}
			catchedSpells.Clear();
			catchBulletOffsets.Clear();
			go_AirDistortShoot.SetActive(value: false);
			break;
		case "CatchFinish":
			state = MonsterState.FlyAroundTarget;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		using EntityQuery entityQuery = UnitDotsSyncSystem.entityMgr.CreateEntityQuery(typeof(SpellConfigComponentData));
		NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
		NativeArray<SpellConfigComponentData> nativeArray2 = entityQuery.ToComponentDataArray<SpellConfigComponentData>(Allocator.Temp);
		for (int i = 0; i < nativeArray2.Length; i++)
		{
			if (nativeArray2[i].ColorType == SpellColorType.Monster)
			{
				UnitDotsSyncSystem.entityMgr.SetComponentEnabled<SpellDestroyTag>(nativeArray[i], value: true);
			}
		}
		base.AfterDead(ref info);
	}
}
