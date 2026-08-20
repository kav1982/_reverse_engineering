using System.Collections;
using UnityEngine;

public class Spell1026Effect : SpellEffectBase
{
	public float stageSpellSizeRatio;

	public SpellChargeEffectBase chargeEffectCtrl;

	private Spell1026TestHoldingSpell spellScript;

	private Transform starScaleTrans;

	private int effectCurrentStage;

	private bool lastFrameIsHolding = true;

	protected override void Awake()
	{
		base.Awake();
		spellScript = (Spell1026TestHoldingSpell)base.Spell;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		starScaleTrans = null;
		lastFrameIsHolding = true;
		chargeEffectCtrl = null;
		effectCurrentStage = 0;
	}

	private void OnDisable()
	{
		if ((bool)chargeEffectCtrl)
		{
			ObjPoolMgr.Inst.RecycleGO(chargeEffectCtrl.gameObject);
		}
	}

	protected override void OnSpawnEffect(SpellEffectSettings effect, Transform trans)
	{
		base.OnSpawnEffect(effect, trans);
		if (effect.Name == "Spell")
		{
			starScaleTrans = trans.Find("Scale");
			starScaleTrans.localScale = Vector3.one * spellScript.spellVolumeRatio;
		}
	}

	protected override void OnFirstFrame()
	{
		base.OnFirstFrame();
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/10261/10261_Charge", delegate(GameObject o)
		{
			chargeEffectCtrl = o.GetComponent<SpellChargeEffectBase>();
			chargeEffectCtrl.AttachTarget = spellScript.tsf_Layer;
			chargeEffectCtrl.IsSkipHolding = spellScript.IsSkipHolding;
			chargeEffectCtrl.ColorType = base.Spell.ColorType;
		});
		if (spellScript.IsSkipHolding)
		{
			ChangeStage(spellScript.CalculateCurrentStage());
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!(chargeEffectCtrl == null))
		{
			int num = spellScript.CalculateCurrentStage();
			if (num > effectCurrentStage && effectCurrentStage != 4 && spellScript.IsHolding)
			{
				ChangeStage(num);
			}
			if (!spellScript.IsHolding && lastFrameIsHolding)
			{
				chargeEffectCtrl.Release();
				lastFrameIsHolding = false;
			}
		}
	}

	private void ChangeStage(int stage)
	{
		effectCurrentStage = stage;
		chargeEffectCtrl.ChangeStage(effectCurrentStage);
		if (stage == 4)
		{
			base.Spell.PlaySE("FinalFlash");
		}
		else
		{
			base.Spell.PlaySE("Flash").pitch = 0.9f + (float)(stage - 1) * 0.1f;
		}
		if ((bool)starScaleTrans)
		{
			starScaleTrans.localScale = Mathf.Pow(stageSpellSizeRatio, stage) * Vector3.one * spellScript.spellVolumeRatio;
		}
	}

	public void CreateHitEffect(Vector3 position)
	{
		for (int i = 1; i <= spellScript.CalculateCurrentStage(); i++)
		{
			if (i <= 3)
			{
				CreateSpriteEffect("Hit_" + i, position);
				continue;
			}
			string effectName = "Hit_" + i;
			Vector3? position2 = position;
			ManualCreateEffect(effectName, null, position2);
		}
	}

	public void CreateShootEffect(Vector3 position)
	{
		string effectName = "Shoot_" + spellScript.CalculateCurrentStage();
		Vector3? position2 = position;
		ManualCreateEffect(effectName, null, position2);
	}

	private IEnumerator CreateTrailIE()
	{
		yield return new WaitForSeconds(Time.fixedDeltaTime);
		for (int i = 1; i <= spellScript.CalculateCurrentStage(); i++)
		{
			ManualCreateEffect("Trail_" + i);
		}
	}

	public void CreateTrail()
	{
		StartCoroutine(CreateTrailIE());
	}
}
