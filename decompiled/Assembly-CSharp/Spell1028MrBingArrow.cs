using UnityEngine;

public class Spell1028MrBingArrow : SpellBase
{
	public static int shootCounter;

	public float subArrowSizeRatio;

	public float subArrowDamageRatio;

	public float subArrowBonusPushForce;

	public float subArrowBonusDuration;

	public float subArrowExtraScatter;

	public VariableFloat subArrowSpeedRatioRange;

	private bool isSubArrow;

	private bool isSubArrowEmitter;

	public float SubArrowEmitInterval;

	private float subEmitTimer;

	private int remainSubArrowCount;

	private float subArrowDynmicDamageRatio = 1f;

	public Transform shadowTrans;

	public SphereCollider HitBox;

	public override void OnEnable()
	{
		base.OnEnable();
		isSubArrow = false;
		HitBox.enabled = false;
		isSubArrowEmitter = false;
		subEmitTimer = 0f;
		remainSubArrowCount = 0;
		subArrowDynmicDamageRatio = 1f;
		shadowTrans.gameObject.SetActive(value: false);
	}

	public override void InitializeCallback()
	{
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (!base.spellCfg.isSplitSpell && !isSubArrow && IsSameCamp(UnitType.Player))
		{
			shootCounter++;
			if (shootCounter % base.spellCfg.int1 == 0)
			{
				isSubArrowEmitter = true;
				base.spellCfg.isSplitSpell = true;
				remainSubArrowCount = (GeneralTool.IsLowFpsOptimizeActive(40f) ? Mathf.FloorToInt((float)base.spellCfg.int2 * GameMgr.Inst.GetFps() / 40f) : base.spellCfg.int2);
				subArrowDynmicDamageRatio = (float)base.spellCfg.int2 / (float)remainSubArrowCount;
				base.endTHunderHitChance = 0f;
				spellEndTeleport = false;
				shootCounter -= base.spellCfg.int1;
				shootCounter = Mathf.Max(0, shootCounter);
			}
		}
		if (isSubArrowEmitter)
		{
			enableFollowTarget = false;
			enableFollowMouse = false;
			base.enableAroundPlayer = false;
		}
		else
		{
			ApplySpeedToVelocity();
			HitBox.enabled = true;
			shadowTrans.gameObject.SetActive(value: true);
			EffectBase.ManualCreateEffect("Spell");
			EffectBase.ManualCreateEffect("Trail");
		}
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		SpellBase spellBase = base.CreateSplitSpell(splitCfg, baseAngle, index);
		if (!isSubArrow || isSubArrowEmitter)
		{
			return spellBase;
		}
		return ModifySplitSubArrow(spellBase);
	}

	public void UpdateShadowDir(Vector3 dir)
	{
		shadowTrans.transform.right = dir;
	}

