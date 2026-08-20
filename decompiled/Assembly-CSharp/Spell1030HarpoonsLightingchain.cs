using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Spell1030HarpoonsLightingchain : MonoBehaviour
{
	public class ChainData
	{
		public GameObject LineObject;

		public LineRenderer LightningLine;

		public float RemainDuration;

		public UnitProperty Target1;

		public UnitProperty Target2;

		public Vector3 Target1Pos = Vector3.zero;

		public Vector3 Target2Pos = Vector3.zero;

		public bool IsInitPoint;

		public GameObject HitEffectObject;
	}

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public LayerMask attackLayer;

	private UnitProperty ownerPpt;

	private UnitProperty stickTarget;

	private Vector3 stickPosition = Vector3.zero;

	private UnitProperty newTarget;

	private Vector3 newTargetPosition = Vector3.zero;

	private float damage;

	private RaycastHit hit;

	private SpellColorType colorType;

	private Wand casterWand;

	public float zipInterval;

	private float zipIntervalTimer;

	private float conductDamageRatio;

	private float conductRange;

	private int conductRemainCounter;

	[Header("Color")]
	public Material mat_ECFrozen;

	public Material mat_ECMucus;

	public Material mat_ECPlayer;

	public Material mat_ECVenom;

	public Material mat_ECFire;

	public Material mat_ECThunder;

	public Material mat_ECVoid;

	private float frozenTime;

	private float spellMucusMoveSpeedRatio = 1f;

	private float spellMucusSpellSpeedRatio = 1f;

	private float spellMucusTime;

	private float venomTime;

	private float venomApplyCount;

	private float burnDamage;

	private float burnTime;

	private float lightCriticalChance;

	private float criticalPullDamageRatio;

	private float criticalPullRange;

	private int criticalPullTargetsCount;

	private float criticalPullDragForce;

	private float sizeRatio = 1f;

	private float finalSizeRatio = 1f;

	private float knockBackRatio = 1f;

	private float finalKnockBackRatio = 1f;

	private bool chargeEnable;

	private Spell3129VoidExplosion.VoidExplosionData voidInfo;

	private List<ChainData> lightningChainData = new List<ChainData>();

	public float chainExistDuration;

	private static HashSet<string> pullCrystalTargetTags = new HashSet<string> { "Monster" };

	private static Collider[] pullCrystalCollidersBuffer = new Collider[256];

	private bool zipTargetIsAlive
	{
		get
		{
			if (stickTarget != null)
			{
				return stickTarget.gameObject.activeSelf;
			}
			return false;
		}
	}

	private void OnEnable()
	{
		frozenTime = 0f;
		spellMucusMoveSpeedRatio = 1f;
		spellMucusSpellSpeedRatio = 1f;
		spellMucusTime = 0f;
		venomTime = 0f;
		venomApplyCount = 0f;
		colorType = SpellColorType.Player;
		burnDamage = 0f;
		burnTime = 0f;
		criticalPullDamageRatio = 0f;
		criticalPullRange = 0f;
		criticalPullTargetsCount = 0;
		criticalPullDragForce = 0f;
		sizeRatio = 1f;
		finalSizeRatio = 1f;
		knockBackRatio = 1f;
		finalKnockBackRatio = 1f;
		chargeEnable = false;
		voidInfo = null;
		zipIntervalTimer = 0f;
		conductDamageRatio = 0f;
		stickTarget = null;
		newTarget = null;
		conductRange = 0f;
		conductRemainCounter = 4;
		lightningChainData.Clear();
	}

	private void Update()
	{
		UpdateLineEffect();
		if (conductRemainCounter <= 0)
		{
			if (lightningChainData.Count > 0)
			{
				return;
			}
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		if (zipTargetIsAlive)
		{
			zipIntervalTimer += Time.deltaTime;
		}
		if (zipIntervalTimer >= zipInterval || !zipTargetIsAlive)
		{
			UnitProperty nearestTargetablePpt = LevelMgr.Inst.CurrentRoomCtrller.GetNearestTargetablePpt(stickPosition, conductRange, new GameObject[1] { stickTarget.gameObject });
			if (nearestTargetablePpt == null)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				return;
			}
			SetNewStickTarget(nearestTargetablePpt);
			zipIntervalTimer -= zipInterval;
		}
	}

	private void LateUpdate()
	{
		if (zipTargetIsAlive)
		{
			stickPosition = stickTarget.transform.position;
		}
	}

	private void SetNewStickTarget(UnitProperty targetPpt)
	{
		damage *= conductDamageRatio;
		DealDamageToConductTarget(targetPpt);
		newTarget = targetPpt;
		SpawnNewLine(isInitPoint: false);
		conductRemainCounter--;
		stickTarget = targetPpt;
		stickPosition = stickTarget.transform.position;
	}

	public void SpawnNewLine(bool isInitPoint)
	{
		ChainData chainData = new ChainData();
		chainData.LineObject = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 10301 + "LightningChainLine", base.transform.position, quaternion.identity);
		chainData.LightningLine = chainData.LineObject.GetComponent<LineRenderer>();
		chainData.LightningLine.positionCount = 2;
		chainData.RemainDuration = chainExistDuration;
		chainData.Target1 = stickTarget;
		chainData.Target2 = newTarget;
		if (stickTarget != null)
		{
			chainData.Target1Pos = stickTarget.transform.position;
		}
		if (newTarget != null)
		{
			chainData.Target2Pos = newTarget.transform.position;
		}
		chainData.IsInitPoint = isInitPoint;
		float num = (GeneralTool.IsLowFpsOptimizeActive(60f) ? Mathf.Pow(GameMgr.Inst.GetFps() / 60f, 5f) : 1f);
		if (UnityEngine.Random.Range(0f, 1f) <= num)
		{
			Vector3 point = (isInitPoint ? chainData.Target1Pos : chainData.Target2Pos);
			chainData.HitEffectObject = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 10301 + "/10301_LightningChainHit", point, quaternion.identity, chainExistDuration);
		}
		lightningChainData.Add(chainData);
	}

	public void UpdateLineEffect()
	{
		for (int num = lightningChainData.Count - 1; num >= 0; num--)
		{
			ChainData chainData = lightningChainData[num];
			chainData.RemainDuration -= Time.deltaTime;
			if ((bool)chainData.HitEffectObject)
			{
				chainData.HitEffectObject.transform.position = (chainData.IsInitPoint ? chainData.Target1Pos : chainData.Target2Pos);
			}
			if (chainData.RemainDuration <= 0f)
			{
				chainData.LightningLine.positionCount = 0;
				ObjPoolMgr.Inst.RecycleGO(chainData.LineObject);
				lightningChainData.Remove(chainData);
			}
			else
			{
				if ((bool)chainData.Target1 && chainData.Target1.gameObject.activeInHierarchy && !chainData.Target1.isUnitDead)
				{
					chainData.Target1Pos = chainData.Target1.transform.position + new Vector3(0f, 0.3f, -0.3f);
				}
				if ((bool)chainData.Target2 && chainData.Target2.gameObject.activeInHierarchy && !chainData.Target2.isUnitDead)
				{
					chainData.Target2Pos = chainData.Target2.transform.position + new Vector3(0f, 0.3f, -0.3f);
				}
				chainData.LightningLine.SetPosition(0, chainData.Target1Pos);
				chainData.LightningLine.SetPosition(1, chainData.Target2Pos);
			}
		}
	}

	private void OnDisable()
	{
		for (int num = lightningChainData.Count - 1; num >= 0; num--)
		{
			lightningChainData[num].LightningLine.positionCount = 0;
			ObjPoolMgr.Inst.RecycleGO(lightningChainData[num].LineObject);
		}
	}

	private void DealDamageToConductTarget(UnitProperty targetPpt)
	{
		TakeDamageInfo takeDamageInfo = targetPpt.TakeDamage(damage, ownerPpt, new TakeDamageInfo
		{
			criticalChance = lightCriticalChance
		});
		if (!chargeEnable || !(casterWand != null))
		{
			return;
		}
		WandConfig wandCfg = casterWand.WandCfg;
		if (wandCfg != null && wandCfg.PostslotSpellHitChargeRatio > 0f)
		{
			casterWand.ChargePostSlots(casterWand.WandCfg.PostslotSpellHitChargeRatio);
			return;
		}
		if (takeDamageInfo.isTargetDead)
		{
			wandCfg = casterWand.WandCfg;
			if (wandCfg != null && wandCfg.PostslotKillEnemyChargeRatio > 0f)
			{
				casterWand.ChargePostSlots(casterWand.WandCfg.PostslotKillEnemyChargeRatio);
				return;
			}
		}
		if (takeDamageInfo.isCriticalDamage)
		{
			wandCfg = casterWand.WandCfg;
			if (wandCfg != null && wandCfg.PostslotCriticalHitChargeRatio > 0f)
			{
				casterWand.ChargePostSlots(casterWand.WandCfg.PostslotCriticalHitChargeRatio);
			}
		}
	}

	public void LightningChainDataIniatialize(UnitProperty ownerPpt, UnitProperty targetPpt, float chainDamage, float detectRange, float conductNewTargetDamageRatio, Wand targetWand = null, SpellBase targetSpell = null)
	{
		this.ownerPpt = ownerPpt;
		stickTarget = targetPpt;
		damage = chainDamage;
		conductDamageRatio = conductNewTargetDamageRatio;
		conductRange = detectRange;
		if (targetSpell != null)
		{
			_ = targetSpell.damageRatio;
			_ = targetSpell.finalDamageRatio;
			frozenTime = targetSpell.spellFrozenTime;
			spellMucusTime = targetSpell.spellMucusTime;
			spellMucusMoveSpeedRatio = targetSpell.spellMucusMoveSpeedRatio;
			spellMucusSpellSpeedRatio = targetSpell.spellMucusSpellSpeedRatio;
			venomTime = targetSpell.spellVenomTime;
			venomApplyCount = targetSpell.spellVenomOnceCount;
			burnDamage = targetSpell.burnHpRatioPerSeconds;
			burnTime = targetSpell.spellBurnTime;
			lightCriticalChance = targetSpell.GetCriticalChance();
			sizeRatio = targetSpell.radiusRatio;
			finalSizeRatio = targetSpell.finalRadiusRatio;
			knockBackRatio = targetSpell.knockbackRatio;
			finalKnockBackRatio = targetSpell.finalKnockbackRatio;
			chargeEnable = targetSpell.wandChargeData != null;
			criticalPullDamageRatio = targetSpell.criticalDragDamagePercent;
			criticalPullRange = targetSpell.criticalDragEffectRadiu;
			criticalPullTargetsCount = targetSpell.criticalDragApllyToCount;
			criticalPullDragForce = targetSpell.criticalDragPullForce;
			colorType = targetSpell.ColorType;
			voidInfo = targetSpell.voidExplosionInfo;
		}
		casterWand = targetWand;
		SetNewStickTarget(targetPpt);
		SpawnNewLine(isInitPoint: true);
	}

	private void ActivePullCrystal(UnitProperty targetPpt)
	{
		float radius = criticalPullRange * sizeRatio * finalSizeRatio;
		Vector3 position = targetPpt.transform.position;
		int num = Mathf.CeilToInt(criticalPullDamageRatio * damage);
		int mask = LayerMask.GetMask("Monster", "Monster_Fly", "Monster_Ghost");
		int[] array = GeneralTool.GetRandomIndex(GeneralTool.GetCollidersNonAlloc(position, radius, pullCrystalCollidersBuffer, pullCrystalTargetTags, mask), criticalPullTargetsCount).ToArray();
		bool flag = false;
		int[] array2 = array;
		foreach (int num2 in array2)
		{
			UnitProperty component = pullCrystalCollidersBuffer[num2].GetComponent<UnitProperty>();
			if (component != null && component != targetPpt)
			{
				TakeDamageInfo info = new TakeDamageInfo
				{
					canRebound = false,
					damage = num,
					attackerPpt = ownerPpt,
					criticalChance = lightCriticalChance
				};
				flag = true;
				SetElementEffect(component);
				Spell3101PullCrystal component2 = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + 31121, base.transform.position, quaternion.identity, 0.5f).GetComponent<Spell3101PullCrystal>();
				component2.SetColor(colorType);
				if (component2.tsf_Layer != null)
				{
					component2.tsf_Layer.localScale = base.transform.localScale;
				}
				component2.SetChainTargetTransform(targetPpt.gameObject.transform, pullCrystalCollidersBuffer[num2].transform);
				component.TakeKnockback((targetPpt.transform.position - pullCrystalCollidersBuffer[num2].transform.position).normalized * criticalPullDragForce * knockBackRatio * finalKnockBackRatio);
				component.TakeDamage(num, ownerPpt, info);
				Vector3 position2 = pullCrystalCollidersBuffer[num2].transform.position + component2.chainBaseHeight;
				component2.CreateHitEffect(colorType, position2);
			}
		}
		if (flag)
		{
			SEMgr.Inst.spell3121Energy.PlaySE().pitch = UnityEngine.Random.Range(0.5f, 1.5f);
		}
	}

	private void SpawnHitObject(Vector3 position)
	{
		string path = "Prefabs/Spell/30071/30071_Hit_" + colorType;
		ObjPoolMgr.Inst.GetGO(path, position, 2f);
	}

	public void SetElementEffect(UnitProperty targetPpt)
	{
		if (spellMucusTime > 0f)
		{
			targetPpt.SetMucus(spellMucusTime, spellMucusMoveSpeedRatio, spellMucusSpellSpeedRatio);
		}
		if (venomTime > 0f)
		{
			targetPpt.SetVenom(venomTime, venomApplyCount);
		}
		if (frozenTime > 0f)
		{
			targetPpt.SetFrozen(frozenTime);
		}
		if (burnTime > 0f)
		{
			targetPpt.SetBurn(burnTime, burnDamage);
		}
		if (voidInfo != null)
		{
			targetPpt.SetVoid(voidInfo);
		}
	}
}
