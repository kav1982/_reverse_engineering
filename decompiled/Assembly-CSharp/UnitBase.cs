using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using SpriteEffectSystem;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.GraphicsIntegration;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using _Scripts.Units;

public class UnitBase : MonoBehaviour
{
	public struct UnitSpellModifier
	{
		public float Damage;

		public float Knockback;

		public float Duration;

		public SpellColorType ColorType;

		public float CriticalChance;

		public PenetrateValue Penetrate;

		public float Float1;

		public float Float2;

		public float Float3;

		public int ReboundCount;

		public Vector3 Direction;

		public float Speed;

		public float Gravity;

		public float CurrentFallSpeed;

		public SpellSpecialMovementType MovementType;

		public float ChaseRotateSpeed;

		public int SplitCount;

		public float SplitDamageRatio;

		public Vector3 SpawnPosition;

		public Entity Shooter;

		public void ApplyToSSP(ref SpellSpawnParams ssp)
		{
			ssp.ConfigComponentData.Damage = new AttributeValue(Damage);
			ssp.ConfigComponentData.Knockback = Knockback;
			ssp.ConfigComponentData.Duration = new AttributeValue(Duration);
			ssp.ConfigComponentData.ColorType = ColorType;
			ssp.ConfigComponentData.CriticalChance = CriticalChance;
			ssp.ConfigComponentData.Penetrate = Penetrate;
			ssp.ConfigComponentData.Float1 = Float1;
			ssp.ConfigComponentData.Float2 = Float2;
			ssp.ConfigComponentData.Float3 = Float3;
			ssp.MovementComponentData.ReboundCount = ReboundCount;
			ssp.MovementComponentData.Speed = Speed;
			ssp.MovementComponentData.Gravity = Gravity;
			ssp.MovementComponentData.CurrentFallSpeed = CurrentFallSpeed;
			ssp.MovementComponentData.Direction = Direction;
			ssp.MovementComponentData.Type = MovementType;
			ssp.MovementComponentData.ChaseRotateSpeed = ChaseRotateSpeed;
			ssp.SplitComponentData.Count = SplitCount;
			ssp.SplitComponentData.DamageRatio = SplitDamageRatio;
			ssp.SpawnPosition = SpawnPosition;
			ssp.SetShooter(Shooter, Shooter);
		}
	}

	[SerializeField]
	protected float moveLerp;

	[SerializeField]
	protected float moveThreshold;

	[SerializeField]
	protected bool isMoveFlip;

	[HideInInspector]
	public UnitProperty myPpt;

	protected UnitProperty targetPpt;

	protected Entity targetEntity;

	protected AnimaEvent animaEvent;

	protected NavInfo navInfo = new NavInfo();

	protected SpellConfig spellCfg1;

	protected SpellConfig spellCfg2;

	protected int navAreaMask = 16;

	public CollisionFilter entityFilter;

	private bool isSingleInitialized;

	private bool isFrame1Initialized;

	private bool isFrame2Initialized;

	private bool frozenBeforeKinematic;

	private Vector3 frozenBeforeRigid;

	private Vector3 frozenBeforeMotion;

	private bool baseIsJumping;

	private float baseJumpUpForce;

	private float baseJumpGravity;

	private float teammateTouchMonsterTimer;

	private GameObject go_SummonDealyDeadEF;

	private LocalSpriteEffectPlayer SummonHPFixDropPlayer;

	private float summonHPGainDamageCheckTimer;

	private GameObject soulMateLinkEffect;

	[HideInInspector]
	public List<UnitProperty> mateSummonsPpts = new List<UnitProperty>();

	[HideInInspector]
	public List<UnitProperty> mateSummonsNotAttackPpts = new List<UnitProperty>();

	protected float bornIdleTimer;

	protected float checkTargetIntervalTimer;

	private float hpRecoverEffectTimer;

	public Animator Anima => myPpt.Anima;

	protected SkeletonAnimation SAnima => myPpt.SAnima;

	protected Rigidbody Rigid => myPpt.Rigid;

	protected UnityEngine.CapsuleCollider CC_Self => myPpt.CC_Self;

	protected float MoveSpeed => myPpt.MoveSpeed;

