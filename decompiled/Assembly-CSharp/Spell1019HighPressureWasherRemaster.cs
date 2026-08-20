using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1019HighPressureWasherRemaster : SpellBase, IOnLaunchFromUnitNotPlayer
{
	public class SubSpellTriggerController : ISpellTriggerController
	{
		public Spell1019HighPressureWasherRemaster mainScript;

		public void InitAndEnable(SlotData[] triggers, SpellShootGroup group)
		{
		}

		public void Disable()
		{
		}

		public void TryTriggerOnStart()
		{
		}

		public bool HasOnStartTrigger()
		{
			return false;
		}

		public bool HasOnOverTrigger()
		{
			return mainScript.TriggerCtrl.HasOnOverTrigger();
		}

		public bool TryTriggerOnOver(Vector3 position, Vector3 direction, out IEnumerable<SpellBase> newSpells)
		{
			newSpells = null;
			return false;
		}

		public bool HasOnMoveTrigger()
		{
			return mainScript.TriggerCtrl.HasOnMoveTrigger();
		}

		public bool UpdateMoveTrigger(float moveDistance, Vector3 position, Vector3 direction, out IEnumerable<SpellBase> newSpells)
		{
			newSpells = null;
			mainScript.AddMoveDis(moveDistance, position, direction);
			return false;
		}

		public bool HasOnHitTrigger()
		{
			return mainScript.TriggerCtrl.HasOnHitTrigger();
		}

		public void AddHitTriggerPoint(Vector3 point)
		{
			mainScript.AddHitPoint(point);
		}

		public bool TryTriggerOnHit(out IEnumerable<SpellBase> newSpells)
		{
			newSpells = null;
			return false;
		}
	}

	public float nodeLife;

	public float bonusWatieTime;

	public float maxRotationLife;

	[HideInInspector]
	public Vector3 shootPosPositionShift = Vector3.zero;

	public float waterSePlayMinInterval;

	private float angleShift;

	private Vector3 directionInLastFrame = Vector3.zero;

	private readonly float extraWatingTime;

	private float extraWatingTimer;

	private bool isPlayerSpell = true;

	private bool keepCastBuffActing;

	private float seTimer;

	private float shootIntervalCounter;

	public Transform shootStartEffectTsf;

	private Vector3 shootStartPosition = Vector3.zero;

	private float splitDirection;

	public Spell1019WaterEffectRemaster previousSpell;

	private bool customAngleShift;

	private bool isSpellHoverStart;

	private SubSpellTriggerController _selfLinkTriggerController;

	public Vector3 FirstWaterPosition;

	private bool isHalfLifeTeleportEffectActive;

	private readonly List<(float, Vector3, Vector3)> _dis = new List<(float, Vector3, Vector3)>();

	public List<SpellBase> _nonPlayerUnitOwnerRepeatShootRecordList { get; set; }

	public override void SingleInitialCallback()
	{
		base.SingleInitialCallback();
		_selfLinkTriggerController = new SubSpellTriggerController
		{
			mainScript = this
		};
	}

	public override void Update()
	{
		base.Update();
		if (seTimer > 0f)
		{
			seTimer -= Time.deltaTime;
		}
		else
		{
			seTimer = 0f;
		}
		if (!base.isFlyFinish)
		{
			if (!isSpellHoverStart)
			{
				UpdateShooterData();
				if (base.SIP.spellIsFall)
				{
					if (base.spellCfg.isSplitSpell)
					{
						shootStartEffectTsf.right = Vector3.up;
					}
					else
					{
						shootStartEffectTsf.right = GetFallingViewDirection();
					}
					if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
					{
						shootStartEffectTsf.position = shootStartPosition;
					}
				}
				else
				{
					if (base.spellCfg.isSplitSpell)
					{
						shootStartEffectTsf.right = base.Direction;
					}
					else
					{
						shootStartEffectTsf.right = Tool2D.GetDir(directionInLastFrame, angleShift);
					}
					shootStartEffectTsf.position = shootStartPosition + shootPosPositionShift + Tool2D.GetDir(directionInLastFrame, -90f) * ((0f - ((float)base.InitialParameter.multiShootCount - 1f)) / 2f + (float)base.InitialParameter.inMultiShootIndex) * base.InitialParameter.multiShootSpace;
				}
			}
			UpdateSpawnData();
		}
		base.DurationTimer += Time.deltaTime;
		if (!(base.DurationTimer > base.spellCfg.duration + base.SpellHoverTime))
		{
			return;
		}
		if (base.DurationTimer >= base.spellCfg.duration)
		{
			EffectBase.ManualRecycleEffect("Start");
		}
		if (!base.isFlyFinish)
		{
			if ((object)previousSpell != null)
			{
				previousSpell.endPoint = true;
			}
			base.isFlyFinish = true;
			if (keepCastBuffActing)
			{
				((Spell1019Effect)EffectBase).StopStartEffect();
				keepCastBuffActing = false;
			}
		}
		if (base.spellSplitCount > 0 && extraWatingTimer < extraWatingTime)
		{
			extraWatingTimer += Time.deltaTime;
		}
	}

	public void AddHitPoint(Vector3 position)
	{
		base.TriggerCtrl?.AddHitTriggerPoint(position);
	}

	public void AddMoveDis(float dis, Vector3 pos, Vector3 dir)
	{
		_dis.Add((dis, pos, dir));
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (_dis.Count != 0)
		{
			float moveDistance = _dis.Select(((float, Vector3, Vector3) e) => e.Item1).Max();
			(float, Vector3, Vector3) tuple = _dis[UnityEngine.Random.Range(0, _dis.Count)];
			_dis.Clear();
			base.TriggerCtrl?.UpdateMoveTrigger(moveDistance, tuple.Item2, tuple.Item3, out var _);
		}
	}

	public override void InitializeCallback()
	{
		directionInLastFrame = Vector3.zero;
		shootIntervalCounter = 0f;
		shootStartPosition = base.transform.position;
		isPlayerSpell = ownerPpt.unitCfg.unitType == UnitType.Player;
		previousSpell = null;
		extraWatingTimer = bonusWatieTime + base.SpellHoverTime;
		keepCastBuffActing = false;
		customAngleShift = false;
		isSpellHoverStart = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		enableNormalTransform = false;
		shootPosPositionShift = Vector3.zero;
		angleShift = 0f;
		_nonPlayerUnitOwnerRepeatShootRecordList = null;
		splitDirection = UnityEngine.Random.Range(0, 360);
		seTimer = 0f;
		FirstWaterPosition = Vector3.zero;
		if (!isPlayerSpell || base.indirectShootByPlayer)
		{
			directionInLastFrame = base.Direction;
		}
		base.spellAroundOwnerCurrentAngle = UnityEngine.Random.Range(0f, 360f);
		if (isPlayerSpell && !base.spellCfg.isSplitSpell && !base.indirectShootByPlayer && !base.SIP.spellIsFall && !base.SIP.shootFromPostSlots && base.shooterWand == PlayerMgr.Inst.SelectedWand)
		{
			keepCastBuffActing = true;
		}
		isHalfLifeTeleportEffectActive = false;
		_dis.Clear();
	}

	public bool CheckValidPlaySE()
	{
		if (seTimer <= 0f)
		{
			seTimer = waterSePlayMinInterval;
			return true;
		}
		return false;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		PlayLoopSE("WaterWave", base.spellCfg.duration);
		if (!customAngleShift)
		{
			if (base.SIP.equalScatter)
			{
				angleShift = 0f - GetEqualScatterMultipleShootInitialDirectionAngleShift();
			}
			else
			{
				angleShift = UnityEngine.Random.Range(base._angle / 2f, (0f - base._angle) / 2f);
			}
		}
		if ((object)base.OwnerSpell == null && ownerPpt == PlayerMgr.Inst.PlayerPpt)
		{
			PlayerMgr.Inst.PlayerCtrller.OnWillTheme6Reposition += OnPlayerWillReposition;
		}
	}

	public override void SpellAroundPlayer()
	{
	}

	private void UpdateShooterData()
	{
		if (isPlayerSpell && !base.spellCfg.isSplitSpell && !base.indirectShootByPlayer && base.OwnerSpell == null)
		{
			if ((bool)base.shooterWand)
			{
				shootStartPosition = base.shooterWand.GetShootPosition();
			}
			else
			{
				shootStartPosition = PlayerMgr.Inst.ShootPoint;
			}
			directionInLastFrame = Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir * IsReverseDirection()));
			directionInLastFrame = ((base.shooterWand != null && base.shooterWand.WandCfg != null && base.shooterWand.WandCfg.specialAbility == WandAbility.FourDirShoot) ? Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir * IsReverseDirection()) + Tool2D.GetDegree(base.SIP.originShootDirection)) : Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir * IsReverseDirection())));
		}
		else if (isOwnerSpellValid())
		{
			shootStartPosition = base.OwnerSpell.transform.position;
		}
		if (base.OwnerPoint != Vector3.zero)
		{
			shootStartPosition = base.OwnerPoint;
		}
		if (base.SIP.spellIsFall && base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			shootStartPosition = GetShootPosition();
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (base.HalfLifeRandomTeleport())
		{
			isHalfLifeTeleportEffectActive = true;
			directionInLastFrame = Tool2D.GetDir();
			if (keepCastBuffActing)
			{
				((Spell1019Effect)EffectBase).StopStartEffect();
				keepCastBuffActing = false;
			}
			return true;
		}
		return false;
	}

	private void UpdateSpawnData()
	{
		shootIntervalCounter += Time.deltaTime;
		if (shootIntervalCounter < base.spellCfg.DPSDamageInterval || (base.DurationTimer >= base.spellCfg.duration && base.SpellHoverTimer >= base.SpellHoverTime))
		{
			return;
		}
		shootIntervalCounter -= base.spellCfg.DPSDamageInterval;
		SpellConfig configCopy = SpellConfig.GetConfigCopy(base.spellCfg.id);
		float num = configCopy.damage * base.spellCfg.DPSDamageInterval;
		if (base.spellCfg.isSplitSpell)
		{
			num *= 0.5f;
		}
		configCopy.damage = Mathf.Ceil(num);
		configCopy.speed *= base.speedRatio * base.finalSpeedRatio;
		configCopy.radius *= base.radiusRatio * base.finalRadiusRatio;
		configCopy.knockback *= base.knockbackRatio * base.finalKnockbackRatio;
		configCopy.isSplitSpell = base.spellCfg.isSplitSpell;
		SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/10192/10192WaterTrail", GetShootPosition()).GetComponent<SpellBase>();
		float degree = (base.spellCfg.isSplitSpell ? 0f : angleShift);
		Vector3 direction = directionInLastFrame;
		if (base.SIP.spellIsFall)
		{
			direction = base.Direction;
		}
		SpellInitialParameter spellInitialParameter = new SpellInitialParameter(ownerPpt, direction, base.spellCfg.id, configCopy, base.shooterWand, preSpellIDs)
		{
			extraCriticalChance = GetCriticalChance(),
			RefractionInfo = base.SIP.RefractionInfo,
			fallExplosionRadius = base.SIP.fallExplosionRadius,
			ColorType = base.ColorType
		};
		if (base.SIP.spellIsFall)
		{
			ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
			if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
			{
				spellInitialParameter.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(base.SIP.finalShootSpatialInfo.Start, base.SIP.finalShootSpatialInfo.Target.Value);
				spellInitialParameter.originShootSpatialInfo = spellInitialParameter.finalShootSpatialInfo.Copy();
			}
		}
		component.Initialize(spellInitialParameter);
		component.ColorType = base.ColorType;
		if (!base.SIP.spellIsFall)
		{
			component.Direction = Tool2D.GetDir(directionInLastFrame, degree);
		}
		component.spellCfg.radius = base.spellCfg.radius;
		component.radiusRatio = base.radiusRatio;
		component.finalRadiusRatio = base.finalRadiusRatio;
		component.spellCfg.damage = Mathf.Ceil(base.spellCfg.damage * base.spellCfg.DPSDamageInterval);
		component.currentSpellMovement = base.currentSpellMovement;
		component.spellAroundOwnerRadius = base.spellAroundOwnerRadius;
		component.voidExplosionInfo = voidExplosionInfo;
		if (!base.SIP.spellIsFall)
		{
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				component.rigid.linearVelocity = Vector3.zero;
			}
			else
			{
				component.rigid.linearVelocity = component.Direction * base.CurrentSpeed;
			}
		}
		component.OwnerSpell = base.OwnerSpell;
		component.rebounceTime = base.rebounceTime;
		component.indirectShootByPlayer = !isPlayerSpell;
		if (isHalfLifeTeleportEffectActive)
		{
			component.indirectShootByPlayer = true;
		}
		Spell1019WaterEffectRemaster spell1019WaterEffectRemaster = (Spell1019WaterEffectRemaster)component;
		spell1019WaterEffectRemaster.mainScript = this;
		spell1019WaterEffectRemaster.SetTriggerController(_selfLinkTriggerController);
		spell1019WaterEffectRemaster.splitDirection = splitDirection;
		spell1019WaterEffectRemaster.nextSpell = null;
		spell1019WaterEffectRemaster.previousSpell = null;
		if ((object)previousSpell != null)
		{
			spell1019WaterEffectRemaster.previousSpell = previousSpell;
			previousSpell.nextSpell = spell1019WaterEffectRemaster;
		}
		else
		{
			spell1019WaterEffectRemaster.previousSpell = null;
			spell1019WaterEffectRemaster.firstPoint = true;
		}
		previousSpell = spell1019WaterEffectRemaster;
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			if (isPlayerSpell && base.OwnerSpell == null)
			{
				component.spellCfg.duration = Vector3.Distance(PlayerMgr.Inst.GetMousePoint(), shootStartPosition) / base.CurrentSpeed;
			}
			else
			{
				component.spellCfg.duration = nodeLife;
			}
			if (base.currentLevelPureCfg.isSplitSpell)
			{
				component.spellCfg.duration *= 0.5f;
			}
		}
		else
		{
			component.spellCfg.duration = Mathf.Max(maxRotationLife, 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius * base.CurrentSpeed));
			if (base.SIP.tags.Contains(SpellTag.Twine))
			{
				component.spellCfg.duration /= 5f;
			}
		}
		component.wandChargeData = base.wandChargeData;
		component.spellAroundOwnerCurrentAngle = base.spellAroundOwnerCurrentAngle;
		if (base.OwnerPoint != Vector3.zero || base.OwnerTsf != null)
		{
			component.OwnerTsf = base.OwnerTsf;
			component.OwnerPoint = base.OwnerPoint;
		}
	}

	public void SetLaserOwnerData(Vector3 laserstartpoint, Vector3 laserDir, bool cancelKeepCastingBuff = true)
	{
		isPlayerSpell = false;
		shootStartPosition = laserstartpoint;
		directionInLastFrame = laserDir;
		if (keepCastBuffActing && cancelKeepCastingBuff)
		{
			keepCastBuffActing = false;
			((Spell1019Effect)EffectBase).StopStartEffect();
		}
	}

	public void SpellEnd(Vector3 pos, Vector3 dir)
	{
		base.transform.position = pos;
		base.Direction = dir;
		base.TriggerCtrl?.TryTriggerOnOver(pos, base.Direction, out var _);
		PoolRecycle();
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		Spell1019HighPressureWasherRemaster obj = (Spell1019HighPressureWasherRemaster)base.CreateSplitSpell(splitCfg, baseAngle, index);
		Vector3 position = obj.transform.position;
		obj.SetLaserOwnerData(position, (position - base.transform.position).normalized);
		return obj;
	}

	public void OnDisable()
	{
		EffectBase.ManualRecycleEffect("Start");
		PlayerMgr.Inst.PlayerCtrller.OnWillTheme6Reposition -= OnPlayerWillReposition;
	}

	private void OnPlayerWillReposition(Vector3 newPos)
	{
		previousSpell = null;
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate5 teammate)
		{
			teammate.currentCastingSpell.Add((this, teammate.GetShootMode()));
			SetLaserOwnerData(teammate.shootPosition.position, Tool2D.IgnoreZPoint(teammate.lastFrameDirection));
		}
		else if (unit is Teammate52 teammate2)
		{
			teammate2.currentCastingSpell.Add(this);
			SetLaserOwnerData(teammate2.shootPosition.position, Tool2D.IgnoreZPoint(teammate2.lastFrameTargetDirection));
		}
	}

	public override void OnNeedTriggerOnOver()
	{
	}

	public Vector3 GetShootPosition()
	{
		if (base.spellCfg.isSplitSpell)
		{
			return base.transform.position;
		}
		Vector3 result = shootStartPosition + Tool2D.GetDir(directionInLastFrame, -90f) * (((0f - ((float)base.InitialParameter.multiShootCount - 1f)) / 2f + (float)base.InitialParameter.inMultiShootIndex) * base.InitialParameter.multiShootSpace);
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation && !base.indirectShootByPlayer)
		{
			result = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
		}
		if (base.SIP.spellIsFall)
		{
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				result.z = 0f - FallInitialHeight;
			}
			else
			{
				result = base.transform.position;
			}
		}
		return result;
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override void HeightFixedUpdate()
	{
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawCube(Tool2D.GetLayerPoint(base.transform.position), Vector3.one * 0.3f);
	}
}
