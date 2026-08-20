using System;
using UnityEngine;

public class Spell9001BulletParabola : SpellBase
{
	public float bounceRatio = 1f;

	private Vector3 landPoint = Vector3.zero;

	public void SetUp(Vector3 landPoint)
	{
		this.landPoint = landPoint;
	}

	public override void InitializeCallback()
	{
		if (landPoint != Vector3.zero && !base.SIP.spellIsFall)
		{
			base.CurrentSpeed = GeneralTool.CannonSpeed(base.spellCfg.upSpeed, 0f - base.transform.position.z, base.spellCfg.gravity, Tool2D.IgnoreZDistance(base.transform.position, landPoint));
			if (base.InitialWithConfig != null)
			{
				base.InitialWithConfig.speed = base.CurrentSpeed;
			}
		}
		if (base.spellAroundOwnerRadius != 0f && !base.SIP.spellIsFall)
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
				rigid.linearVelocity = base.Direction * base.CurrentSpeed;
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
			PoolRecycle();
			return;
		}
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!(base.transform.position.z >= 0f) || base.SIP.spellIsFall)
		{
			return;
		}
		base.transform.position = Tool2D.IgnoreZPoint(base.transform);
		if (spellMucusTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.ColliderRadius * base.transform.localScale.x * 2f);
		}
		if (spellVenomTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.ColliderRadius * base.transform.localScale.x * 2f, spellVenomTime * 2f);
		}
		if (spellFrozenTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(base.transform), base.ColliderRadius * base.transform.localScale.x * 2f);
		}
		if (lastOutputDamageFrame == Time.frameCount && base.remainRefractCount > 0)
		{
			if ((bool)TryRefract())
			{
				base.CurrentUpSpeed *= -1f;
				if (createReboundEffect && base.spellAroundOwnerRadius == 0f)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_TransparentRebound", Tool2D.GetLayerPoint(base.transform), 0.3f).transform.localScale = Vector3.one * base.transform.localScale.x * base.ColliderRadius;
				}
			}
		}
		else if (base.rebounceTime > 0)
		{
			base.rebounceTime--;
			base.CurrentUpSpeed = (0f - base.CurrentUpSpeed) * bounceRatio;
			if (createReboundEffect && base.spellAroundOwnerRadius == 0f)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_TransparentRebound", Tool2D.GetLayerPoint(base.transform), 0.3f).transform.localScale = Vector3.one * base.transform.localScale.x * base.ColliderRadius;
			}
			HeightFixedUpdate();
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
			HitEFAndRecycle();
		}
	}

	public override void PoolRecycle()
	{
		landPoint = Vector3.zero;
		base.PoolRecycle();
	}

	public override TakeDamageInfo OutputDamage(UnitProperty unitPpt, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		string text = unitPpt.gameObject.tag;
		UnitProperty unitProperty;
		if (text == "Monster" || text == "Destructible")
		{
			unitProperty = ownerPpt;
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
		unitProperty = ownerPpt;
		if ((object)unitProperty != null)
		{
			UnitBase unitBas = unitProperty.UnitBas;
			if ((object)unitBas != null && (object)unitBas.SummonerSpellBase != null)
			{
				damageRecordeType = ownerPpt.UnitBas.SummonerSpellBase.spellCfg.abilityType;
			}
		}
		return base.OutputDamage(unitPpt, info, damageRecordeType);
	}
}
