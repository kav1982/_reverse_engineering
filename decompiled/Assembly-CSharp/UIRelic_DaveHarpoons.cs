using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

public class UIRelic_DaveHarpoons : MonoBehaviour
{
	private enum UIState
	{
		Idle,
		Hiding,
		Fade
	}

	private static readonly int Transparency = Shader.PropertyToID("_Transparency");

	public CanvasGroup cg;

	public Image image_Fill;

	public Image image_Background;

	public Color color_LackAmmo;

	public Color color_HasAmmo;

	public Animator anima_Cooldown;

	public float fadeTime;

	public RectTransform ShootingAlertRectTrans;

	public float RotateSpeed;

	private float RotateAngle;

	public Text HarpoonsTipChargeText;

	private float attackAccountRequire;

	public int MaxHarpoons;

	private int CurrentHarpoonsRemain;

	private int CurrentMaxHarpoons;

	private bool IsFocusing;

	public Transform AimRingRootTrans;

	public SpriteRenderer AimRingSprite;

	public SpriteRenderer AimArrowSprite;

	private float AimMarkTransprency;

	public float AimModeStartDuration;

	public float AimModeTimeSlowRatio;

	private float AimModeTimer;

	public Transform AimShadowTransform;

	public SpriteRenderer AimShadowSprite;

	public float AimShadowTransparency;

	public float AimModeExtraScatterPerSeconds;

	public static float ShootKnockBackRatio = 1f;

	private bool holdingHarpoonsGun;

	private bool atLeastOneWandHasHarpoonsPassive;

	private bool playTimeSlowSE;

	private AudioSource timeSlowSE;

	private int activedBonusHarpoonsCount;

	private UIState uiState;

	private bool initializedFinished;

	private float fadeTimer;

	private Entity hitCounterEntity;

	private bool hitCounterCreated;

	public RelicConfig RelicCfg { get; private set; }

	private EntityManager ettMgr => World.DefaultGameObjectInjectionWorld.EntityManager;

	public void Initialize(RelicConfig relicCfg)
	{
		initializedFinished = true;
		RelicCfg = relicCfg;
		image_Fill.fillAmount = 0f;
		attackAccountRequire = SpellConfig.dic[10311].int1;
		AimModeTimer = 0f;
		UpdateHarpoonsMaxCapacity();
		CurrentHarpoonsRemain = CurrentMaxHarpoons;
		AimShadowSprite.color = new Color(1f, 1f, 1f, 0f);
		AimRingSprite.material.SetFloat(Transparency, 0f);
		AimArrowSprite.material.SetFloat(Transparency, 0f);
		AimMarkTransprency = 0f;
		EnsureHitCounterEntity();
	}

	private void EnsureHitCounterEntity()
	{
		if (hitCounterCreated)
		{
			return;
		}
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(ComponentType.ReadOnly<HarpoonsHitCounter>());
		if (entityQuery.CalculateEntityCount() == 0)
		{
			hitCounterEntity = ettMgr.CreateEntity(typeof(HarpoonsHitCounter));
			ettMgr.SetName(hitCounterEntity, "HarpoonsHitCounterSingleton");
			ettMgr.SetComponentData(hitCounterEntity, new HarpoonsHitCounter
			{
				HitCount = 0
			});
		}
		else
		{
			using NativeArray<Entity> nativeArray = entityQuery.ToEntityArray(Allocator.Temp);
			hitCounterEntity = nativeArray[0];
		}
		hitCounterCreated = true;
	}

	private void OnDestroy()
	{
		ResetFocusCheck();
		if (!hitCounterCreated)
		{
			return;
		}
		World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
		if (defaultGameObjectInjectionWorld != null && defaultGameObjectInjectionWorld.IsCreated)
		{
			EntityManager entityManager = defaultGameObjectInjectionWorld.EntityManager;
			if (hitCounterEntity != Entity.Null && entityManager.Exists(hitCounterEntity))
			{
				entityManager.DestroyEntity(hitCounterEntity);
			}
			hitCounterCreated = false;
		}
	}

