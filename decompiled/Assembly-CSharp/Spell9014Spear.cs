using UnityEngine;

public class Spell9014Spear : SpellBase
{
	public Transform spriteRoot;

	public Transform colliderRoot;

	public Transform shadowRoot;

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
		colliderRoot.transform.up = base.Direction;
	}

	public override void Update()
	{
		base.Update();
		spriteRoot.transform.up = base.Direction;
		colliderRoot.transform.up = base.Direction;
		shadowRoot.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		shadowRoot.transform.up = base.Direction;
		base.DurationTimer += Time.deltaTime;
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
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	public override TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		if (info == null)
		{
			info = new TakeDamageInfo();
		}
		switch (targetGO.tag)
		{
		case "Teammate":
			info.teammateTakeDamageRatio = 3f;
			break;
		}
		return OutputDamage(targetGO.GetComponent<UnitProperty>(), info);
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		SEMgr.Inst.elite9BladeHit.PlaySE();
		return base.OutputDamage(unitPpt, info);
	}
}
