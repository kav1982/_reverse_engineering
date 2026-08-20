using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spell1010SnakeWalk : SpellBase
{
	[Space(50f)]
	[Header("Rotate")]
	public Transform tsf_Model;

	[Range(0f, 180f)]
	public float fullAngle;

	public float rotateSpeed;

	public ShockParam FallShockParam;

	[Header("Attack")]
	public LayerMask playerAttackLayer;

	public LayerMask monsterAttackLayer;

	public float chaseTargetSpeedLerpRatio;

	private float originalBodyWidth;

	private float originalBodyShadowWidth;

	[HideInInspector]
	public Vector3 originalDirection;

	private float currentRadian;

	private float attackIntervalTimer;

	private new Vector3? lastAroundTargetPosition;

	private bool fallOver;

	private readonly List<Vector3> fallPositions = new List<Vector3>();

	private LayerMask attackLayer
	{
		get
		{
			if (!IsSameCamp(UnitType.Player))
			{
				return monsterAttackLayer;
			}
			return playerAttackLayer;
		}
	}

	private Spell1010Effect Effect => (Spell1010Effect)EffectBase;

	protected override float InFallingReboundingGravity => base.InFallingReboundingGravity * 0.5f;

	protected override float FallingReboundForce => base.FallingReboundForce * 1.2f;

	public override void InitializeCallback()
	{
		ApplySpeedToVelocity();
		lastAroundTargetPosition = null;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			currentRadian = MathF.PI / 2f;
		}
		else
		{
			currentRadian = 4.712389f;
		}
		fallOver = !base.SIP.spellIsFall;
		fallPositions.Clear();
		originalDirection = base.Direction;
		sc_Rebound.enabled = false;
	}

	public override void FixedUpdate()
	{
		Effect.PushNewPosition(base.transform.position);
		base.FixedUpdate();
	}

	public override void Update()
	{
		base.Update();
		if (base.SIP.spellIsFall)
		{
			UpdateWithFall();
		}
		else
		{
			UpdateWithOutFall();
		}
		base.Direction = base.Direction.normalized;
	}

	private void UpdateWithFall()
	{
		float num = (1f + base.SIP.bounsSpeed / base.level1Cfg.speed + base.SIP.extraSpeedRatio) * base.SIP.finalSpeedRatio * 1.5f;
		attackIntervalTimer += Time.deltaTime * num;
		if (attackIntervalTimer >= base.spellCfg.DPSDamageInterval)
		{
			attackIntervalTimer = 0f;
			Vector3[] bodyPoints = Effect.GetBodyPoints();
			foreach (Vector3 point in fallPositions)
			{
				if (!(bodyPoints.Select((Vector3 body) => Tool2D.IgnoreZDistanceSqr(body, point)).Prepend(float.MaxValue).Min() >= 0.5f))
				{
					PlaySE("Hit");
					MakeFallDamageAtPosition(point);
				}
			}
		}
		if (fallOver && Effect.GetBodyPoints().Length <= 2)
		{
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}

	private void UpdateWithOutFall()
	{
		attackIntervalTimer += Time.deltaTime * GetLowFPSTimeScale();
		if (attackIntervalTimer >= base.spellCfg.DPSDamageInterval)
		{
			attackIntervalTimer -= base.spellCfg.DPSDamageInterval;
			foreach (var (collider, lookAt) in GetTargetInBody())
			{
				if (AOETriggerIn(collider))
				{
					CreateHitEffectLookAt(lookAt, Tool2D.IgnoreZPoint(collider.transform, base.transform.position.z));
				}
				if ((bool)TryRefract(collider.gameObject))
				{
					originalDirection = base.Direction;
				}
			}
		}
		base.DurationTimer += Time.deltaTime * GetLowFPSTimeScale();
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
		tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
		if (tsf_Layer.localScale.x <= 0f)
		{
			PoolRecycle();
		}
	}

	private void MakeFallDamageAtPosition(Vector3 point)
	{
		Effect.CreateFallingExplosion(point);
		foreach (Collider fallingGroundDamageTarget in GetFallingGroundDamageTargets(point))
		{
			AOETriggerIn(fallingGroundDamageTarget);
		}
	}

	protected override bool HalfLifeRandomTeleport()
	{
		if (base.HalfLifeRandomTeleport())
		{
			MonoBehaviour.print(rotateSpeed);
			int num = UnityEngine.Random.Range(0, 360);
			base.Direction = Tool2D.GetDir(num);
			currentRadian += 90f;
			float degree = Mathf.Sin(currentRadian) * fullAngle / 2f;
			Vector3 dir = Tool2D.GetDir(base.Direction, degree);
			Vector3 linearVelocity = dir * base.CurrentSpeed;
			rigid.linearVelocity = linearVelocity;
			tsf_Model.up = dir;
			return true;
		}
		return false;
	}

	private List<(Collider targets, Vector3 attackDir)> GetTargetInBody()
	{
		List<Collider> list = new List<Collider>();
		List<Vector3> list2 = new List<Vector3>();
		RaycastHit[] array = new RaycastHit[64];
		Vector3[] bodyPoints = Effect.GetBodyPoints();
		for (int i = 0; i < bodyPoints.Length - 3; i += 3)
		{
			Vector3 normalized = (bodyPoints[i + 3].IgnoreZ() - bodyPoints[i].IgnoreZ()).normalized;
			float maxDistance = Vector3.Distance(bodyPoints[i].IgnoreZ(), bodyPoints[i + 3].IgnoreZ());
			Ray ray = new Ray(bodyPoints[i].IgnoreZ(), normalized);
			float radius = base.ColliderRadius * base.transform.lossyScale.x;
			int num = Physics.SphereCastNonAlloc(ray, radius, array, maxDistance, attackLayer);
			for (int j = 0; j < num; j++)
			{
				RaycastHit raycastHit = array[j];
				if ((raycastHit.collider.gameObject.layer != LayerMask.NameToLayer("Default") || !(raycastHit.collider.GetComponentInParent<SpellBase>() == null)) && !list.Contains(raycastHit.collider))
				{
					list.Add(raycastHit.collider);
					Vector3 item = (raycastHit.transform.position.IgnoreZ() - bodyPoints[i].IgnoreZ()).IgnoreZ();
					list2.Add(item);
				}
			}
		}
		return list.Zip(list2).ToList();
	}

	public override void SpellAroundPlayer()
	{
		Vector3 aroundTargetBasePoint = GetAroundTargetBasePoint();
		if (lastAroundTargetPosition.HasValue)
		{
			Vector3 offset = aroundTargetBasePoint - lastAroundTargetPosition.Value;
			Effect.OffsetBody(offset);
		}
		lastAroundTargetPosition = aroundTargetBasePoint;
		if (base.SIP.spellIsFall)
		{
			base.SpellAroundPlayer();
			Quaternion quaternion = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(to: new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed), from: Vector2.right));
			tsf_Model.rotation = quaternion * Quaternion.Euler(0f, 0f, -90f);
			return;
		}
		float num = 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
		base.spellAroundOwnerCurrentAngle += num;
		Vector3 v = aroundTargetBasePoint + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius;
		base.Direction = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
		base.transform.position = Tool2D.IgnoreZPoint(v, base.transform.position.z);
		tsf_Model.up = Tool2D.GetDir(base.spellAroundOwnerCurrentAngle + 90f);
		SpellAroundPlayerUpdateMoveTrigger(num);
	}

	protected override void SpellFollowTarget()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellFollowTarget();
			Quaternion quaternion = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(to: new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed), from: Vector2.right));
			tsf_Model.rotation = quaternion * Quaternion.Euler(0f, 0f, -90f);
			return;
		}
		if (!base.SpellFollowHaveTarget)
		{
			spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
		}
		if (base.SpellFollowHaveTarget)
		{
			base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime * chaseTargetSpeedLerpRatio);
		}
		currentRadian += rotateSpeed * Time.deltaTime;
		float degree = Mathf.Sin(currentRadian) * fullAngle / 2f;
		Vector3 dir = Tool2D.GetDir(base.Direction, degree);
		Vector3 linearVelocity = dir * base.CurrentSpeed;
		rigid.linearVelocity = linearVelocity;
		tsf_Model.up = dir;
	}

	protected override void SpellFollowOwner()
	{
		float t = Mathf.Abs(Mathf.Abs(Tool2D.IgnoreZAngleWithSign(base.Direction, GetSpellFollowToOwnerDirection())) - 90f) / 90f;
		base.SpellFollowOwner();
		float num = 0.4f;
		currentRadian += rotateSpeed * Time.deltaTime;
		float degree = Mathf.Sin(currentRadian) * fullAngle / 2f;
		Vector3 dir = Tool2D.GetDir(base.Direction.normalized, degree);
		Vector3 linearVelocity = dir * base.CurrentSpeed * Mathf.Lerp(1f - num, 1f + num, t);
		rigid.linearVelocity = linearVelocity;
		tsf_Model.up = dir;
	}

	protected override void SpellFollowMouse()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellFollowMouse();
			Quaternion quaternion = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(to: new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed), from: Vector2.right));
			tsf_Model.rotation = quaternion * Quaternion.Euler(0f, 0f, -90f);
			return;
		}
		Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		base.Direction = Vector3.Lerp(base.Direction, ToPointDir(mousePoint) * base.CurrentSpeed, base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp);
		currentRadian += rotateSpeed * Time.deltaTime;
		float degree = Mathf.Sin(currentRadian) * fullAngle / 2f;
		Vector3 dir = Tool2D.GetDir(base.Direction.normalized, degree);
		Vector3 linearVelocity = dir * base.CurrentSpeed;
		rigid.linearVelocity = linearVelocity;
		tsf_Model.up = dir;
	}

	protected override void SpellNormalTransform()
	{
		if (base.SIP.spellIsFall)
		{
			base.SpellNormalTransform();
			Quaternion quaternion = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(to: new Vector2(base.Direction.x * base.CurrentSpeed, base.CurrentUpSpeed + base.Direction.y * base.CurrentSpeed), from: Vector2.right));
			tsf_Model.rotation = quaternion * Quaternion.Euler(0f, 0f, -90f);
			return;
		}
		currentRadian += rotateSpeed * Time.deltaTime;
		float degree = Mathf.Sin(currentRadian) * fullAngle / 2f;
		base.Direction = Tool2D.GetDir(originalDirection, degree);
		Vector3 linearVelocity = base.Direction * base.CurrentSpeed;
		rigid.linearVelocity = linearVelocity;
		tsf_Model.up = base.Direction;
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override void OnFallingGround()
	{
		CreateHitEffect();
		CamController.Inst.SetShock(FallShockParam);
		EffectBase.PlayFallingGroundSound();
		EffectBase.ManualCreateEffect("FallExplosionExt");
		MakeFallingGroundDamageToAround();
		fallPositions.Add(base.transform.position.IgnoreZ());
		if (!OnFallingGroundTryRebound())
		{
			Effect.disappearSpeed = base.CurrentSpeed + Mathf.Abs(base.CurrentUpSpeed);
			base.currentSpellMovement = SpellSpecialMovementType.Normal;
			ChangeCurrentSpeed(base.CurrentSpeed * 0.0001f);
			base.CurrentUpSpeed *= 0.0001f;
			fallOver = true;
			Effect.HideHead();
		}
	}

	protected override TakeDamageInfo CreateDefaultTakeDamageInfo(UnitProperty unit)
	{
		TakeDamageInfo takeDamageInfo = base.CreateDefaultTakeDamageInfo(unit);
		takeDamageInfo.damage = Mathf.CeilToInt(base.spellCfg.damage * base.spellCfg.DPSDamageInterval);
		return takeDamageInfo;
	}
}