	public override void Update()
	{
		base.Update();
		UpdateSubEmitState();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (!(base.DurationTimer > base.spellCfg.duration))
		{
			return;
		}
		if (!base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			base.CurrentSpeed = 0f;
		}
		if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
		{
			base.SpellHoverTimer += Time.deltaTime;
			return;
		}
		if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
		{
			PoolRecycle();
			return;
		}
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	private void UpdateSubEmitState()
	{
		if (isSubArrowEmitter)
		{
			subEmitTimer += Time.deltaTime;
			if (subEmitTimer >= SubArrowEmitInterval - (float)base.spellCfg.level * 0.01f && remainSubArrowCount > 0)
			{
				remainSubArrowCount--;
				subEmitTimer -= SubArrowEmitInterval - (float)base.spellCfg.level * 0.01f;
				CreateSubArrow();
			}
		}
	}

	private SpellBase ModifySplitSubArrow(SpellBase targetSpell)
	{
		Spell1028MrBingArrow spell1028MrBingArrow = (Spell1028MrBingArrow)targetSpell;
		spell1028MrBingArrow.isSubArrow = true;
		spell1028MrBingArrow.wandChargeData = base.wandChargeData;
		spell1028MrBingArrow.DurationTimer -= subArrowBonusDuration;
		spell1028MrBingArrow.spellCfg.knockback += subArrowBonusPushForce;
		spell1028MrBingArrow.spellCfg.speed *= subArrowSpeedRatioRange.RandomResult();
		spell1028MrBingArrow.CurrentSpeed = spell1028MrBingArrow.spellCfg.speed;
		spell1028MrBingArrow.spellCfg.damage = Mathf.Ceil((spell1028MrBingArrow.spellCfg.damage - spell1028MrBingArrow.SIP.finalDamageExtra) * subArrowDamageRatio) + spell1028MrBingArrow.SIP.finalDamageExtra;
		spell1028MrBingArrow.transform.localScale *= subArrowSizeRatio;
		((Spell1028Effect)spell1028MrBingArrow.EffectBase).AddSubArrowColor = true;
		if (spell1028MrBingArrow.enableAroundPlayer)
		{
			float degree = Random.Range(0f, 360f);
			spell1028MrBingArrow.transform.position += Tool2D.GetDir(degree) * spell1028MrBingArrow.spellAroundOwnerRadius;
			spell1028MrBingArrow.spellAroundOwnerCurrentAngle = degree;
		}
		if (base.OwnerSpell == null)
		{
			spell1028MrBingArrow.OwnerTsf = ownerPpt.transform;
		}
		else if (base.OwnerPoint != Vector3.zero)
		{
			spell1028MrBingArrow.OwnerTsf = base.OwnerSpell.transform;
		}
		spell1028MrBingArrow.OwnerTsf = base.OwnerTsf;
		spell1028MrBingArrow.indirectShootByPlayer = true;
		return spell1028MrBingArrow;
	}

	private void CreateSubArrow()
	{
		SpellConfig spellConfig = SpellConfig.dic[base.spellCfg.id];
		Spell1028MrBingArrow spell1028MrBingArrow = (Spell1028MrBingArrow)ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellConfig.prefab, base.transform.position).GetComponent<SpellBase>();
		SpellInitialParameter initialParameter = base.InitialParameter.Copy();
		spell1028MrBingArrow.isSubArrow = true;
		spell1028MrBingArrow.Initialize(initialParameter);
		spell1028MrBingArrow.wandChargeData = base.wandChargeData;
		spell1028MrBingArrow.DurationTimer -= subArrowBonusDuration;
		spell1028MrBingArrow.spellCfg.knockback += subArrowBonusPushForce;
		spell1028MrBingArrow.spellCfg.speed *= subArrowSpeedRatioRange.RandomResult();
		spell1028MrBingArrow.CurrentSpeed = spell1028MrBingArrow.spellCfg.speed;
		spell1028MrBingArrow.spellCfg.damage = Mathf.Ceil((spell1028MrBingArrow.spellCfg.damage - spell1028MrBingArrow.SIP.finalDamageExtra) * subArrowDamageRatio) + spell1028MrBingArrow.SIP.finalDamageExtra;
		spell1028MrBingArrow.Direction = Tool2D.GetDir(spell1028MrBingArrow.Direction, Random.Range((0f - subArrowExtraScatter) / 2f, subArrowExtraScatter / 2f));
		spell1028MrBingArrow.transform.localScale *= subArrowSizeRatio;
		((Spell1028Effect)spell1028MrBingArrow.EffectBase).AddSubArrowColor = true;
		if (spell1028MrBingArrow.enableAroundPlayer)
		{
			float degree = Random.Range(0f, 360f);
			spell1028MrBingArrow.transform.position += Tool2D.GetDir(degree) * spell1028MrBingArrow.spellAroundOwnerRadius;
			spell1028MrBingArrow.spellAroundOwnerCurrentAngle = degree;
		}
		if (base.OwnerSpell == null)
		{
			spell1028MrBingArrow.OwnerTsf = ownerPpt.transform;
		}
		else if (base.OwnerPoint != Vector3.zero)
		{
			spell1028MrBingArrow.OwnerTsf = base.OwnerSpell.transform;
		}
		spell1028MrBingArrow.OwnerTsf = base.OwnerTsf;
		spell1028MrBingArrow.OwnerPoint = base.OwnerPoint;
		spell1028MrBingArrow.indirectShootByPlayer = true;
	}
}
