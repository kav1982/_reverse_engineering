using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1025DragonBreath : SpellBase, IOnLaunchFromSpellEventHandle, IOnLaunchFromUnitNotPlayer
{
	public float attackDistanceIncreaseSpeed;

	public float minAttackDistanceRatio;

	public float speedToExtraAttackDistanceRatio;

	public float flameBaseWidth;

	public float flameBaseCircleRadiu;

	public float recoilInDuration;

	public float minScatter;

	public GameObject toP;

	private float minAttackDistance;

	private float attackTimer;

	private bool shootFromTrigger;

	private Vector3 lastFrameDirection = Vector3.zero;

	private bool keepCastBuffApplied;

	private float powerUpStackDamageRatio;

	public float fallFireBaseEffectRadiu;

	public float fallFirePreAttackTime;

	public Vector3[] lastFallFireLinePoints;

	public Vector3 currentFallDamageCenter;

	public float RotateMovementFireWidth;

	private static readonly Collider[] attackTargetBuffer = new Collider[200];

	private static readonly HashSet<string> attackTargetTags = new HashSet<string> { "Monster", "Destructible", "RollBall", "Butterfly", "Brittleness" };

	public float maxAttackDistance { get; private set; }

	public float currentAttackDistance { get; private set; }

	public override void InitializeCallback()
	{
		base.InitializeCallback();
		minAttackDistance = base.spellCfg.float1 * minAttackDistanceRatio;
		currentAttackDistance = minAttackDistance;
		maxAttackDistance = base.spellCfg.float1 * (1f + speedToExtraAttackDistanceRatio * base.CurrentSpeed * base.radiusRatio * base.finalRadiusRatio);
		attackTimer = 0f;
		lastFrameDirection = base.Direction;
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		powerUpStackDamageRatio = 0f;
		base.wandShootAngle = Mathf.Max(minScatter, base.wandShootAngle);
		keepCastBuffApplied = false;
		PlayerRegisterKeepCastingBuff();
		if (IsSameCamp(UnitType.Player))
		{
			PlayLoopSE("Loop", base.spellCfg.duration + base.SpellHoverTime);
		}
		PlaySE("Start");
		EffectBase.ManualCreateEffect("Start");
		if (base.SIP.spellIsFall)
		{
			base.CurrentSpeed = 0f;
			base.enableAroundPlayer = true;
			enableFollowMouse = true;
			enableFollowTarget = true;
			base.SIP.finalShootSpatialInfo.Target = base.SIP.finalShootSpatialInfo.Target.Value.IgnoreZ();
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (base.SIP.spellIsFall)
		{
			EffectBase.ManualCreateEffect("FireLine");
		}
		else
		{
			EffectBase.ManualCreateEffect((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? "RotationSpell" : "Spell");
		}
	}

	private void PlayerRegisterKeepCastingBuff()
	{
		if (ownerPpt.unitCfg.unitType == UnitType.Player && !base.spellCfg.isSplitSpell && !keepCastBuffApplied && !base.indirectShootByPlayer && !base.SIP.spellIsFall && !base.SIP.shootFromPostSlots && base.shooterWand == PlayerMgr.Inst.SelectedWand)
		{
			keepCastBuffApplied = true;
		}
	}

	protected override void InitSpeedAndPosition()
	{
		base.InitSpeedAndPosition();
		if (base.SIP.spellIsFall)
		{
			ChangeCurrentSpeed(0f);
			base.CurrentUpSpeed = 0f;
		}
	}

	private void PlayerUnRegisterKeepCastingBuff()
	{
		_ = keepCastBuffApplied;
	}

	public override void Update()
	{
		base.Update();
		IncreaseAttackDistanceInDuration();
		UpdatePowerUpTimer();
		if (base.SIP.spellIsFall)
		{
			lastFallFireLinePoints = GetFallingFireLine();
			Vector3[] groundPoints = (from e in lastFallFireLinePoints.Where((Vector3 e) => Mathf.Approximately(e.z, 0f)).Select((Vector3 e, int i) => (e, i)).Where<(Vector3, int)>(delegate((Vector3 e, int i) e)
				{
					float num = (float)e.i * 0.4f + 0.35f;
					return base.DurationTimer >= num;
				})
				select e.e).ToArray();
			((Spell1025Effect)EffectBase).UpdateFallFireLine(lastFallFireLinePoints.Select(Tool2D.GetLayerPoint).ToArray());
			((Spell1025Effect)EffectBase).UpdateFallGround(groundPoints);
			FallingDamageUpdate(groundPoints);
		}
		else
		{
			DealDamageToAllEnemyInRange();
			UpdateShootPoint();
			UpdateShootDir();
		}
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				PlayerUnRegisterKeepCastingBuff();
				keepCastBuffApplied = false;
			}
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (!base.SIP.spellIsFall && base.currentSpellMovement != SpellSpecialMovementType.Rotation)
		{
			ApplyRecoilToOwnerUnit();
		}
		toP.transform.position = base.transform.position + base.Direction * currentAttackDistance;
	}

	public override void SpellAroundPlayer()
	{
		if (base.SIP.spellIsFall)
		{
			Vector3 position = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius + GetAroundTargetBasePoint();
			position.z = base.transform.position.z;
			base.transform.position = position;
		}
		base.SpellAroundPlayer();
	}

	private Vector3[] GetFallingFireLine()
	{
		List<Vector3> list = new List<Vector3>();
		int num = base.rebounceTime;
		float num2 = base.spellAroundOwnerCurrentAngle;
		Vector3 direction = base.Direction;
		if (base.spellCfg.isSplitSpell)
		{
			list.Add(base.transform.position);
			AppendReboundingFireLinePoints(list);
		}
		else
		{
			AppendFirstFireLinePoints(list);
		}
		List<GameObject> list2 = (from e in GetFallingGroundDamageTargets()
			select e.gameObject).ToList();
		while (true)
		{
			Vector3? vector = null;
			if (base.remainRefractCount > 0 && list2.Count > 0)
			{
				refractedTargets.Clear();
				UnitProperty unitProperty = TryRefract(list2.ToArray());
				if ((bool)unitProperty)
				{
					list2.Add(unitProperty.gameObject);
					vector = unitProperty.transform.position;
				}
			}
			if (num > 0 && !vector.HasValue)
			{
				num--;
				vector = base.transform.position + base.Direction * 2f;
			}
			if (!vector.HasValue)
			{
				break;
			}
			AppendReboundingFireLinePoints(list);
		}
		base.spellAroundOwnerCurrentAngle = num2;
		base.Direction = direction;
		return list.ToArray();
		void AppendFirstFireLinePoints(List<Vector3> points)
		{
			points.Add(base.transform.position);
			switch (base.currentSpellMovement)
			{
			case SpellSpecialMovementType.Normal:
				points.Add(base.SIP.finalShootSpatialInfo.Target.Value);
				break;
			case SpellSpecialMovementType.ChaseEnemy:
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
				if (spellFollowTargetPpt == null)
				{
					points.Add(base.SIP.finalShootSpatialInfo.Target.Value);
				}
				else if (!enableFollowTarget)
				{
					points.Add(base.SIP.finalShootSpatialInfo.Target.Value);
				}
				else
				{
					float maxDistanceDelta = spellFollowTargetRotateSpeed * 0.15f;
					Vector3 vector2 = Vector3.MoveTowards(base.SIP.finalShootSpatialInfo.Target.Value, spellFollowTargetPpt.transform.position.IgnoreZ(), maxDistanceDelta);
					points.Add(vector2);
					base.Direction = Tool2D.IgnoreZPoint(vector2 - base.transform.position).normalized;
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
				points.Add(PlayerMgr.Inst.GetMousePoint());
				base.Direction = Tool2D.IgnoreZPoint(points[points.Count - 1] - points[points.Count - 2]).normalized;
				break;
			case SpellSpecialMovementType.Rotation:
			{
				float num3 = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius) * 5f;
				Vector3 vector3 = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				base.spellAroundOwnerCurrentAngle += num3 * 0.5f;
				Vector3 vector4 = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				base.spellAroundOwnerCurrentAngle += num3 * 0.5f;
				Vector3 vector5 = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				points.RemoveAt(points.Count - 1);
				points.Add(Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + vector3, 0f - FallInitialHeight));
				points.Add(Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + vector4, (0f - FallInitialHeight) * 0.5f));
				points.Add(Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + vector5));
				break;
			}
			case SpellSpecialMovementType.ChaseOwner:
			{
				Vector3? spellFollowToOwnerPoint = GetSpellFollowToOwnerPoint();
				if (!spellFollowToOwnerPoint.HasValue)
				{
					points.Add(base.SIP.finalShootSpatialInfo.Target.Value);
				}
				else
				{
					points.Add(spellFollowToOwnerPoint.Value);
					base.Direction = Tool2D.IgnoreZPoint(spellFollowToOwnerPoint.Value - base.transform.position).normalized;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		void AppendReboundingFireLinePoints(List<Vector3> points)
		{
			float fallDamageRange = GetFallDamageRange();
			Vector3 item = points[points.Count - 1] + base.Direction * fallDamageRange * 0.5f;
			item.z = -3f;
			switch (base.currentSpellMovement)
			{
			case SpellSpecialMovementType.Normal:
				points.Add(item);
				points.Add(Tool2D.IgnoreZPoint(points[points.Count - 1] + base.Direction * fallDamageRange));
				break;
			case SpellSpecialMovementType.ChaseEnemy:
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
				if (spellFollowTargetPpt == null)
				{
					points.Add(item);
					points.Add(Tool2D.IgnoreZPoint(points[points.Count - 1] + base.Direction * fallDamageRange));
				}
				else
				{
					Vector3 position = base.transform.position;
					base.transform.position = points[points.Count - 1] + base.Direction * 0.01f;
					base.transform.position = position;
					if (!base.SpellFollowHaveTarget)
					{
						points.Add(item);
						points.Add(Tool2D.IgnoreZPoint(points[points.Count - 1] + base.Direction * fallDamageRange));
					}
					else
					{
						base.Direction = Tool2D.DirMoveTowards(base.Direction, spellFollowTargetPpt.transform.position.IgnoreZ() - points[points.Count - 1], spellFollowTargetRotateSpeed * 8f);
						float num5 = Mathf.Min(Vector3.Distance(spellFollowTargetPpt.transform.position.IgnoreZ(), points[points.Count - 1]), fallDamageRange);
						Vector3 item4 = points[points.Count - 1] + base.Direction * num5;
						points.Add(item);
						points.Add(item4);
					}
				}
				break;
			case SpellSpecialMovementType.ChaseMouse:
			{
				Vector3 v4 = points[points.Count - 1];
				Vector3 v5 = points[points.Count - 1] + base.Direction * fallDamageRange * 2f;
				Vector3 v6 = points[points.Count - 1] + Tool2D.GetDir(base.Direction, 90f) * fallDamageRange * 2f;
				Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint();
				v6.z = item.z;
				v5.z = item.z;
				for (int j = 0; j <= 20; j++)
				{
					float t2 = (float)j / 20f;
					Vector3 item3 = GeneralTool.CubicBezierCurve(v4, v5, v6, mousePoint, t2);
					points.Add(item3);
				}
				base.Direction = Tool2D.IgnoreZPoint(points[points.Count - 1] - points[points.Count - 2]).normalized;
				break;
			}
			case SpellSpecialMovementType.Rotation:
			{
				float num4 = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius) * 5f;
				base.spellAroundOwnerCurrentAngle += num4 * 0.5f;
				Vector3 vector6 = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				base.spellAroundOwnerCurrentAngle += num4 * 0.5f;
				Vector3 vector7 = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				points.Add(Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + vector6, -3f));
				points.Add(Tool2D.IgnoreZPoint(GetAroundTargetBasePoint() + vector7));
				break;
			}
			case SpellSpecialMovementType.ChaseOwner:
			{
				Vector3? spellFollowToOwnerPoint2 = GetSpellFollowToOwnerPoint();
				if (!spellFollowToOwnerPoint2.HasValue)
				{
					points.Add(item);
					points.Add(Tool2D.IgnoreZPoint(points[points.Count - 1] + base.Direction * fallDamageRange));
				}
				else
				{
					Vector3 v = points[points.Count - 1];
					Vector3 v2 = points[points.Count - 1] + base.Direction * fallDamageRange * 2f;
					Vector3 v3 = points[points.Count - 1] + Tool2D.GetDir(base.Direction, 90f) * fallDamageRange * 2f;
					v3.z = item.z;
					v2.z = item.z;
					for (int i = 0; i <= 20; i++)
					{
						float t = (float)i / 20f;
						Vector3 item2 = GeneralTool.CubicBezierCurve(v, v2, v3, spellFollowToOwnerPoint2.Value, t);
						points.Add(item2);
					}
					base.Direction = Tool2D.IgnoreZPoint(points[points.Count - 1] - points[points.Count - 2]).normalized;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public float GetFallDamageRange()
	{
		return (fallFireBaseEffectRadiu + 0.06f * base._angle * 0.2f + base.SIP.fallExplosionRadius) * base.radiusRatio * base.finalDamageRatio * base.finalRadiusRatio * base.SIP.radiuDecreaseRatio;
	}

	private void FallingDamageUpdate(Vector3[] groundPoints)
	{
		attackTimer += Time.deltaTime;
		if (attackTimer < base.spellCfg.DPSDamageInterval || base.DurationTimer < fallFirePreAttackTime)
		{
			return;
		}
		attackTimer -= base.spellCfg.DPSDamageInterval;
		for (int i = 0; i < groundPoints.Length; i++)
		{
			Vector3 startPoint = (currentFallDamageCenter = groundPoints[i]);
			float fallDamageRange = GetFallDamageRange();
			int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(startPoint, fallDamageRange, attackTargetBuffer, attackTargetTags);
			for (int j = 0; j < collidersNonAlloc; j++)
			{
				AOETriggerIn(attackTargetBuffer[j]);
			}
		}
	}

	private void UpdatePowerUpTimer()
	{
		if (!base.isFlyFinish)
		{
			FlamePowerUp();
		}
	}

	private void FlamePowerUp()
	{
		powerUpStackDamageRatio += base.spellCfg.float3 / 100f * Time.deltaTime;
		base.spellCfg.damage = Mathf.Ceil(SpellConfig.dic[base.spellCfg.id].damage * (base.damageRatio + powerUpStackDamageRatio) * base.finalDamageRatio) + base.InitialParameter.finalDamageExtra;
	}

	private void ApplyRecoilToOwnerUnit()
	{
		if (!base.isFlyFinish && !base.spellCfg.isSplitSpell && !shootFromTrigger && !(base.OwnerSpell != null) && !base.indirectShootByPlayer)
		{
			float num = 1f;
			if (IsSameCamp(UnitType.Player))
			{
				num = SpellShootGroupExtend.GetRecoilRatio(base.shooterWand).CurrentMulRatio;
			}
			ownerPpt.TakeKnockback(recoilInDuration * Time.deltaTime * -base.Direction * num);
		}
	}

	private void IncreaseAttackDistanceInDuration()
	{
		float num = 1f + speedToExtraAttackDistanceRatio * base.CurrentSpeed * base.radiusRatio * base.finalRadiusRatio;
		currentAttackDistance = Mathf.Clamp(currentAttackDistance + attackDistanceIncreaseSpeed * num * Time.deltaTime, minAttackDistance * num, base.spellCfg.float1 * num);
	}

	private void UpdateShootPoint()
	{
		if (base.SIP.finalMovementType == SpellSpecialMovementType.Rotation && base.SIP.tags.Contains(SpellTag.Twine))
		{
			base.transform.position = GetAroundTargetBasePoint();
			UpdateRepeatShootPos();
		}
		else
		{
			if (base.isFlyFinish || base.spellCfg.isSplitSpell || shootFromTrigger || (base.OwnerSpell != null && !isOwnerSpellValid()) || base.indirectShootByPlayer)
			{
				return;
			}
			if (isOwnerSpellValid())
			{
				base.transform.position = base.OwnerSpell.transform.position;
				UpdateRepeatShootPos();
			}
			else if (base.OwnerTsf != null)
			{
				base.transform.position = Tool2D.IgnoreZPoint(base.OwnerTsf.transform.position);
				UpdateRepeatShootPos();
			}
			else if (ownerPpt.unitCfg.unitType == UnitType.Player)
			{
				if ((bool)base.shooterWand)
				{
					base.transform.position = base.shooterWand.GetShootPosition();
				}
				else
				{
					base.transform.position = PlayerMgr.Inst.ShootPoint;
				}
				UpdateRepeatShootPos();
			}
			else if (isOwnerpptValid())
			{
				base.transform.position = ownerPpt.transform.position;
				UpdateRepeatShootPos();
			}
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (base.HalfLifeRandomTeleport())
		{
			rigid.linearVelocity = Vector3.zero;
			PlayerUnRegisterKeepCastingBuff();
			return true;
		}
		return false;
	}

	private void UpdateRepeatShootPos()
	{
		base.transform.position += Tool2D.GetDir(base.Direction, -90f) * (((0f - ((float)base.InitialParameter.multiShootCount - 1f)) / 2f + (float)base.InitialParameter.inMultiShootIndex) * base.InitialParameter.multiShootSpace);
	}

	private void UpdateShootDir()
	{
		if (base.isFlyFinish || base.indirectShootByPlayer)
		{
			return;
		}
		bool flag = false;
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
		{
			base.Direction = (PlayerMgr.Inst.GetMousePoint(base.transform.position.z) - base.transform.position).normalized;
			flag = true;
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy && base.SpellFollowHaveTarget)
		{
			if (base.SpellFollowHaveTarget)
			{
				base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime);
			}
			else
			{
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			}
			flag = true;
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
			base.spellAroundOwnerCurrentAngle += num;
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle);
			flag = true;
		}
		else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
		{
			base.Direction = GetSpellFollowToOwnerDirection();
			flag = true;
		}
		if (!flag && !base.spellCfg.isSplitSpell && !shootFromTrigger && (!(base.OwnerSpell != null) || !isOwnerSpellValid()) && !(base.OwnerSpell != null))
		{
			if (ownerPpt.unitCfg.unitType == UnitType.Player)
			{
				base.Direction = ((base.shooterWand != null && base.shooterWand.WandCfg != null && base.shooterWand.WandCfg.specialAbility == WandAbility.FourDirShoot) ? Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir) + Tool2D.GetDegree(base.SIP.originShootDirection)) : Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir)));
			}
			else if (base.OwnerTsf != null)
			{
				base.Direction = base.OwnerTsf.right;
			}
		}
		base.Direction *= IsReverseDirection();
		base.transform.right = base.Direction;
		lastFrameDirection = base.Direction;
	}

	private Vector3 GetAngleCheckCenterPoint()
	{
		if (base.wandShootAngle >= 90f)
		{
			return base.transform.position;
		}
		return Tool2D.IgnoreZPoint(base.transform.position - lastFrameDirection.normalized * (flameBaseWidth / 2f * Mathf.Tan(base.wandShootAngle / 2f * (MathF.PI / 180f))) * IsReverseDirection());
	}

	private float GetRotateMovementMaxAttackDistance()
	{
		return base.spellAroundOwnerRadius + RotateMovementFireWidth / 2f * base.radiusRatio * base.finalRadiusRatio;
	}

	private float GetRotateMovementMinAttackDistance()
	{
		return base.spellAroundOwnerRadius - RotateMovementFireWidth * base.radiusRatio * base.finalRadiusRatio;
	}

	private void DealDamageToAllEnemyInRange()
	{
		attackTimer += Time.deltaTime;
		if (attackTimer < base.spellCfg.DPSDamageInterval)
		{
			return;
		}
		attackTimer -= base.spellCfg.DPSDamageInterval;
		float radius = ((base.currentSpellMovement == SpellSpecialMovementType.Rotation) ? GetRotateMovementMaxAttackDistance() : currentAttackDistance);
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, radius, attackTargetBuffer, attackTargetTags);
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			Collider collider = attackTargetBuffer[i];
			if (!collider.gameObject.activeInHierarchy || !canAttack(collider.transform.position))
			{
				continue;
			}
			if (collider.gameObject.CompareAnyTag("RollBall", "Butterfly"))
			{
				SpellBase componentInParent = collider.GetComponentInParent<SpellBase>();
				if (!(componentInParent is Spell1002RollBall spell1002RollBall))
				{
					if (componentInParent is Spell1003Butterfly spell1003Butterfly && !spell1003Butterfly.IsSameCamp(this))
					{
						spell1003Butterfly.HitEFAndRecycle();
					}
				}
				else if (!spell1002RollBall.IsSameCamp(this))
				{
					spell1002RollBall.TakeDamage(Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval));
				}
			}
			else if (collider.gameObject.CompareAnyTag("Monster", "Player", "Teammate"))
			{
				UnitProperty component = collider.gameObject.GetComponent<UnitProperty>();
				OutputDamage(info: new TakeDamageInfo
				{
					canRebound = false,
					damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval)
				}, targetGO: collider.gameObject);
				CreateHitEffect(rotation: Quaternion.LookRotation(Tool2D.IgnoreZPoint(component.transform.position) - Tool2D.IgnoreZPoint(base.transform.position)) * Quaternion.Euler(0f, -90f, 0f), position: collider.transform.position + new Vector3(0f, 0.3f, 0f));
				component.TakeKnockback(Tool2D.IgnoreZV2ToV1Normal(collider.transform, base.transform) * base.spellCfg.knockback);
			}
			else
			{
				TakeDamageInfo info2 = new TakeDamageInfo
				{
					canRebound = false,
					damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval)
				};
				OutputDamage(collider.gameObject, info2);
			}
		}
		bool canAttack(Vector3 pos)
		{
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				float rotateMovementMinAttackDistance = GetRotateMovementMinAttackDistance();
				rotateMovementMinAttackDistance *= rotateMovementMinAttackDistance;
				return Tool2D.IgnoreZDistanceSqr(pos, base.transform.position) >= rotateMovementMinAttackDistance;
			}
			if (Tool2D.IgnoreZDistanceSqr(pos, base.transform.position) <= flameBaseCircleRadiu * flameBaseCircleRadiu)
			{
				return true;
			}
			if (Tool2D.GetAngleBetweenTwoDirection(base.Direction, Tool2D.IgnoreZPoint(pos) - Tool2D.IgnoreZPoint(GetAngleCheckCenterPoint())) < base.wandShootAngle / 2f)
			{
				return Tool2D.IgnoreZDistanceSqr(pos, base.transform.position) <= maxAttackDistance * maxAttackDistance;
			}
			return false;
		}
	}

	public override void PoolRecycle()
	{
		if (!base.SIP.spellIsFall)
		{
			base.transform.position += lastFrameDirection * maxAttackDistance / 2f;
		}
		else
		{
			base.transform.position = lastFallFireLinePoints[^1];
		}
		base.PoolRecycle();
		PlayerUnRegisterKeepCastingBuff();
		PlaySE("End");
		shootFromTrigger = false;
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		if (triggerOrNull != null)
		{
			shootFromTrigger = true;
		}
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate5 teammate)
		{
			base.OwnerTsf = teammate.shootPosition;
		}
		else if (unit is Teammate52 teammate2)
		{
			base.OwnerTsf = teammate2.shootPosition;
		}
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.canRebound = false;
		takeDamageInfo.damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval);
		return takeDamageInfo;
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		if (base.SIP.spellIsFall)
		{
			Quaternion value = Quaternion.LookRotation(Tool2D.IgnoreZPoint(unit.transform.position) - currentFallDamageCenter) * Quaternion.Euler(0f, -90f, 0f);
			CreateHitEffect(unit.transform.position + new Vector3(0f, 0.3f, 0f), value);
		}
		return base.MakeDamageToUnit(unit);
	}
}