	private void Update()
	{
		ShootingAlertRectTrans.localRotation = Quaternion.Euler(0f, 0f, RotateAngle);
		RotateAngle += RotateSpeed * Time.deltaTime;
		if (!initializedFinished)
		{
			return;
		}
		UpdateHarpoonsMaxCapacity();
		UpdateHarpoonsCountUIState();
		UpdateHarpoonRemainCountDots();
		UpdateHarpoonsCountText();
		UpdateBackGroundColor();
		UpdateHarpoonsKnockBackRatio();
		if (GameMgr.IsMobile_Static)
		{
			MobileMgr.inst.UpdateSkillCD(image_Fill.fillAmount, CurrentHarpoonsRemain + "/" + CurrentMaxHarpoons, CurrentHarpoonsRemain > 0);
		}
		switch (uiState)
		{
		case UIState.Idle:
		case UIState.Hiding:
		{
			HarpoonsHitCounter componentData = ettMgr.GetComponentData<HarpoonsHitCounter>(hitCounterEntity);
			image_Fill.fillAmount = ((CurrentHarpoonsRemain < CurrentMaxHarpoons) ? ((float)componentData.HitCount / attackAccountRequire) : 0f);
			if (ControlMgr.Inst.IsActiveSkillKeyUp())
			{
				ShootHarpoons();
			}
			break;
		}
		case UIState.Fade:
			if (ControlMgr.Inst.IsActiveSkillKeyUp() && ShootHarpoons())
			{
				return;
			}
			fadeTimer += PlayerMgr.Inst.PlayerDeltaTime;
			cg.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeTime);
			if (fadeTimer >= fadeTime)
			{
				fadeTimer = 0f;
				uiState = UIState.Hiding;
			}
			break;
		default:
			Debug.LogError(uiState);
			break;
		}
		UpdateHarpoonsAimTimerState();
	}

	private void UpdateHarpoonsCountUIState()
	{
		cg.alpha = (atLeastOneWandHasHarpoonsPassive ? 1 : 0);
	}

	private void UpdateHarpoonsMaxCapacity()
	{
		int num = 0;
		atLeastOneWandHasHarpoonsPassive = false;
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if ((bool)wand && wand.WandCfg != null && wand.passiveDaveHarpoonsEnable)
			{
				num += wand.DaveHarpoonsSpellCount;
				atLeastOneWandHasHarpoonsPassive = true;
			}
		}
		int num2 = 0;
		if (PlayerMgr.Inst.ItemCtrller != null)
		{
			num2 = ((PlayerMgr.Inst.ItemCtrller.relic_HarpoonsHeadExtend != null) ? PlayerMgr.Inst.ItemCtrller.relic_HarpoonsHeadExtend.int1.result : 0);
			num2 += ((PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage != null) ? PlayerMgr.Inst.ItemCtrller.relicCfg_ExtraSkillUsage.int1.result : 0);
		}
		if (ScriptableObjMgr.Inst.testCtrller.isBW)
		{
			num2 += 12;
		}
		CurrentMaxHarpoons = num * MaxHarpoons + num2;
		if (activedBonusHarpoonsCount < num2)
		{
			Debug.Log(activedBonusHarpoonsCount + " " + num2);
			CurrentHarpoonsRemain += num2 - activedBonusHarpoonsCount;
			activedBonusHarpoonsCount = num2;
		}
		CurrentHarpoonsRemain = Mathf.Min(CurrentMaxHarpoons, CurrentHarpoonsRemain);
	}

	private void UpdateHarpoonsKnockBackRatio()
	{
		ShootKnockBackRatio = Mathf.Lerp(ShootKnockBackRatio, 1f, 2f * Time.deltaTime);
	}

	private void UpdateBackGroundColor()
	{
		image_Background.color = ((CurrentHarpoonsRemain <= 0) ? color_LackAmmo : color_HasAmmo);
	}

	private void UpdateHarpoonsRemainCount()
	{
		if (CurrentHarpoonsRemain == CurrentMaxHarpoons)
		{
			Spell1031Shotgun.HitCount = 0;
		}
		if (!((float)Spell1031Shotgun.HitCount < attackAccountRequire))
		{
			if (CurrentHarpoonsRemain < CurrentMaxHarpoons)
			{
				CurrentHarpoonsRemain++;
				anima_Cooldown.Play("Cooldown", 0, 0f);
				Spell1031Shotgun.HitCount -= (int)attackAccountRequire;
			}
			else
			{
				Spell1031Shotgun.HitCount = 0;
			}
		}
	}

	private void UpdateHarpoonRemainCountDots()
	{
		if (!hitCounterCreated)
		{
			return;
		}
		World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
		if (defaultGameObjectInjectionWorld == null || !defaultGameObjectInjectionWorld.IsCreated)
		{
			return;
		}
		EntityManager entityManager = defaultGameObjectInjectionWorld.EntityManager;
		if (!entityManager.Exists(hitCounterEntity))
		{
			hitCounterEntity = entityManager.CreateEntity(typeof(HarpoonsHitCounter));
			entityManager.SetName(hitCounterEntity, "HarpoonsHitCounterSingleton");
			entityManager.SetComponentData(hitCounterEntity, new HarpoonsHitCounter
			{
				HitCount = 0
			});
			return;
		}
		if (!entityManager.HasComponent<HarpoonsHitCounter>(hitCounterEntity))
		{
			entityManager.AddComponentData(hitCounterEntity, new HarpoonsHitCounter
			{
				HitCount = 0
			});
			return;
		}
		HarpoonsHitCounter componentData = ettMgr.GetComponentData<HarpoonsHitCounter>(hitCounterEntity);
		if (CurrentHarpoonsRemain == CurrentMaxHarpoons)
		{
			componentData.HitCount = 0;
		}
		if ((float)componentData.HitCount < attackAccountRequire)
		{
			ettMgr.SetComponentData(hitCounterEntity, componentData);
			return;
		}
		if (CurrentHarpoonsRemain < CurrentMaxHarpoons)
		{
			CurrentHarpoonsRemain++;
		}
		if (CurrentHarpoonsRemain < CurrentMaxHarpoons)
		{
			anima_Cooldown.Play("Cooldown", 0, 0f);
			componentData.HitCount -= (int)attackAccountRequire;
		}
		else
		{
			componentData.HitCount = 0;
		}
		ettMgr.SetComponentData(hitCounterEntity, componentData);
	}

	private void UpdateHarpoonsCountText()
	{
		HarpoonsTipChargeText.text = CurrentHarpoonsRemain + " / " + CurrentMaxHarpoons;
	}

	private void UpdateHarpoonsAimTimerState()
	{
		if (!PlayerMgr.Inst.PlayerCtrller.CanMotion || PlayerMgr.Inst.PlayerGO == null)
		{
			return;
		}
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		bool flag = false;
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand != null && wand.WandCfg != null && wand.passiveDaveHarpoonsEnable && !wand.passiveAutoWand)
			{
				flag = true;
			}
		}
		if (ControlMgr.Inst.IsPressingActiveSkillKey() && !ControlMgr.Inst.IsActiveSkillKeyUp())
		{
			AimModeTimer += Time.unscaledDeltaTime;
			if (AimModeTimer >= AimModeStartDuration)
			{
				if (!playTimeSlowSE)
				{
					timeSlowSE = SEMgr.Inst.PlaySE("SE_DaveTimeSlow", SEPlayMode.Replay, 1);
					timeSlowSE.volume = DataMgr.settingData.GetFinalSound();
					playTimeSlowSE = true;
					CamController.Inst.FocusOnTransform(6.4f, 0.6f, PlayerMgr.Inst.PlayerCtrller.transform);
					CamController.Inst.finialClampAfterFocus = true;
				}
				Vector3 vector = (entityManager.HasComponent<LocalTransform>(PlayerMgr.Inst.PlayerEtt) ? ((Vector3)entityManager.GetComponentData<LocalTransform>(PlayerMgr.Inst.PlayerEtt).Position) : PlayerMgr.Inst.PlayerPoint);
				AimShadowTransform.position = vector;
				AimShadowSprite.color = new Color(1f, 1f, 1f, Mathf.Clamp(AimShadowSprite.color.a + Time.unscaledDeltaTime * 2f, 0f, AimShadowTransparency));
				AimRingRootTrans.position = vector + new Vector3(0f, 0.3f, 0f);
				if (GameMgr.IsMobile_Static)
				{
					AimRingRootTrans.right = TopUI.inst.uI_AimSkill.aimDir.normalized;
				}
				else
				{
					AimRingRootTrans.right = (PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint - vector).normalized;
				}
				AimMarkTransprency = Mathf.Lerp(AimMarkTransprency, 1f, 20f * Time.unscaledDeltaTime);
				SetAimRingTransparency(AimMarkTransprency);
				if ((bool)PlayerMgr.Inst.SelectedWand && PlayerMgr.Inst.SelectedWand.passiveDaveHarpoonsEnable && flag)
				{
					PlayerMgr.Inst.PlayerCtrller.ShowAndUpdateDaveHarpoonsGun();
					holdingHarpoonsGun = true;
				}
				if (holdingHarpoonsGun)
				{
					PlayerMgr.Inst.PlayerCtrller.UpdateDaveHarpoonsGun();
				}
				TimeScaleMgr.Inst.AddNewTimeScaleModifyRequest(AimModeTimeSlowRatio, Time.unscaledDeltaTime + 0.02f, 3f, TimeScaleMgr.ManagerState.FadeTo, Time.timeScale, 1f, 20f, affectSEPitch: false);
			}
		}
		else
		{
			AimShadowSprite.color = new Color(1f, 1f, 1f, Mathf.Clamp(AimShadowSprite.color.a - Time.unscaledDeltaTime * 4f, 0f, AimShadowTransparency));
			AimMarkTransprency = Mathf.Lerp(AimMarkTransprency, 0f, 30f * Time.unscaledDeltaTime);
			SetAimRingTransparency(AimMarkTransprency);
			if (CamController.Inst.isFocussing)
			{
				CamController.Inst.FocusRecover(0.3f);
			}
			if (holdingHarpoonsGun)
			{
				PlayerMgr.Inst.PlayerCtrller.HideDaveHarpoonsGun();
				holdingHarpoonsGun = false;
			}
			AimModeTimer = 0f;
			playTimeSlowSE = false;
			if (timeSlowSE != null && timeSlowSE.volume >= 0.05f)
			{
				timeSlowSE.volume = Mathf.Lerp(timeSlowSE.volume, 0f, Time.deltaTime * 6f);
			}
		}
	}

	private void ResetFocusCheck()
	{
		if (IsFocusing)
		{
			CamController.Inst.FocusRecover(0.6f);
			CamController.Inst.finialClampAfterFocus = false;
			IsFocusing = false;
		}
	}

	private void SetAimRingTransparency(float transparency)
	{
		AimRingSprite.material.SetFloat(Transparency, transparency);
		AimArrowSprite.material.SetFloat(Transparency, transparency);
	}

	private bool ShootHarpoons()
	{
		if (holdingHarpoonsGun)
		{
			PlayerMgr.Inst.PlayerCtrller.HideDaveHarpoonsGun();
			holdingHarpoonsGun = false;
		}
		if (!PlayerMgr.Inst.PlayerCtrller.CanMotion || TopUI.inst.uI_AimSkill.skillCancle)
		{
			TopUI.inst.uI_AimSkill.useSkillDir = false;
			TopUI.inst.uI_AimSkill.skillCancle = false;
			return false;
		}
		if (CurrentHarpoonsRemain <= 0)
		{
			TopUI.inst.uI_AimSkill.useSkillDir = false;
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		List<UnitProperty> list = new List<UnitProperty>();
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (CurrentHarpoonsRemain <= 0)
			{
				break;
			}
			if (!(wand != null) || wand.WandCfg == null || !wand.passiveDaveHarpoonsEnable)
			{
				continue;
			}
			float extraScatterMulti = ((AimModeTimer >= AimModeStartDuration) ? ((AimModeTimer - AimModeStartDuration) * AimModeExtraScatterPerSeconds) : 2f);
			if (AimModeTimer >= AimModeStartDuration)
			{
				wand.ShootDaveHarpoonsDotsAim(CurrentHarpoonsRemain, extraScatterMulti);
				CurrentHarpoonsRemain = 0;
			}
			else
			{
				TopUI.inst.uI_AimSkill.useSkillDir = false;
				CurrentHarpoonsRemain -= wand.ShootDaveHarpoonsDots(CurrentHarpoonsRemain);
			}
			if (wand.passiveAutoWandShooterData != null && wand.passiveAutoWand && PlayerMgr.Inst.GetAutoWandScript(wand) != null)
			{
				flag3 = true;
				UnitProperty myPpt = PlayerMgr.Inst.GetAutoWandScript(wand).myPpt;
				if (!list.Contains(myPpt))
				{
					list.Add(myPpt);
				}
			}
			else
			{
				flag2 = true;
			}
			flag = true;
		}
		if (flag)
		{
			image_Fill.fillAmount = 0f;
			cg.alpha = 1f;
			ShockParam shockParam = default(ShockParam);
			shockParam.radius = 0.05f;
			shockParam.speed = 2f;
			shockParam.time = 0.16f;
			ShockParam shockParam2 = shockParam;
			float num = ((AimModeTimer >= AimModeStartDuration) ? (1f / (Mathf.Max(1f, AimModeTimer - AimModeStartDuration) * 2f)) : 1f);
			CamController.Inst.SetShock(shockParam2, (PlayerMgr.Inst.GetMousePoint() - PlayerMgr.Inst.PlayerPoint).normalized);
			if (flag2)
			{
				PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt);
				Vector3 position = PlayerMgr.Inst.PlayerPpt.transform.position;
				Vector3 normalized = (PlayerMgr.Inst.GetMousePoint() - position).normalized;
				playerPpt.TakeKnockback(-normalized * 6f);
				World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(PlayerMgr.Inst.PlayerEtt, playerPpt);
			}
			if (flag3)
			{
				foreach (UnitProperty item in list)
				{
					Vector3 normalized2 = (PlayerMgr.Inst.GetMousePoint() - item.transform.position).normalized;
					item.TakeKnockback(-normalized2 * 6f * ShootKnockBackRatio * num);
				}
			}
			ShootKnockBackRatio *= 0.5f;
			AimModeTimer = 0f;
		}
		return false;
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