	protected float CurrentHPRatio
	{
		get
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			return componentData.unitCfg.currentHP / componentData.unitCfg.maxHP;
		}
	}

	public bool IsLocked
	{
		get
		{
			if (myPpt.Affect_InAbyss)
			{
				return true;
			}
			if (myPpt.FronzenState == UnitProperty.Affect_FrozenState.Frozening)
			{
				return true;
			}
			return false;
		}
	}

	public bool HaveTarget
	{
		get
		{
			if (EntityIsValid(targetEntity))
			{
				if (targetEntity == PlayerMgr.Inst.PlayerEtt && !PlayerMgr.Inst.PlayerCtrller.IsVisible)
				{
					return false;
				}
				if (myPpt.unitCfg.unitType == UnitType.Teammate)
				{
					return GetComponentData<UnitProperty_Dots>(targetEntity).CanBeTarget;
				}
				return true;
			}
			return false;
		}
	}

	public Vector3 TargetPoint => Tool2D.IgnoreZPoint(UnitDotsSyncSystem.entityMgr.GetComponentData<LocalTransform>(targetEntity).Position);

	protected Vector3 TargetPointIgnoreZ => TargetPoint;

	public bool IsFlipped
	{
		get
		{
			if (myPpt.SR_Models.Length != 0 && myPpt.SR_Models[0].flipX)
			{
				return true;
			}
			return false;
		}
	}

	public bool deadStayed { get; protected set; }

	public bool isFalling => baseJumpUpForce < 0f;

	public float BaseJumpUpForce => baseJumpUpForce;

	public SpellBase SummonerSpellBase { get; protected set; }

	public Vector3 CurrentMotion { get; protected set; } = Vector3.zero;


	private void SingleInitial()
	{
		if (isSingleInitialized)
		{
			return;
		}
		isSingleInitialized = true;
		myPpt = GetComponent<UnitProperty>();
		if (Anima != null)
		{
			animaEvent = Anima.GetComponent<AnimaEvent>();
			if (animaEvent != null)
			{
				animaEvent.DoAction = AnimaAction;
			}
		}
		navInfo.moveThreshold = moveThreshold;
		SingleInitialCallback();
	}

	public virtual void SingleInitialCallback()
	{
	}

	public void EveryInitial()
	{
		SingleInitial();
		isFrame1Initialized = false;
		isFrame2Initialized = false;
		baseIsJumping = false;
		SummonerSpellBase = null;
		teammateTouchMonsterTimer = 0f;
		mateSummonsPpts.Clear();
		mateSummonsNotAttackPpts.Clear();
		deadStayed = false;
		CurrentMotion = Vector3.zero;
		bornIdleTimer = 0f;
		checkTargetIntervalTimer = 0f;
		EveryInitialCallback();
	}

	public virtual void EveryInitialCallback()
	{
	}

	public void Frame1Initial()
	{
		if (isFrame1Initialized)
		{
			return;
		}
		isFrame1Initialized = true;
		if (SummonerSpellBase != null)
		{
			if (SummonerSpellBase.ownerPpt.gameObject.activeInHierarchy)
			{
				if (SummonerSpellBase.ownerPpt.unitCfg.unitType == UnitType.Player)
				{
					PlayerMgr.Inst.SummonsRegister(myPpt);
				}
				else
				{
					SummonerSpellBase.ownerPpt.UnitBas.MateSummonsRegister(myPpt);
				}
			}
			else
			{
				myPpt.AnnouncedDeath();
			}
		}
		Frame1InitialCallback();
		if ((bool)SummonerSpellBase)
		{
			CreateSummonEffect();
		}
	}

	public virtual void Frame1InitialCallback()
	{
	}

	private void Frame2initial()
	{
		if (!isFrame2Initialized && isFrame1Initialized)
		{
			isFrame2Initialized = true;
			Frame2InitialCallback();
		}
	}

	public virtual void Frame2InitialCallback()
	{
	}

	protected virtual void UpdateHPRecoverEffect()
	{
		if (!(CurrentHPRatio >= 1f) && (bool)SummonerSpellBase && !(SummonerSpellBase.SpellSummonHPRecover <= 0f))
		{
			hpRecoverEffectTimer -= Time.deltaTime;
			if (!(hpRecoverEffectTimer > 0f))
			{
				hpRecoverEffectTimer = 0.5f;
				SpriteEffectAnima anima = ABResources.LoadAsset<SpriteEffectAnima>("Prefabs/Spell/31081/31081_Recover");
				Vector3 layerPoint = Tool2D.GetLayerPoint(base.transform.position);
				layerPoint.z -= 0.3f;
				SpellSpriteEffectController.Inst.PlayEffect(anima, new EffectPlayParam
				{
					Position = layerPoint,
					Scale = myPpt.tsf_Layer.localScale * 1.5f,
					Color = new Color(1f, 1f, 1f, DataMgr.settingData.SummonTransparent),
					FilpX = (UnityEngine.Random.Range(0, 2) == 0)
				});
			}
		}
	}

	public virtual void Update()
	{
		if (!EntityIsValid(myPpt.myEntity))
		{
			return;
		}
		Frame2initial();
		Frame1Initial();
		UpdateHPRecoverEffect();
		UpdateSoulMateEffect();
		if (IsLocked)
		{
			return;
		}
		if (EntityIsValid(myPpt.myEntity))
		{
			LocalTransform component = GetComponentData<LocalTransform>();
			if (CurrentMotion != Vector3.zero)
			{
				Vector3 v = CurrentMotion;
				component = component.Translate(v.GetFloat3() * Time.deltaTime);
			}
			if (baseIsJumping)
			{
				baseJumpUpForce += baseJumpGravity * Time.deltaTime;
				if (baseJumpUpForce != 0f)
				{
					component = component.Translate(new float3(0f, 0f, (0f - baseJumpUpForce) * Time.deltaTime));
				}
			}
			base.transform.position = component.Position;
			SetComponentData(component);
		}
		if ((bool)SummonerSpellBase)
		{
			SummonerSpellBase.transform.position = base.transform.position;
			if (CurrentMotion.sqrMagnitude > 0.01f)
			{
				SummonerSpellBase.Direction = CurrentMotion.normalized;
			}
			SummonHpRecoverOrTakedamage();
			if (myPpt.unitCfg.currentHP < (float)Mathf.FloorToInt(myPpt.unitCfg.maxHP * SummonerSpellBase.SpellSummonInstantDeathHpRatio))
			{
				myPpt.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
				{
					isInstanceDeath = false
				});
			}
		}
	}

	protected virtual void SummonHpRecoverOrTakedamage(bool independentEffect = false)
	{
		if (!SummonerSpellBase)
		{
			return;
		}
		summonHPGainDamageCheckTimer += Time.deltaTime;
		if (summonHPGainDamageCheckTimer < 1f)
		{
			return;
		}
		summonHPGainDamageCheckTimer -= 1f;
		if (independentEffect)
		{
			if (SummonerSpellBase.SpellSummonHPRecover > 0f)
			{
				myPpt.HPRecovery(SummonerSpellBase.SpellSummonHPRecover);
			}
			if (SummonerSpellBase.SpellSummonHPFixDropAmount > 0f)
			{
				myPpt.TakeDamage(SummonerSpellBase.SpellSummonHPFixDropAmount, myPpt, new TakeDamageInfo
				{
					beHitColor = false,
					beHitShake = false
				});
			}
		}
		else
		{
			int num = Mathf.RoundToInt(SummonerSpellBase.SpellSummonHPFixDropAmount - SummonerSpellBase.SpellSummonHPRecover);
			if (num > 0)
			{
				myPpt.TakeDamage(num, myPpt, new TakeDamageInfo
				{
					beHitColor = false,
					beHitShake = false
				});
			}
			else if (num < 0 && myPpt.unitCfg.currentHP < myPpt.unitCfg.maxHP)
			{
				myPpt.HPRecovery(-num);
			}
		}
	}

	public void UpdateSoulMateEffect()
	{
		if ((bool)soulMateLinkEffect && (bool)SummonerSpellBase && !(SummonerSpellBase.SIP.SummonFollowOwnerThroughMapChance <= 0f))
		{
			soulMateLinkEffect.SetActive(LevelMgr.Inst.CurrentRoomCtrller.enableEnterNextRoom);
			soulMateLinkEffect.transform.localPosition = new Vector3(0f, 0f - soulMateLinkEffect.transform.parent.localPosition.y, 0.2f);
			soulMateLinkEffect.transform.right = Tool2D.IgnoreZV2ToV1(SummonerSpellBase.ownerPpt.transform.position, base.transform.position);
		}
	}

	public virtual void BeforeTakeDamage(TakeDamageInfo info)
	{
	}

	public virtual void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
	}

	public virtual void AfterTakeDamage(TakeDamageInfo info)
	{
	}

	public virtual void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
	}

	public virtual void BeforeAnnouncedDeath_Dots(ref TakeDamageInfo_Dots info)
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		if (componentData.unitCfg.unitType != UnitType.Boss && componentData.unitCfg.unitType != UnitType.Elite)
		{
			return;
		}
		if (!deadStayed)
		{
			info.stopAnnouncedDeath = true;
			deadStayed = true;
			BossDeadStay();
			if (info.spell.Config.AbilityType == SpellAbilityType.JudgementBlade)
			{
				DataMgr.selectedWorldData.SetFindSet7();
			}
		}
		else
		{
			info.stopAnnouncedDeath = true;
		}
	}

	public virtual void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (myPpt.unitCfg.unitType == UnitType.Teammate || myPpt.unitCfg.unitType == UnitType.TeammateNotAttack)
		{
			DeathWormSpawnCheck();
		}
	}

	public void DeathExplodeSpawnCheck(TakeDamageInfo info, float hpBeforeDeath)
	{
		if (!(SummonerSpellBase != null) || !(SummonerSpellBase.SpellSummonDeathExplodeRange > 0f) || info.isTeammateThrough || myPpt.unitCfg.id == 705101 || myPpt.unitCfg.id == 705201)
		{
			return;
		}
		info.isPlayDeadSE = false;
		GetSummonExplodePrefabPathByColorType getSummonExplodePrefabPathByColorType = new GetSummonExplodePrefabPathByColorType();
		GameObject gO = ObjPoolMgr.Inst.GetGO(getSummonExplodePrefabPathByColorType.Get(SummonerSpellBase.ColorType), base.transform.position, 1f);
		float spellEnhancedSize = GeneralTool.GetSpellEnhancedSize(SummonerSpellBase.SpellSummonDeathExplodeRange, SummonerSpellBase);
		gO.transform.localScale = Vector3.one * spellEnhancedSize;
		SEMgr.Inst.relic_SummonsExplode.PlaySE().pitch = UnityEngine.Random.Range(0.7f, 1.3f);
		int num = Mathf.CeilToInt(GeneralTool.GetSpellEnhancedDamage(hpBeforeDeath, SummonerSpellBase) * SummonerSpellBase.SpellSummonDeathExplodeHpDamageRatio);
		List<UnityEngine.Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, spellEnhancedSize, "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness");
		for (int i = 0; i < collidersByTag.Count; i++)
		{
			if (collidersByTag[i].tag == "RollBall" || collidersByTag[i].tag == "Butterfly")
			{
				SpellBase componentInParent = collidersByTag[i].GetComponentInParent<SpellBase>();
				if (!componentInParent.IsSameCamp(UnitType.Player))
				{
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(num);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
					else
					{
						MonoBehaviour.print(componentInParent.spellCfg.abilityType);
					}
				}
			}
			else
			{
				UnitProperty component = collidersByTag[i].GetComponent<UnitProperty>();
				if (component != null && component.gameObject.activeSelf)
				{
					TakeDamageInfo info2 = new TakeDamageInfo
					{
						canRebound = false,
						attackerType = AttackerType.NothingSpecial,
						damage = num
					};
					SummonerSpellBase.OutputDamage(component, info2, SpellAbilityType.TeammateSacrifice);
				}
			}
		}
	}

	private void DeathWormSpawnCheck()
	{
		if (SummonerSpellBase != null && SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount > 0)
		{
			float num = UnityEngine.Random.Range(0f, 360f);
			for (int i = 0; i < SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount; i++)
			{
				Vector3 v = base.transform.position + Tool2D.GetDir(num + 360f / (float)SummonerSpellBase.SpellSummonAfterDeadSpawnWormCount * (float)i) * 0.5f;
				ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 705101, Tool2D.GetNavMeshPointIngoreZ(Tool2D.IgnoreZPoint(v)), Quaternion.identity).GetComponent<SpellWorm>().ApplySpellEffect(SummonerSpellBase);
			}
		}
	}

	public virtual void AnimaAction(string animaName)
	{
	}

	public virtual void Theme6Reposition(Vector3 changeValue)
	{
		if (myPpt.isRigidLerp_Dots)
		{
			PhysicsGraphicalSmoothing componentData = GetComponentData<PhysicsGraphicalSmoothing>();
			componentData.ApplySmoothing = 0;
			componentData.CurrentVelocity.Linear = GetComponentData<PhysicsVelocity>().Linear;
			SetComponentData(componentData);
			LocalToWorld componentData2 = GetComponentData<LocalToWorld>();
			Vector3 vector = base.transform.position + changeValue;
			componentData2.Value.c3 = new float4(vector.x, vector.y, vector.z, componentData2.Value.c3.w);
			SetComponentData(componentData2);
		}
		base.transform.position += changeValue;
		SyncDotsPosition();
	}

	protected virtual void BossDeadStay()
	{
		base.enabled = false;
		if (Anima != null)
		{
			Anima.speed = 0f;
		}
		if (SAnima != null)
		{
			SAnima.timeScale = 0f;
		}
		Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	protected virtual void BossDeadStayKeepPresents()
	{
		Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		myPpt.enabled = false;
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.BossDeadStay();
		SetComponentData(componentData);
		GetComponent<BossDeadCreateEffect>().CreateEffect();
	}

	protected virtual void CreateSummonEffect()
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_SummonEffect", GetSummonEffectPosition(), GetSummonEffectSize(), 2f);
		if (SummonerSpellBase.SIP.SummonFollowOwnerThroughMapChance > 0f)
		{
			if (this is Teammate1 || this is Teammate2 || this is Teammate3 || this is Teammate4 || this is Teammate5 || this is Teammate6 || this is Teammate7)
			{
				GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/31271/31271_Aura", 0.5f);
				gO.transform.position = GetSummonEffectPosition();
				gO.transform.localScale = GetSummonEffectSize();
			}
		}
	}

	protected virtual Vector3 GetSummonEffectSize()
	{
		return myPpt.tsf_Layer.localScale;
	}

	protected virtual Vector3 GetSummonEffectPosition()
	{
		return Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.GroundEffect);
	}

	public void SetMove(Vector3 motion, bool isFlip = true)
	{
		CurrentMotion = Tool2D.IgnoreZPoint(CurrentMotion);
		CurrentMotion = Vector3.Lerp(CurrentMotion, motion, moveLerp * Time.deltaTime);
		if (isFlip && isMoveFlip && myPpt.SR_Models != null)
		{
			SetFlip(CurrentMotion.x);
		}
	}

	protected virtual void SetFlip(float motionX)
	{
		if (Mathf.Abs(motionX) > 0.01f)
		{
			myPpt.SetFlip(motionX < 0f);
		}
	}

	public void SetSingleFlip(SpriteRenderer sr, float motionX, bool srFlip = true)
	{
		SetSingleFlip(sr, motionX < 0f, srFlip);
	}

	public void SetSingleFlip(SpriteRenderer sr, bool flipX, bool srFlip = true)
	{
		if (srFlip)
		{
			sr.flipX = flipX;
		}
		sr.material.SetFloat(GameConstManaged.shaderFlipXIndex, (!flipX) ? 1 : (-1));
	}

	public void SetMRFlip(MeshRenderer mr, float motionX)
	{
		SetMRFlip(mr, motionX < 0f);
	}

	public void SetMRFlip(MeshRenderer mr, bool flipX)
	{
		mr.material.SetFloat(GameConstManaged.shaderFlipXIndex, (!flipX) ? 1 : (-1));
	}

	public void GetNearestTarget(bool checkWall = false)
	{
		GetNearestTarget(base.transform.position, checkWall);
	}

	public void GetNearestTargetWithTimer(bool checkWall = false)
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer > 1f)
		{
			checkTargetIntervalTimer = 0f;
			GetNearestTarget(base.transform.position, checkWall);
		}
	}

	protected void GetNearestTarget(Vector3 checkPoint, bool checkWall = false)
	{
		switch (myPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			targetEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetableEntity(checkPoint, checkWall);
			break;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			targetEntity = LevelMgr.Inst.CurrentRoomCtrller.GetNearestFriendlyEntity(checkPoint, checkWall);
			break;
		default:
			Debug.LogError(myPpt.unitCfg.unitType);
			break;
		}
	}

	protected void GetNearestTargetPlayerFirst()
	{
		UnitType unitType = myPpt.unitCfg.unitType;
		if ((uint)unitType > 2u && (uint)(unitType - 3) <= 5u)
		{
			if (PlayerMgr.Inst.PlayerCtrller.IsVisible)
			{
				targetEntity = PlayerMgr.Inst.PlayerEtt;
			}
			else
			{
				GetNearestTarget();
			}
		}
		else
		{
			Debug.LogError(myPpt.unitCfg.unitType);
		}
	}

	protected void GetRandomTarget()
	{
		switch (myPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			targetPpt = LevelMgr.Inst.CurrentRoomCtrller.GetRandomTargetablePpt();
			break;
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			targetPpt = PlayerMgr.Inst.GetRandomPpt();
			break;
		default:
			Debug.LogError(myPpt.unitCfg.unitType);
			break;
		}
	}

	protected float ToTargetDistance()
	{
		if (!HaveTarget)
		{
			Debug.LogError("没有目标，却想获得到目标的距离？");
			return 1000f;
		}
		return Tool2D.IgnoreZDistance(base.transform.position, TargetPoint);
	}

	protected float ToPointDistance(Vector3 point)
	{
		return Tool2D.IgnoreZDistance(base.transform.position, point);
	}

	protected float ToTargetDistanceSqr()
	{
		if (!HaveTarget)
		{
			Debug.LogError("没有目标，却想获得到目标的距离？");
			return 1000000f;
		}
		return (Tool2D.IgnoreZPoint(base.transform) - Tool2D.IgnoreZPoint(TargetPoint)).sqrMagnitude;
	}

	public float GetBodyColliderRadius()
	{
		if ((bool)myPpt.CC_Self)
		{
			return myPpt.CC_Self.radius;
		}
		if (TryGetComponent<UnityEngine.BoxCollider>(out var component))
		{
			return component.size.x / 2f;
		}
		if (TryGetComponent<UnityEngine.SphereCollider>(out var component2))
		{
			return component2.radius;
		}
		return 0f;
	}

	public void LosePlayerTarget()
	{
		if (HaveTarget && targetEntity == PlayerMgr.Inst.PlayerEtt)
		{
			targetEntity = Entity.Null;
		}
	}

	public virtual void LoseTarget()
	{
		for (int i = 0; i < mateSummonsPpts.Count; i++)
		{
			mateSummonsPpts[i].UnitBas.LoseTarget();
		}
		for (int j = 0; j < mateSummonsNotAttackPpts.Count; j++)
		{
			mateSummonsNotAttackPpts[j].UnitBas.LoseTarget();
		}
		targetEntity = Entity.Null;
	}

	public Vector3 ToPointDir(Transform targetT)
	{
		return ToPointDir(targetT.position);
	}

	protected Vector3 ToPointDir(Vector3 point)
	{
		return Tool2D.IgnoreZV2ToV1Normal(point, base.transform.position);
	}

	protected Vector3 ToPointDir(Vector3 point, float angleOffset)
	{
		return Tool2D.GetDir(ToPointDir(point), angleOffset);
	}

	protected Vector3 ToTargetDelta()
	{
		return Tool2D.IgnoreZV2ToV1(TargetPoint, base.transform.position);
	}

	protected Vector3 ToTargetDir()
	{
		return Tool2D.IgnoreZV2ToV1Normal(TargetPoint, base.transform.position);
	}

	protected Vector3 ToTargetDir(float angleOffset)
	{
		return Tool2D.GetDir(ToTargetDir(), angleOffset);
	}

	protected float ToPointDistanceSqr(Vector3 position)
	{
		return (base.transform.position - position).sqrMagnitude;
	}

	protected float ToPointDegree(Vector3 point)
	{
		return Tool2D.GetDegree(ToPointDir(point));
	}

	protected float ToTargetDegree()
	{
		return Tool2D.GetDegree(ToPointDir(TargetPoint));
	}

	protected void SetNavMeshArea(int layer)
	{
		navAreaMask = layer;
	}

	public void GetNavInfo(Vector3 targetPoint)
	{
		NavMeshPath navMeshPath = Tool2D.GetNavMeshPath(base.transform.position, targetPoint, navAreaMask);
		navInfo.corners = navMeshPath.corners;
		navInfo.currentCornerIndex = 0;
		navInfo.allCornerArrived = false;
		CheckNavInfo();
	}

	public void GetNavInfoWithTimer(Vector3 targetPoint)
	{
		if (navInfo.corners == null)
		{
			GetNavInfo(targetPoint);
		}
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer > 0.1f)
		{
			checkTargetIntervalTimer = 0f;
			GetNavInfo(targetPoint);
		}
	}

	public void CheckNavInfo()
	{
		for (int i = navInfo.currentCornerIndex; i < navInfo.corners.Length && Tool2D.IgnoreZPoint(base.transform.position - navInfo.corners[i]).sqrMagnitude < moveThreshold * moveThreshold; i++)
		{
			navInfo.currentCornerIndex++;
			if (navInfo.currentCornerIndex == navInfo.corners.Length)
			{
				navInfo.allCornerArrived = true;
				navInfo.currentCornerIndex = navInfo.corners.Length - 1;
			}
		}
	}

	protected void JumpStart_Dots(float upForce, float gravity)
	{
		if (!baseIsJumping)
		{
			baseIsJumping = true;
			baseJumpUpForce = upForce;
			baseJumpGravity = gravity;
			if (base.transform.position.z > 0f)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
				LocalTransform componentData = GetComponentData<LocalTransform>();
				componentData.Position = base.transform.position;
				SetComponentData(componentData);
			}
			UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
			componentData2.JumpStartSetting();
			SetComponentData(componentData2);
		}
	}

	protected void JumpStop_Dots()
	{
		if (baseIsJumping)
		{
			baseIsJumping = false;
			baseJumpUpForce = 0f;
			baseJumpGravity = 0f;
			PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
			componentData.Linear.z = 0f;
			SetComponentData(componentData);
			base.transform.position = Tool2D.IgnoreZPoint(base.transform);
			LocalTransform componentData2 = GetComponentData<LocalTransform>();
			componentData2.Position = base.transform.position;
			SetComponentData(componentData2);
			UnitProperty_Dots componentData3 = GetComponentData<UnitProperty_Dots>();
			componentData3.JumpStopSetting();
			SetComponentData(componentData3);
		}
	}

	protected void JumpRebounce(float bounceRatio)
	{
		baseJumpUpForce = (0f - bounceRatio) * baseJumpUpForce;
	}

	public void SummonsInitial(SpellBase spellBase)
	{
		SummonerSpellBase = spellBase;
		UnitConfig unitConfig = UnitConfig.map[myPpt.unitCfg.id];
		int num = 0;
		if (spellBase.SpellSummonGainOwnerHpRatio > 0f)
		{
			UnitBase unitBas = spellBase.ownerPpt.UnitBas;
			num = ((!(unitBas is Teammate5) && !(unitBas is Teammate5FuseController)) ? Mathf.CeilToInt(PlayerMgr.Inst.PlayerPpt.unitCfg.maxHP * spellBase.SpellSummonGainOwnerHpRatio) : Mathf.CeilToInt(spellBase.ownerPpt.unitCfg.maxHP * spellBase.SpellSummonGainOwnerHpRatio));
		}
		myPpt.unitCfg.maxHP = (unitConfig.maxHP + (float)num) * spellBase.SpellSummonHPRatio * spellBase.SpellSUmmonFinalHpRatio;
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP;
		myPpt.tsf_Layer.localScale = Vector3.one * Mathf.Pow(spellBase.SpellSummonHPRatio * spellBase.SpellSUmmonFinalHpRatio, 0.5f) * spellBase.spellVolumeRatio;
		myPpt.unitCfg.moveSpeed *= spellBase.SpellSummonMoveRatio;
		if (myPpt.Anima != null)
		{
			myPpt.Anima.SetFloat("MoveSpeed", spellBase.SpellSummonMoveRatio);
		}
		if (SummonHPFixDropPlayer == null)
		{
			string text = (GameMgr.IsHarmony_Static ? "Prefabs/Spell/30151/30151_HPDrop_H" : "Prefabs/Spell/30151/30151_HPDrop");
			SummonHPFixDropPlayer = Object.Instantiate(ABResources.LoadAsset<GameObject>(text), myPpt.tsf_Layer).GetComponent<LocalSpriteEffectPlayer>();
			SummonHPFixDropPlayer.transform.Translate(0f, 0f, -0.3f, Space.Self);
		}
		SummonHPFixDropPlayer.gameObject.SetActive(spellBase.SpellSummonHPFixDropAmount > 0f);
		SummonSoulLinkEffectToggle(SummonerSpellBase.SIP.SummonFollowOwnerThroughMapChance > 0f);
		SummonGhostEffectToggle(state: false);
	}

	public void SummonGhostEffectToggle(bool state)
	{
		if (go_SummonDealyDeadEF == null)
		{
			go_SummonDealyDeadEF = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/EF/EF_SummonDelayDead"), myPpt.tsf_Layer);
			go_SummonDealyDeadEF.transform.localPosition = Vector3.zero;
		}
		go_SummonDealyDeadEF.SetActive(state);
	}

	public void SummonSoulLinkEffectToggle(bool state)
	{
		if (!soulMateLinkEffect)
		{
			soulMateLinkEffect = Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/EF/EF_SoulMateCircle"), myPpt.tsf_Layer);
			soulMateLinkEffect.transform.localPosition = new Vector3(0f, 0f, 1.05f);
			soulMateLinkEffect.GetComponent<Spell3127SoulMateEffectController>().SetFollowTarget(SummonerSpellBase.ownerPpt);
		}
		soulMateLinkEffect.SetActive(state);
	}

	protected void SummonsTouchMonster()
	{
		if (myPpt.AlreadyDead)
		{
			return;
		}
		teammateTouchMonsterTimer += Time.deltaTime;
		if (!(teammateTouchMonsterTimer >= 0.33f))
		{
			return;
		}
		for (int num = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts.Count - 1; num >= 0; num--)
		{
			if (LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].CanTouch)
			{
				float num2 = LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].CC_Self.radius * LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].transform.localScale.x;
				if ((base.transform.position - LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].transform.position).sqrMagnitude < (CC_Self.radius * base.transform.localScale.x + num2) * (CC_Self.radius * base.transform.localScale.x + num2))
				{
					teammateTouchMonsterTimer = 0f;
					if (!GameMgr.IsHarmony_Static)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", base.transform.position, 1f);
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].transform.position, 1f);
					}
					myPpt.TakeDamage(6f, LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num]);
					LevelMgr.Inst.CurrentRoomCtrller.TargetablePpts[num].TakeDamage(6f, myPpt);
					break;
				}
			}
		}
	}

	public virtual void SummonsThrough()
	{
		for (int num = mateSummonsPpts.Count - 1; num >= 0; num--)
		{
			mateSummonsPpts[num].UnitBas.SummonsThrough();
		}
		for (int num2 = mateSummonsNotAttackPpts.Count - 1; num2 >= 0; num2--)
		{
			mateSummonsNotAttackPpts[num2].UnitBas.SummonsThrough();
		}
	}

	public void SummonsTheme6Check()
	{
		if (myPpt.unitCfg.theme6Reposition)
		{
			Vector3 chapter3RepositionChangeValue = LevelMgr.Inst.CurrentRoomCtrller.GetChapter3RepositionChangeValue(base.transform);
			if (chapter3RepositionChangeValue != Vector3.zero)
			{
				Theme6Reposition(chapter3RepositionChangeValue);
			}
		}
		for (int i = 0; i < mateSummonsPpts.Count; i++)
		{
			mateSummonsPpts[i].UnitBas.SummonsTheme6Check();
		}
		for (int j = 0; j < mateSummonsNotAttackPpts.Count; j++)
		{
			mateSummonsNotAttackPpts[j].UnitBas.SummonsTheme6Check();
		}
	}

	public void MateSummonsRegister(UnitProperty registerPpt)
	{
		if (registerPpt.unitCfg.unitType != UnitType.Teammate && registerPpt.unitCfg.unitType != UnitType.TeammateNotAttack)
		{
			Debug.LogError(registerPpt.unitCfg.unitType);
		}
		else
		{
			if (mateSummonsPpts.Contains(registerPpt) || mateSummonsNotAttackPpts.Contains(registerPpt))
			{
				return;
			}
			int num = 1000;
			if (registerPpt.UnitBas.SummonerSpellBase != null && registerPpt.UnitBas.SummonerSpellBase.spellCfg.summonLimit != 0)
			{
				num = registerPpt.UnitBas.SummonerSpellBase.spellCfg.summonLimit;
			}
			if (num > 0)
			{
				List<UnitProperty> obj = ((registerPpt.unitCfg.unitType == UnitType.Teammate) ? mateSummonsPpts : mateSummonsNotAttackPpts);
				UnitProperty unitProperty = null;
				int num2 = 0;
				foreach (UnitProperty item in obj)
				{
					if (item.UnitBas.SummonerSpellBase.spellCfg.abilityType == registerPpt.UnitBas.SummonerSpellBase.spellCfg.abilityType && !item.isUnitDead)
					{
						if (unitProperty == null)
						{
							unitProperty = item;
						}
						num2++;
						if (num2 >= num)
						{
							unitProperty.TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
							{
								isInstanceDeath = false
							});
							break;
						}
					}
				}
			}
			if (registerPpt.unitCfg.unitType == UnitType.Teammate)
			{
				mateSummonsPpts.Add(registerPpt);
			}
			else
			{
				mateSummonsNotAttackPpts.Add(registerPpt);
			}
		}
	}

	public bool MateSummonsIsLimitReached(SpellConfig summonSpell)
	{
		int num = summonSpell.summonLimit;
		if (num <= 0)
		{
			num = 999999;
		}
		return (from e in mateSummonsPpts.Concat(mateSummonsNotAttackPpts)
			where e.UnitBas.SummonerSpellBase.spellCfg.abilityType == summonSpell.abilityType
			select e).Count(delegate(UnitProperty e)
		{
			if (!(e.UnitBas is Teammate teammate))
			{
				return false;
			}
			if (teammate.FusionData.IsFusing)
			{
				return true;
			}
			return !e.isUnitDead && teammate.FusionData.MaxFusionLevel - teammate.FusionData.CurrentFusionLevel <= 0;
		}) >= num;
	}

	public void MateSummonsUnregister(UnitProperty ppt)
	{
		if (ppt.unitCfg.unitType == UnitType.Teammate)
		{
			mateSummonsPpts.Remove(ppt);
		}
		else if (ppt.unitCfg.unitType == UnitType.TeammateNotAttack)
		{
			mateSummonsNotAttackPpts.Remove(ppt);
		}
		else
		{
			Debug.LogError(ppt.unitCfg.unitType);
		}
	}

	public void MateSummonsAllDead(bool instanseDeath = false)
	{
		for (int num = mateSummonsPpts.Count - 1; num >= 0; num--)
		{
			mateSummonsPpts[num].TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
			{
				isInstanceDeath = instanseDeath
			});
		}
		for (int num2 = mateSummonsNotAttackPpts.Count - 1; num2 >= 0; num2--)
		{
			mateSummonsNotAttackPpts[num2].TeammateAnnounceDeath(new TeammateAnnounceDeathInfo
			{
				isInstanceDeath = instanseDeath
			});
		}
	}

	public virtual void SetFrozen()
	{
		frozenBeforeRigid = Rigid.linearVelocity;
		frozenBeforeKinematic = Rigid.isKinematic;
		frozenBeforeMotion = CurrentMotion;
		CurrentMotion = Vector3.zero;
		Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		SyncDotsVelocity();
		if (Anima != null)
		{
			Anima.speed = 0f;
		}
		if (SAnima != null)
		{
			SAnima.timeScale = 0f;
		}
	}

	public virtual void SetUnfrozen()
	{
		if (!GetComponentData<UnitProperty_Dots>().disabled)
		{
			Rigid.isKinematic = frozenBeforeKinematic;
			if (!Rigid.isKinematic)
			{
				Rigid.linearVelocity = frozenBeforeRigid;
			}
			SyncDotsRigidKindmatic();
			SyncDotsVelocity();
			CurrentMotion = frozenBeforeMotion;
		}
		if (Anima != null)
		{
			Anima.speed = 1f;
		}
		if (SAnima != null)
		{
			SAnima.timeScale = 1f;
		}
	}

	public void FallAbyss()
	{
		Rigid.isKinematic = true;
		SyncDotsRigidKindmatic();
		JumpStop_Dots();
	}

	public virtual float GetSummonUnitRealMoveSpeed()
	{
		return (MoveSpeed + SummonerSpellBase.bonusSpeed) * 0.6f * SummonerSpellBase.GetSummonValueRatio().moveSpeedRatio * SummonerSpellBase.SIP.speedDecreaseRatio;
	}

	public bool EntityIsValid(Entity entity)
	{
		if (entity != Entity.Null && UnitDotsSyncSystem.entityMgr.Exists(entity))
		{
			return UnitDotsSyncSystem.entityMgr.HasComponent<LocalTransform>(entity);
		}
		return false;
	}

	public T GetComponentData<T>(Entity entity = default(Entity)) where T : unmanaged, IComponentData
	{
		if (entity == Entity.Null)
		{
			return UnitDotsSyncSystem.entityMgr.GetComponentData<T>(myPpt.myEntity);
		}
		return UnitDotsSyncSystem.entityMgr.GetComponentData<T>(entity);
	}

	public T GetComponentObject<T>(Entity entity)
	{
		return UnitDotsSyncSystem.entityMgr.GetComponentObject<T>(entity);
	}

	public void SetComponentData<T>(T component, Entity entity = default(Entity)) where T : unmanaged, IComponentData
	{
		if (entity == Entity.Null)
		{
			UnitDotsSyncSystem.entityMgr.SetComponentData(myPpt.myEntity, component);
		}
		else
		{
			UnitDotsSyncSystem.entityMgr.SetComponentData(entity, component);
		}
	}

	protected void SetDotsLayer(uint layer)
	{
		PhysicsCollider pc = GetComponentData<PhysicsCollider>();
		DTool.SetCollider(in pc, layer);
		SetComponentData(pc);
	}

	protected void SetDotsCCEnable(bool isOpen)
	{
		UnitDotsSyncSystem.SetColliderEnable(isOpen, this);
	}

	protected void SyncDotsRigidKindmatic()
	{
		PhysicsMassOverride componentData = GetComponentData<PhysicsMassOverride>();
		if (myPpt.Rigid.isKinematic)
		{
			componentData.IsKinematic = 1;
			componentData.SetVelocityToZero = 1;
		}
		else
		{
			componentData.IsKinematic = 0;
			componentData.SetVelocityToZero = 0;
		}
		SetComponentData(componentData);
	}

	protected void SyncDotsPosition()
	{
		LocalTransform componentData = GetComponentData<LocalTransform>();
		componentData.Position = base.transform.position;
		SetComponentData(componentData);
	}

	public void SyncDotsPositionSafe()
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			LocalTransform componentData = GetComponentData<LocalTransform>();
			componentData.Position = base.transform.position;
			SetComponentData(componentData);
		}
	}

	protected void SyncDotsVelocity()
	{
		PhysicsVelocity componentData = GetComponentData<PhysicsVelocity>();
		componentData.Linear = Rigid.linearVelocity;
		SetComponentData(componentData);
	}

	public void DotsAnnouncedDeath()
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.AnnouncedDeath(myPpt.myEntity);
			SetComponentData(componentData);
		}
	}

	public void DotsAnnouncedDeath(TakeDamageInfo_Dots info)
	{
		if (EntityIsValid(myPpt.myEntity))
		{
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.AnnouncedDeath(info, myPpt.myEntity);
			SetComponentData(componentData);
		}
	}

	public static UnitSpellModifier GetSSPModifier(in SpellSpawnParams ssp)
	{
		UnitSpellModifier result = default(UnitSpellModifier);
		result.Damage = ssp.ConfigComponentData.Damage.Base;
		result.Knockback = ssp.ConfigComponentData.Knockback;
		result.Duration = ssp.ConfigComponentData.Duration.Base;
		result.ColorType = ssp.ConfigComponentData.ColorType;
		result.CriticalChance = ssp.ConfigComponentData.CriticalChance;
		result.Penetrate = ssp.ConfigComponentData.Penetrate;
		result.Float1 = ssp.ConfigComponentData.Float1;
		result.Float2 = ssp.ConfigComponentData.Float2;
		result.Float3 = ssp.ConfigComponentData.Float3;
		result.ReboundCount = ssp.MovementComponentData.ReboundCount;
		result.Speed = ssp.MovementComponentData.Speed;
		result.Gravity = ssp.MovementComponentData.Gravity;
		result.CurrentFallSpeed = ssp.MovementComponentData.CurrentFallSpeed;
		result.Direction = ssp.MovementComponentData.Direction;
		result.MovementType = ssp.MovementComponentData.Type;
		result.ChaseRotateSpeed = ssp.MovementComponentData.ChaseRotateSpeed;
		result.SplitCount = ssp.SplitComponentData.Count;
		result.SplitDamageRatio = ssp.SplitComponentData.DamageRatio;
		result.SpawnPosition = ssp.SpawnPosition;
		result.Shooter = ssp.Shooter;
		return result;
	}

	public void ShootSpell(SpellSpawnParams ssp)
	{
		ssp.MovementComponentData.Speed *= myPpt.affect_MucusSpellSpeedRatio;
		if (GameMgr.IsMobile_Static && ssp.ConfigComponentData.AbilityType == SpellAbilityType.Bullet)
		{
			ssp.MovementComponentData.Speed *= 0.8f;
		}
		UnitDotsSyncSystem.ShootSpell(ssp);
	}
}
