using UnityEngine;

public class Spell1014Rainbow : SpellBase
{
	private static float RanbowAroundOwnerAngle = 0f;

	public static readonly string[] Colors = new string[8] { "Red", "Orange", "Yellow", "Green", "Cyan", "Blue", "Purple", "Void" };

	[Space(50f)]
	public float minAngle;

	public ShockParam shock;

	public float followMouseLerpBias;

	public float minSpeed;

	private bool hadShootShock;

	protected override float FallingReboundForce => Mathf.Max(base.CurrentUpSpeed * -0.6f, 0.2f * InFallingReboundingGravity);

	public override void InitializeCallback()
	{
		hadShootShock = false;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellAroundOwnerCurrentAngle = RanbowAroundOwnerAngle + 51.42857f * (float)base.InitialParameter.inSpellShootIndex;
		}
		base.penetrateTime++;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		float num = Mathf.Max(base.wandShootAngle, minAngle);
		if (!base.spellCfg.isSplitSpell)
		{
			base.Direction = Tool2D.GetDir(base.originalShootDirection, (0f - num) / 2f + num / 7f * (float)base.InitialParameter.inSpellShootIndex) * IsReverseDirection();
		}
		ApplySpeedToVelocity();
	}

	public override void Update()
	{
		base.Update();
		if (base.shouldCameraShock && !hadShootShock)
		{
			hadShootShock = true;
			CamController.Inst.SetShock(shock);
		}
		if (!base.isFlyFinish && !base.SIP.spellIsFall)
		{
			if (base.CurrentSpeed >= minSpeed)
			{
				base.CurrentSpeed = Mathf.Lerp(base.CurrentSpeed, -1f, base.spellCfg.float1 * Time.deltaTime);
			}
			else
			{
				base.CurrentSpeed = 0f;
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration) || base.SIP.spellIsFall)
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
		base.transform.localScale = Vector3.one * (base.transform.localScale.x - 5f * Time.deltaTime * 2f);
		if (base.transform.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	protected override void SpellFollowMouse()
	{
		if (base.SIP.spellIsFall)
		{
			rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
			return;
		}
		Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(mousePoint) * base.CurrentSpeed / 2f, (base.CurrentSpeed + followMouseLerpBias) * Time.deltaTime * spellFollowMouseLerp);
		if (base.CurrentSpeed > 0.1f)
		{
			base.Direction = rigid.linearVelocity.normalized;
		}
	}

	protected override void SpellFollowTarget()
	{
		base.SpellFollowTarget();
		if (!base.SpellFollowHaveTarget)
		{
			rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
		}
	}

	protected override void SpellNormalTransform()
	{
		base.SpellNormalTransform();
		rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
	}

	public override void HitEFAndRecycle()
	{
		SpawnHitEffect();
		PoolRecycle();
	}

	private void SpawnHitEffect()
	{
		CreateHitEffect();
	}

	protected override void InitSpeedAndPosition()
	{
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.speed *= 0.7f;
			if (base.SIP.originShootSpatialInfo != null && base.SIP.finalShootSpatialInfo != null)
			{
				base.SIP.finalShootSpatialInfo = base.SIP.originShootSpatialInfo;
			}
		}
		base.InitSpeedAndPosition();
	}

	protected override void OnFallingGround()
	{
		PlaySE("Hit");
		base.OnFallingGround();
	}

	protected override bool OnFallingGroundTryRebound()
	{
		ChangeCurrentSpeed(base.CurrentSpeed * 0.4f);
		return base.OnFallingGroundTryRebound();
	}
}
