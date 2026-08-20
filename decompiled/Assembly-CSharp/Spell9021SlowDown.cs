using UnityEngine;

public class Spell9021SlowDown : SpellBase
{
	private float slowDownTime;

	public AnimationCurve slowDownCurve;

	private float moveDistance;

	public void SetSlowDown(float slowDownTime, Vector3 diration, float distance)
	{
		this.slowDownTime = slowDownTime;
		base.Direction = diration;
		moveDistance = distance;
	}

	public override void FixedUpdate()
	{
		if (slowDownTime > base.DurationTimer)
		{
			base.CurrentSpeed = moveDistance * (slowDownCurve.Evaluate((base.DurationTimer + Time.fixedDeltaTime) / slowDownTime) - slowDownCurve.Evaluate(base.DurationTimer / slowDownTime)) / Time.fixedDeltaTime;
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		else
		{
			base.CurrentSpeed = 0.01f;
		}
		base.FixedUpdate();
	}

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
	}

	public override void Update()
	{
		base.Update();
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

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		TakeDamageInfo result = base.MakeDamageToUnit(unit);
		if (effectPrefix == 90212 || effectPrefix == 90222)
		{
			SEMgr.Inst.PlaySE("SE_Spell9022Hit");
		}
		return result;
	}

	protected override bool OnHitDestructible(UnitProperty go)
	{
		if (effectPrefix == 90212 || effectPrefix == 90222)
		{
			SEMgr.Inst.PlaySE("SE_Spell9022Hit");
		}
		return base.OnHitDestructible(go);
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		if (!base.SIP.spellIsFall)
		{
			base.CreateHitEffect(position, rotation);
		}
	}
}
