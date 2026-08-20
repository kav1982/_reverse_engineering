using UnityEngine;

public class Spell9016ChaseBullet : SpellBase
{
	private UnitProperty targetPpt;

	private Vector3 deltaDistance;

	public LineRenderer aimRenderer;

	public override void InitializeCallback()
	{
		targetPpt = PlayerMgr.Inst.PlayerCtrller.myPpt;
		base.Direction = Tool2D.IgnoreZPoint(targetPpt.transform.position - base.transform.position).normalized;
		deltaDistance = Tool2D.IgnoreZPoint(base.transform.position - targetPpt.transform.position);
		rigid.linearVelocity = Vector3.zero;
		aimRenderer.SetPosition(0, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position, 0f - base.Height)));
		aimRenderer.SetPosition(1, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position, 0f - base.Height)));
		aimRenderer.SetPosition(2, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position, 0f - base.Height)));
		aimRenderer.SetPosition(3, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position, 0f - base.Height)));
		aimRenderer.enabled = true;
	}

	public override void Update()
	{
		aimRenderer.SetPosition(0, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Vector3.Lerp(base.transform.position, targetPpt.transform.position, 0f), 0f - base.Height)));
		aimRenderer.SetPosition(1, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Vector3.Lerp(base.transform.position, targetPpt.transform.position, 0.1f), 0f - base.Height)));
		aimRenderer.SetPosition(2, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Vector3.Lerp(base.transform.position, targetPpt.transform.position, 0.9f), 0f - base.Height)));
		aimRenderer.SetPosition(3, Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(Vector3.Lerp(base.transform.position, targetPpt.transform.position, 1f), 0f - base.Height)));
		deltaDistance += base.Direction * base.CurrentSpeed * Time.deltaTime;
		base.transform.position = Tool2D.IgnoreZPoint(targetPpt.transform.position + deltaDistance, base.transform.position.z);
		base.Update();
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
		else
		{
			aimRenderer.enabled = false;
		}
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		SEMgr.Inst.spell1001Hit.PlaySE();
		return base.OutputDamage(unitPpt, info);
	}
}
