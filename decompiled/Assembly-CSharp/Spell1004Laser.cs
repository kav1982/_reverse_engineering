using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class Spell1004Laser : SpellBase
{
	[Space(50f)]
	public EffectTransparencyController TransparencyController;

	public LayerMask playerAttackLayer;

	public LayerMask monsterAttackLayer;

	public LayerMask playerAttackLayerNoWall;

	public LayerMask monsterAttackLayerNoWall;

	public int circleCount;

	public float followTargetTimeInterval;

	[ColorUsage(true, true)]
	public Color HarmoniousMonsterBubbleColor;

	public ColorByColorType BubbleColors;

	public Material HarmoniousMonsterMaterial;

	public MaterialsByColorType Materials;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public float duration;

	public float maxLength;

	private float laserOrignalWidth;

	private float shadowOrignalWidth;

	private float durationTimer;

	private bool spawndLaser;

	public float mouseChaseRatio;

	private List<UnitProperty> HitedEnemy;

	private bool removeOtherMovement;

	public AnimationCurve laserWidth;

	public AnimationCurve laserNodeSize;

	private SpriteRenderer[] laserNodes;

	private bool rebounded;

	private float _remainDistance;

	private bool shootDone;

	private readonly List<Vector3> _laserPoints = new List<Vector3>();

	private readonly List<Vector3> _nodePoints = new List<Vector3>();

	public float hoverRecheckTargetInterval;

	private float hoverRecheckTargetTimer;

	public override void InitializeCallback()
	{
		spawndLaser = false;
		rebounded = false;
		enableFollowMouse = false;
		enableFollowTarget = false;
		enableNormalTransform = false;
		base.enableAroundPlayer = false;
		laserNodes = null;
		HitedEnemy = new List<UnitProperty>();
		shootDone = false;
		removeOtherMovement = false;
		_laserPoints.Clear();
		_nodePoints.Clear();
		hoverRecheckTargetTimer = 0f;
		float item = base.SIP.RefractionInfo?.radius ?? 0f;
		if (base.spellCfg.level > 1)
		{
			item = 9999f;
		}
		int item2 = base.SIP.RefractionInfo?.count ?? 0;
		base.SIP.RefractionInfo = (item2, item);
		base.remainRefractCount += base.spellCfg.int1;
		if (base.spellCfg.level > 1)
		{
			base.remainRefractCount += base.penetrateTime;
			base.penetrateTime = 0;
		}
		base.SpellHoverTimer = 0f;
		lr_Laser.enabled = false;
		Material sharedMaterial = Materials.Get(base.ColorType);
		if (base.ColorType == SpellColorType.Monster && GameMgr.IsHarmony_Static)
		{
			sharedMaterial = HarmoniousMonsterMaterial;
		}
		lr_Laser.sharedMaterial = sharedMaterial;
		lr_Shadow.enabled = false;
		_remainDistance = maxLength + base.spellCfg.speed;
		TransparencyController.ForceNoTransparent = (bool)ownerPpt && !ownerPpt.gameObject.CompareAnyTag("Player", "Teammate");
		TransparencyController.UpdateTransparent();
	}

	protected override void TryActionOnStartTriggerOnInitialize()
	{
	}

	public override void Update()
	{
		if (!spawndLaser)
		{
			spawndLaser = true;
			if (base.SIP.spellIsFall)
			{
				SpawnFallLaser();
			}
			else
			{
				SpawnLaser();
			}
			SpawnLaserEffect();
			base.TryActionOnStartTriggerOnInitialize();
		}
		durationTimer += Time.deltaTime;
		if (durationTimer >= duration)
		{
			durationTimer = 0f;
			base.isFlyFinish = true;
			PoolRecycle();
			return;
		}
		if (durationTimer >= duration / 2f && base.SpellHoverTimer < base.SpellHoverTime && !shootDone)
		{
			base.SpellHoverTimer += Time.deltaTime;
			durationTimer -= Time.deltaTime;
			hoverRecheckTargetTimer += Time.deltaTime;
			if (!(hoverRecheckTargetTimer >= hoverRecheckTargetInterval) || base.SIP.spellIsFall)
			{
				return;
			}
			hoverRecheckTargetTimer -= hoverRecheckTargetInterval;
			LayerMask layerMask = ((!isThroughWall) ? (IsSameCamp(UnitType.Player) ? playerAttackLayer : monsterAttackLayer) : (IsSameCamp(UnitType.Player) ? playerAttackLayerNoWall : monsterAttackLayerNoWall));
			new List<Collider>();
			for (int i = 0; i < _laserPoints.Count - 1; i++)
			{
				Vector3 normalized = Tool2D.IgnoreZPoint(_laserPoints[i + 1] - _laserPoints[i]).normalized;
				float maxDistance = Mathf.Pow(Tool2D.IgnoreZDistanceSqr(_laserPoints[i + 1], _laserPoints[i]), 2f);
				if (!Physics.Raycast(_laserPoints[i], normalized, out var hitInfo, maxDistance, layerMask))
				{
					continue;
				}
				if (hitInfo.transform.tag == "SpellRebound")
				{
					SpellBase component = hitInfo.transform.GetComponent<SpellBase>();
					if (component != null)
					{
						if (IsSameCamp(component) || (component.spellCfg.abilityType != SpellAbilityType.Butterfly && component.spellCfg.abilityType != SpellAbilityType.Rollball))
						{
							continue;
						}
						CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
						if (component is Spell1003Butterfly spell1003Butterfly && !IsSameCamp(spell1003Butterfly))
						{
							spell1003Butterfly.Break();
							continue;
						}
						((Spell1002RollBall)component).TakeDamage(base.spellCfg.damage);
						if (base.penetrateTime <= 0)
						{
							base.SpellHoverTimer = base.SpellHoverTime;
							break;
						}
						base.penetrateTime--;
					}
					else
					{
						Debug.LogError(hitInfo.transform.name);
					}
				}
				else if (hitInfo.transform.tag == "Player" || hitInfo.transform.tag == "Teammate" || hitInfo.transform.tag == "Monster")
				{
					CheckIfHoverValidToDamageTarget(hitInfo, _laserPoints, _nodePoints);
					PlaySE("Shoot");
					if (base.penetrateTime > 0)
					{
						base.penetrateTime--;
					}
					if (base.penetrateTime <= 0)
					{
						base.SpellHoverTimer = base.SpellHoverTime;
						break;
					}
				}
			}
			return;
		}
		float num = laserWidth.Evaluate(Mathf.Clamp(durationTimer / duration, 0.2f, 0.99f));
		lr_Laser.widthMultiplier = laserOrignalWidth * base.transform.localScale.x * num;
		lr_Shadow.widthMultiplier = shadowOrignalWidth * base.transform.localScale.x * num;
		if (laserNodes != null)
		{
			num = laserNodeSize.Evaluate(Mathf.Min(0.99f, durationTimer / duration));
			for (int j = 0; j < laserNodes.Length; j++)
			{
				laserNodes[j].material.SetFloat("_Size", num);
			}
		}
	}

	public override void SingleInitialCallback()
	{
		laserOrignalWidth = lr_Laser.widthMultiplier;
		shadowOrignalWidth = lr_Shadow.widthMultiplier;
	}

	private void SpawnLaser()
	{
		LayerMask layerMask = ((!isThroughWall) ? (IsSameCamp(UnitType.Player) ? playerAttackLayer : monsterAttackLayer) : (IsSameCamp(UnitType.Player) ? playerAttackLayerNoWall : monsterAttackLayerNoWall));
		_laserPoints.Clear();
		_nodePoints.Clear();
		base.transform.position = base.transform.position.IgnoreZ() + new Vector3(0f, 0f, -0.3f);
		_laserPoints.Add(base.transform.position);
		_nodePoints.Add(base.transform.position);
		bool flag = false;
		int num = 0;
		durationTimer = 0f;
		Collider[] array = Physics.OverlapSphere(base.transform.position, base.ColliderRadius, layerMask);
		if (array.Length != 0)
		{
			Collider[] array2 = array;
			foreach (Collider collider in array2)
			{
				if (!collider.CompareTag("Untagged") && !collider.CompareTag("Spell") && !collider.CompareTag("Butterfly") && !collider.CompareTag("RollBall") && !collider.CompareTag("SpellRebound"))
				{
					flag = true;
					CreateHitEffect(base.transform.position, Quaternion.LookRotation(-base.Direction));
					if (!HitedEnemy.Contains(collider.GetComponent<UnitProperty>()))
					{
						OutputDamage(array[0].transform.gameObject);
					}
					if (base.penetrateTime <= 0)
					{
						shootDone = true;
					}
					break;
				}
			}
		}
		if (base.penetrateTime <= 0 && flag)
		{
			return;
		}
		RaycastHit hitInfo;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			float num2 = MathF.PI * 2f * base.spellAroundOwnerRadius / (float)circleCount;
			base.transform.position = GetAroundTargetBasePoint() + base.Direction * base.spellAroundOwnerRadius + new Vector3(0f, 0f, base.transform.position.z);
			_laserPoints[0] = base.transform.position;
			_nodePoints[0] = base.transform.position;
			base.Direction = Tool2D.IgnoreZPoint(Tool2D.GetDir(base.Direction, 90f));
			for (int j = 0; j < circleCount; j++)
			{
				_remainDistance -= 0.4f;
				if (!removeOtherMovement)
				{
					base.Direction = Tool2D.IgnoreZPoint(Tool2D.GetDir(base.Direction, 360f / (float)circleCount));
				}
				_laserPoints.Add(base.transform.position + base.Direction * num2);
				Ray ray = new Ray(base.transform.position, base.Direction);
				if (Physics.Raycast(ray, out hitInfo, num2, layerMask))
				{
					if (hitInfo.transform.tag != "Untagged" && hitInfo.transform.tag != "SpellRebound" && hitInfo.transform.tag != "Wall" && hitInfo.transform.tag != "SolidObj" && hitInfo.transform.tag != "InteractiveObj")
					{
						_nodePoints.Add(hitInfo.point);
						CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
					}
					if (hitInfo.transform.tag == "Untagged" || hitInfo.transform.tag == "Wall" || hitInfo.transform.tag == "SolidObj" || hitInfo.transform.tag == "InteractiveObj")
					{
						base.transform.position = hitInfo.point;
						if (_remainDistance <= 0f)
						{
							break;
						}
					}
					else if (hitInfo.transform.tag == "SpellRebound")
					{
						SpellBase component = hitInfo.transform.GetComponent<SpellBase>();
						if (component != null)
						{
							if (!IsSameCamp(component) && (component.spellCfg.abilityType == SpellAbilityType.Butterfly || component.spellCfg.abilityType == SpellAbilityType.Rollball))
							{
								_nodePoints.Add(hitInfo.point);
								CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
								if (component is Spell1003Butterfly spell1003Butterfly && !IsSameCamp(spell1003Butterfly))
								{
									spell1003Butterfly.Break();
								}
								else
								{
									((Spell1002RollBall)component).TakeDamage(base.spellCfg.damage);
									if (base.penetrateTime <= 0)
									{
										shootDone = true;
										break;
									}
									base.penetrateTime--;
								}
							}
						}
						else
						{
							Debug.LogError(hitInfo.transform.name);
						}
					}
					else if (hitInfo.transform.tag == "Brittleness")
					{
						hitInfo.transform.GetComponent<UnitProperty>().AnnouncedDeath();
						_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 1f;
						base.transform.position = hitInfo.point + base.Direction * 0.1f;
						if (_remainDistance <= 0f)
						{
							shootDone = true;
							break;
						}
					}
					else
					{
						if (_laserPoints.Count > 0)
						{
							_nodePoints.RemoveAt(_nodePoints.Count - 1);
							_nodePoints.Add(hitInfo.point);
						}
						CheckIfValidToDamageTarget(hitInfo);
						if (base.penetrateTime <= 0)
						{
							_laserPoints.Add(hitInfo.point);
							shootDone = true;
							break;
						}
						base.penetrateTime--;
						if (removeOtherMovement)
						{
							normalMovement(_laserPoints, _nodePoints, layerMask, _remainDistance);
							break;
						}
					}
				}
				base.transform.position = ray.origin + base.Direction * num2;
			}
			return;
		}
		while (true)
		{
			num++;
			if (num >= 500)
			{
				Debug.LogError("死循环 请检查");
				break;
			}
			float num3 = 1f;
			switch (base.currentSpellMovement)
			{
			case SpellSpecialMovementType.ChaseMouse:
			{
				Vector3 shootWorldPoint = PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint;
				List<Vector3> laserPoints = _laserPoints;
				Vector3 b = Tool2D.IgnoreZV2ToV1Normal(shootWorldPoint, laserPoints[laserPoints.Count - 1]);
				base.Direction = Tool2D.IgnoreZPoint(Vector3.Lerp(base.Direction, b, spellFollowMouseLerp * mouseChaseRatio));
				break;
			}
			case SpellSpecialMovementType.ChaseEnemy:
				if (base.SpellFollowHaveTarget)
				{
					base.Direction = Tool2D.IgnoreZPoint(Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * followTargetTimeInterval));
				}
				break;
			case SpellSpecialMovementType.ChaseOwner:
			{
				float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(base.Direction, GetSpellFollowToOwnerDirection())) - 90f) / 90f;
				base.Direction = Tool2D.DirMoveTowardsTargetInCounterClockWise(base.Direction, GetSpellFollowToOwnerDirection(), base.CurrentSpeed * spellFollowTargetRotateSpeed * followTargetTimeInterval);
				float num4 = 0.4f;
				num3 *= Mathf.Lerp(1f - num4, 1f + num4, t);
				break;
			}
			}
			float maxDistance = base.CurrentSpeed * followTargetTimeInterval * num3;
			if (Physics.Raycast(base.transform.position, base.Direction, out hitInfo, maxDistance, layerMask))
			{
				if (hitInfo.transform.tag != "Untagged" && hitInfo.transform.tag != "SpellRebound" && hitInfo.transform.tag != "InteractiveObj")
				{
					_nodePoints.Add(hitInfo.point);
					_laserPoints.Add(hitInfo.point);
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point);
					base.transform.position = hitInfo.point;
					CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				if (hitInfo.transform.tag == "Untagged" || hitInfo.transform.tag == "InteractiveObj")
				{
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 0.1f;
					base.transform.position = hitInfo.point + base.Direction * 0.1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
					continue;
				}
				if (hitInfo.transform.tag == "Wall" || hitInfo.transform.tag == "SolidObj")
				{
					if (base.rebounceTime > 0 || (base.penetrateTime > 0 && base.spellCfg.level != 1))
					{
						if (base.rebounceTime > 0)
						{
							base.rebounceTime--;
						}
						else if (base.penetrateTime > 0)
						{
							base.penetrateTime--;
						}
						base.Direction = Vector3.Reflect(base.Direction, hitInfo.normal);
						CheckIfValidToDamageTarget(hitInfo);
						continue;
					}
					break;
				}
				if (hitInfo.transform.tag == "SpellRebound")
				{
					SpellBase component2 = hitInfo.transform.GetComponent<SpellBase>();
					if (component2 != null)
					{
						if (!IsSameCamp(component2) && (component2.spellCfg.abilityType == SpellAbilityType.Butterfly || component2.spellCfg.abilityType == SpellAbilityType.Rollball))
						{
							_laserPoints.Add(hitInfo.point);
							_nodePoints.Add(hitInfo.point);
							_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point);
							base.transform.position = hitInfo.point;
							CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
							if (component2 is Spell1003Butterfly spell1003Butterfly2 && !IsSameCamp(spell1003Butterfly2))
							{
								spell1003Butterfly2.Break();
							}
							else
							{
								((Spell1002RollBall)component2).TakeDamage(base.spellCfg.damage);
								if (base.penetrateTime <= 0)
								{
									shootDone = true;
									break;
								}
								base.penetrateTime--;
								base.transform.position += base.Direction * 0.1f;
								_remainDistance -= 1f;
								if (_remainDistance <= 0f)
								{
									break;
								}
							}
						}
						_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 1f;
						base.transform.position = hitInfo.point + base.Direction * 0.1f;
						if (_remainDistance <= 0f)
						{
							break;
						}
					}
					else
					{
						Debug.LogError(hitInfo.transform.name);
					}
					continue;
				}
				if (hitInfo.transform.CompareTag("Brittleness"))
				{
					hitInfo.transform.GetComponent<UnitProperty>().AnnouncedDeath();
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 1f;
					base.transform.position = hitInfo.point + base.Direction * 0.1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
					continue;
				}
				CheckIfValidToDamageTarget(hitInfo);
				if (base.remainRefractCount > 0)
				{
					if (!TryRefract(hitInfo.transform.gameObject) && base.spellCfg.level == 1)
					{
						break;
					}
					base.transform.position += base.Direction * 0.1f;
					_remainDistance -= 1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				else
				{
					if (base.penetrateTime <= 0)
					{
						shootDone = true;
						break;
					}
					base.penetrateTime--;
					base.transform.position += base.Direction * 0.1f;
					_remainDistance -= 1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				if (removeOtherMovement)
				{
					normalMovement(_laserPoints, _nodePoints, layerMask, _remainDistance);
					break;
				}
			}
			else
			{
				base.transform.position += base.Direction * base.CurrentSpeed * followTargetTimeInterval * num3;
				_laserPoints.Add(base.transform.position);
				_remainDistance -= base.CurrentSpeed * followTargetTimeInterval * num3;
				if (_remainDistance <= 0f)
				{
					_nodePoints.Add(base.transform.position);
					break;
				}
			}
		}
	}

	private void SpawnFallLaser()
	{
		_laserPoints.Clear();
		_nodePoints.Clear();
		if (base.spellCfg.isSplitSpell)
		{
			_laserPoints.Add(base.transform.position);
			_nodePoints.Add(base.transform.position);
			PushFallingReboundPoints(base.transform.position + base.Direction * 4f);
			Transform obj = base.transform;
			List<Vector3> nodePoints = _nodePoints;
			obj.position = nodePoints[nodePoints.Count - 1];
			EffectBase.CreateSpriteEffect("Hit", base.transform.position, Quaternion.Euler(0f, 0f, 90f));
		}
		else
		{
			SpawnFallFirstLaser();
		}
		EffectBase.CreateSpriteEffect("Hit", base.transform.position, Quaternion.Euler(0f, 0f, 90f));
		MakeFallingGroundDamageToAround();
		List<GameObject> list = (from e in GetFallingGroundDamageTargets()
			select e.gameObject).ToList();
		while (true)
		{
			Vector3? vector = null;
			if (base.remainRefractCount > 0 && lastOutputDamageFrame == Time.frameCount && list.Count > 0)
			{
				UnitProperty unitProperty = TryRefract(list.ToArray());
				list.Clear();
				if ((bool)unitProperty)
				{
					list.Add(unitProperty.gameObject);
					vector = unitProperty.transform.position;
				}
			}
			if (base.rebounceTime > 0 && !vector.HasValue)
			{
				base.rebounceTime--;
				vector = base.transform.position + base.Direction * 4f;
			}
			if (vector.HasValue)
			{
				PushFallingReboundPoints(vector.Value);
				Transform obj2 = base.transform;
				List<Vector3> nodePoints2 = _nodePoints;
				obj2.position = nodePoints2[nodePoints2.Count - 1];
				EffectBase.CreateSpriteEffect("Hit", base.transform.position, Quaternion.Euler(0f, 0f, 90f));
				MakeFallingGroundDamageToAround();
				continue;
			}
			break;
		}
	}

	private void SpawnFallFirstLaser_Normal()
	{
		Vector3 position = base.transform.position;
		Vector3 item = new Vector3(position.x, position.y + FallInitialHeight, 0f);
		item -= base.Direction * 2f;
		_laserPoints.Add(item);
		_nodePoints.Add(item);
		_laserPoints.Add(new Vector3(position.x, position.y, 0f));
		_nodePoints.Add(new Vector3(position.x, position.y, 0f));
	}

	private void SpawnFallFirstLaser()
	{
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
		case SpellSpecialMovementType.ChaseMouse:
			SpawnFallFirstLaser_Normal();
			break;
		case SpellSpecialMovementType.ChaseEnemy:
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			if (base.SpellFollowHaveTarget)
			{
				ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
				if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
				{
					float maxDistanceDelta = spellFollowTargetRotateSpeed * 0.15f;
					base.transform.position = Vector3.MoveTowards(base.transform.position, spellFollowTargetPpt.transform.position, maxDistanceDelta);
					Vector3 vector3 = Tool2D.IgnoreZPoint(base.SIP.finalShootSpatialInfo.Target.Value, -8f);
					Vector3 v2 = Tool2D.IgnoreZPoint(vector3, -5f);
					for (int k = 1; k <= 20; k++)
					{
						float t2 = (float)k / 20f;
						Vector3 item3 = GeneralTool.QuadraticBezierCurve(vector3, v2, base.transform.position, t2);
						_laserPoints.Add(item3);
					}
					List<Vector3> nodePoints5 = _nodePoints;
					List<Vector3> laserPoints5 = _laserPoints;
					nodePoints5.Add(laserPoints5[laserPoints5.Count - 1]);
					List<Vector3> laserPoints6 = _laserPoints;
					Vector3 vector4 = laserPoints6[laserPoints6.Count - 1];
					List<Vector3> laserPoints7 = _laserPoints;
					base.Direction = Tool2D.IgnoreZPoint(vector4 - laserPoints7[laserPoints7.Count - 2]).normalized;
					break;
				}
			}
			SpawnFallFirstLaser_Normal();
			break;
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / 20f) * Time.deltaTime;
			for (int j = 1; j <= 20; j++)
			{
				base.spellAroundOwnerCurrentAngle += num * 2f;
				Vector3 item2 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				item2.z = 0f - (float)(20 - j + 1) / 20f * 4f;
				_laserPoints.Add(item2);
			}
			List<Vector3> nodePoints3 = _nodePoints;
			List<Vector3> laserPoints4 = _laserPoints;
			nodePoints3.Add(laserPoints4[laserPoints4.Count - 1]);
			Transform obj2 = base.transform;
			List<Vector3> nodePoints4 = _nodePoints;
			obj2.position = nodePoints4[nodePoints4.Count - 1];
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3? spellFollowToOwnerPoint = GetSpellFollowToOwnerPoint();
			if (!spellFollowToOwnerPoint.HasValue)
			{
				SpawnFallFirstLaser_Normal();
				break;
			}
			Vector3 vector = Tool2D.IgnoreZPoint(base.SIP.finalShootSpatialInfo.Target.Value, -8f);
			Vector3 v = Tool2D.IgnoreZPoint(vector, -4f);
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				Vector3 item = GeneralTool.QuadraticBezierCurve(vector, v, spellFollowToOwnerPoint.Value, t);
				_laserPoints.Add(item);
			}
			List<Vector3> nodePoints = _nodePoints;
			List<Vector3> laserPoints = _laserPoints;
			nodePoints.Add(laserPoints[laserPoints.Count - 1]);
			Transform obj = base.transform;
			List<Vector3> nodePoints2 = _nodePoints;
			obj.position = nodePoints2[nodePoints2.Count - 1];
			List<Vector3> laserPoints2 = _laserPoints;
			Vector3 vector2 = laserPoints2[laserPoints2.Count - 1];
			List<Vector3> laserPoints3 = _laserPoints;
			base.Direction = Tool2D.IgnoreZPoint(vector2 - laserPoints3[laserPoints3.Count - 2]).normalized;
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void PushFallingReboundPoints_Normal(Vector3 target, Vector3 startPoint, float centerPointHeight, int step)
	{
		Vector3 v = 0.5f * (startPoint + target).IgnoreZ();
		v.z = 0f - centerPointHeight;
		for (int i = 1; i <= step; i++)
		{
			float t = (float)i / (float)step;
			Vector3 item = GeneralTool.QuadraticBezierCurve(startPoint, v, target, t);
			_laserPoints.Add(item);
		}
	}

	private void PushFallingReboundPoints(Vector3 target)
	{
		rebounded = true;
		Vector3 vector;
		if (_laserPoints.Count <= 0)
		{
			vector = base.transform.position;
		}
		else
		{
			List<Vector3> laserPoints = _laserPoints;
			vector = laserPoints[laserPoints.Count - 1];
		}
		Vector3 vector2 = vector;
		float num = Vector3.Distance(target, vector2) * 1f;
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
			PushFallingReboundPoints_Normal(target, vector2, num, 20);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		{
			base.transform.position += base.Direction * 0.02f;
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			if (!base.SpellFollowHaveTarget)
			{
				PushFallingReboundPoints_Normal(target, vector2, num, 20);
				break;
			}
			float num3 = Vector3.Distance(spellFollowTargetPpt.transform.position, base.transform.position);
			Vector3 v5 = base.Direction * num3 * 0.5f + base.transform.position;
			v5.z = 0f - num;
			base.Direction = Tool2D.IgnoreZPoint(Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), spellFollowTargetRotateSpeed * followTargetTimeInterval * 20f * 5f));
			Vector3 v6 = base.transform.position + base.Direction * num3;
			for (int l = 1; l <= 20; l++)
			{
				float t4 = (float)l / 20f;
				Vector3 item4 = GeneralTool.QuadraticBezierCurve(vector2, v5, v6, t4);
				_laserPoints.Add(item4);
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 v3 = base.Direction + base.transform.position;
			Vector3 v4 = PlayerMgr.Inst.GetMousePoint() + UnityEngine.Random.insideUnitSphere;
			v4.z = 0f;
			v3.z = 0f - num;
			for (int j = 1; j <= 20; j++)
			{
				float t2 = (float)j / 20f;
				Vector3 item2 = GeneralTool.QuadraticBezierCurve(vector2, v3, v4, t2);
				_laserPoints.Add(item2);
			}
			List<Vector3> laserPoints2 = _laserPoints;
			Vector3 vector3 = laserPoints2[laserPoints2.Count - 1];
			List<Vector3> laserPoints3 = _laserPoints;
			base.Direction = Tool2D.IgnoreZPoint(vector3 - laserPoints3[laserPoints3.Count - 2]).normalized;
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num2 = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / 20f) * Time.deltaTime;
			for (int k = 1; k <= 20; k++)
			{
				base.spellAroundOwnerCurrentAngle += num2;
				Vector3 item3 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				float t3 = (float)k / 20f;
				item3.z = GeneralTool.QuadraticBezierCurve(Vector3.zero, new Vector3(0f, 0f, 0f - num), Vector3.zero, t3).z;
				_laserPoints.Add(item3);
			}
			base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3 v = base.Direction * 4f + base.transform.position;
			Vector3 v2 = Tool2D.GetDir(base.Direction, -90f) * 4f + base.transform.position;
			v.z = 0f - num;
			v2.z = 0f - num;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				Vector3 item = GeneralTool.CubicBezierCurve(vector2, v, v2, PlayerMgr.Inst.PlayerCtrller.transform.position, t);
				_laserPoints.Add(item);
			}
			base.Direction = Tool2D.GetDir(base.Direction, -90f);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		List<Vector3> nodePoints = _nodePoints;
		List<Vector3> laserPoints4 = _laserPoints;
		nodePoints.Add(laserPoints4[laserPoints4.Count - 1]);
	}

	private void SpawnLaserEffect()
	{
		lr_Laser.widthMultiplier = laserOrignalWidth * base.transform.localScale.x;
		lr_Shadow.widthMultiplier = shadowOrignalWidth * base.transform.localScale.x;
		lr_Laser.positionCount = _laserPoints.Count;
		lr_Shadow.positionCount = _laserPoints.Count;
		for (int i = 0; i < _laserPoints.Count; i++)
		{
			lr_Laser.SetPosition(i, Tool2D.GetLayerPoint(_laserPoints[i]));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(_laserPoints[i], 1.05f));
		}
		if (_laserPoints.Count > 1)
		{
			SpawnBubble();
		}
		laserNodes = new SpriteRenderer[_nodePoints.Count];
		for (int j = 0; j < _nodePoints.Count; j++)
		{
			laserNodes[j] = SpawnNodeSpriteRender();
			InitialSpriteMaterial(laserNodes[j]);
		}
		lr_Laser.enabled = true;
		lr_Shadow.enabled = true;
	}

	private void normalMovement(List<Vector3> _laserPoints, List<Vector3> _nodePoints, LayerMask _attackLayer, float _remainDistance)
	{
		int num = 0;
		while (true)
		{
			num++;
			if (num >= 300)
			{
				Debug.LogError("死循环 请检查");
				break;
			}
			if (Physics.Raycast(base.transform.position, base.Direction, out var hitInfo, _remainDistance, _attackLayer))
			{
				if (hitInfo.transform.tag != "Untagged" && hitInfo.transform.tag != "SpellRebound")
				{
					_laserPoints.Add(hitInfo.point);
					_nodePoints.Add(hitInfo.point);
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point);
					base.transform.position = hitInfo.point;
					CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
				}
				if (hitInfo.transform.tag == "Untagged")
				{
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 0.1f;
					base.transform.position = hitInfo.point + base.Direction * 0.1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				else if (hitInfo.transform.tag == "Wall" || hitInfo.transform.tag == "SolidObj")
				{
					base.transform.position = hitInfo.point;
					if (base.rebounceTime <= 0 && (base.penetrateTime <= 0 || base.spellCfg.level == 1))
					{
						break;
					}
					if (base.rebounceTime > 0)
					{
						base.rebounceTime--;
					}
					else if (base.penetrateTime > 0)
					{
						base.penetrateTime--;
					}
					base.Direction = Vector3.Reflect(base.Direction, hitInfo.normal);
					CheckIfValidToDamageTarget(hitInfo);
				}
				else if (hitInfo.transform.tag == "SpellRebound")
				{
					SpellBase component = hitInfo.transform.GetComponent<SpellBase>();
					if (component != null)
					{
						if (!IsSameCamp(component) && (component.spellCfg.abilityType == SpellAbilityType.Butterfly || component.spellCfg.abilityType == SpellAbilityType.Rollball))
						{
							_laserPoints.Add(hitInfo.point);
							_nodePoints.Add(hitInfo.point);
							_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point);
							base.transform.position = hitInfo.point;
							CreateHitEffect(hitInfo.point, Quaternion.LookRotation(-base.Direction));
							if (component is Spell1003Butterfly spell1003Butterfly && !IsSameCamp(spell1003Butterfly))
							{
								spell1003Butterfly.Break();
							}
							else
							{
								((Spell1002RollBall)component).TakeDamage(base.spellCfg.damage);
								if (base.penetrateTime <= 0)
								{
									shootDone = true;
									break;
								}
								base.penetrateTime--;
								base.transform.position += base.Direction * 0.1f;
								_remainDistance -= 1f;
								if (_remainDistance <= 0f)
								{
									break;
								}
							}
						}
						_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 1f;
						base.transform.position = hitInfo.point + base.Direction * 0.1f;
						if (_remainDistance <= 0f)
						{
							break;
						}
					}
					else
					{
						Debug.LogError(hitInfo.transform.name);
					}
				}
				else if (hitInfo.transform.tag == "Brittleness")
				{
					hitInfo.transform.GetComponent<UnitProperty>().AnnouncedDeath();
					_remainDistance -= Vector3.Distance(base.transform.position, hitInfo.point) + 1f;
					base.transform.position = hitInfo.point + base.Direction * 0.1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				else if (hitInfo.transform.tag == "Player" || hitInfo.transform.tag == "Teammate" || hitInfo.transform.tag == "Monster")
				{
					CheckIfValidToDamageTarget(hitInfo);
					if (base.penetrateTime <= 0)
					{
						shootDone = true;
						break;
					}
					base.penetrateTime--;
					base.transform.position += base.Direction * 0.1f;
					_remainDistance -= 1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				else
				{
					if (base.penetrateTime <= 0)
					{
						break;
					}
					base.penetrateTime--;
					base.transform.position += base.Direction * 0.1f;
					_remainDistance -= 1f;
					if (_remainDistance <= 0f)
					{
						break;
					}
				}
				continue;
			}
			base.transform.position += base.Direction * _remainDistance;
			_laserPoints.Add(base.transform.position);
			_nodePoints.Add(base.transform.position);
			break;
		}
	}

	private void CheckIfValidToDamageTarget(RaycastHit _hit)
	{
		base.transform.position += base.Direction * 0.1f;
		UnitProperty component = _hit.transform.gameObject.GetComponent<UnitProperty>();
		if (!HitedEnemy.Contains(component))
		{
			OutputDamage(_hit.transform.gameObject);
			HitedEnemy.Add(component);
		}
	}

	private void CheckIfHoverValidToDamageTarget(RaycastHit _hit, List<Vector3> _laserPoints, List<Vector3> _nodePoints)
	{
		UnitProperty component = _hit.transform.gameObject.GetComponent<UnitProperty>();
		if (!HitedEnemy.Contains(component))
		{
			OutputDamage(_hit.transform.gameObject);
			HitedEnemy.Add(component);
		}
		if (base.penetrateTime <= 0 || base.spellCfg.level <= 1)
		{
			return;
		}
		foreach (UnitProperty monsterPpt in LevelMgr.Inst.CurrentRoomCtrller.MonsterPpts)
		{
			if (!HitedEnemy.Contains(monsterPpt))
			{
				_laserPoints.Add(_hit.point);
				_nodePoints.Add(_hit.point);
				base.Direction = Tool2D.IgnoreZPoint(monsterPpt.transform.position - _hit.point).normalized;
				removeOtherMovement = true;
				break;
			}
		}
	}

	public override TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		if (targetGO.CompareTag("RollBall") || targetGO.CompareTag("Butterfly"))
		{
			targetGO = targetGO.transform.parent.gameObject;
		}
		if (targetGO.CompareTag("SolidObj") || targetGO.CompareTag("Untagged") || targetGO.CompareTag("InteractiveObj"))
		{
			return info;
		}
		if (targetGO.CompareTag("SpellRebound"))
		{
			SpellBase component = targetGO.GetComponent<SpellBase>();
			if (component != null && !IsSameCamp(component))
			{
				if (component is Spell1003Butterfly spell1003Butterfly && !IsSameCamp(spell1003Butterfly))
				{
					EffectBase.CreateSpriteEffect("HitTarget", targetGO.transform.position);
					spell1003Butterfly.Break();
				}
				else if (component.spellCfg.abilityType == SpellAbilityType.Rollball)
				{
					EffectBase.CreateSpriteEffect("HitTarget", targetGO.transform.position);
					((Spell1002RollBall)component).TakeDamage(base.spellCfg.damage);
				}
			}
			return info;
		}
		EffectBase.CreateSpriteEffect("HitTarget", targetGO.transform.position);
		return base.OutputDamage(targetGO, info);
	}

	public void InitialSpriteMaterial(SpriteRenderer Trail)
	{
		Material material = Trail.material;
		material = (Trail.material = UnityEngine.Object.Instantiate(material));
	}

	private Mesh SpawnBubbleMesh(Vector3[] posArr)
	{
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < posArr.Length; i++)
		{
			Vector3 item = posArr[i];
			list.Add(item);
			list.Add(item);
			if (i > 0)
			{
				list2.Add(i * 2 - 2);
				list2.Add(i * 2 - 2 + 2);
				list2.Add(i * 2 - 2 + 1);
				list2.Add(i * 2 - 2 + 1);
				list2.Add(i * 2 - 2 + 2);
				list2.Add(i * 2 - 2 + 3);
			}
		}
		return new Mesh
		{
			vertices = list.ToArray(),
			triangles = list2.ToArray()
		};
	}

	private void SpawnBubble()
	{
		ObjPoolMgr.Inst.GetGO(GetRelationResourcePath("Bubble"), delegate(GameObject go)
		{
			go.transform.localScale = Vector3.one;
			go.transform.position = Vector3.zero;
			SkinnedMeshRenderer componentInChildren2 = go.GetComponentInChildren<SkinnedMeshRenderer>();
			if ((bool)componentInChildren2.sharedMesh)
			{
				UnityEngine.Object.Destroy(componentInChildren2.sharedMesh);
			}
			Vector3[] array = new Vector3[lr_Laser.positionCount];
			lr_Laser.GetPositions(array);
			componentInChildren2.sharedMesh = SpawnBubbleMesh(array);
			Color color2 = BubbleColors.Get(base.ColorType);
			if (base.ColorType == SpellColorType.Monster && GameMgr.IsHarmony_Static)
			{
				color2 = HarmoniousMonsterBubbleColor;
			}
			color2.a *= DataMgr.settingData.FinalSpellTransparent * DataMgr.settingData.FinalSpellTransparent;
			go.GetComponentInChildren<VisualEffect>().SetVector4("BubbleColor", color2);
		}, 1f);
		if (!rebounded)
		{
			return;
		}
		ObjPoolMgr.Inst.GetGO(GetRelationResourcePath("Bubble"), delegate(GameObject go)
		{
			go.transform.localScale = Vector3.one;
			go.transform.position = Vector3.zero;
			SkinnedMeshRenderer componentInChildren = go.GetComponentInChildren<SkinnedMeshRenderer>();
			if ((bool)componentInChildren.sharedMesh)
			{
				UnityEngine.Object.Destroy(componentInChildren.sharedMesh);
			}
			Vector3[] posArr = new Vector3[2]
			{
				lr_Laser.GetPosition(0),
				lr_Laser.GetPosition(1)
			};
			SpawnBubbleMesh(posArr);
			componentInChildren.sharedMesh = SpawnBubbleMesh(posArr);
			Color color = BubbleColors.Get(base.ColorType);
			if (base.ColorType == SpellColorType.Monster && GameMgr.IsHarmony_Static)
			{
				color = HarmoniousMonsterBubbleColor;
			}
			color.a *= DataMgr.settingData.FinalSpellTransparent * DataMgr.settingData.FinalSpellTransparent;
			go.GetComponentInChildren<VisualEffect>().SetVector4("BubbleColor", color);
		}, 1f);
	}

	private SpriteRenderer SpawnNodeSpriteRender()
	{
		return ObjPoolMgr.Inst.GetGO(GetRelationResourcePath("Node_" + base.ColorType), 1f).GetComponentInChildren<SpriteRenderer>();
	}

	public override void CreateHitEffect(Vector3? position = null, Quaternion? rotation = null)
	{
		rotation *= Quaternion.Euler(0f, 90f, 180f);
		base.CreateHitEffect(position, rotation);
	}

	protected override void InitSpeedAndPosition()
	{
		if (!base.SIP.spellIsFall)
		{
			base.InitSpeedAndPosition();
		}
		else if (base.SIP.finalShootSpatialInfo != null)
		{
			Vector3? target = base.SIP.finalShootSpatialInfo.Target;
			if (!target.HasValue)
			{
				Debug.LogWarning("坠落法术却没有攻击目标点的信息？");
				return;
			}
			Vector3 position = (base.spellCfg.isSplitSpell ? base.SIP.finalShootSpatialInfo.Start : target.Value);
			position.z = 0f;
			base.transform.position = position;
		}
	}

	protected override TakeDamageInfo MakeDamageToUnit(UnitProperty unit)
	{
		EffectBase.CreateSpriteEffect("HitTarget", unit.transform.position);
		return base.MakeDamageToUnit(unit);
	}
}
