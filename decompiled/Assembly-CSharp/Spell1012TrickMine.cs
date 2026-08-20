using System.Collections.Generic;
using UnityEngine;

public class Spell1012TrickMine : SpellBase
{
	[Space(50f)]
	public Animator anima;

	private Spell1012Effect effect;

	public Transform RotateTransform;

	public float rotateSpeed;

	public float slotdownRatio;

	public float stopThreshold;

	public float explosionBeforeTime;

	public float fallInCliffDisntace;

	public ShockParam shockParam;

	[Range(0f, 1f)]
	public float splitMiniSpeedRatio = 0.5f;

	private float currentRotateSpeed;

	private bool isStop;

	private bool isExplosionBefore;

	private bool isDetonated;

	private bool affect_InAbyss;

	private Vector3 affect_AbyssPoint;

	private static Collider[] targetBuffer = new Collider[256];

	private static HashSet<string> targetTags = new HashSet<string> { "Monster", "Destructible", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness", "Player", "Teammate" };

	public override void SingleInitialCallback()
	{
		effect = (Spell1012Effect)EffectBase;
		base.SingleInitialCallback();
	}

	protected override bool OnAOEHitSpell(SpellBase spell)
	{
		return MakeDamageToSpell(spell);
	}

	protected override bool MakeDamageToSpell(SpellBase spell)
	{
		if (!(spell is Spell1012TrickMine spell1012TrickMine))
		{
			if (spell is Spell1013Meteor spell1013Meteor)
			{
				spell1013Meteor.SkipHover();
				return true;
			}
		}
		else if (spell1012TrickMine != this)
		{
			spell1012TrickMine.Detonated();
			return true;
		}
		return base.MakeDamageToSpell(spell);
	}

	protected override bool OnAOEHitUnit(UnitProperty unit)
	{
		MakeDamageToUnit(unit);
		return true;
	}

	protected override bool OnAOEHitWallAndSolidObj(Collider col)
	{
		if (!col.CompareTag("SolidObj"))
		{
			return false;
		}
		UnitProperty component = col.GetComponent<UnitProperty>();
		if (component != null)
		{
			component.TakeDamage(this);
		}
		return false;
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.isUndifferDamage = true;
		takeDamageInfo.canRebound = false;
		takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(unit.transform, base.transform) * base.spellCfg.knockback;
		return takeDamageInfo;
	}

	private void Explode()
	{
		if (!base.SIP.spellIsFall)
		{
			base.isFlyFinish = true;
		}
		float num = (base.SIP.spellIsFall ? GetFallingGroundDamageRadius() : base.spellCfg.radius);
		SpellEffectBase effectBase = EffectBase;
		float? size = num * 1.4f;
		effectBase.CreateSpriteEffect("Explosion", null, null, size);
		if (!SpellEffectBase.FullTransparency)
		{
			string typePostfix = null;
			SpellColorType colorType = base.ColorType;
			if (colorType == SpellColorType.Fire || colorType == SpellColorType.Thunder || colorType == SpellColorType.Void)
			{
				typePostfix = base.ColorType.ToString();
			}
			SpellTraceTools.CreateTrace(10121, Tool2D.IgnoreZPoint(base.transform), num, typePostfix);
		}
		PlaySE("Explosion");
		CamController.Inst.SetShock(shockParam);
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, num, targetBuffer, targetTags);
		isDetonated = true;
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			AOETriggerIn(targetBuffer[i]);
		}
		if (spellMucusTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
		if (spellVenomTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius, spellVenomTime * 2f);
		}
		if (spellFrozenTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
	}

	public void Detonated()
	{
		if (!isDetonated && !base.SIP.spellIsFall)
		{
			isDetonated = true;
			Explode();
			PoolRecycle();
		}
	}

