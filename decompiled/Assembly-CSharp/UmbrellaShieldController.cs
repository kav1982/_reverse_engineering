using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class UmbrellaShieldController : MonoBehaviour
{
	public class ShieldSplitData
	{
		public List<Vector3> points = new List<Vector3>();

		public float Damage;

		public float Duration;

		public Wand wand;
	}

	private class WandMpTransaction
	{
		private readonly Wand _wand;

		public float CurrentMp;

		public float MaxMp { get; private set; }

		public WandMpTransaction(Wand wand)
		{
			_wand = wand;
			Reset();
		}

		public void Commit()
		{
			_wand.CurrentMP = CurrentMp;
		}

		public void Reset()
		{
			CurrentMp = _wand.CurrentMP;
			MaxMp = _wand.MaxMP;
		}

		public void CostMP(float amount)
		{
			_wand.CostMp(amount);
			CurrentMp -= amount;
		}
	}

	private static float? _mp2hpRatio;

	private Entity ShieldEntity;

	private EntityQuery query;

	public int damageBlockThisFrame;

	public Dictionary<Wand, Entity> dicForEcsEntities = new Dictionary<Wand, Entity>();

	private static float? _invincibleTime;

	private static float? _damageRadius;

	private static int? _mp2damageRatio;

	[Tooltip("护盾大小")]
	public float ShieldSize = 1f;

	[Tooltip("护盾碰撞器")]
	public Collider ShieldCollider;

	[Tooltip("护盾跟随的目标")]
	public Transform ShieldAttachTrans;

	private GameObject Shield;

	private Spell40121MPRatioEffect ShieldMPRatioEffect;

	private Animator Anima;

	private ParticleSystem HitParticle;

	private UnitProperty ProtectTarget;

	private float InvincibleTimer;

	private static readonly int HitID = Animator.StringToHash("Hit");

	private static readonly int BreakID = Animator.StringToHash("Break");

	private SpellInitialParameter sip;

	[HideInInspector]
	public float spellMucusTime;

	[HideInInspector]
	public float spellMucusMoveSpeedRatio = 1f;

	[HideInInspector]
	public float spellMucusSpellSpeedRatio = 1f;

	[HideInInspector]
	public float spellVenomTime;

	[HideInInspector]
	public int spellVenomBonusStackCount;

	[HideInInspector]
	public int spellVenomOnceCount;

	[HideInInspector]
	public float spellFrozenTime;

	private Spell3129VoidExplosion.VoidExplosionData voidExplosionInfo;

	private static readonly Collider[] thunderColliderBuffer = new Collider[256];

	private static readonly HashSet<string> thunderColliderTags = new HashSet<string> { "Monster", "Destructible", "Butterfly", "RollBall", "Brittleness" };

	private int multiShootCount = 1;

	private List<ShieldSplitData> splitData = new List<ShieldSplitData>();

	private bool isShieldOn;

	private bool queryCreated;

	public int maxBlockDamage;

	public static float Mp2HpRatio
	{
		get
		{
			float valueOrDefault = _mp2hpRatio.GetValueOrDefault();
			if (!_mp2hpRatio.HasValue)
			{
				valueOrDefault = SpellConfig.dic[40121].float2 / 100f;
				_mp2hpRatio = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public static float InvincibleTime
	{
		get
		{
			float valueOrDefault = _invincibleTime.GetValueOrDefault();
			if (!_invincibleTime.HasValue)
			{
				valueOrDefault = SpellConfig.dic[40121].float1;
				_invincibleTime = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public static float DamageRadius
	{
		get
		{
			float valueOrDefault = _damageRadius.GetValueOrDefault();
			if (!_damageRadius.HasValue)
			{
				valueOrDefault = SpellConfig.dic[40121].float3;
				_damageRadius = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public static int Mp2DamageRatio
	{
		get
		{
			int valueOrDefault = _mp2damageRatio.GetValueOrDefault();
			if (!_mp2damageRatio.HasValue)
			{
				valueOrDefault = SpellConfig.dic[40121].int1;
				_mp2damageRatio = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	public bool CanUnderDamage => GetTargetWands().Any((Wand e) => e.CurrentMP >= GetWandOneHpAsMp(e.MaxMP));

	public static float mpCostRatio { get; private set; } = 1f;


	public float spellBurnTime { get; set; }

	public int burnDamagePerSeconds { get; set; }

	public int spellSplitCount { get; set; }

	public float criticalDragDamagePercent { get; set; }

	public float criticalDragEffectRadiu { get; set; }

	public int criticalDragApllyToCount { get; set; }

	public float criticalDragPullForce { get; set; }

	public float endThunderHitRadiu { get; set; }

	public float endThunderHitPercent { get; set; }

	public float endTHunderHitChance { get; set; }

	private void OnEnable()
	{
		World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
		if (defaultGameObjectInjectionWorld != null && defaultGameObjectInjectionWorld.IsCreated)
		{
			if (query != default(EntityQuery))
			{
				query.Dispose();
			}
			query = defaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<Spell4012MagicShieldData>());
			queryCreated = true;
		}
	}

	private void OnDisable()
	{
		if ((bool)Shield)
		{
			BreakShieldObject();
		}
		SafeDisposeQuery();
	}

	private void OnDestroy()
	{
		SafeDisposeQuery();
	}

	private void SafeDisposeQuery()
	{
		if (!queryCreated)
		{
			return;
		}
		try
		{
			World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
			if (defaultGameObjectInjectionWorld != null && defaultGameObjectInjectionWorld.IsCreated && defaultGameObjectInjectionWorld.EntityManager.IsQueryValid(query))
			{
				query.Dispose();
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			queryCreated = false;
			query = default(EntityQuery);
		}
	}

	private void Update()
	{
		UpdateWandDictionary();
		UpdateShieldState();
		UpdateShieldMPRatioState();
		maxBlockDamage = UpdateMaxBlockDamage();
		CheckBlockDamage();
		UpdateSplitData();
		InvincibleTimer -= Time.deltaTime;
	}

	private void LateUpdate()
	{
		if ((bool)Shield)
		{
			Shield.transform.position = ShieldAttachTrans.position;
		}
	}

	public void Hide()
	{
		if ((bool)Shield)
		{
			Shield.SetActive(value: false);
		}
	}

	public void Show()
	{
		if ((bool)Shield)
		{
			Shield.SetActive(value: true);
		}
	}

	private void UpdateWandDictionary()
	{
		EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
		List<Wand> list = (from kv in dicForEcsEntities
			where kv.Value == Entity.Null || !em.Exists(kv.Value)
			select kv.Key).ToList();
		using NativeArray<Entity> nativeArray = query.ToEntityArray(Allocator.TempJob);
		foreach (Wand item in list)
		{
			foreach (Entity item2 in nativeArray)
			{
				if (em.Exists(item2) && em.GetComponentData<SpellComponentData>(item2).Wand == item)
				{
					dicForEcsEntities[item] = item2;
					break;
				}
			}
		}
	}

	private Wand[] GetTargetWands()
	{
		List<Wand> list = PlayerMgr.Inst.Wands.Where((Wand w) => (object)w != null && w.passiveUmbrellaEnable && w.passiveUmbrellaMpFull && w.WandCfg != null).ToList();
		if (list.Count == 0)
		{
			return Array.Empty<Wand>();
		}
		return list.ToArray();
	}

	private float GetTargetWandsMpRatio()
	{
		Wand[] targetWands = GetTargetWands();
		float num = targetWands.Sum((Wand e) => e.CurrentMP);
		float num2 = targetWands.Sum((Wand e) => e.MaxMP);
		if (num2 == 0f)
		{
			return 0f;
		}
		return num / num2;
	}

	public void UpdateShieldBuffState([CanBeNull] Wand targetWand)
	{
		spellMucusTime = 0f;
		spellMucusMoveSpeedRatio = 1f;
		spellMucusSpellSpeedRatio = 1f;
		spellVenomTime = 0f;
		spellVenomBonusStackCount = 0;
		spellVenomOnceCount = 0;
		spellFrozenTime = 0f;
		spellBurnTime = 0f;
		burnDamagePerSeconds = 0;
		spellSplitCount = 0;
		criticalDragDamagePercent = 0f;
		criticalDragEffectRadiu = 0f;
		criticalDragApllyToCount = 0;
		criticalDragPullForce = 0f;
		endThunderHitRadiu = 0f;
		endThunderHitPercent = 0f;
		endTHunderHitChance = 0f;
		multiShootCount = 1;
		voidExplosionInfo = null;
		sip = null;
		SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
		mpCostRatio = 1f;
		List<SlotData> list = new List<SlotData>();
		if ((bool)targetWand)
		{
			list.AddRange(targetWand.GetWandAllEnhanceList());
			builder.ApplyWandEffect(targetWand, null, multiWand: true);
		}
		foreach (SlotData item in list)
		{
			SpellConfig finalConfig = item.GetFinalConfig();
			if (finalConfig.mpCostMulDivCorrection != 0f)
			{
				mpCostRatio *= finalConfig.mpCostMulDivCorrection / 100f;
			}
			switch (finalConfig.abilityType)
			{
			case SpellAbilityType.MucusCrystal:
				spellMucusTime = Mathf.Max(finalConfig.float1);
				spellMucusMoveSpeedRatio *= finalConfig.float2 / 100f;
				spellMucusSpellSpeedRatio *= finalConfig.float3 / 100f;
				break;
			case SpellAbilityType.VenomCrystal:
				spellVenomTime = Mathf.Max(finalConfig.float1, spellVenomTime);
				spellVenomBonusStackCount += finalConfig.int1;
				spellVenomOnceCount += finalConfig.int2;
				break;
			case SpellAbilityType.Multishot:
				multiShootCount += finalConfig.int1;
				break;
			case SpellAbilityType.SpellSplit:
				spellSplitCount += finalConfig.int1;
				break;
			case SpellAbilityType.DeathInfect:
				if (voidExplosionInfo == null)
				{
					voidExplosionInfo = new Spell3129VoidExplosion.VoidExplosionData();
				}
				voidExplosionInfo.ExplosionRange = Mathf.Max(voidExplosionInfo.ExplosionRange, finalConfig.float1 * GetRadiusRatio());
				voidExplosionInfo.HpToDmgRatio += finalConfig.float2 / 100f;
				voidExplosionInfo.InstantKillRatio = Mathf.Max(finalConfig.float3 / 100f, voidExplosionInfo.InstantKillRatio);
				break;
			case SpellAbilityType.Frozen:
				spellFrozenTime += finalConfig.float1;
				break;
			case SpellAbilityType.ThunderCrystal:
				endThunderHitRadiu = Mathf.Max(endThunderHitRadiu, finalConfig.float1);
				endThunderHitPercent += finalConfig.float2 / 100f;
				endTHunderHitChance = Mathf.Max(endTHunderHitChance, finalConfig.float3 / 100f);
				break;
			case SpellAbilityType.PullForceCrystal:
				criticalDragDamagePercent = Mathf.Max((float)finalConfig.int1 / 100f, criticalDragDamagePercent);
				criticalDragApllyToCount += finalConfig.int2;
				criticalDragPullForce = Mathf.Max(finalConfig.float2, criticalDragPullForce);
				criticalDragEffectRadiu = Mathf.Max(finalConfig.float1, criticalDragEffectRadiu);
				break;
			case SpellAbilityType.FireCrystal:
				spellBurnTime = Mathf.Max(spellBurnTime, finalConfig.float2);
				burnDamagePerSeconds += finalConfig.int1;
				break;
			}
		}
		SpellShootGroup spellShootGroup = new SpellShootGroup();
		SpellShootData spellShootData = new SpellShootData(new SlotData
		{
			id = 10001
		}, spellShootGroup)
		{
			EnhanceList = list.ToArray()
		};
		spellShootGroup.Shoots = new SpellShootData[1] { spellShootData };
		builder.ApplySpellShootDataEffect(spellShootData);
		sip = builder.Build(ShootSpellSpatialInfo.ToPoint(Vector3.zero, Vector3.zero));
		if ((bool)targetWand)
		{
			mpCostRatio *= targetWand.GetWandMpCorrection();
			if (targetWand != sip.shooterWand)
			{
				SIPProcessWandEffect(sip, targetWand);
			}
		}
		if (sip.radiuDecreaseRatio < 1f)
		{
			sip.finalDamageRatio *= 1f + GeneralTool.GetSpellRadiusToDamageRatio(DamageRadius * ShieldSize * GetRadiusRatio(), sip.radiuDecreaseRatio, sip.radiuDcreaseTransIntoDamageRatio);
		}
	}

	private void SIPProcessWandEffect(SpellInitialParameter data, Wand shooterWand)
	{
		WandConfig wandCfg = shooterWand.WandCfg;
		data.finalDamageRatio *= shooterWand.WandCfg.damageCorrection / 100f;
		data.extraSizeRatio += PlayerMgr.Inst.EffectRadiuRatioFromWandAbility() / 100f;
		data.finalDamageRatio *= shooterWand.passiveDamageRatio;
		data.extraCriticalChance += shooterWand.WandCfg.criticalChance / 100f;
		data.extraScatter += shooterWand.WandCfg.GetWandScatter();
		data.equalScatter = shooterWand.passiveEqualAngleDistribution;
		data.zeroAngleShift = data.equalScatter;
		data.finalMovementType = data.shooterWand.spellFinalMovementType;
		switch (shooterWand.WandCfg.specialAbility)
		{
		case WandAbility.RandomAllColor:
		{
			data.ColorType = SpellTools.GetRandomColor();
			data.IsRandomColor = true;
			int? num2 = data.ColorType.ToSpellId(1);
			if (num2.HasValue)
			{
				data.extraEnhanceIds.Add(num2.Value);
			}
			break;
		}
		case WandAbility.RandomBaseColor:
		{
			data.ColorType = SpellTools.GetRandomBaseColor();
			data.IsRandomColor = true;
			int? num = data.ColorType.ToSpellId(1);
			if (num.HasValue)
			{
				data.extraEnhanceIds.Add(num.Value);
			}
			break;
		}
		case WandAbility.HigherKnockBack:
			data.finalKnockBackRatio *= wandCfg.float2 / 100f;
			break;
		case WandAbility.LowerFriendlyFire:
			data.undifferDamageRatio *= wandCfg.float1 / 100f;
			break;
		case WandAbility.RandomRadiu:
			data.aroundCasterRadiuRatio = UnityEngine.Random.Range(wandCfg.float1, wandCfg.float2);
			break;
		case WandAbility.SpellReverseDirection:
			data.reverseDirection = !data.reverseDirection;
			break;
		}
	}

	private void UpdateShieldState()
	{
		Wand[] targetWands = GetTargetWands();
		if ((bool)Shield && targetWands.Length == 0)
		{
			BreakShieldObject();
		}
		if (!Shield && targetWands.Length != 0)
		{
			SpawnShieldObject();
		}
		if ((bool)Shield)
		{
			Shield.transform.localScale = ShieldSize * ShieldAttachTrans.lossyScale;
		}
	}

	private void UpdateShieldMPRatioState()
	{
		if ((bool)ShieldMPRatioEffect)
		{
			ShieldMPRatioEffect.Ratio = GetTargetWandsMpRatio();
		}
	}

	private void SpawnShieldObject()
	{
		if (!Shield.LogIf("不能重复生成护盾物体"))
		{
			isShieldOn = true;
			SEMgr.Inst.spell4012UmbrellaSpawn.PlaySE();
			Shield = UnityEngine.Object.Instantiate(ABResources.LoadAsset<GameObject>("Prefabs/Spell/4012/40121Shield"));
			Shield.SetActive(value: true);
			Shield.transform.localRotation = Quaternion.Euler(-75f, 0f, 0f);
			Shield.transform.position = ShieldAttachTrans.position;
			ShieldMPRatioEffect = Shield.GetComponent<Spell40121MPRatioEffect>();
			Anima = Shield.GetComponent<Animator>();
			HitParticle = Shield.transform.Find("HitParticle").GetComponent<ParticleSystem>();
			if ((bool)ShieldCollider)
			{
				ShieldCollider.enabled = true;
			}
			InvincibleTimer = 0f;
		}
	}

	private void BreakShieldObject()
	{
		if (!Shield.LogIfNot("没有保护罩，无法丢弃"))
		{
			Animator a = Anima;
			DOTween.Sequence().AppendInterval(0.05f).AppendCallback(delegate
			{
				a.SetTrigger(BreakID);
			});
			UnityEngine.Object.Destroy(Shield, 2f);
			SEMgr.Inst.spell4012UmbrellaBreak.PlaySE();
			Shield = null;
			ShieldMPRatioEffect = null;
			Anima = null;
			HitParticle = null;
			if ((bool)ShieldCollider)
			{
				ShieldCollider.enabled = false;
			}
		}
	}

	private (int remainHp, float costMp) CostMpByHp(int hp, bool onlyCheck = false)
	{
		WandMpTransaction[] array = (from e in GetTargetWands()
			select new WandMpTransaction(e)).ToArray();
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		while (hp > 0 && array.Length != 0)
		{
			if (num2 >= array.Length)
			{
				num2 = 0;
			}
			float? num4 = CostMpByOneHpUseWand(array[num2]);
			num2++;
			if (num4.HasValue)
			{
				hp--;
				num = 0;
				num3 += num4.Value;
				continue;
			}
			num++;
			if (num > array.Length)
			{
				break;
			}
		}
		if (!onlyCheck)
		{
			WandMpTransaction[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Commit();
			}
		}
		return (hp, num3);
	}

	private float? CostMpByOneHpUseWand(WandMpTransaction wand)
	{
		float wandOneHpAsMp = GetWandOneHpAsMp(wand.MaxMp);
		if (wand.CurrentMp < wandOneHpAsMp)
		{
			return null;
		}
		wand.CostMP(wandOneHpAsMp);
		return wandOneHpAsMp;
	}

	public int UnderDamage(float damage)
	{
		if (damage <= 0f || InvincibleTimer > 0f)
		{
			return 0;
		}
		Wand[] targetWands = GetTargetWands();
		(int remainHp, float costMp) tuple = CostMpByHp((int)damage);
		int item = tuple.remainHp;
		float item2 = tuple.costMp;
		Wand[] array = targetWands;
		foreach (Wand wand in array)
		{
			Entity value;
			bool num = !dicForEcsEntities.TryGetValue(wand, out value);
			bool flag = value == Entity.Null;
			bool flag2 = !World.DefaultGameObjectInjectionWorld.EntityManager.Exists(value);
			bool flag3 = !World.DefaultGameObjectInjectionWorld.EntityManager.HasBuffer<MagicShieldDmgEvent>(value);
			if (!(num || flag || flag2 || flag3))
			{
				float mp = item2 * GetDamageRatio() / (float)targetWands.Length;
				UpdateShieldBuffState(wand);
				MakeDamageDots(mp, wand);
			}
		}
		InvincibleTimer = InvincibleTime;
		HitParticle.Play();
		SEMgr.Inst.spell4012UmbrellaHit.PlaySE();
		Anima.SetTrigger(HitID);
		return item;
	}

	private void MakeDamageDots(float mp, Wand wand)
	{
		Entity entity = dicForEcsEntities[wand];
		if (entity == Entity.Null || !World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<LocalTransform>(entity))
		{
			return;
		}
		float num = (float)Mp2DamageRatio * mp;
		ShieldSplitData shieldSplitData = null;
		if (spellSplitCount > 0)
		{
			shieldSplitData = new ShieldSplitData();
			splitData.Add(shieldSplitData);
			shieldSplitData.Damage = num * 0.33f;
			shieldSplitData.Duration = 0.25f;
			shieldSplitData.wand = wand;
		}
		float num2 = DamageRadius * ShieldSize * GetRadiusRatio();
		for (int i = 0; i < multiShootCount; i++)
		{
			Vector3 vector = base.transform.position + Tool2D.GetDir(360f / (float)multiShootCount * (float)i + 90f) * num2 / 2f;
			if (multiShootCount == 1)
			{
				vector = base.transform.position;
			}
			Effect_Explosion(vector, GetRadiusRatio());
			World.DefaultGameObjectInjectionWorld.EntityManager.GetBuffer<MagicShieldDmgEvent>(entity).Add(new MagicShieldDmgEvent
			{
				damage = num,
				Position = vector,
				radius = num2
			});
			if (spellSplitCount > 0)
			{
				for (int j = 0; j < spellSplitCount; j++)
				{
					shieldSplitData.points.Add(vector + Tool2D.GetDir(360f / (float)spellSplitCount * (float)j) * num2 * 0.5f);
				}
			}
		}
	}

	private int DamageCanBlockCurrent()
	{
		Wand[] targetWands = GetTargetWands();
		if (targetWands.Length == 0)
		{
			return 0;
		}
		int num = 0;
		Wand[] array = targetWands;
		foreach (Wand wand in array)
		{
			float wandOneHpAsMp = GetWandOneHpAsMp(wand.MaxMP);
			num += Mathf.FloorToInt(wand.CurrentMP / wandOneHpAsMp);
		}
		return num;
	}

	private int UpdateMaxBlockDamage()
	{
		return DamageCanBlockCurrent();
	}

	private void CheckBlockDamage()
	{
		int num = damageBlockThisFrame;
		if (num > 0 && isShieldOn)
		{
			UnderDamage(num);
			damageBlockThisFrame = 0;
		}
	}

	public float GetDamageRatio()
	{
		if (sip == null)
		{
			return 1f;
		}
		return (sip.extraDamageRatio + 1f) * sip.finalDamageRatio;
	}

	public float GetCriticalChance()
	{
		if (sip == null)
		{
			return 0f;
		}
		return sip.extraCriticalChance;
	}

	public static float GetWandOneHpAsMp(float maxMp)
	{
		return SpellConfig.dic[40121].float2;
	}

	private void UpdateSplitData()
	{
		for (int i = 0; i < splitData.Count; i++)
		{
			splitData[i].Duration -= Time.deltaTime;
			if (!(splitData[i].Duration <= 0f))
			{
				continue;
			}
			foreach (Vector3 point in splitData[i].points)
			{
				MakeSplitDamage(point, splitData[i].Damage, splitData[i].wand);
			}
			splitData.Remove(splitData[i]);
		}
	}

	private void MakeSplitDamage(Vector3 position, float dmg, Wand wand)
	{
		EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		Entity entity = dicForEcsEntities[wand];
		if (entityManager.HasBuffer<MagicShieldDmgEvent>(entity))
		{
			float radius = DamageRadius * ShieldSize * GetRadiusRatio() * 0.5f;
			Effect_Explosion(position, GetRadiusRatio() * 0.5f);
			entityManager.GetBuffer<MagicShieldDmgEvent>(entity).Add(new MagicShieldDmgEvent
			{
				damage = dmg,
				Position = position,
				radius = radius
			});
		}
	}

	public float GetRadiusRatio()
	{
		float num = 1f;
		if (!ProtectTarget)
		{
			return num;
		}
		UnitType unitType = ProtectTarget.unitCfg.unitType;
		if (unitType != 0 && unitType != UnitType.Teammate && unitType != UnitType.TeammateNotAttack)
		{
			return num;
		}
		if (sip == null)
		{
			return num;
		}
		num += sip.extraSizeRatio;
		return num * sip.finalSizeRatio;
	}

	private void Effect_Explosion(Vector3 targetPos, float radiusRatio)
	{
		GameObject gO = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/4012/4012_Explosion", targetPos, 2f);
		gO.transform.rotation = Quaternion.Euler(-75f, 0f, 0f);
		gO.transform.localScale = DamageRadius * ShieldSize * radiusRatio * Vector3.one;
		gO.GetComponent<Spell40121ExplosionMPRatioEffect>().Ratio = GetTargetWandsMpRatio();
	}
}
