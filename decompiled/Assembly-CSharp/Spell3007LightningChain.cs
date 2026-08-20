using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Spell3007LightningChain : MonoBehaviour
{
	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public LayerMask attackLayer;

	private UnitProperty ownerPpt;

	private Transform tsf_Spell1;

	private Transform tsf_Spell2;

	private float damage;

	private RaycastHit hit;

	private SpellColorType colorType;

	private Wand casterWand;

	private SpellBase spell1;

	private SpellBase spell2;

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

	private static HashSet<string> pullCrystalTargetTags = new HashSet<string> { "Monster" };

	private static Collider[] pullCrystalCollidersBuffer = new Collider[256];

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
		spell1 = null;
		spell2 = null;
		lr_Laser.colorGradient = lr_Laser.colorGradient.GetGradientWithTransparent(new float[2] { 1f, 1f }, DataMgr.settingData.FinalSpellTransparent);
		lr_Shadow.colorGradient = lr_Shadow.colorGradient.GetGradientWithTransparent(new float[2] { 1f, 1f }, DataMgr.settingData.FinalSpellTransparent);
	}

	private void Update()
	{
		if (!tsf_Spell1.gameObject.activeSelf || !tsf_Spell2.gameObject.activeSelf || tsf_Spell1.gameObject == null || tsf_Spell2.gameObject == null)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		if (spell1 is Spell4019BiAnLethalBlade spell4019BiAnLethalBlade)
		{
			Spell4019BiAnLethalBlade spell4019BiAnLethalBlade2 = (Spell4019BiAnLethalBlade)spell1;
			if (spell4019BiAnLethalBlade.currentState == Spell4019BiAnLethalBlade.BladeState.Follow || spell4019BiAnLethalBlade2.currentState == Spell4019BiAnLethalBlade.BladeState.Follow)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
				return;
			}
		}
		if (Physics.Raycast(tsf_Spell1.position, tsf_Spell2.position - tsf_Spell1.position, out hit, 100f, attackLayer) && (tsf_Spell1.position - hit.point).sqrMagnitude <= (tsf_Spell1.position - tsf_Spell2.position).sqrMagnitude)
		{
			UnitProperty component = hit.transform.GetComponent<UnitProperty>();
			SetElementEffect(component);
			TakeDamageInfo takeDamageInfo = component.TakeDamage(damage, ownerPpt, new TakeDamageInfo
			{
				criticalChance = lightCriticalChance
			});
			if (hit.transform.tag == "Monster")
			{
				if (takeDamageInfo.isCriticalDamage)
				{
					ActivePullCrystal(takeDamageInfo.beHitPpt);
				}
				if (chargeEnable && casterWand != null)
				{
					WandConfig wandCfg = casterWand.WandCfg;
					if (wandCfg != null && wandCfg.PostslotSpellHitChargeRatio > 0f)
					{
						casterWand.ChargePostSlots(casterWand.WandCfg.PostslotSpellHitChargeRatio);
					}
					else
					{
						if (takeDamageInfo.isTargetDead)
						{
							wandCfg = casterWand.WandCfg;
							if (wandCfg != null && wandCfg.PostslotKillEnemyChargeRatio > 0f)
							{
								casterWand.ChargePostSlots(casterWand.WandCfg.PostslotKillEnemyChargeRatio);
								goto IL_027d;
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
				}
			}
			goto IL_027d;
		}
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(tsf_Spell1.position) + new Vector3(0f, 0f, 0.05f));
		lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(tsf_Spell2.position) + new Vector3(0f, 0f, 0.05f));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(tsf_Spell1, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(tsf_Spell2, 1.05f));
		return;
		IL_027d:
		SpawnHitObject(hit.transform.position);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		SEMgr.Inst.spell3007Hit.PlaySE();
	}

	public void Iniatialize(UnitProperty ownerPpt, Transform tsf_Spell1, Transform tsf_Spell2, float chainDamage, Wand targetWand = null, SpellBase targetSpell = null, SpellBase targetSpell2 = null)
	{
		if (!tsf_Spell1.gameObject.activeSelf || !tsf_Spell1.gameObject.activeSelf)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			return;
		}
		this.ownerPpt = ownerPpt;
		spell1 = targetSpell;
		spell2 = targetSpell2;
		this.tsf_Spell1 = tsf_Spell1;
		this.tsf_Spell2 = tsf_Spell2;
		damage = chainDamage;
		float f = 1f;
		if (targetSpell != null)
		{
			f = targetSpell.damageRatio * targetSpell.finalDamageRatio;
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
			switch (targetSpell.ColorType)
			{
			case SpellColorType.Frozen:
				if (lr_Laser.material != mat_ECFrozen)
				{
					lr_Laser.material = mat_ECFrozen;
				}
				colorType = SpellColorType.Frozen;
				break;
			case SpellColorType.Mucus:
				if (lr_Laser.material != mat_ECMucus)
				{
					lr_Laser.material = mat_ECMucus;
				}
				colorType = SpellColorType.Mucus;
				break;
			case SpellColorType.Player:
				if (lr_Laser.material != mat_ECPlayer)
				{
					lr_Laser.material = mat_ECPlayer;
				}
				colorType = SpellColorType.Player;
				break;
			case SpellColorType.Venom:
				if (lr_Laser.material != mat_ECVenom)
				{
					lr_Laser.material = mat_ECVenom;
				}
				colorType = SpellColorType.Venom;
				break;
			case SpellColorType.Fire:
				if (lr_Laser.material != mat_ECFire)
				{
					lr_Laser.material = mat_ECFire;
				}
				colorType = SpellColorType.Fire;
				break;
			case SpellColorType.Thunder:
				if (lr_Laser.material != mat_ECThunder)
				{
					lr_Laser.material = mat_ECThunder;
				}
				colorType = SpellColorType.Thunder;
				break;
			case SpellColorType.Void:
				if (lr_Laser.material != mat_ECVoid)
				{
					lr_Laser.material = mat_ECVoid;
				}
				colorType = SpellColorType.Void;
				break;
			default:
				if (lr_Laser.material != mat_ECPlayer)
				{
					lr_Laser.material = mat_ECPlayer;
				}
				colorType = SpellColorType.Player;
				break;
			}
		}
		else
		{
			lr_Laser.material = mat_ECPlayer;
		}
		lr_Laser.startWidth = Mathf.Pow(f, 0.3333f) * targetSpell.spellVolumeRatio;
		lr_Shadow.startWidth = Mathf.Pow(f, 0.3333f) * targetSpell.spellVolumeRatio;
		casterWand = targetWand;
		Update();
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