	public override void InitializeCallback()
	{
		currentRotateSpeed = ((rotateSpeed * (float)Random.Range(0, 2) == 0f) ? rotateSpeed : (0f - rotateSpeed));
		isStop = false;
		isExplosionBefore = false;
		isDetonated = false;
		if (!base.SIP.spellIsFall)
		{
			base.rebounceTime = 100;
		}
		affect_InAbyss = false;
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.gravity = 0f;
		}
		ApplySpeedToVelocity();
	}

	public override void Update()
	{
		base.Update();
		if (affect_InAbyss)
		{
			if (base.transform.position != affect_AbyssPoint)
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, affect_AbyssPoint, 4f * Time.deltaTime);
			}
			float num = base.transform.localScale.x - Time.deltaTime;
			if (num < 0f)
			{
				base.isFlyFinish = true;
				PoolRecycle();
			}
			else
			{
				base.transform.localScale = Vector3.one * num;
			}
		}
		if (isStop)
		{
			enableFollowMouse = false;
			enableFollowTarget = false;
			enableNormalTransform = false;
			base.enableAroundPlayer = false;
		}
		if (!isExplosionBefore && base.DurationTimer >= base.spellCfg.duration + base.SpellHoverTime - explosionBeforeTime)
		{
			isExplosionBefore = true;
			anima.SetTrigger("ExplodeBefore");
		}
		if (!isStop && EffectBase.FirstFrameIsRun)
		{
			RotateTransform.Rotate(0f, 0f, currentRotateSpeed * Time.deltaTime);
			if (base.transform.position.z >= 0f && !base.SIP.spellIsFall)
			{
				if (!affect_InAbyss && GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(base.transform), base.ColliderRadius * base.transform.localScale.x, "Abyss", "Abyss") != null)
				{
					affect_InAbyss = true;
					affect_AbyssPoint = Tool2D.IgnoreZPoint(base.transform.position);
					rigid.linearVelocity = Vector3.zero;
				}
				if ((Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 8) - base.transform.position).sqrMagnitude > fallInCliffDisntace * fallInCliffDisntace)
				{
					affect_InAbyss = true;
					affect_AbyssPoint = Tool2D.IgnoreZPoint(base.transform.position);
					rigid.linearVelocity = Vector3.zero;
				}
				if (base.CurrentSpeed <= stopThreshold)
				{
					isStop = true;
					base.transform.position = Tool2D.IgnoreZPoint(base.transform);
					ChangeVelocityZ(0f);
					ChangeGravity(0f);
					base.CurrentSpeed = 0f;
					rigid.linearVelocity = Vector3.zero;
				}
				else if (base.CurrentUpSpeed < 0f)
				{
					base.transform.position = Tool2D.IgnoreZPoint(base.transform);
					ChangeVelocityZ((0f - base.CurrentUpSpeed) * slotdownRatio);
					base.CurrentSpeed *= slotdownRatio;
					rigid.linearVelocity = rigid.linearVelocity.normalized * base.CurrentSpeed;
					currentRotateSpeed *= slotdownRatio;
				}
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (!base.SIP.spellIsFall)
		{
			float num2 = base.DurationTimer + base.SpellHoverTimer;
			if (base.SpellHoverTime + base.spellCfg.duration - num2 < 1f)
			{
				effect.StartTwinkle(1f);
			}
		}
		if (base.DurationTimer > base.spellCfg.duration && !base.SIP.spellIsFall)
		{
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
				return;
			}
			Explode();
			PoolRecycle();
		}
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit))
		{
			return false;
		}
		Explode();
		TryRefractOrPenetrateOrRecycleOnHitTarget(unit.gameObject);
		return false;
	}

	protected override bool OnHitDestructible(UnitProperty go)
	{
		Explode();
		TryRefractOrPenetrateOrRecycleOnHitTarget(go.gameObject);
		return true;
	}

	protected override bool OnHitBrittleness(GameObject go)
	{
		OutputDamage(go);
		return true;
	}

	protected override bool OnHitSpell(SpellBase spell)
	{
		if (spell is Spell1002RollBall spell2 && !IsSameCamp(spell2))
		{
			Explode();
			TryRefractOrPenetrateOrRecycleOnHitTarget(spell.gameObject);
			return true;
		}
		return false;
	}

	public override void OnCollisionEnter(Collision collision)
	{
		base.OnCollisionEnter(collision);
		if (collision.gameObject.CompareTag("SolidObj"))
		{
			Explode();
			PoolRecycle();
		}
	}

	protected override SpellInitialParameter CreateSplitSpellInitialParameter(SpellConfig config, Vector3 shootDirection)
	{
		SpellInitialParameter spellInitialParameter = base.CreateSplitSpellInitialParameter(config, shootDirection);
		spellInitialParameter.finalSpeedRatio *= splitMiniSpeedRatio;
		return spellInitialParameter;
	}

	protected override Vector3 GetSplitSpellPosition(Vector3 splitDirection)
	{
		return base.transform.position;
	}

	public override void HitEFAndRecycle()
	{
		Explode();
		base.HitEFAndRecycle();
	}

	protected override void OnFallingGround()
	{
		Explode();
		OnFallingGroundTryReboundOrRecycle();
	}
}
