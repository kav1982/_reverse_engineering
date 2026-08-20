using System;
using UnityEngine;

public class Spell1019WaterEffectRemaster : SpellBase
{
	public Spell1019HighPressureWasherRemaster mainScript;

	public Spell1019WaterEffectRemaster previousSpell;

	public Spell1019WaterEffectRemaster nextSpell;

	public bool endPoint;

	public bool firstPoint;

	public LineRenderer line;

	public LineRenderer shadowLine;

	public float rotationNodeExtraLife;

	private bool ending;

	public float fallSpeed;

	public float maxHeight;

	private Vector3 effectSpawnPos = Vector3.zero;

	public float liquidRadiu;

	private UnitProperty lastRefractTarget;

	public Spell1019WaterEffectRemaster[] beforePointSplitBulletList { get; set; }

	public float splitDirection { get; set; }

	public float stackFallSpeed { get; set; }

	public override void InitializeCallback()
	{
		mainScript = null;
		previousSpell = null;
		nextSpell = null;
		endPoint = false;
		firstPoint = false;
		line.positionCount = 0;
		shadowLine.positionCount = 0;
		beforePointSplitBulletList = new Spell1019WaterEffectRemaster[base.spellSplitCount];
		splitDirection = 0f;
		base.enableAroundPlayer = true;
		enableFollowTarget = true;
		ending = false;
		stackFallSpeed = 0f;
		effectSpawnPos = Vector3.zero;
		lastRefractTarget = null;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		base.endThunderHitPercent = 0f;
	}

	public void SetTriggerController(Spell1019HighPressureWasherRemaster.SubSpellTriggerController tc)
	{
		base.TriggerCtrl = tc;
	}

