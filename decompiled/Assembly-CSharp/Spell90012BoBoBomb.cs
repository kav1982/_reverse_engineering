using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell90012BoBoBomb : SpellBase
{
	private Vector3 landPoint = Vector3.zero;

	private bool enableHitTrigger;

	public override void InitializeCallback()
	{
		if (landPoint != Vector3.zero)
		{
			base.CurrentSpeed = GeneralTool.CannonSpeed(base.spellCfg.upSpeed, 0f - base.transform.position.z, base.spellCfg.gravity, Tool2D.IgnoreZDistance(base.transform.position, landPoint));
		}
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
			base.spellCfg.upSpeed = 0f;
			base.spellCfg.gravity = 0f;
			base.CurrentUpSpeed = 0f;
			base.spellCfg.duration = SpellConfig.dic[10011].duration + base.spellCfg.duration - SpellConfig.dic[base.spellCfg.id].duration;
			Update();
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		if (!base.spellCfg.isSplitSpell)
		{
			SEMgr.Inst.PlaySE("SE_Spell" + effectPrefix + "Shoot");
		}
		else
		{
			rigid.linearVelocity *= 0.8f;
		}
		base.transform.localScale = Vector3.one * base.radiusRatio * base.finalRadiusRatio;
	}

	public void HandleSpecialCase()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			base.spellCfg.speed = (10f + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
			base.CurrentSpeed = base.spellCfg.speed;
			enableHitTrigger = true;
			rigid.linearVelocity = Vector3.zero;
		}
	}

	public override void Update()
	{
		base.Update();
		if (!base.isFlyFinish)
		{
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				base.spellAroundOwnerCurrentAngle += 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
				Vector3 vector = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				base.Direction = ToPointDir(vector);
				base.transform.position = Tool2D.IgnoreZPoint(vector, base.transform.position.z);
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
			{
				Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
				rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, ToPointDir(mousePoint) * base.CurrentSpeed, base.spellCfg.speed * Time.deltaTime * spellFollowMouseLerp);
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
			{
				if (spellFollowTargetPpt == null || !spellFollowTargetPpt.gameObject.activeSelf)
				{
					spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
				}
				if (spellFollowTargetPpt != null)
				{
					base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
				}
				rigid.linearVelocity = Tool2D.IgnoreZPoint(base.Direction) * base.CurrentSpeed;
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
			{
				float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(base.Direction, GetSpellFollowToOwnerDirection())) - 90f) / 90f;
				base.Direction = Tool2D.DirMoveTowardsTargetInCounterClockWise(base.Direction, GetSpellFollowToOwnerDirection(), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.fixedDeltaTime);
				float num = 0.4f;
				rigid.linearVelocity = base.Direction * base.CurrentSpeed * Mathf.Lerp(1f - num, 1f + num, t);
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			if (base.SpellHoverTime > 0f)
			{
				base.SpellHoverTimer += Time.deltaTime;
				if (base.SpellHoverTimer < base.SpellHoverTime)
				{
					return;
				}
			}
			if (base.spellSplitCount != 0)
			{
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					Explosion();
				}
				PoolRecycle();
				return;
			}
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					Explosion();
				}
				PoolRecycle();
				return;
			}
		}
		if (!(base.transform.position.z >= 0f))
		{
			return;
		}
		if (base.rebounceTime > 0)
		{
			base.rebounceTime--;
			base.CurrentUpSpeed = 0f - base.CurrentUpSpeed;
			Explosion();
			if (createReboundEffect && base.spellAroundOwnerRadius == 0f)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_TransparentRebound", Tool2D.GetLayerPoint(base.transform), 0.3f).transform.localScale = Vector3.one * base.transform.localScale.x * base.ColliderRadius;
			}
			if (base.rebounceTime <= 0)
			{
				if (sc_Rebound != null)
				{
					sc_Rebound.enabled = false;
				}
				else if (bc_Rebound != null)
				{
					bc_Rebound.enabled = false;
				}
			}
		}
		else
		{
			Explosion();
			PoolRecycle();
		}
	}

	protected override void OnFallingGround()
	{
		base.OnFallingGround();
		SEMgr.Inst.PlaySE("SE_Spell" + effectPrefix + "Explosion");
	}

	public override void TriggerIn(Collider other)
	{
		if (!enableHitTrigger)
		{
			return;
		}
		switch (other.tag)
		{
		case "Monster":
			if (ownerPpt != null && ownerPpt.gameObject.activeInHierarchy && ownerPpt.tag != "Monster")
			{
				Explosion();
				TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			}
			break;
		case "Destructible":
			Explosion();
			TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			break;
		case "RollBall":
			Explosion();
			TryRefractOrPenetrateOrRecycleOnHitTarget(other.gameObject);
			break;
		case "Wall":
		case "SolidObj":
			if (!isThroughWall && base.spellAroundOwnerRadius == 0f && base.rebounceTime <= 0)
			{
				Explosion();
			}
			break;
		}
	}

	private void Explosion()
	{
		List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(base.transform.position, base.spellCfg.radius, "Monster", "Destructible", "Spell", "RollBall", "Butterfly", "Brittleness");
		EffectBase.ManualCreateEffect("Explosion");
		SEMgr.Inst.PlaySE("SE_Spell" + effectPrefix + "Explosion");
		foreach (Collider item in collidersByTag.Where((Collider e) => e.gameObject.activeInHierarchy))
		{
			if (item.gameObject.CompareAnyTag("Spell", "RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1002RollBall spell1002RollBall))
				{
					if (componentInParent is Spell1003Butterfly spell1003Butterfly)
					{
						spell1003Butterfly.HitEFAndRecycle();
					}
				}
				else
				{
					spell1002RollBall.TakeDamage(base.spellCfg.damage);
				}
			}
			else if (item.gameObject.CompareAnyTag("Monster"))
			{
				UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
				OutputDamage(info: new TakeDamageInfo
				{
					canRebound = false,
					knockbackForce = Tool2D.IgnoreZPoint(item.transform.position - base.transform.position).normalized * base.spellCfg.knockback
				}, targetGO: item.gameObject);
				Vector3 lookAt = Tool2D.IgnoreZPoint((item.transform.position - base.transform.position).normalized);
				Vector3 value = item.transform.position + new Vector3(0f, 0.3f, 0f);
				CreateHitEffectLookAt(lookAt, value);
				component.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(item.transform, base.transform) * base.spellCfg.knockback);
			}
			else
			{
				TakeDamageInfo info2 = new TakeDamageInfo
				{
					canRebound = false,
					knockbackForce = Tool2D.IgnoreZV2ToV1Normal(item.transform, base.transform) * base.spellCfg.knockback
				};
				OutputDamage(item.gameObject, info2);
			}
		}
		CreateCircleElementGroundEffect();
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		Spell90012BoBoBomb obj = (Spell90012BoBoBomb)base.CreateSplitSpell(splitCfg, baseAngle, index);
		obj.gameObject.transform.position = Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, -1f);
		obj.OwnerTsf = base.OwnerTsf;
		obj.spellCfg.upSpeed = base.spellCfg.upSpeed / 2f;
		obj.spellCfg.gravity = base.spellCfg.gravity;
		obj.CurrentSpeed = base.CurrentSpeed * 2f;
		obj.spellCfg.damage = Mathf.Ceil(base.spellCfg.damage * 0.35f);
		return obj;
	}

	public override void PoolRecycle()
	{
		landPoint = Vector3.zero;
		enableHitTrigger = false;
		base.PoolRecycle();
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		string text = unitPpt.gameObject.tag;
		if (text == "Monster" || text == "Destructible")
		{
			UnitProperty unitProperty = ownerPpt;
			if ((object)unitProperty != null)
			{
				UnitBase unitBas = unitProperty.UnitBas;
				if ((object)unitBas != null)
				{
					SpellBase summonerSpellBase = unitBas.SummonerSpellBase;
					if ((object)summonerSpellBase != null && summonerSpellBase.TriggerCtrl != null)
					{
						ownerPpt.UnitBas.SummonerSpellBase.TriggerCtrl.AddHitTriggerPoint(unitPpt.transform.position);
					}
				}
			}
		}
		return base.OutputDamage(unitPpt, info, SpellAbilityType.TeammateSprite);
	}
}
