using Unity.Collections;

public struct SpellConfig_Struct
{
	public int id;

	public int priceCoin;

	public int priceHP;

	public FixedString64Bytes icon;

	public FixedString64Bytes iconH;

	public int level;

	public bool canCompound;

	public ItemDropType dropType;

	public bool needActivate;

	public float shootIntervalAddSubRevise;

	public float criticalChance;

	public float coolDownAddSubRevise;

	public float coolDownRatio;

	public float angle;

	public int slotCost;

	public int slotNumModifyValue;

	public SpellType useType;

	public FixedString64Bytes prefab;

	public int mpCost;

	public int shootCount;

	public float damage;

	public bool isDPS;

	public float DPSDamageInterval;

	public bool isKeepCasting;

	public bool canCancelCasting;

	public bool isParentTypeSpell;

	public float speed;

	public float duration;

	public float knockback;

	public float recoil;

	public float mpCostAddSubCorrection;

	public float mpCostMulDivCorrection;

	public bool haveEffecforMissileSpell;

	public bool haveEffecforSummonSpell;

	public SpellAbilityType abilityType;

	public float upSpeed;

	public float gravity;

	public float radius;

	public int summonID;

	public int summonLimit;

	public float float1;

	public float float2;

	public float float3;

	public int int1;

	public int int2;

	public int int3;

	public bool isSplitSpell;

	public bool playShootSE;

	public static SpellConfig_Struct FromSpellConfigClass(SpellConfig config)
	{
		FixedString64Bytes fixedString64Bytes = new FixedString64Bytes(config.icon ?? "");
		FixedString64Bytes fixedString64Bytes2 = new FixedString64Bytes(config.iconH ?? "");
		FixedString64Bytes fixedString64Bytes3 = new FixedString64Bytes(config.prefab ?? "");
		SpellConfig_Struct result = default(SpellConfig_Struct);
		result.id = config.id;
		result.priceCoin = config.priceCoin;
		result.priceHP = config.priceHP;
		result.icon = fixedString64Bytes;
		result.iconH = fixedString64Bytes2;
		result.level = config.level;
		result.canCompound = config.canCompound;
		result.dropType = config.dropType;
		result.needActivate = config.needActivate;
		result.shootIntervalAddSubRevise = config.shootIntervalAddSubRevise;
		result.criticalChance = config.criticalChance;
		result.coolDownAddSubRevise = config.coolDownAddSubRevise;
		result.coolDownRatio = config.coolDownRatio;
		result.angle = config.angle;
		result.slotCost = config.slotCost;
		result.slotNumModifyValue = config.slotNumModifyValue;
		result.useType = config.useType;
		result.prefab = fixedString64Bytes3;
		result.mpCost = config.mpCost;
		result.shootCount = config.shootCount;
		result.damage = config.damage;
		result.isDPS = config.isDPS;
		result.DPSDamageInterval = config.DPSDamageInterval;
		result.isKeepCasting = config.isKeepCasting;
		result.canCancelCasting = config.canCancelCasting;
		result.isParentTypeSpell = config.isParentTypeSpell;
		result.speed = config.speed;
		result.duration = config.duration;
		result.knockback = config.knockback;
		result.recoil = config.recoil;
		result.mpCostAddSubCorrection = config.mpCostAddSubCorrection;
		result.mpCostMulDivCorrection = config.mpCostMulDivCorrection;
		result.haveEffecforMissileSpell = config.haveEffecforMissileSpell;
		result.haveEffecforSummonSpell = config.haveEffecforSummonSpell;
		result.abilityType = config.abilityType;
		result.upSpeed = config.upSpeed;
		result.gravity = config.gravity;
		result.radius = config.radius;
		result.summonID = config.summonID;
		result.summonLimit = config.summonLimit;
		result.float1 = config.float1;
		result.float2 = config.float2;
		result.float3 = config.float3;
		result.int1 = config.int1;
		result.int2 = config.int2;
		result.int3 = config.int3;
		result.isSplitSpell = config.isSplitSpell;
		result.playShootSE = config.playShootSE;
		return result;
	}
}
