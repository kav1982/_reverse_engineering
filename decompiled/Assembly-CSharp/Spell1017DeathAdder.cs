using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1017DeathAdder : SpellBase, IOnLaunchFromUnitNotPlayer, IOnLaunchFromSpellEventHandle
{
	[Space(20f)]
	public LineRenderer Line;

	public LineRenderer ShadowLine;

	public MaterialsByColorType Materials;

	public Transform ExplodePos;

	public ShockParam shockParam;

	public float maxShiftDistance;

	private Vector3 shooterPosition;

	private Vector3 targetPosition;

	private Vector3 lerppos1;

	private Vector3 lerppos2;

	private Vector3 lerppos1ShiftDirection;

	private Vector3 lerppos2ShiftDirection;

	public float lerpposShiftSpeed;

	public float lerpposShiftSpeedDown;

	private float lerpSpeed;

	private float dissolveProcess;

	private bool isCircle;

	public Vector3 trailZaxisHeightShift;

	[Min(0f)]
	public float lightningChainFloatingSpeedDownratio;

	private float originFloatingSpeed;

	public Vector3 tempPos;

	public VariableFloat tempRnage;

	private Vector3 BoomPosition;

	private bool SetAll;

	public float realLifeTime;

	public float chaseTargetRadiuRatio;

	public float minTrailWidthRatio;

	public float maxTrailWidthRatio;

	public float traceDuration;

	public float hitEffectHoveLerpSpeed;

	private bool hoverSetTrailOnce;

	public float hoverRecheckTargetInterval;

	private float hoverRecheckTargetTimer;

	private List<Collider> AttackedColliders = new List<Collider>();

	public float rebounceStartTime;

	public float rebounceBaseMoveRatio;

	public float rebounceSpeedConvertToMoveSpeedRatio;

	private bool isRebounceSpell;

	private Vector3 chaseTargetPoint = Vector3.zero;

	private bool overrideTargetPositionBySplit;

	private static readonly int IDSpeed = Shader.PropertyToID("_Speed");

	private static readonly int IDDissolveProcess = Shader.PropertyToID("_DissolveProcess");

	public event Func<SlotData, SlotData> UpgradeEvent;

	public override void InitializeCallback()
	{
		SetAll = false;
		BoomPosition = Vector3.zero;
		targetPosition = Vector3.zero;
		this.UpgradeEvent = null;
		hoverSetTrailOnce = false;
		hoverRecheckTargetTimer = 0f;
		AttackedColliders.Clear();
		isRebounceSpell = false;
		chaseTargetPoint = Vector3.zero;
		overrideTargetPositionBySplit = false;
		base.enableAroundPlayer = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.radius = GetFallingGroundDamageRadius();
		}
		isCircle = false;
		lerpSpeed = lerpposShiftSpeed;
		Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		lerppos1ShiftDirection = new Vector3(insideUnitSphere.x, insideUnitSphere.y, 0f - Mathf.Abs(insideUnitSphere.z));
		insideUnitSphere = UnityEngine.Random.insideUnitSphere;
		lerppos2ShiftDirection = new Vector3(insideUnitSphere.x, insideUnitSphere.y, 0f - Mathf.Abs(insideUnitSphere.z));
		Line.sharedMaterial = Materials.Get(base.ColorType);
		Line.enabled = false;
		ShadowLine.enabled = false;
		if (base.SIP.ShootCause is ShootCause.BySplit && base.spellCfg.isSplitSpell && ownerPpt != PlayerMgr.Inst.PlayerPpt)
		{
			overrideTargetPositionBySplit = true;
		}
	}

	public override void OnFirstFrame()
	{
		Line.enabled = true;
		ShadowLine.enabled = true;
		base.transform.localScale = Vector3.one;
		tsf_Layer.localScale = Vector3.one * base.spellCfg.radius;
		dissolveProcess = 0f;
		originFloatingSpeed = 3f;
		Line.material.SetFloat(IDSpeed, 3f);
		Line.startWidth = Mathf.Clamp(Mathf.Pow(base.damageRatio * base.finalDamageRatio, 0.3333f), minTrailWidthRatio, maxTrailWidthRatio);
		base.spellCfg.duration = SpellConfig.dic[base.spellCfg.id].duration;
	}

	public override void Update()
	{
		base.Update();
		if (!SetAll)
		{
			SetAll = true;
			shooterPosition = base.transform.position;
			ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
			if (finalShootSpatialInfo == null || !finalShootSpatialInfo.Target.HasValue)
			{
				Debug.LogWarning("法术缺少目标位置");
				return;
			}
			targetPosition = base.SIP.finalShootSpatialInfo.Target.Value;
			if (overrideTargetPositionBySplit)
			{
				targetPosition = base.transform.position;
			}
			float num = Mathf.Min(maxShiftDistance, Vector3.Distance(shooterPosition, targetPosition));
			Vector3 vector = targetPosition - shooterPosition;
			lerppos1 = shooterPosition + vector * UnityEngine.Random.Range(0.2f, 0.8f) + tempPos + UnityEngine.Random.insideUnitSphere.normalized * tempRnage.RandomResult() * num;
			lerppos2 = shooterPosition + vector * UnityEngine.Random.Range(0.2f, 0.8f) + tempPos + UnityEngine.Random.insideUnitSphere.normalized * tempRnage.RandomResult() * num;
			List<Collider> list = null;
			if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				float num2 = UnityEngine.Random.Range(0f, 360f);
				bool flag = false;
				list = ((!IsSameCamp(UnitType.Player)) ? GeneralTool.GetCollidersByTag(GetAroundTargetBasePoint(), base.spellCfg.radius / 2f + base.spellAroundOwnerRadius, "Player", "Teammate", "RollBall", "Butterfly") : GeneralTool.GetCollidersByTag(GetAroundTargetBasePoint(), base.spellCfg.radius / 2f + base.spellAroundOwnerRadius, "Monster", "RollBall", "Butterfly"));
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						float num3 = Vector3.Distance(list[i].gameObject.transform.position, GetAroundTargetBasePoint());
						if (num3 >= base.spellAroundOwnerRadius - base.spellCfg.radius && num3 <= base.spellAroundOwnerRadius + base.spellCfg.radius)
						{
							if (!list[i].CompareTag("RollBall") && !list[i].CompareTag("Butterfly"))
							{
								BoomPosition = GetAroundTargetBasePoint() + (list[i].gameObject.transform.position - GetAroundTargetBasePoint()).normalized * base.spellAroundOwnerRadius;
								flag = true;
								break;
							}
							if (!list[i].GetComponentInParent<SpellBase>().IsSameCamp(this))
							{
								BoomPosition = GetAroundTargetBasePoint() + (list[i].gameObject.transform.position - GetAroundTargetBasePoint()).normalized * base.spellAroundOwnerRadius;
								flag = true;
								break;
							}
						}
					}
				}
				if (!flag)
				{
					BoomPosition = GetAroundTargetBasePoint() + Tool2D.GetDir(num2) * base.spellAroundOwnerRadius;
				}
				else
				{
					Vector3 vector2 = (BoomPosition - base.transform.position).IgnoreZ();
					num2 = Vector2.SignedAngle(Vector2.up, new Vector2(vector2.x, vector2.y));
				}
				if (base.SIP.spellIsFall && base.SIP.tags.Contains(SpellTag.Twine))
				{
					BoomPosition.z = 0f;
				}
				targetPosition = BoomPosition;
				SetAroundCirclePoints(num2, GetAroundTargetBasePoint(), base.spellAroundOwnerRadius);
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
			{
				UnitProperty nearestTargetablePpt = GetNearestTargetablePpt(targetPosition, checkWall: true);
				Vector3 position = targetPosition;
				float num4 = 0f;
				if (nearestTargetablePpt != null)
				{
					position = nearestTargetablePpt.gameObject.transform.position;
					num4 = Vector3.Distance(position, targetPosition);
				}
				if (num4 > chaseTargetRadiuRatio)
				{
					BoomPosition = targetPosition + (position - targetPosition).normalized * chaseTargetRadiuRatio;
					targetPosition = BoomPosition;
				}
				else
				{
					BoomPosition = position;
					targetPosition = position;
				}
				BoomPosition = targetPosition;
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse)
			{
				targetPosition = PlayerMgr.Inst.GetMousePoint();
				BoomPosition = targetPosition;
			}
			else if (base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
			{
				targetPosition = ownerPpt.transform.position;
				BoomPosition = targetPosition;
			}
			else
			{
				BoomPosition = targetPosition;
			}
			if (!SpellEffectBase.FullTransparency)
			{
				GetEffect("GroundMark", BoomPosition, Quaternion.identity, traceDuration).GetComponent<Spell1017Trace>().SetAll(base.ColorType, base.spellCfg.radius / 2f);
			}
			CamController.Inst.SetShock(shockParam);
			ExplodePos.transform.position = BoomPosition;
			EffectBase.ManualCreateEffect("Explode");
			PlaySE("All");
			base.DurationTimer = base.spellCfg.duration - realLifeTime;
			list = GeneralTool.GetCollidersByTag(BoomPosition, base.spellCfg.radius, "Monster", "Destructible", "Player", "Teammate", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness");
			list.Reverse();
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].CompareTag("RollBall") || list[j].CompareTag("Butterfly"))
				{
					SpellBase componentInParent = list[j].GetComponentInParent<SpellBase>();
					if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
					{
						((Spell1002RollBall)componentInParent).TakeDamage(base.spellCfg.damage);
					}
					else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
					{
						((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
					}
					else
					{
						MonoBehaviour.print(componentInParent.spellCfg.abilityType);
					}
				}
				else if (list[j].CompareTag("Brittleness") || list[j].CompareTag("Destructible") || list[j].CompareTag("SolidObj"))
				{
					OutputDamage(list[j].gameObject, new TakeDamageInfo
					{
						canRebound = false
					});
				}
				else if (!list[j].CompareTag("Spell"))
				{
					TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
					takeDamageInfo.canRebound = false;
					takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(list[j].transform.position, BoomPosition) * base.spellCfg.knockback;
					takeDamageInfo.isUndifferDamage = true;
					if (list[j] != null && list[j].gameObject.activeSelf)
					{
						TakeDamageInfo takeDamageInfo2 = OutputDamage(list[j].gameObject, takeDamageInfo);
						if (!refractedTargets.Contains(list[j].gameObject))
						{
							refractedTargets.Add(list[j].gameObject);
						}
						if (takeDamageInfo2.isCriticalDamage)
						{
							base.virtualRealPosition = BoomPosition;
							CheckIfPullCrystalIsValidToAttack(takeDamageInfo2, list[j].GetComponent<UnitProperty>());
						}
					}
					if (takeDamageInfo.isTargetDead)
					{
						if (base.ShootData?.Spell != null && !IsSameCamp(takeDamageInfo.beHitPpt.unitCfg.unitType) && takeDamageInfo.beHitPpt.unitCfg.triggerDeadEvent)
						{
							KillSomeOne(takeDamageInfo.beHitPpt.transform.position);
						}
					}
					else
					{
						EffectBase.CreateSpriteEffect("Hit", Tool2D.IgnoreZPoint(list[j].transform.position, 0.3f));
					}
				}
				AttackedColliders.Add(list[j]);
			}
			if ((!isRebounceSpell || base.currentSpellMovement != SpellSpecialMovementType.Rotation) && (!base.spellCfg.isSplitSpell || base.rebounceTime > 0 || base.currentSpellMovement != SpellSpecialMovementType.Rotation) && !base.SIP.spellIsFall)
			{
				EffectBase.ManualCreateEffect("Charge");
			}
			base.transform.position = targetPosition;
			if (base.currentSpellMovement != SpellSpecialMovementType.Rotation)
			{
				UnitProperty unitProperty = TryRefract();
				if ((bool)unitProperty)
				{
					Spell1017DeathAdder spell1017DeathAdder = CreateSubDeathAdder();
					spell1017DeathAdder.SIP.finalShootSpatialInfo.Target = unitProperty.transform.position;
					spell1017DeathAdder.refractedTargets.AddRange(refractedTargets);
					spell1017DeathAdder.remainRefractCount = base.remainRefractCount;
				}
			}
			base.isFlyFinish = true;
		}
		if (base.SpellHoverTimer < base.SpellHoverTime && base.rebounceTime <= 0)
		{
			base.SpellHoverTimer += Time.deltaTime;
			base.DurationTimer -= Time.deltaTime;
			hoverRecheckTargetTimer += Time.deltaTime;
			if (!hoverSetTrailOnce)
			{
				hoverSetTrailOnce = true;
				Line.material.SetFloat(IDSpeed, 0f);
				ShadowLine.material.SetFloat(IDSpeed, 0f);
				originFloatingSpeed = Mathf.Lerp(originFloatingSpeed, 0f, lightningChainFloatingSpeedDownratio * Time.deltaTime);
				Line.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess);
				ShadowLine.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess);
				dissolveProcess += Time.deltaTime * 5f;
				if (!isCircle)
				{
					float num5 = (1f + base._angle / 100f) * lerpSpeed;
					lerppos1 += lerppos1ShiftDirection * num5;
					lerppos2 += lerppos2ShiftDirection * num5;
					lerpSpeed = Mathf.Lerp(lerpSpeed, 0f, Time.deltaTime * lerpposShiftSpeedDown);
					SetCurvePointsPosition();
				}
			}
			else
			{
				float value = originFloatingSpeed;
				float num6 = Time.deltaTime * 5f;
				if (base.SpellHoverTime > 0f)
				{
					((Spell1017Effect)EffectBase).PauseEffect();
					num6 *= 1f / base.SpellHoverTime / 2f;
					originFloatingSpeed = Mathf.Lerp(originFloatingSpeed, 0f, hitEffectHoveLerpSpeed * 3f * Time.deltaTime);
				}
				else
				{
					originFloatingSpeed = Mathf.Lerp(originFloatingSpeed, 0f, lightningChainFloatingSpeedDownratio * Time.deltaTime);
				}
				Line.material.SetFloat(IDSpeed, value);
				ShadowLine.material.SetFloat(IDSpeed, value);
				Line.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess + num6);
				ShadowLine.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess + num6);
				dissolveProcess += num6;
			}
			if (hoverRecheckTargetTimer >= hoverRecheckTargetInterval)
			{
				hoverRecheckTargetTimer -= hoverRecheckTargetInterval;
				List<Collider> collidersByTag = GeneralTool.GetCollidersByTag(BoomPosition, base.spellCfg.radius, "Monster", "Destructible", "Player", "Teammate", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness");
				for (int k = 0; k < collidersByTag.Count; k++)
				{
					if (!AttackedColliders.Contains(collidersByTag[k]))
					{
						if (collidersByTag[k].CompareTag("RollBall") || collidersByTag[k].CompareTag("Butterfly"))
						{
							SpellBase componentInParent2 = collidersByTag[k].GetComponentInParent<SpellBase>();
							if (componentInParent2.spellCfg.abilityType == SpellAbilityType.Rollball)
							{
								((Spell1002RollBall)componentInParent2).TakeDamage(base.spellCfg.damage);
							}
							else if (componentInParent2.spellCfg.abilityType == SpellAbilityType.Butterfly)
							{
								((Spell1003Butterfly)componentInParent2).HitEFAndRecycle();
							}
							else
							{
								MonoBehaviour.print(componentInParent2.spellCfg.abilityType);
							}
						}
						else if (collidersByTag[k].CompareTag("Brittleness") || collidersByTag[k].CompareTag("Destructible") || collidersByTag[k].CompareTag("SolidObj"))
						{
							OutputDamage(collidersByTag[k].gameObject, new TakeDamageInfo
							{
								canRebound = false
							});
						}
						else if (!collidersByTag[k].CompareTag("Spell"))
						{
							TakeDamageInfo takeDamageInfo3 = new TakeDamageInfo();
							takeDamageInfo3.canRebound = false;
							takeDamageInfo3.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collidersByTag[k].transform.position, BoomPosition) * base.spellCfg.knockback;
							takeDamageInfo3.isUndifferDamage = true;
							OutputDamage(collidersByTag[k].gameObject, takeDamageInfo3);
							if (takeDamageInfo3.isTargetDead)
							{
								if (base.ShootData?.Spell != null && !IsSameCamp(takeDamageInfo3.beHitPpt.unitCfg.unitType) && takeDamageInfo3.beHitPpt.unitCfg.triggerDeadEvent)
								{
									KillSomeOne(takeDamageInfo3.beHitPpt.transform.position);
								}
							}
							else
							{
								EffectBase.CreateSpriteEffect("Hit", Tool2D.IgnoreZPoint(collidersByTag[k].transform.position, 0.3f));
							}
						}
					}
					AttackedColliders.Add(collidersByTag[k]);
				}
			}
		}
		else
		{
			float value2 = originFloatingSpeed;
			if (base.SpellHoverTime > 0f)
			{
				((Spell1017Effect)EffectBase).ResumeEffect();
			}
			Line.material.SetFloat(IDSpeed, value2);
			ShadowLine.material.SetFloat(IDSpeed, value2);
			originFloatingSpeed = Mathf.Lerp(originFloatingSpeed, 0f, lightningChainFloatingSpeedDownratio * Time.deltaTime);
			Line.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess);
			ShadowLine.material.SetFloat(IDDissolveProcess, -1.7f + dissolveProcess);
			dissolveProcess += Time.deltaTime * 5f;
			if (!isCircle && Time.timeScale != 0f)
			{
				float num7 = (1f + base._angle / 100f) * lerpSpeed;
				lerppos1 += lerppos1ShiftDirection * num7;
				lerppos2 += lerppos2ShiftDirection * num7;
				lerpSpeed = Mathf.Lerp(lerpSpeed, 0f, Time.deltaTime * lerpposShiftSpeedDown);
				SetCurvePointsPosition();
			}
		}
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
				return;
			}
			PoolRecycle();
		}
		if (base.rebounceTime > 0 && base.DurationTimer >= rebounceStartTime)
		{
			Spell1017DeathAdder spell1017DeathAdder2 = CreateSubDeathAdder();
			if (base.currentSpellMovement != SpellSpecialMovementType.Rotation && spellFollowMouseLerp == 0f && spellFollowTargetRotateSpeed == 0f)
			{
				Vector3 value3 = base.transform.position + base.Direction * (rebounceBaseMoveRatio + base.spellCfg.speed * rebounceSpeedConvertToMoveSpeedRatio);
				spell1017DeathAdder2.SIP.finalShootSpatialInfo.Target = value3;
			}
			else if (spellFollowTargetRotateSpeed != 0f)
			{
				spell1017DeathAdder2.SIP.finalShootSpatialInfo.Target = chaseTargetPoint;
			}
			else if (spellFollowMouseLerp != 0f)
			{
				Vector3 vector3 = Vector3.Lerp(BoomPosition + base.Direction.normalized * (rebounceBaseMoveRatio + base.spellCfg.speed), PlayerMgr.Inst.GetMousePoint(), (rebounceBaseMoveRatio + base.spellCfg.speed) * Time.deltaTime);
				spell1017DeathAdder2.SIP.finalShootSpatialInfo.Target = vector3;
				spell1017DeathAdder2.Direction = Vector3.Lerp(base.Direction, PlayerMgr.Inst.GetMousePoint() - vector3, spellFollowMouseLerp * (base.spellCfg.speed + 1f));
			}
		}
		if (base.rebounceTime > 0 && spellFollowTargetRotateSpeed != 0f)
		{
			if (chaseTargetPoint == Vector3.zero)
			{
				chaseTargetPoint = BoomPosition;
			}
			if (!base.SpellFollowHaveTarget)
			{
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			}
			if (base.SpellFollowHaveTarget)
			{
				base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), (rebounceBaseMoveRatio + base.spellCfg.speed) * 15f * spellFollowTargetRotateSpeed * Time.deltaTime);
			}
			chaseTargetPoint += base.Direction * (rebounceBaseMoveRatio + base.spellCfg.speed) * 5f * Time.deltaTime;
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		return false;
	}

	private Spell1017DeathAdder CreateSubDeathAdder()
	{
		SpellConfig spellConfig = SpellConfig.dic[base.spellCfg.id];
		float degree = UnityEngine.Random.Range(0f, 360f);
		SpellBase component = ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellConfig.prefab, base.transform.position).GetComponent<SpellBase>();
		SpellInitialParameter spellInitialParameter = base.InitialParameter.Copy();
		spellInitialParameter.fallExplosionRadius = 0f;
		component.Initialize(spellInitialParameter);
		if (component.enableAroundPlayer)
		{
			component.transform.position += Tool2D.GetDir(degree) * component.spellAroundOwnerRadius;
		}
		if (base.OwnerSpell == null)
		{
			component.OwnerTsf = ownerPpt.transform;
		}
		else if (base.OwnerPoint != Vector3.zero)
		{
			component.OwnerTsf = base.OwnerSpell.transform;
		}
		component.OwnerPoint = base.OwnerPoint;
		component.spellAroundOwnerCurrentAngle = degree;
		component.undifferDamageRatio = base.undifferDamageRatio;
		component.indirectShootByPlayer = true;
		component.shouldCameraShock = false;
		component.Direction = base.Direction;
		component.rebounceTime = --base.rebounceTime;
		base.rebounceTime = 0;
		base.spellSplitCount = 0;
		component.spellCfg.isSplitSpell = base.spellCfg.isSplitSpell;
		Spell1017DeathAdder component2 = component.GetComponent<Spell1017DeathAdder>();
		component2.ShootData = base.ShootData;
		component2.isRebounceSpell = true;
		return component2;
	}

	public override void PoolRecycle()
	{
		base.lightningChainDamage = 0f;
		base.PoolRecycle();
	}

	public void SetAroundCirclePoints(float initialDegree, Vector3 centerPoint, float radiu)
	{
		isCircle = true;
		Vector3 vector = new Vector3(0f, 0f, base.transform.position.z);
		centerPoint = centerPoint.IgnoreZ();
		for (int i = 0; i < Line.positionCount; i++)
		{
			Vector3 vector2 = centerPoint + Tool2D.GetDir(initialDegree + 360f / (float)(Line.positionCount - 2) * (float)i) * radiu;
			Vector3 rootPoint = vector2 + vector;
			if (base.SIP.spellIsFall)
			{
				rootPoint.z *= (float)i / (float)Line.positionCount;
			}
			Line.SetPosition(i, Tool2D.GetLayerPoint(rootPoint));
			ShadowLine.SetPosition(i, Tool2D.IgnoreZPoint(vector2, 1.05f));
		}
	}

	private void SetCurvePointsPosition()
	{
		for (int i = 0; i < Line.positionCount; i++)
		{
			Vector3 vector = GeneralTool.CubicBezierCurve(shooterPosition, lerppos1, lerppos2, targetPosition, (float)i / ((float)Line.positionCount - 1f));
			Line.SetPosition(i, Tool2D.GetLayerPoint(vector) + trailZaxisHeightShift);
			ShadowLine.SetPosition(i, Tool2D.IgnoreZPoint(vector, 1.05f));
		}
	}

	private void Display_ShowUpgradeText()
	{
		if (base.ShootData != null)
		{
			if (base.ShootData.Spell.id == 10173)
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002012.GetText(), UITextFloatType.Normal, base.transform.position);
			}
			else
			{
				ObjPoolMgr.Inst.GetUIGO("Prefabs/UI/UITextFloat").GetComponent<UITextFloat>().Initialize(1002011.GetText() + (base.ShootData.Spell.id - 10170), UITextFloatType.Normal, base.transform.position);
			}
		}
	}

	private void SpellUpgrade()
	{
		if (base.ShootData == null)
		{
			return;
		}
		base.ShootData.Spell.specialInt -= base.spellCfg.int1;
		base.ShootData.Spell.id++;
		this.UpgradeEvent?.Invoke(base.ShootData.Spell);
		Display_ShowUpgradeText();
		DataMgr.selectedWorldData.GalleryUnlock(GalleryCategory.Spell, base.ShootData.Spell.id);
		foreach (Wand wand in PlayerMgr.Inst.Wands)
		{
			if (wand.WandCfg.GetValidSlotsData(normal: true, post: true).Contains(base.ShootData.Spell))
			{
				UIPlayerDataMgr.Inst.WandUpdate(PlayerMgr.Inst.Wands.IndexOf(wand));
				return;
			}
		}
		if (PlayerMgr.Inst.BaData.bagSpellDatas.Contains(base.ShootData.Spell))
		{
			UIPlayerDataMgr.Inst.UpdateBag();
		}
	}

	public void KillSomeOne(Vector3 deadPoint)
	{
		if (base.ShootData?.Spell == null)
		{
			return;
		}
		base.ShootData.Spell.specialInt++;
		SpellEffectBase effectBase = EffectBase;
		Vector3? position = Tool2D.IgnoreZPoint(deadPoint, base.transform.position.z);
		effectBase.ManualCreateEffect("DeadHit", null, position);
		PlaySE("DeadHit");
		if (base.ShootData.Spell.specialInt >= base.spellCfg.int1)
		{
			int id = base.ShootData.Spell.id;
			if (id > 10170 && id < 10173)
			{
				SpellUpgrade();
			}
		}
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		base.OwnerTsf = base.transform;
		base.OwnerPoint = base.transform.position;
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		base.OwnerTsf = base.transform;
		base.OwnerPoint = base.transform.position;
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override SpellInitialParameter CreateSplitSpellInitialParameter(SpellConfig config, Vector3 shootDirection)
	{
		SpellInitialParameter spellInitialParameter = base.CreateSplitSpellInitialParameter(config, shootDirection);
		spellInitialParameter.fallExplosionRadius = 0f;
		return spellInitialParameter;
	}
}
