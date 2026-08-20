using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1011DisintegrationRay : SpellBase, IOnLaunchFromSpellEventHandle, IOnLaunchFromUnitNotPlayer
{
	[Space(50f)]
	public LayerMask playerAttackLayer;

	public LayerMask monsterAttackLayer;

	public LineRenderer lr_Line;

	public LineRenderer lr_Shadow;

	public Transform tsf_CtrlledNode;

	public float rayRadius;

	public float aroundOwnerCircleLength;

	public ShockParam shockParam;

	private readonly List<GameObject> attackedGOs = new List<GameObject>();

	private readonly List<float> attackedTimers = new List<float>();

	private LayerMask attackLayer;

	private float laserWidth;

	private bool isPlayerSpell;

	private Vector3 originPoint;

	private readonly List<(Vector3 pos, Vector3 dir)> rayNodes = new List<(Vector3, Vector3)>();

	private Vector3 laserStartPoint;

	private Vector3 laserShootDir;

	private bool keepCastBuffApplied;

	private Vector3 aroundPos = Vector3.zero;

	private float angleShift;

	private float reboundBonusLifeTimeLeft;

	private float fallDamageTimer;

	private float baseSpellAroundOwnerCurrentAngle;

	private bool rebounded;

	private bool CanMakeFallingDamage => fallDamageTimer <= 0f;

	public override void InitializeCallback()
	{
		rebounded = false;
		baseSpellAroundOwnerCurrentAngle = base.spellAroundOwnerCurrentAngle;
		keepCastBuffApplied = false;
		isPlayerSpell = ownerPpt.unitCfg.unitType == UnitType.Player;
		if (base.SIP.spellIsFall)
		{
			originPoint = base.transform.position;
		}
		if (!isPlayerSpell || base.spellCfg.isSplitSpell)
		{
			originPoint = base.transform.position;
		}
		if (!isPlayerSpell || base.indirectShootByPlayer)
		{
			laserShootDir = base.Direction;
			laserStartPoint = base.transform.position;
		}
		attackLayer = (IsSameCamp(UnitType.Player) ? playerAttackLayer : monsterAttackLayer);
		reboundBonusLifeTimeLeft = base.extraReboundTime * base.reboundAddTime;
		tsf_CtrlledNode.gameObject.SetActive(value: false);
		laserWidth = base.transform.localScale.x / 2f * rayRadius;
		lr_Line.startWidth = laserWidth;
		lr_Line.endWidth = laserWidth;
		lr_Shadow.startWidth = laserWidth;
		lr_Shadow.endWidth = laserWidth;
		lr_Line.positionCount = 2;
		lr_Shadow.positionCount = 2;
		aroundPos = Vector3.zero;
		for (int i = 0; i < lr_Line.positionCount; i++)
		{
			lr_Line.SetPosition(i, Vector3.zero);
			lr_Shadow.SetPosition(i, Vector3.zero);
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			tsf_CtrlledNode.gameObject.SetActive(value: false);
		}
		else if (spellFollowTargetRotateSpeed != 0f)
		{
			tsf_CtrlledNode.gameObject.SetActive(value: true);
		}
		else
		{
			tsf_CtrlledNode.gameObject.SetActive(value: true);
		}
		enableFollowMouse = false;
		enableFollowTarget = false;
		enableNormalTransform = false;
		base.enableAroundPlayer = false;
		if (base.SIP.equalScatter)
		{
			angleShift = GetEqualScatterMultipleShootInitialDirectionAngleShift();
		}
		else
		{
			angleShift = UnityEngine.Random.Range((0f - base._angle) / 2f, base._angle / 2f);
		}
		PlayLoopSE("Loop", base.spellCfg.duration + base.SpellHoverTime);
		if (isPlayerSpell && !base.spellCfg.isSplitSpell && !keepCastBuffApplied && !base.indirectShootByPlayer && !base.SIP.spellIsFall && !base.SIP.shootFromPostSlots && base.shooterWand == PlayerMgr.Inst.SelectedWand)
		{
			keepCastBuffApplied = true;
		}
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		tsf_CtrlledNode.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		if (base.SIP.spellIsFall)
		{
			UpdateLaserStartPointWithFall();
		}
		else
		{
			UpdateLaserStartPoint();
		}
		if (base.shouldCameraShock)
		{
			CamController.Inst.SetShock(shockParam);
		}
		for (int num = attackedTimers.Count - 1; num >= 0; num--)
		{
			attackedTimers[num] -= Time.deltaTime;
			if (attackedTimers[num] <= 0f)
			{
				attackedTimers.RemoveAt(num);
				attackedGOs.RemoveAt(num);
			}
		}
		if (base.SIP.spellIsFall)
		{
			fallDamageTimer -= Time.deltaTime;
			UpdateLaserPointsWithFall();
			if (CanMakeFallingDamage)
			{
				fallDamageTimer = base.spellCfg.DPSDamageInterval;
			}
		}
		else
		{
			UpdateLaserPoints();
			UpdateLaserDamage();
		}
		Transform obj = base.transform;
		List<(Vector3 pos, Vector3 dir)> list = rayNodes;
		obj.position = list[list.Count - 1].pos;
		tsf_CtrlledNode.position = Tool2D.GetLayerPoint(rayNodes[0].pos);
		lr_Line.positionCount = rayNodes.Count;
		lr_Shadow.positionCount = rayNodes.Count;
		for (int i = 0; i < rayNodes.Count; i++)
		{
			lr_Line.SetPosition(i, Tool2D.GetLayerPoint(rayNodes[i].pos));
			lr_Shadow.SetPosition(i, Tool2D.IgnoreZPoint(rayNodes[i].pos, 1.05f));
		}
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				aroundPos = GetAroundTargetBasePoint();
				_ = keepCastBuffApplied;
			}
			if (base.SpellHoverTime > 0f && base.SpellHoverTimer < base.SpellHoverTime)
			{
				base.SpellHoverTimer += Time.deltaTime;
			}
			else if ((!base.spellCfg.isSplitSpell && base.spellSplitCount != 0) || base.TriggerCtrl.HasOnOverTrigger())
			{
				PoolRecycle();
			}
			else
			{
				PoolRecycle();
			}
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (base.HalfLifeRandomTeleport())
		{
			laserStartPoint = base.transform.position;
			laserShootDir = Tool2D.GetDir();
			base.indirectShootByPlayer = true;
			return true;
		}
		return false;
	}

	private void UpdateLaserStartPoint()
	{
		if (base.spellCfg.isSplitSpell)
		{
			return;
		}
		if (base.OwnerSpell is Spell1015ArcaneNova)
		{
			laserStartPoint = base.OwnerSpell.transform.position;
		}
		else
		{
			if (!isPlayerSpell || base.OwnerSpell != null || base.indirectShootByPlayer)
			{
				return;
			}
			if (base.OwnerSpell == null)
			{
				if ((bool)base.shooterWand)
				{
					laserStartPoint = base.shooterWand.GetShootPosition();
				}
				else
				{
					laserStartPoint = PlayerMgr.Inst.ShootPoint;
				}
				laserShootDir = ((base.shooterWand != null && base.shooterWand.WandCfg != null && base.shooterWand.WandCfg.specialAbility == WandAbility.FourDirShoot) ? Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir * IsReverseDirection()) + angleShift + Tool2D.GetDegree(base.SIP.originShootDirection)) : Tool2D.GetDir(Tool2D.GetDegree(PlayerMgr.Inst.PlayerDir * IsReverseDirection()) + angleShift));
			}
			else if (base.OwnerPoint != Vector3.zero)
			{
				laserStartPoint = base.OwnerPoint;
			}
		}
	}

	private void UpdateLaserStartPointWithFall()
	{
	}

	private void ClearRefractionDataWhenUpdateLaserPoints()
	{
		(int, float)? refractionInfo = base.SIP.RefractionInfo;
		if (refractionInfo.HasValue)
		{
			base.remainRefractCount = base.SIP.RefractionInfo.Value.count;
		}
		else
		{
			base.remainRefractCount = 0;
		}
		refractedTargets.Clear();
	}

	private void PushFallingReboundPoints_Normal(Vector3 target, Vector3 startPoint, float centerPointHeight, int step)
	{
		Vector3 v = 0.5f * (startPoint + target).IgnoreZ();
		v.z = 0f - centerPointHeight;
		Vector3 normalized = Tool2D.IgnoreZPoint(target - startPoint).normalized;
		for (int i = 1; i <= step; i++)
		{
			float t = (float)i / (float)step;
			Vector3 item = GeneralTool.QuadraticBezierCurve(startPoint, v, target, t);
			rayNodes.Add((item, normalized));
		}
	}

	private void PushFallingReboundPoints(Vector3 target)
	{
		rebounded = true;
		Vector3 position = base.transform.position;
		float num = Vector3.Distance(target, position) * 1.8f;
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.Normal:
			PushFallingReboundPoints_Normal(target, position, num, 20);
			break;
		case SpellSpecialMovementType.ChaseEnemy:
		{
			base.transform.position += base.Direction * 0.01f;
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			if (!base.SpellFollowHaveTarget)
			{
				PushFallingReboundPoints_Normal(target, position, num, 20);
				break;
			}
			float b = Vector3.Distance(spellFollowTargetPpt.transform.position, base.transform.position);
			b = Mathf.Min(2f, b);
			base.Direction = Tool2D.DirMoveTowards(base.Direction, spellFollowTargetPpt.transform.position - base.transform.position, spellFollowTargetRotateSpeed * 3f);
			Vector3 v4 = base.transform.position + base.Direction * b;
			Vector3 v5 = Tool2D.IgnoreZPoint(target, 0f - num);
			for (int j = 1; j <= 20; j++)
			{
				float t2 = (float)j / 20f;
				Vector3 vector2 = GeneralTool.QuadraticBezierCurve(position, v5, v4, t2);
				List<(Vector3 pos, Vector3 dir)> list3 = rayNodes;
				Vector3 item = vector2 - list3[list3.Count - 1].pos;
				rayNodes.Add((vector2, item));
			}
			break;
		}
		case SpellSpecialMovementType.ChaseMouse:
		{
			Vector3 v6 = base.transform.position + base.Direction * 6f;
			Vector3 v7 = base.transform.position + Tool2D.GetDir(base.Direction, 90f) * 6f;
			v6.z = (0f - num) * 0.5f;
			v7.z = (0f - num) * 0.5f;
			for (int k = 1; k <= 20; k++)
			{
				float t3 = (float)k / 20f;
				Vector3 vector3 = GeneralTool.CubicBezierCurve(position, v6, v7, PlayerMgr.Inst.GetMousePoint(), t3);
				List<(Vector3 pos, Vector3 dir)> list4 = rayNodes;
				List<(Vector3 pos, Vector3 dir)> list5 = rayNodes;
				list4.Add((vector3, vector3 - list5[list5.Count - 1].pos));
			}
			break;
		}
		case SpellSpecialMovementType.Rotation:
		{
			float num2 = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / 20f) * 0.02f;
			for (int l = 1; l <= 20; l++)
			{
				base.spellAroundOwnerCurrentAngle += num2;
				Vector3 item2 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				float t4 = (float)l / 20f;
				item2.z = GeneralTool.QuadraticBezierCurve(Vector3.zero, new Vector3(0f, 0f, 0f - num), Vector3.zero, t4).z;
				Vector3 item3;
				if (rayNodes.Count == 1)
				{
					List<(Vector3 pos, Vector3 dir)> list6 = rayNodes;
					item3 = list6[list6.Count - 1].dir;
				}
				else
				{
					List<(Vector3 pos, Vector3 dir)> list7 = rayNodes;
					Vector3 item4 = list7[list7.Count - 1].pos;
					List<(Vector3 pos, Vector3 dir)> list8 = rayNodes;
					item3 = item4 - list8[list8.Count - 2].pos;
				}
				rayNodes.Add((item2, item3));
			}
			break;
		}
		case SpellSpecialMovementType.ChaseOwner:
		{
			Vector3 v = base.transform.position + base.Direction * 6f;
			Vector3 v2 = base.transform.position + Tool2D.GetDir(base.Direction, 90f) * 6f;
			v.z = (0f - num) * 0.5f;
			v2.z = (0f - num) * 0.5f;
			Vector3 v3 = GetSpellFollowToOwnerPoint() ?? base.transform.position;
			for (int i = 1; i <= 20; i++)
			{
				float t = (float)i / 20f;
				Vector3 vector = GeneralTool.CubicBezierCurve(position, v, v2, v3, t);
				List<(Vector3 pos, Vector3 dir)> list = rayNodes;
				List<(Vector3 pos, Vector3 dir)> list2 = rayNodes;
				list.Add((vector, vector - list2[list2.Count - 1].pos));
			}
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void SpawnFallFirstLaser_Normal()
	{
		Vector3 item = originPoint;
		rayNodes.Add((item, base.Direction));
		rayNodes.Add((base.SIP.finalShootSpatialInfo.Target.Value, base.Direction));
	}

	private void SpawnFallFirstLaser()
	{
		switch (base.currentSpellMovement)
		{
		case SpellSpecialMovementType.ChaseMouse:
			base.SIP.finalShootSpatialInfo = ShootSpellSpatialInfo.ToPoint(base.SIP.finalShootSpatialInfo.Start, PlayerMgr.Inst.GetMousePoint());
			base.Direction = base.SIP.finalShootSpatialInfo.Direction;
			SpawnFallFirstLaser_Normal();
			break;
		case SpellSpecialMovementType.Normal:
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
					base.transform.position = Vector3.MoveTowards(base.SIP.finalShootSpatialInfo.Target.Value, spellFollowTargetPpt.transform.position, maxDistanceDelta);
					Vector3 v2 = Tool2D.IgnoreZPoint(originPoint, -5f);
					for (int k = 1; k <= 20; k++)
					{
						float t2 = (float)k / 20f;
						Vector3 vector5 = GeneralTool.QuadraticBezierCurve(originPoint, v2, base.transform.position, t2);
						Vector3 item3 = base.Direction;
						if (rayNodes.Count > 0)
						{
							List<(Vector3 pos, Vector3 dir)> list6 = rayNodes;
							item3 = vector5 - list6[list6.Count - 1].pos;
						}
						rayNodes.Add((vector5, item3));
					}
					List<(Vector3 pos, Vector3 dir)> list7 = rayNodes;
					base.Direction = Tool2D.IgnoreZPoint(list7[list7.Count - 1].dir).normalized;
					break;
				}
			}
			SpawnFallFirstLaser_Normal();
			break;
		case SpellSpecialMovementType.Rotation:
		{
			float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / 20f) * 0.02f;
			for (int j = 1; j <= 20; j++)
			{
				base.spellAroundOwnerCurrentAngle += num * 2f;
				Vector3 vector3 = GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
				vector3.z = 0f - (float)(20 - j) / 20f * 4f;
				Vector3 item2 = base.Direction;
				if (rayNodes.Count > 0)
				{
					Vector3 vector4 = vector3;
					List<(Vector3 pos, Vector3 dir)> list4 = rayNodes;
					item2 = vector4 - list4[list4.Count - 1].pos;
				}
				rayNodes.Add((vector3, item2));
			}
			Transform obj2 = base.transform;
			List<(Vector3 pos, Vector3 dir)> list5 = rayNodes;
			obj2.position = list5[list5.Count - 1].pos;
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
				Vector3 vector2 = GeneralTool.QuadraticBezierCurve(vector, v, spellFollowToOwnerPoint.Value, t);
				Vector3 item = base.Direction;
				if (rayNodes.Count > 0)
				{
					List<(Vector3 pos, Vector3 dir)> list = rayNodes;
					item = vector2 - list[list.Count - 1].pos;
				}
				rayNodes.Add((vector2, item));
			}
			Transform obj = base.transform;
			List<(Vector3 pos, Vector3 dir)> list2 = rayNodes;
			obj.position = list2[list2.Count - 1].pos;
			List<(Vector3 pos, Vector3 dir)> list3 = rayNodes;
			base.Direction = Tool2D.IgnoreZPoint(list3[list3.Count - 1].dir).normalized;
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	private void UpdateLaserDamage()
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			Vector3 vector = (base.isFlyFinish ? aroundPos : GetAroundTargetBasePoint());
			Collider[] source = Physics.OverlapSphere(vector, base.spellAroundOwnerRadius + 0.1f, attackLayer);
			Collider[] inInnerRangeTargets = Physics.OverlapSphere(vector, base.spellAroundOwnerRadius - 0.8f, attackLayer);
			{
				foreach (Collider item in source.Where((Collider e) => !inInnerRangeTargets.Contains(e)))
				{
					OutputDamage(item.gameObject, new TakeDamageInfo
					{
						canRebound = false,
						damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval),
						knockbackForce = (item.transform.position - vector).normalized * base.spellCfg.knockback
					});
				}
				return;
			}
		}
		for (int i = 0; i < rayNodes.Count - 1; i++)
		{
			RaycastHit[] array = Physics.SphereCastAll(rayNodes[i].pos, laserWidth, rayNodes[i + 1].pos - rayNodes[i].pos, Vector3.Distance(rayNodes[i + 1].pos, rayNodes[i].pos), attackLayer);
			for (int j = 0; j < array.Length; j++)
			{
				OutputDamage(array[j].transform.gameObject, new TakeDamageInfo
				{
					canRebound = false,
					damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval),
					knockbackForce = rayNodes[i].dir.normalized * base.spellCfg.knockback
				});
			}
		}
	}

	private void UpdateLaserPoints()
	{
		ClearRefractionDataWhenUpdateLaserPoints();
		if (base.isFlyFinish || Time.timeScale == 0f)
		{
			return;
		}
		rayNodes.Clear();
		int num = base.rebounceTime;
		float num2 = base.spellCfg.speed;
		int num3 = 0;
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			float num4 = MathF.PI * base.spellAroundOwnerRadius * base.spellAroundOwnerRadius / aroundOwnerCircleLength;
			for (int i = 0; (float)i < num4; i++)
			{
				Vector3 item = GetAroundTargetBasePoint() + Tool2D.GetDir(Tool2D.IgnoreZPoint(base.Direction), 360f / num4 * (float)i) * base.spellAroundOwnerRadius;
				rayNodes.Add((item, base.Direction));
			}
			rayNodes.Add(rayNodes[0]);
			return;
		}
		if (base.currentSpellMovement == SpellSpecialMovementType.ChaseMouse || base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy || base.currentSpellMovement == SpellSpecialMovementType.Normal || base.currentSpellMovement == SpellSpecialMovementType.ChaseOwner)
		{
			Vector3 vector = base.Direction;
			float num5 = 1f;
			bool flag = false;
			if (!base.spellCfg.isSplitSpell)
			{
				if (base.OwnerPoint != Vector3.zero && base.currentSpellMovement == SpellSpecialMovementType.Rotation)
				{
					rayNodes.Add((base.OwnerPoint, vector));
				}
				else
				{
					rayNodes.Add((laserStartPoint + Tool2D.GetDir(laserShootDir, -90f - angleShift) * (base.InitialParameter.multiShootCount - 1) / 2f * base.InitialParameter.multiShootSpace + Tool2D.GetDir(laserShootDir, 90f - angleShift) * base.InitialParameter.inMultiShootIndex * base.InitialParameter.multiShootSpace, vector));
					vector = laserShootDir;
				}
			}
			else
			{
				rayNodes.Add((originPoint, vector));
			}
			if (base.currentSpellMovement == SpellSpecialMovementType.ChaseEnemy)
			{
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			}
			while (true)
			{
				num3++;
				if (num3 >= 500)
				{
					Debug.LogError("死循环 请检查");
					break;
				}
				switch (base.currentSpellMovement)
				{
				case SpellSpecialMovementType.ChaseMouse:
				{
					Vector3 shootWorldPoint = PlayerMgr.Inst.PlayerCtrller.ShootWorldPoint;
					List<(Vector3 pos, Vector3 dir)> list = rayNodes;
					Vector3 b = Tool2D.IgnoreZV2ToV1Normal(shootWorldPoint, list[list.Count - 1].pos);
					vector = Vector3.Lerp(vector, b, spellFollowMouseLerp);
					break;
				}
				case SpellSpecialMovementType.ChaseEnemy:
					if (base.SpellFollowHaveTarget && !flag)
					{
						Vector3 vector3 = Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, rayNodes[rayNodes.Count - 1].pos);
						vector = Tool2D.DirMoveTowards(vector, vector3, aroundOwnerCircleLength * spellFollowTargetRotateSpeed);
						if (vector == vector3)
						{
							flag = true;
						}
					}
					break;
				case SpellSpecialMovementType.ChaseOwner:
					spellFollowTargetPpt = ownerPpt;
					if (base.SpellFollowHaveTarget && !flag)
					{
						Vector3 vector2 = Tool2D.IgnoreZV2ToV1Normal(spellFollowTargetPpt.transform.position, rayNodes[rayNodes.Count - 1].pos);
						vector = Tool2D.DirMoveTowards(vector, vector2, aroundOwnerCircleLength * spellFollowTargetRotateSpeed);
						if (vector == vector2)
						{
							flag = true;
						}
					}
					break;
				}
				if (rayNodes.Count >= 2)
				{
					List<(Vector3 pos, Vector3 dir)> list2 = rayNodes;
					Vector3 item2 = list2[list2.Count - 2].pos;
					List<(Vector3 pos, Vector3 dir)> list3 = rayNodes;
					GameObject[] array = HasTargetInLine(item2, list3[list3.Count - 1].pos).ToArray();
					if (array.Length != 0)
					{
						Transform obj = base.transform;
						List<(Vector3 pos, Vector3 dir)> list4 = rayNodes;
						obj.position = list4[list4.Count - 1].pos;
						base.Direction = vector.normalized;
						if ((bool)TryRefract(array))
						{
							vector = base.Direction;
						}
					}
				}
				if (!isThroughWall && Physics.Raycast(rayNodes[rayNodes.Count - 1].pos, vector, out var hitInfo, aroundOwnerCircleLength * num5, LayerMask.GetMask("Wall")))
				{
					num2 -= Vector3.Distance(rayNodes[rayNodes.Count - 1].pos, hitInfo.point);
					rayNodes.Add((hitInfo.point, vector));
					EffectBase.CreateSpriteEffect("HitWall", hitInfo.point);
					if (num <= 0)
					{
						break;
					}
					if (reboundBonusLifeTimeLeft > 0f)
					{
						reboundBonusLifeTimeLeft -= Time.deltaTime;
						base.DurationTimer -= Time.deltaTime;
						PlayLoopSE("Loop", base.spellCfg.duration - base.DurationTimer + base.SpellHoverTime);
					}
					num--;
					vector = Vector3.Reflect(vector, hitInfo.normal);
				}
				else
				{
					rayNodes.Add((rayNodes[rayNodes.Count - 1].pos + vector * aroundOwnerCircleLength * num5, vector));
					num2 -= aroundOwnerCircleLength * num5;
					if (num2 <= 0f)
					{
						break;
					}
				}
			}
			base.Direction = vector.normalized;
			return;
		}
		Vector3 vector4 = base.Direction;
		if (!base.spellCfg.isSplitSpell)
		{
			if (base.OwnerPoint != Vector3.zero && base.currentSpellMovement == SpellSpecialMovementType.Rotation)
			{
				rayNodes.Add((base.OwnerPoint, vector4));
			}
			else
			{
				rayNodes.Add((laserStartPoint + Tool2D.GetDir(laserShootDir, -90f - angleShift) * (base.InitialParameter.multiShootCount - 1) / 2f * base.InitialParameter.multiShootSpace + Tool2D.GetDir(laserShootDir, 90f - angleShift) * base.InitialParameter.inMultiShootIndex * base.InitialParameter.multiShootSpace, vector4));
				vector4 = laserShootDir;
			}
		}
		else
		{
			rayNodes.Add((originPoint, vector4));
		}
		while (true)
		{
			num3++;
			if (num3 >= 100)
			{
				Debug.LogError("死循环 请检查");
				break;
			}
			if (!isThroughWall && Physics.Raycast(rayNodes[rayNodes.Count - 1].pos, vector4, out var hitInfo2, num2, LayerMask.GetMask("Wall")))
			{
				num2 -= Vector3.Distance(rayNodes[rayNodes.Count - 1].pos, hitInfo2.point);
				rayNodes.Add((hitInfo2.point, vector4));
				EffectBase.CreateSpriteEffect("HitWall", hitInfo2.point);
				if (num <= 0)
				{
					break;
				}
				if (reboundBonusLifeTimeLeft > 0f)
				{
					reboundBonusLifeTimeLeft -= Time.deltaTime;
					base.DurationTimer -= Time.deltaTime;
					PlayLoopSE("Loop", base.spellCfg.duration - base.DurationTimer + base.SpellHoverTime);
				}
				num--;
				vector4 = Vector3.Reflect(vector4, hitInfo2.normal);
				continue;
			}
			rayNodes.Add((rayNodes[rayNodes.Count - 1].pos + vector4 * num2, vector4));
			break;
		}
		base.Direction = vector4.normalized;
	}

	public override UnitProperty GetMiniMalAngleTargetablePpt(bool checkWall = false)
	{
		if (base.SIP.spellIsFall)
		{
			return base.GetMiniMalAngleTargetablePpt();
		}
		switch (ownerPpt.unitCfg.unitType)
		{
		case UnitType.Player:
		case UnitType.Teammate:
		case UnitType.TeammateNotAttack:
			return LevelMgr.Inst.CurrentRoomCtrller.GetMinimalAngleTargetablePpt(laserStartPoint, laserShootDir, checkWall);
		case UnitType.Monster:
		case UnitType.Elite:
		case UnitType.Boss:
		case UnitType.WillAttack:
		case UnitType.NotAttack:
		case UnitType.Brittleness:
			return PlayerMgr.Inst.GetMinimalAngleTargetablePpt(laserStartPoint, laserShootDir, checkWall);
		default:
			Debug.LogError(ownerPpt.unitCfg.unitType);
			return null;
		}
	}

	private void UpdateLaserPointsWithFall()
	{
		ClearRefractionDataWhenUpdateLaserPoints();
		base.spellAroundOwnerCurrentAngle = baseSpellAroundOwnerCurrentAngle;
		rayNodes.Clear();
		ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
		if (finalShootSpatialInfo == null || !finalShootSpatialInfo.Target.HasValue)
		{
			return;
		}
		base.Direction = base.SIP.finalShootSpatialInfo.Direction;
		base.transform.position = laserStartPoint;
		if (base.spellCfg.isSplitSpell)
		{
			rayNodes.Add((base.SIP.finalShootSpatialInfo.Start, base.Direction));
			Transform obj = base.transform;
			List<(Vector3 pos, Vector3 dir)> list = rayNodes;
			obj.position = list[list.Count - 1].pos;
			PushFallingReboundPoints(base.transform.position + base.Direction * 2f);
			List<(Vector3 pos, Vector3 dir)> list2 = rayNodes;
			base.Direction = Tool2D.IgnoreZPoint(list2[list2.Count - 1].dir).normalized;
		}
		else
		{
			SpawnFallFirstLaser();
		}
		Transform obj2 = base.transform;
		List<(Vector3 pos, Vector3 dir)> list3 = rayNodes;
		obj2.position = list3[list3.Count - 1].pos;
		if (CanMakeFallingDamage)
		{
			MakeFallingGroundDamageToAround();
		}
		int num = base.rebounceTime;
		List<GameObject> list4 = (from e in GetFallingGroundDamageTargets()
			select e.gameObject).ToList();
		while (true)
		{
			Vector3? vector = null;
			if (base.remainRefractCount > 0 && list4.Count > 0)
			{
				refractedTargets.Clear();
				UnitProperty unitProperty = TryRefract(list4.ToArray());
				if ((bool)unitProperty)
				{
					list4.Add(unitProperty.gameObject);
					vector = unitProperty.transform.position;
				}
			}
			if (num > 0 && !vector.HasValue)
			{
				num--;
				vector = base.transform.position + base.Direction * 2f;
			}
			if (vector.HasValue)
			{
				PushFallingReboundPoints(vector.Value);
				Transform obj3 = base.transform;
				List<(Vector3 pos, Vector3 dir)> list5 = rayNodes;
				obj3.position = list5[list5.Count - 1].pos;
				List<(Vector3 pos, Vector3 dir)> list6 = rayNodes;
				base.Direction = Tool2D.IgnoreZPoint(list6[list6.Count - 1].dir).normalized;
				if (CanMakeFallingDamage)
				{
					MakeFallingGroundDamageToAround();
				}
				continue;
			}
			break;
		}
	}

	private IEnumerable<GameObject> HasTargetInLine(Vector3 start, Vector3 end)
	{
		start = Tool2D.IgnoreZPoint(start);
		end = Tool2D.IgnoreZPoint(end);
		return from e in Physics.SphereCastAll(start, laserWidth, end, Vector3.Distance(start, end), LayerMask.GetMask("Monster", "Monster_Fly", "Monster_Ghost"))
			select e.transform.gameObject into e
			where !refractedTargets.Contains(e)
			select e;
	}

	public override TakeDamageInfo OutputDamage(GameObject targetGO, TakeDamageInfo info = null, SpellAbilityType? damageRecordeType = null)
	{
		if (targetGO.CompareTag("RollBall") || targetGO.CompareTag("Butterfly"))
		{
			targetGO = targetGO.transform.parent.gameObject;
		}
		if (attackedGOs.Contains(targetGO))
		{
			return info;
		}
		if (targetGO.tag == "SolidObj" || targetGO.tag == "Untagged" || targetGO.tag == "InteractiveObj")
		{
			return info;
		}
		if (targetGO.tag == "SpellRebound")
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
					CreateHitEffect(targetGO.transform.position + GetLaserHeight());
					((Spell1002RollBall)component).TakeDamage(Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval));
				}
			}
			attackedGOs.Add(targetGO);
			attackedTimers.Add(base.spellCfg.DPSDamageInterval);
			return info;
		}
		if (targetGO.tag == "Destructible" || targetGO.tag == "Brittleness" || targetGO.tag == "Wall")
		{
			CreateHitEffect(targetGO.transform.position + GetLaserHeight());
		}
		if (IsSameCamp(UnitType.Player))
		{
			if (targetGO.tag == "Monster")
			{
				CreateHitEffect(targetGO.transform.position + GetLaserHeight());
			}
		}
		else if (targetGO.tag == "Player" || targetGO.tag == "Teammate")
		{
			CreateHitEffect(targetGO.transform.position + GetLaserHeight());
		}
		attackedGOs.Add(targetGO);
		attackedTimers.Add(base.spellCfg.DPSDamageInterval);
		return base.OutputDamage(targetGO, info);
	}

	private static Vector3 GetLaserHeight()
	{
		return new Vector3(0f, 0.3f, 0f);
	}

	private void SpawnBubble(Vector3[] posArr)
	{
		ObjPoolMgr.Inst.GetGO(GetRelationResourcePath("Bubble"), delegate(GameObject go)
		{
			go.transform.position = Vector3.zero;
			go.transform.localScale = Vector3.one;
			ObjPoolMgr.Inst.RecycleGO(go, 2f);
			go.GetComponent<EffectController>().ECChangeColor(base.ColorType);
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			float num = 0f;
			for (int i = 0; i < posArr.Length; i++)
			{
				list.Add(Tool2D.GetLayerPoint(posArr[i]));
				list.Add(Tool2D.GetLayerPoint(posArr[i]));
				if (i > 0)
				{
					list2.Add(i * 2 - 2);
					list2.Add(i * 2 - 2 + 2);
					list2.Add(i * 2 - 2 + 1);
					list2.Add(i * 2 - 2 + 1);
					list2.Add(i * 2 - 2 + 2);
					list2.Add(i * 2 - 2 + 3);
					num += Vector3.Distance(rayNodes[i].pos, rayNodes[i - 1].pos);
				}
			}
			Mesh sharedMesh = new Mesh
			{
				vertices = list.ToArray(),
				triangles = list2.ToArray()
			};
			go.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh = sharedMesh;
		});
	}

	public override void PoolRecycle()
	{
		if (rayNodes.Count > 1)
		{
			SpawnBubble(rayNodes.Select(((Vector3 pos, Vector3 dir) e) => e.pos).ToArray());
			if (rebounded)
			{
				SpawnBubble(new Vector3[2]
				{
					rayNodes[0].pos,
					rayNodes[1].pos
				});
			}
		}
		PlaySE("End");
		base.PoolRecycle();
	}

	public void SetLaserOwnerData(Vector3 laserstartpoint, Vector3 laserDir)
	{
		isPlayerSpell = false;
		laserStartPoint = laserstartpoint;
		laserShootDir = Tool2D.IgnoreZPoint(Tool2D.GetDir(Tool2D.GetDegree(laserDir) + angleShift));
	}

	public void IOnLaunchFromSpellEventHandle(SpellBase ownerSpell, SlotData triggerOrNull)
	{
		SetLaserOwnerData(base.transform.position, Tool2D.IgnoreZPoint(base.Direction));
	}

	public void OnLaunchFromUnitNotPlayer(UnitBase unit)
	{
		if (unit is Teammate5 teammate)
		{
			isPlayerSpell = false;
			teammate.currentCastingSpell.Add((this, teammate.GetShootMode()));
			SetLaserOwnerData(teammate.shootPosition.position, Tool2D.IgnoreZPoint(teammate.lastFrameDirection));
		}
		else if (unit is Teammate52 teammate2)
		{
			teammate2.currentCastingSpell.Add(this);
			SetLaserOwnerData(teammate2.shootPosition.position, Tool2D.IgnoreZPoint(teammate2.lastFrameTargetDirection));
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
}
