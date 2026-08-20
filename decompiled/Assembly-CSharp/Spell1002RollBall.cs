using System;
using UnityEngine;

public class Spell1002RollBall : SpellBase
{
	[Space(50f)]
	public Transform tsf_Rotate;

	public float liquidGenerateInterval;

	public ShockParam shootShock;

	public ShockParam hitShock;

	private float perimeter;

	private float currentHP;

	private bool hadShootShock;

	private Vector3 liquidLastRecordPoint;

	private float liquidGenerateIntervalTimer;

	private float affect_SpikesTimer;

	private Vector3 affect_AbyssPoint;

	private bool affect_InAbyss;

	private Spell1002Effect Effect;

	private float checkTargetIntervalTimer;

	private float liquidRadius => base.ColliderRadius * base.transform.localScale.x / 1.6f;

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		Effect = (Spell1002Effect)EffectBase;
		checkTargetIntervalTimer = 0f;
	}

	public override void InitializeCallback()
	{
		liquidGenerateIntervalTimer = 0f;
		liquidLastRecordPoint = base.transform.position;
		affect_InAbyss = false;
		hadShootShock = false;
		perimeter = base.ColliderRadius * tsf_Layer.localScale.x * 2f * MathF.PI;
		currentHP = Mathf.CeilToInt(base.spellCfg.float1);
		ApplySpeedToVelocity();
		if (base.enableAroundPlayer && base.currentSpellMovement == SpellSpecialMovementType.Rotation && !base.SIP.spellIsFall)
		{
			base.spellAroundOwnerCurrentAngle += 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
			Vector3 vector = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
			base.Direction = ToPointDir(vector);
			base.transform.position = Tool2D.IgnoreZPoint(vector);
		}
		else if (!base.SIP.spellIsFall)
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position);
		}
		if (!base.SIP.spellIsFall)
		{
			SpawnSphereLiquid();
		}
	}

	public override void Update()
	{
		if (!base.SIP.spellIsFall && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			CheckIfOutMap();
		}
		if (affect_InAbyss)
		{
			enableFollowMouse = false;
			enableFollowTarget = false;
			enableNormalTransform = false;
			base.enableAroundPlayer = false;
		}
		base.Update();
		if (base.shouldCameraShock && !hadShootShock)
		{
			hadShootShock = true;
			CamController.Inst.SetShock(shootShock);
		}
		if (!base.SIP.spellIsFall)
		{
			if (affect_InAbyss)
			{
				if (base.transform.position != affect_AbyssPoint)
				{
					base.transform.position = Vector3.MoveTowards(base.transform.position, affect_AbyssPoint, 4f * Time.deltaTime);
				}
				float num = base.transform.localScale.x - Time.deltaTime;
				if (num < 0f)
				{
					PoolRecycle();
				}
				else
				{
					base.transform.localScale = Vector3.one * num;
				}
			}
			else if (GeneralTool.HaveCollider(Tool2D.IgnoreZPoint(base.transform), base.transform.localScale.x * base.ColliderRadius, "Abyss", "Abyss") != null)
			{
				FallInAbyss();
			}
			affect_SpikesTimer += Time.deltaTime;
			if (affect_SpikesTimer >= 0.33f && GeneralTool.HaveCollider(base.transform.position, base.transform.localScale.x * base.ColliderRadius, "GroundNeedle", "Invisible") != null)
			{
				affect_SpikesTimer = 0f;
				TakeDamage(3f);
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_DropBlood", base.transform.position, 1f);
			}
		}
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
				SpawnRectangleLiquid();
				SpawnSphereLiquid();
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
				return;
			}
		}
		if (!base.SIP.spellIsFall)
		{
			liquidGenerateIntervalTimer += Time.deltaTime;
			if (liquidGenerateIntervalTimer > liquidGenerateInterval && !base.isFlyFinish)
			{
				liquidGenerateIntervalTimer = 0f;
				SpawnRectangleLiquid();
			}
		}
	}

	private void CheckIfOutMap()
	{
		if ((Tool2D.GetNavMeshPointIngoreZ(base.transform.position, 8) - base.transform.position).sqrMagnitude > 1f)
		{
			FallInAbyss();
		}
	}

	private void FallInAbyss()
	{
		affect_InAbyss = true;
		base.spellSplitCount = 0;
		affect_AbyssPoint = Tool2D.IgnoreZPoint(base.transform.position);
		rigid.linearVelocity = Vector3.zero;
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		SpellSpecialMovementType spellSpecialMovementType = base.currentSpellMovement;
		if ((spellSpecialMovementType == SpellSpecialMovementType.Normal || spellSpecialMovementType == SpellSpecialMovementType.ChaseEnemy) && base.Direction == Vector3.zero && !base.isFlyFinish)
		{
			base.Direction = Tool2D.IgnoreZPoint(UnityEngine.Random.insideUnitSphere, 0f).normalized;
			rigid.linearVelocity = base.Direction.normalized * base.spellCfg.speed;
		}
	}

	public override void SpellAroundPlayer()
	{
		base.SpellAroundPlayer();
		Rotate(base.Direction * base.CurrentSpeed);
	}

	protected override void SpellFollowMouse()
	{
		base.SpellFollowMouse();
		Rotate(rigid.linearVelocity);
	}

	protected override void SpellFollowTarget()
	{
		checkTargetIntervalTimer += Time.deltaTime;
		if (checkTargetIntervalTimer >= 1f)
		{
			checkTargetIntervalTimer = 0f;
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		base.SpellFollowTarget();
		Rotate(rigid.linearVelocity);
	}

	protected override void SpellNormalTransform()
	{
		base.SpellNormalTransform();
		Rotate(rigid.linearVelocity);
	}

	private void SpawnSphereLiquid()
	{
		Vector3 point = Tool2D.IgnoreZPoint(base.transform.position);
		if (spellMucusTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(point, liquidRadius);
		}
		if (spellVenomTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(point, liquidRadius, spellVenomTime * 2f);
		}
		if (spellFrozenTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(point, liquidRadius);
		}
		liquidLastRecordPoint = point;
	}

	private void SpawnRectangleLiquid()
	{
		Vector3 vector = Tool2D.IgnoreZPoint(base.transform.position);
		if ((vector - liquidLastRecordPoint).sqrMagnitude <= 1f)
		{
			if (spellMucusTime > 0f)
			{
				LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(liquidLastRecordPoint, vector, liquidRadius);
			}
			if (spellVenomTime > 0f)
			{
				LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(liquidLastRecordPoint, vector, liquidRadius, spellVenomTime * 2f);
			}
			if (spellFrozenTime > 0f)
			{
				LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(liquidLastRecordPoint, vector, liquidRadius);
			}
		}
		liquidLastRecordPoint = vector;
	}

	private void Rotate(Vector3 velocity)
	{
		Vector3 vector = new Vector3(velocity.y / perimeter * 360f, 0f, (0f - velocity.x) / perimeter * 360f);
		tsf_Rotate.Rotate(vector * Time.deltaTime, Space.World);
	}

	private void HitCheckEF()
	{
		if (spellMucusTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(base.transform), liquidRadius * 2f);
		}
		if (spellVenomTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(base.transform), liquidRadius * 2f, spellVenomTime * 2f);
		}
		if (spellFrozenTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(base.transform), liquidRadius * 2f);
		}
	}

	public void TakeDamage(float damage)
	{
		if (!(currentHP <= 0f))
		{
			currentHP -= damage;
			if (currentHP <= 0f)
			{
				HitEFAndRecycle();
				PlaySE("Hit");
			}
			else
			{
				Effect.PlayBeHitEffect();
			}
		}
	}

	protected override void OnHitUnknownTagObject(Collider col)
	{
		if (col.CompareTag("Cliff"))
		{
			OnHitWallAndSolidObj(col);
		}
	}

	protected override bool OnHitBrittleness(GameObject go)
	{
		go.GetComponent<UnitProperty>().AnnouncedDeath();
		return true;
	}

	protected override bool OnHitDestructible(UnitProperty go)
	{
		UnitProperty component = go.GetComponent<UnitProperty>();
		return OnHitUnit(component);
	}

	protected override bool OnHitUnit(UnitProperty unit)
	{
		if (IsSameCamp(unit.unitCfg.unitType))
		{
			return false;
		}
		float num = unit.unitCfg.currentHP;
		MakeDamageToUnit(unit);
		CreateHitEffect();
		if (num < base.spellCfg.damage)
		{
			base.spellCfg.damage -= num;
			UpdateSizeByDamageAndVolumeRatio();
			TryRefractOrPenetrate(unit.gameObject);
		}
		else
		{
			TryRefractOrPenetrateOrRecycleOnHitTarget(unit.gameObject);
		}
		return true;
	}

	public override TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		if (base.shouldCameraShock)
		{
			CamController.Inst.SetShock(hitShock);
		}
		HitCheckEF();
		return base.OutputDamage(targetGO, info);
	}

	public override void HitEFAndRecycle()
	{
		if (base.shouldCameraShock)
		{
			CamController.Inst.SetShock(hitShock);
		}
		SpawnRectangleLiquid();
		HitCheckEF();
		CreateHitEffect();
		base.HitEFAndRecycle();
	}
}
