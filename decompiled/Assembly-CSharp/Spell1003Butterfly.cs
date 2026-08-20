using UnityEngine;

public class Spell1003Butterfly : SpellBase
{
	[Header("\ufffd\ufffd\ufffdڿ\ufffd\ufffd\ufffdͼƬ\ufffd\ufffdת\ufffd\ufffdTransform")]
	public Transform FilpModel;

	private bool isTrace;

	private float checkTargetIntervalTimer;

	protected override float FallInitialHeight => Random.Range(5f, 7f);

	protected override float FallSpeedRatio => 0.8f;

	protected override float InFallingReboundingGravity => 10f;

	public void SetIsTraceFalse()
	{
		isTrace = false;
	}

	public void Break()
	{
		HitEFAndRecycle();
	}

	public override void InitializeCallback()
	{
		isTrace = false;
		ApplySpeedToVelocity();
		checkTargetIntervalTimer = 0f;
	}

	public override void Update()
	{
		if (!base.isFlyFinish && !isTrace)
		{
			base.CurrentSpeed = Mathf.Lerp(base.CurrentSpeed, 0f, base.spellCfg.float1 * Time.deltaTime);
			UnitType unitType = ownerPpt.unitCfg.unitType;
			if (unitType == UnitType.Player || unitType == UnitType.Teammate || unitType == UnitType.TeammateNotAttack)
			{
				if (base.CurrentSpeed <= base.spellCfg.speed * base.spellCfg.float2)
				{
					base.CurrentSpeed = base.spellCfg.speed * base.spellCfg.float2;
					isTrace = true;
					spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
				}
			}
			else if (base.CurrentSpeed <= base.spellCfg.float2)
			{
				base.CurrentSpeed = base.spellCfg.float2;
				isTrace = true;
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			}
		}
		base.Update();
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime * GetLowFPSTimeScale(5f);
		}
		if (base.DurationTimer > base.spellCfg.duration)
		{
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
		if (rigid.linearVelocity.x > 0f && FilpModel.transform.localScale.x < 0.5f)
		{
			FilpModel.transform.localScale = Vector3.one;
		}
		else if (rigid.linearVelocity.x <= 0f && FilpModel.transform.localScale.x > 0.5f)
		{
			FilpModel.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	protected override void SpellFollowMouse()
	{
		if (!isTrace)
		{
			rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
		}
		else
		{
			base.SpellFollowMouse();
		}
	}

	protected override void SpellFollowTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer >= 1f)
		{
			checkTargetIntervalTimer = 0f;
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		base.Direction = Tool2D.DirMoveTowards(dir2: (!(spellFollowTargetPpt == null)) ? ToPointDir(spellFollowTargetPpt.transform.position) : rigid.linearVelocity.normalized, dir1: base.Direction, maxDelta: base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
		rigid.linearVelocity = base.Direction * base.CurrentSpeed;
	}

	protected override void SpellNormalTransform()
	{
		base.SpellNormalTransform();
		if (!isTrace)
		{
			rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
			return;
		}
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer >= 1f)
		{
			checkTargetIntervalTimer = 0f;
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		Vector3 vector = ((spellFollowTargetPpt == null) ? rigid.linearVelocity.normalized : ToPointDir(spellFollowTargetPpt.transform.position));
		Vector3 linearVelocity = Vector3.Lerp(rigid.linearVelocity, vector * base.CurrentSpeed, base.spellCfg.float1 * Time.deltaTime);
		rigid.linearVelocity = linearVelocity;
		base.Direction = rigid.linearVelocity.normalized;
	}

	protected override UnitProperty TryRefract(params GameObject[] hitTarget)
	{
		UnitProperty unitProperty = base.TryRefract(hitTarget);
		if (!unitProperty)
		{
			return null;
		}
		isTrace = false;
		return unitProperty;
	}

	protected override void InitSpeedAndPosition()
	{
		if (!base.SIP.spellIsFall)
		{
			base.InitSpeedAndPosition();
			return;
		}
		float speed = base.spellCfg.speed;
		base.spellCfg.speed /= 2.8f;
		base.InitSpeedAndPosition();
		base.spellCfg.speed = speed;
	}
}