	public override void Update()
	{
		base.Update();
		if (firstPoint)
		{
			mainScript.FirstWaterPosition = base.transform.position;
		}
		if (!base.SIP.spellIsFall)
		{
			base.DurationTimer += Time.deltaTime;
		}
		if (base.spellCfg.isSplitSpell && base.currentSpellMovement != SpellSpecialMovementType.Rotation && base.DurationTimer <= base.spellCfg.duration / 2f && base.transform.position.z < maxHeight && !base.SIP.spellIsFall)
		{
			base.transform.position += new Vector3(0f, 0f, stackFallSpeed * Time.deltaTime);
			stackFallSpeed += fallSpeed * Time.deltaTime;
			if (base.transform.position.z <= maxHeight)
			{
				stackFallSpeed = 0f;
			}
		}
		if (base.DurationTimer + ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? rotationNodeExtraLife : 0f) > base.spellCfg.duration && !base.SIP.spellIsFall)
		{
			if (!ending)
			{
				ending = true;
				if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					base.spellAroundOwnerCurrentAngle += 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
					base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
					rigid.linearVelocity = Tool2D.IgnoreZPoint(base.Direction) * base.CurrentSpeed;
				}
				base.enableAroundPlayer = false;
				spellFollowMouseLerp = 0f;
				spellFollowTargetRotateSpeed = 0f;
			}
			stackFallSpeed += fallSpeed * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, stackFallSpeed);
			if (base.transform.position.z >= 0f && !base.SIP.spellIsFall)
			{
				base.DurationTimer = base.spellCfg.duration + mainScript.nodeLife;
			}
		}
		float duration = base.spellCfg.duration;
		duration += mainScript.nodeLife;
		if (base.DurationTimer >= duration && !base.SIP.spellIsFall && !base.isFlyFinish)
		{
			base.isFlyFinish = true;
			rigid.linearVelocity = Vector3.zero;
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, maxHeight);
			}
			base.CurrentSpeed = 0f;
			if (previousSpell != null && previousSpell.gameObject.activeSelf)
			{
				effectSpawnPos = previousSpell.transform.position;
			}
			else
			{
				effectSpawnPos = base.transform.position;
			}
			if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				CreateHitEffect(effectSpawnPos);
				PlaySoundEffect();
				PoolRecycle();
			}
			else
			{
				CreateHitEffect(effectSpawnPos);
				PlaySoundEffect();
				PoolRecycle();
			}
		}
	}

	public override Vector3 GetAroundTargetBasePoint()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation && mainScript.SIP.tags.Contains(SpellTag.Twine))
		{
			return mainScript.GetAroundTargetBasePoint();
		}
		return base.GetAroundTargetBasePoint();
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		PlaySoundEffect();
		return base.MakeDamageToUnit(unit);
	}

	protected override void MakeDamageToDestructible(UnitProperty go)
	{
		PlaySoundEffect();
		base.MakeDamageToDestructible(go);
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		UpdateLineRenderers();
	}

	public AudioSource PlaySoundEffect()
	{
		if (mainScript.CheckValidPlaySE())
		{
			return PlaySE("Fall");
		}
		return null;
	}

	public void UpdateLineRenderers()
	{
		line.positionCount = 2;
		shadowLine.positionCount = 2;
		Vector3 position = base.transform.position;
		Vector3 vector = position;
		if (previousSpell == null || previousSpell.lastRefractTarget != lastRefractTarget)
		{
			if (!base.SIP.spellIsFall)
			{
				vector += base.Direction * 0.2f;
			}
		}
		else
		{
			vector = previousSpell.transform.position;
		}
		if (base.SIP.spellIsFall)
		{
			position.z -= 0.2f;
			vector.z -= 0.2f;
		}
		line.SetPosition(0, Tool2D.GetLayerPoint(position));
		line.SetPosition(1, Tool2D.GetLayerPoint(vector));
		if (!base.SIP.spellIsFall || base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			shadowLine.SetPosition(0, Tool2D.IgnoreZPoint(position, 1.05f));
			shadowLine.SetPosition(1, Tool2D.IgnoreZPoint(vector, 1.05f));
		}
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.canRebound = false;
		return takeDamageInfo;
	}

	public override void TriggerIn(Collider other)
	{
		if (previousSpell != null && previousSpell.gameObject.activeSelf)
		{
			effectSpawnPos = previousSpell.transform.position;
		}
		else
		{
			effectSpawnPos = base.transform.position;
		}
		base.TriggerIn(other);
	}

	public void SetGroundLiquid(Vector3 spawnPoint, Vector3 spawnPoint2)
	{
		if (spawnPoint2 == Vector3.zero)
		{
			spawnPoint2 = spawnPoint;
		}
		if (mainScript.ColorType == SpellColorType.Mucus)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(spawnPoint), liquidRadiu);
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(spawnPoint), Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu);
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu);
		}
		else if (mainScript.ColorType == SpellColorType.Venom)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(spawnPoint), liquidRadiu, spellVenomTime * 2f);
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(spawnPoint), Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu, spellVenomTime * 2f);
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu, spellVenomTime * 2f);
		}
		else if (mainScript.ColorType != SpellColorType.Fire && mainScript.ColorType != SpellColorType.Void)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(spawnPoint), liquidRadiu);
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(spawnPoint), Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu);
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(spawnPoint2), liquidRadiu);
		}
	}

	public override void PoolRecycle()
	{
		EndThunderAttackCheck();
		if (nextSpell != null && nextSpell.gameObject.activeSelf)
		{
			SetGroundLiquid(base.transform.position, nextSpell.transform.position);
		}
		else
		{
			SetGroundLiquid(base.transform.position, Vector3.zero);
		}
		if (endPoint)
		{
			mainScript.SpellEnd(Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, maxHeight), base.Direction);
		}
		line.positionCount = 0;
		shadowLine.positionCount = 0;
		if (mainScript.previousSpell == this)
		{
			mainScript.previousSpell = null;
		}
		if (previousSpell != null)
		{
			previousSpell.nextSpell = null;
			previousSpell.UpdateLineRenderers();
			previousSpell = null;
		}
		if (nextSpell != null)
		{
			nextSpell.previousSpell = null;
			nextSpell.UpdateLineRenderers();
			nextSpell = null;
		}
		UpdateLineRenderers();
		base.OwnerTsf = null;
		base.OwnerPoint = Vector3.zero;
		beforePointSplitBulletList = null;
		line.positionCount = 0;
		shadowLine.positionCount = 0;
		ObjPoolMgr.Inst.RecycleGO(base.gameObject);
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		EffectBase.CreateSpriteEffect("Hit", position, rotation);
		EffectBase.CreateSpriteEffect("Fall", position, rotation);
	}

	protected override UnitProperty TryRefract(params GameObject[] hitTarget)
	{
		UnitProperty unitProperty = (lastRefractTarget = base.TryRefract(hitTarget));
		if ((bool)unitProperty)
		{
			base.spellCfg.duration += 1f;
		}
		return unitProperty;
	}
}
