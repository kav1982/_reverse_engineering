using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1021MagicBreaker : SpellBase, IOnLaunchFromUnitNotPlayer
{
	private enum SlashStage
	{
		Before,
		Normal,
		After
	}

	private float fallDelayRecycleTime;

	private float delayRecycleTimer;

	public GameObject swordTrigger;

	public Transform Trigger;

	public Transform bladeDistanceAndRotateCenter;

	public float initialDistance;

	public Vector3 bladeBaseHeight;

	private static bool staticCounterclockMovementCounter;

	[HideInInspector]
	public int counterclockMovement = 1;

	public AnimationCurve swordSlashSpeedCurve;

	public float fadeinTime;

	public float fadeoutTime;

	private float fadeTimer;

	public float preAndPostCastPercent;

	public float defaultRotateAnglePerSecond;

	private float slashTime;

	private float slashTimer;

	private SlashStage currentSlashStage;

	[HideInInspector]
	public Vector3 slashInitialDirection = Vector3.zero;

	private float slashAngle;

	public float initialAngleBaseShift;

	private float initialAngleShiftAmount;

	public BoxCollider hitBox;

	public float bladeSpriteEdgeLength;

	public float swordSizeBaseRatio;

	public float minSlashAngle;

	public float angleToSpeedRatio;

	public Transform normalCenterTransform;

	public Transform centerTransform;

	[Header("Sound")]
	public VariableFloat soundSlashPitch;

	public VariableFloat soundHitPitch;

	public float minEffectRadiuRatio;

	public float bladeWitdthPow;

	public float sizePow;

	public float ZaxisHeight;

	public float SwordFallSpeedRatio;

	public float SwordFallDelayRecycleTime;

	public ShockParam SwordFallGroundShock;

	private readonly HashSet<GameObject> fallHitTargets = new HashSet<GameObject>();

	protected override float FallInitialHeight => 10.5f;

	private bool AuthorizeCheck()
	{
		if (!ownerPpt.CompareTag("Player") && !ownerPpt.CompareTag("Teammate"))
		{
			return false;
		}
		return true;
	}

	private void ResetParameter()
	{
		fadeTimer = 0f;
		slashTime = 0f;
		slashTimer = 0f;
		slashAngle = 0f;
		currentSlashStage = SlashStage.Before;
		slashInitialDirection = Vector3.zero;
		initialAngleShiftAmount = 0f;
		if (base.SIP.spellIsFall)
		{
			enableFollowMouse = true;
			enableFollowTarget = true;
			base.enableAroundPlayer = true;
		}
		else
		{
			enableFollowMouse = false;
			enableFollowTarget = false;
			base.enableAroundPlayer = false;
		}
		delayRecycleTimer = 0f;
		fallDelayRecycleTime = 0f;
	}

	private void SetInitialDistance()
	{
		if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			bladeDistanceAndRotateCenter.localPosition = new Vector3(initialDistance, 0f, 0f);
			Trigger.localPosition = new Vector3(initialDistance, 0f, 0f);
		}
		else
		{
			bladeDistanceAndRotateCenter.localPosition = new Vector3(base.spellAroundOwnerRadius * base.radiusRatio * base.finalRadiusRatio, 0f, 0f);
			Trigger.localPosition = new Vector3(base.spellAroundOwnerRadius * base.radiusRatio * base.finalRadiusRatio, 0f, 0f);
		}
	}

	private void SetSlashDirection()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			counterclockMovement = 1;
			return;
		}
		if (base.spellCfg.isSplitSpell)
		{
			counterclockMovement = 1;
			return;
		}
		staticCounterclockMovementCounter = !staticCounterclockMovementCounter;
		counterclockMovement = (staticCounterclockMovementCounter ? 1 : (-1));
	}

	private void CalculateSlashInitialDirection()
	{
		float num = Mathf.Max(minSlashAngle, base.wandShootAngle);
		if (!base.spellCfg.isSplitSpell)
		{
			Vector3 oldDir = base.originalShootDirection * IsReverseDirection();
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				oldDir = Tool2D.IgnoreZPoint(Random.insideUnitSphere).normalized;
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
			{
				oldDir = ToPointDir(PlayerMgr.Inst.GetMousePoint(base.transform.position.z));
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
			{
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
				if (base.SpellFollowHaveTarget)
				{
					oldDir = ((ToPointDir(spellFollowTargetPpt.transform) == Vector3.zero) ? Tool2D.IgnoreZPoint(Random.insideUnitSphere.normalized) : ToPointDir(spellFollowTargetPpt.transform));
				}
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
			{
				oldDir = ToPointDir(ownerPpt.transform.position);
			}
			if (base.InitialParameter.zeroAngleShift)
			{
				oldDir = base.Direction;
			}
			slashAngle = num;
			float num2 = base.spellCfg.speed / SpellConfig.dic[base.spellCfg.id].speed;
			if (num > SpellConfig.dic[base.spellCfg.id].angle)
			{
				num2 *= 1f + (num - SpellConfig.dic[base.spellCfg.id].angle) * angleToSpeedRatio;
			}
			slashTime = slashAngle / defaultRotateAnglePerSecond / num2;
			base.originalShootDirection = oldDir;
			base.Direction = Tool2D.GetDir(oldDir, slashAngle / 2f * (float)counterclockMovement * -1f);
			slashInitialDirection = base.Direction;
			base.transform.right = base.Direction;
		}
		else
		{
			slashAngle = num;
			float num3 = base.spellCfg.speed / SpellConfig.dic[base.spellCfg.id].speed;
			if (num > SpellConfig.dic[base.spellCfg.id].angle)
			{
				num3 *= 1f + (num - SpellConfig.dic[base.spellCfg.id].angle) * angleToSpeedRatio;
			}
			slashTime = slashAngle / defaultRotateAnglePerSecond / num3;
			slashInitialDirection = base.Direction;
			base.transform.right = base.Direction;
		}
	}

	private void UpdateHitBoxSize()
	{
		float num = ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? base.spellAroundOwnerRadius : 0f);
		hitBox.size = new Vector3(bladeSpriteEdgeLength * swordSizeBaseRatio * base.radiusRatio * base.finalRadiusRatio, Mathf.Pow(base.radiusRatio * base.finalRadiusRatio, bladeWitdthPow), ZaxisHeight * 2f * hitBox.transform.localScale.y);
		hitBox.transform.localPosition = new Vector3(Mathf.Max(0f, (base.radiusRatio * base.finalRadiusRatio - 1f) * bladeSpriteEdgeLength * swordSizeBaseRatio / 2f + num * base.radiusRatio * base.finalRadiusRatio), 0f, hitBox.transform.localPosition.z);
	}

	public override void InitializeCallback()
	{
		if (base.SIP.spellIsFall)
		{
			tsf_Layer = normalCenterTransform;
		}
		else
		{
			tsf_Layer = centerTransform;
		}
		fallHitTargets.Clear();
		if (!AuthorizeCheck())
		{
			PoolRecycle();
			return;
		}
		ResetParameter();
		if (base.radiusRatio * base.finalRadiusRatio <= minEffectRadiuRatio && !base.SIP.spellIsFall)
		{
			base.spellCfg.radius = base.spellCfg.radius / base.radiusRatio / base.finalRadiusRatio;
			base.radiusRatio = minEffectRadiuRatio;
			base.finalRadiusRatio = 1f;
			base.spellCfg.radius = base.spellCfg.radius * base.radiusRatio * base.finalRadiusRatio;
		}
		SetInitialDistance();
		base.transform.localScale = Vector3.one * Mathf.Pow(base.damageRatio * base.finalDamageRatio, sizePow) * base.spellVolumeRatio;
		SetSlashDirection();
		if (!base.SIP.spellIsFall)
		{
			CalculateSlashInitialDirection();
			UpdateHitBoxSize();
			CorrectLayerOnce();
		}
		swordTrigger.SetActive(value: false);
		centerTransform.localPosition = tsf_Layer.localPosition;
		PlaySE("Slash").pitch = soundSlashPitch.RandomResult();
		if (base.SIP.spellIsFall)
		{
			ApplySpeedToVelocity();
			fallDelayRecycleTime = SwordFallDelayRecycleTime + base.SpellHoverTime;
			base.spellCfg.speed = (SpellConfig.dic[base.spellCfg.id].speed * SwordFallSpeedRatio + base.bonusSpeed) * base.speedRatio * base.finalSpeedRatio;
			base.CurrentSpeed = base.spellCfg.speed * GameConst.spellFallAngleCos;
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (!base.spellCfg.isSplitSpell)
		{
			if (initialAngleShiftAmount != 0f)
			{
				slashInitialDirection = Tool2D.GetDir(slashInitialDirection, Random.Range(0f - initialAngleShiftAmount, initialAngleShiftAmount));
			}
			else
			{
				float num = Mathf.Min(initialAngleBaseShift, slashAngle);
				slashInitialDirection = Tool2D.GetDir(slashInitialDirection, Random.Range(0f - num, num) * (float)base.InitialParameter.multiShootCount);
			}
		}
		centerTransform.localPosition = new Vector3(0f, 0f, tsf_Layer.localPosition.z);
		if (base.SIP.spellIsFall)
		{
			EffectBase.ManualCreateEffect("SpellFall");
			EffectBase.ManualCreateEffect("Shadow");
			base.Direction = rigid.linearVelocity.normalized;
			InitSpeedAndPosition();
		}
		else
		{
			EffectBase.ManualCreateEffect("Spell");
			EffectBase.ManualCreateEffect("NormalShadow");
			base.transform.position = ((base.virtualRealPosition == Vector3.zero) ? (GetAroundTargetBasePoint() + base.Direction * 0.05f + bladeBaseHeight) : (base.virtualRealPosition + base.Direction * 0.05f + bladeBaseHeight));
			swordTrigger.SetActive(value: true);
		}
	}

	public override void Update()
	{
		base.Update();
		if (!base.isFlyFinish)
		{
			if (base.SIP.spellIsFall)
			{
				return;
			}
			float degree = swordSlashSpeedCurve.Evaluate(Mathf.Clamp(slashTimer / slashTime, 0f, 1f)) * slashAngle * (float)counterclockMovement;
			base.Direction = Tool2D.IgnoreZPoint(Tool2D.GetDir(slashInitialDirection, degree));
			base.transform.right = base.Direction;
			switch (currentSlashStage)
			{
			case SlashStage.Before:
				base.transform.position = ((base.virtualRealPosition == Vector3.zero) ? (GetAroundTargetBasePoint() + base.Direction * 0.05f + bladeBaseHeight) : (base.virtualRealPosition + base.Direction * 0.05f + bladeBaseHeight));
				slashTimer += Time.deltaTime;
				if (fadeTimer < fadeinTime)
				{
					fadeTimer += Time.deltaTime;
					break;
				}
				currentSlashStage = SlashStage.Normal;
				fadeTimer = 0f;
				break;
			case SlashStage.Normal:
				if (slashTimer >= slashTime * (1f - preAndPostCastPercent))
				{
					if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
					{
						base.SpellHoverTimer += Time.deltaTime;
						break;
					}
					currentSlashStage = SlashStage.After;
					swordTrigger.SetActive(value: false);
				}
				else
				{
					base.transform.position = ((base.virtualRealPosition == Vector3.zero) ? (GetAroundTargetBasePoint() + base.Direction * 0.05f + bladeBaseHeight) : (base.virtualRealPosition + base.Direction * 0.05f + bladeBaseHeight));
					slashTimer += Time.deltaTime;
				}
				break;
			case SlashStage.After:
				if (base.SpellHoverTime <= 0f && slashAngle >= 10f)
				{
					slashInitialDirection = Tool2D.GetDir(slashInitialDirection, 5f * base.spellCfg.speed * (float)counterclockMovement * Time.deltaTime);
				}
				if (fadeTimer < fadeoutTime)
				{
					swordTrigger.SetActive(value: false);
					fadeTimer += Time.deltaTime;
					break;
				}
				base.isFlyFinish = true;
				if (base.SpellHoverTime <= 0f)
				{
					slashTimer += Time.deltaTime;
				}
				break;
			}
			return;
		}
		delayRecycleTimer += Time.deltaTime;
		if (delayRecycleTimer < fallDelayRecycleTime)
		{
			if (base.SIP.spellIsFall)
			{
				OnGroundDealDamageToNearTargets();
			}
		}
		else
		{
			PoolRecycle();
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (slashTimer <= slashTime * (1f - preAndPostCastPercent) / 2f || halfLifeTeleportCount <= 0)
		{
			return false;
		}
		halfLifeTeleportCount--;
		Vector3 position = base.transform.position;
		SpawnHalfLifeTeleportEffect(position);
		position += Random.insideUnitSphere.IgnoreZ().normalized * Random.Range(3f, 5f) * base.radiusRatio * base.finalRadiusRatio;
		base.transform.position = position;
		base.OwnerPoint = position;
		base.indirectShootByPlayer = true;
		SpawnHalfLifeTeleportEffect(position);
		slashTimer = 0f;
		base.CurrentSpeed += 2f;
		base.virtualRealPosition = Vector3.zero;
		slashInitialDirection = Tool2D.GetDir();
		return false;
	}

	protected override void InitSpeedAndPosition()
	{
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.speed += 10f;
		}
		base.InitSpeedAndPosition();
	}

	private void OnGroundDealDamageToNearTargets()
	{
		foreach (Collider item in from e in GeneralTool.GetCollidersByTag(base.transform.position, base.transform.localScale.x, "Monster", "Destructible", "Spell", "RollBall", "Butterfly")
			where e.gameObject.activeInHierarchy && !fallHitTargets.Contains(e.gameObject)
			select e)
		{
			if (item.gameObject.CompareAnyTag("Spell"))
			{
				TryReflectTargetSpell(item.GetComponentInParent<SpellBase>());
			}
			else if (item.gameObject.CompareAnyTag("RollBall", "Butterfly"))
			{
				SpellBase componentInParent = item.GetComponentInParent<SpellBase>();
				if (componentInParent.IsSameCamp(this))
				{
					continue;
				}
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
			else if (item.gameObject.CompareAnyTag("Monster", "Destructible"))
			{
				UnitProperty component = item.gameObject.GetComponent<UnitProperty>();
				TakeDamageInfo info = new TakeDamageInfo
				{
					canRebound = false
				};
				if (!IsSameCamp(component.unitCfg.unitType))
				{
					OutputDamage(item.gameObject, info);
				}
				CreateHitEffect(item.transform.position);
			}
			fallHitTargets.Add(item.gameObject);
		}
	}

	private void tryRecycleSpell(SpellBase _spellBase)
	{
		if (!_spellBase.IsSameCamp(ownerPpt.unitCfg.unitType) && _spellBase.spellCfg.abilityType != SpellAbilityType.Dash)
		{
			CreateHitEffect(_spellBase.transform.position);
			_spellBase.PoolRecycle();
		}
	}

	public override void TriggerIn(Collider other)
	{
		UnitProperty component = other.GetComponent<UnitProperty>();
		switch (other.tag)
		{
		case "Wall":
			break;
		case "SolidObj":
			break;
		case "Spell":
			TryReflectTargetSpell(other.GetComponentInParent<SpellBase>());
			break;
		case "Player":
		case "Teammate":
			if (ownerPpt != null && ownerPpt.tag != "Player" && ownerPpt.tag != "Teammate")
			{
				OutputDamage(component, new TakeDamageInfo
				{
					canRebound = false
				});
			}
			break;
		case "Monster":
			if (ownerPpt != null && ownerPpt.gameObject.activeInHierarchy && ownerPpt.tag != "Monster")
			{
				CreateHitEffect(component.transform.position);
				OutputDamage(component, new TakeDamageInfo
				{
					canRebound = false
				});
			}
			break;
		case "Destructible":
			CreateHitEffect(component.transform.position);
			OutputDamage(other.gameObject);
			break;
		case "RollBall":
		{
			Spell1002RollBall componentInParent2 = other.GetComponentInParent<Spell1002RollBall>();
			if (!IsSameCamp(componentInParent2))
			{
				CreateHitEffect(componentInParent2.transform.position);
				componentInParent2.TakeDamage(base.spellCfg.damage);
			}
			break;
		}
		case "Butterfly":
		{
			Spell1003Butterfly componentInParent = other.GetComponentInParent<Spell1003Butterfly>();
			if (!IsSameCamp(componentInParent))
			{
				componentInParent.Break();
			}
			break;
		}
		case "Brittleness":
			OutputDamage(other.gameObject, new TakeDamageInfo
			{
				isPlayHitSE = false
			});
			break;
		}
	}

	protected override void OnFallingGround()
	{
		CamController.Inst.SetShock(SwordFallGroundShock);
		MakeFallingGroundDamageToAround();
		OnFallingGroundTryReboundOrRecycle();
		CreateHitEffect(base.transform.position);
	}

	protected override void OnFallingGroundTryReboundOrRecycle()
	{
		if (!OnFallingGroundTryRebound())
		{
			base.isFlyFinish = true;
			base.CurrentSpeed *= 0.001f;
			base.CurrentUpSpeed *= 0.001f;
			rigid.linearVelocity = base.CurrentSpeed * base.Direction;
			EffectBase.ManualCreateEffect("GroundTrace");
		}
		else
		{
			fallHitTargets.Clear();
			PlaySE("Slash").pitch = soundSlashPitch.RandomResult();
		}
	}

	protected override bool TryFallingRebound()
	{
		if (base.rebounceTime > 0 && base.CurrentUpSpeed < 0f)
		{
			base.CurrentUpSpeed = 18f;
			rigid.linearVelocity = base.Direction * base.CurrentSpeed / 2f;
			base.InFallRebounding = true;
			base.rebounceTime--;
			return true;
		}
		return false;
	}

	protected override bool OnAOEHitSpell(SpellBase spell)
	{
		TryReflectTargetSpell(spell);
		return false;
	}

	protected override bool OnAOEHitUnit(UnitProperty unit)
	{
		if (unit.gameObject.CompareTag("Monster"))
		{
			CreateHitEffect(unit.transform.position);
			CreateHitEffect(unit.transform.position);
			fallHitTargets.Add(unit.gameObject);
		}
		return base.OnAOEHitUnit(unit);
	}

	protected override bool OnAOEHitDestructible(UnitProperty go)
	{
		CreateHitEffect(go.transform.position);
		CreateHitEffect(go.transform.position);
		fallHitTargets.Add(go.gameObject);
		return base.OnAOEHitDestructible(go);
	}

	private void TryReflectTargetSpell(SpellBase _spellBase)
	{
		if (IsSameCamp(_spellBase) || !_spellBase.CanChangeTeam)
		{
			return;
		}
		if (base.currentLevelPureCfg.level == 1 || !SpellConfig.dic.ContainsKey(_spellBase.spellCfg.id))
		{
			tryRecycleSpell(_spellBase);
			return;
		}
		UnitProperty unitProperty = _spellBase.ownerPpt;
		if (ownerPpt.CompareTag("Monster"))
		{
			_spellBase.ChangeTeamToMonster(ownerPpt);
		}
		else
		{
			if (!ownerPpt.CompareTag("Player") && !ownerPpt.CompareTag("Teammate"))
			{
				tryRecycleSpell(_spellBase);
				return;
			}
			_spellBase.ChangeTeamToPlayer();
		}
		float num = Mathf.Max(_spellBase.rigid.linearVelocity.magnitude, SpellConfig.dic[_spellBase.spellCfg.id].speed) * 1.5f;
		if (base.currentLevelPureCfg.level == 2)
		{
			_spellBase.rigid.linearVelocity = base.originalShootDirection.normalized * num;
		}
		else
		{
			Vector3 normalized = (unitProperty.transform.position - _spellBase.transform.position).normalized;
			_spellBase.rigid.linearVelocity = normalized * num;
		}
		_spellBase.Direction = _spellBase.rigid.linearVelocity.normalized;
	}

	private void CreateHitEffect(Vector3 targetPosition)
	{
		base.CreateHitEffect(targetPosition + new Vector3(0f, 0.3f, 0f) + Tool2D.IgnoreZPoint(Random.insideUnitSphere) * 0.2f);
		PlaySE("Hit").pitch = soundHitPitch.RandomResult();
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		Spell1021MagicBreaker spell1021MagicBreaker = (Spell1021MagicBreaker)base.CreateSplitSpell(splitCfg, baseAngle, index);
		if (!base.SIP.spellIsFall)
		{
			spell1021MagicBreaker.virtualRealPosition = base.transform.position + base.originalShootDirection * splitCfg.radius;
		}
		Vector3 right = (spell1021MagicBreaker.Direction = spell1021MagicBreaker.InitialParameter.shootDirection);
		spell1021MagicBreaker.transform.right = right;
		spell1021MagicBreaker.slashInitialDirection = right;
		spell1021MagicBreaker.originalShootDirection = right;
		spell1021MagicBreaker.counterclockMovement = counterclockMovement;
		if (!base.SIP.spellIsFall)
		{
			spell1021MagicBreaker.transform.position = spell1021MagicBreaker.virtualRealPosition;
		}
		return spell1021MagicBreaker;
	}

	public override void PoolRecycle()
	{
		if (!base.SIP.spellIsFall)
		{
			base.virtualRealPosition = base.transform.position + base.originalShootDirection * base.spellCfg.radius;
		}
		base.PoolRecycle();
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate52 teammate)
		{
			ownerPpt = teammate.myPpt;
		}
	}
}
